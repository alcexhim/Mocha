//
//  MochaClassLibraryDataFormat.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2020 Mike Becker's Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using Mocha.Core;
using Mocha.Core.TransactionOperations;
using UniversalEditor.Accessors;
using UniversalEditor.IO;
using UniversalEditor.ObjectModels.FileSystem;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot.TransactionOperations;

namespace UniversalEditor.Plugins.Mocha.DataFormats.MochaBinary
{
	public class MochaBinaryDataFormat : Slick.SlickBinaryDataFormat
	{
		private static DataFormatReference _dfr = null;
		protected override DataFormatReference MakeReferenceInternal()
		{
			if (_dfr == null)
			{
				_dfr = base.MakeReferenceInternal();
				_dfr.Capabilities.Clear();
				_dfr.Capabilities.Add(typeof(MochaClassLibraryObjectModel), DataFormatCapabilities.All);
			}
			return _dfr;
		}

		protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeLoadInternal(objectModels);
			objectModels.Push(new FileSystemObjectModel());
		}

		private struct LIBRARY_INFO
		{
			public int instanceCount;
			public int attributeValueCount;
			public int relationshipCount;
		}
		private struct INSTANCE_INFO
		{
			public int instanceIndex;
			public int attributeValueCount;
		}

		protected override void AfterLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.AfterLoadInternal(objectModels);

			if (objectModels.Count < 2) throw new ObjectModelNotSupportedException("must have a FileSystemObjectModel and a MochaClassLibraryObjectModel in the stack");

			FileSystemObjectModel fsom = (objectModels.Pop() as FileSystemObjectModel);
			if (fsom == null) throw new ObjectModelNotSupportedException();

			ObjectModel om = objectModels.Pop();

			MochaSnapshotObjectModel mcx = (om as MochaSnapshotObjectModel);
			MochaClassLibraryObjectModel mcl = (om as MochaClassLibraryObjectModel);
			if (mcl == null && mcx == null) throw new ObjectModelNotSupportedException();

			List<Guid> _instanceGuids = new List<Guid>();
			List<string> _stringTable = new List<string>();

			LIBRARY_INFO[] library_info = null;

			// COMMON TABLES
			#region Guid Table
			{
				File fGlobalIdentifiers = fsom.FindFile("GlobalIdentifiers");
				using (MemoryAccessor ma = new MemoryAccessor(fGlobalIdentifiers.GetData()))
				{
					int instanceCount = ma.Reader.ReadInt32();
					for (int i = 0; i < instanceCount; i++)
					{
						_instanceGuids.Add(ma.Reader.ReadGuid());
					}
				}
			}
			#endregion
			#region String Table
			{
				File f = fsom.FindFile("StringTable");
				using (MemoryAccessor ma = new MemoryAccessor(f.GetData()))
				{
					int stringTableCount = ma.Reader.ReadInt32();
					for (int i = 0; i < stringTableCount; i++)
					{
						string value = ma.Reader.ReadNullTerminatedString();
						_stringTable.Add(value);
					}
				}
			}
			#endregion

			// MCL-SPECIFIC
			#region Libraries
			File fLibraries = fsom.FindFile("Libraries");
			if (mcl != null && fLibraries != null)
			{
				using (MemoryAccessor ma = new MemoryAccessor(fLibraries.GetData()))
				{
					int libraryCount = ma.Reader.ReadInt32();
					library_info = new LIBRARY_INFO[libraryCount];

					for (int i = 0; i < libraryCount; i++)
					{
						MochaLibrary library = new MochaLibrary();
						library.ID = ma.Reader.ReadGuid();
						library_info[i].instanceCount = ma.Reader.ReadInt32();
						// library_info[i].attributeValueCount = ma.Reader.ReadInt32();
						library_info[i].relationshipCount = ma.Reader.ReadInt32();

						mcl.Libraries.Add(library);
					}
				}
			}
			else
			{
				Console.Error.WriteLine("mocha: mcl: error: ignoring 'Libraries' section found in non-mcl file");
			}
			#endregion

			INSTANCE_INFO[] instance_info = null;
			#region Instances
			File fInstances = fsom.FindFile("Instances");
			if (mcl != null && fInstances != null)
			{
				using (MemoryAccessor ma = new MemoryAccessor(fInstances.GetData()))
				{
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						int instanceCount = ma.Reader.ReadInt32();
						instance_info = new INSTANCE_INFO[instanceCount];
						for (int j = 0; j < instanceCount; j++)
						{
							instance_info[j].instanceIndex = ma.Reader.ReadInt32();
							instance_info[j].attributeValueCount = ma.Reader.ReadInt32();
							MochaInstanceFlags flags = (MochaInstanceFlags)ma.Reader.ReadInt32();

							int? index = null;
							if ((flags & MochaInstanceFlags.HasIndex) == MochaInstanceFlags.HasIndex)
							{
								index = ma.Reader.ReadInt32();
							}

							MochaInstance inst = new MochaInstance();
							inst.ID = _instanceGuids[instance_info[j].instanceIndex];
							inst.Index = index;
							mcl.Libraries[i].Instances.Add(inst);
						}
					}
				}
			}
			else
			{
				Console.Error.WriteLine("mocha: mcl: error: ignoring 'Instances' section found in non-mcl file");
			}
			#endregion

			#region Attributes
			File fAttributes = fsom.FindFile("Attributes");
			if (mcl != null && fAttributes != null)
			{
				using (MemoryAccessor ma = new MemoryAccessor(fAttributes.GetData()))
				{
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
						{
							for (int k = 0; k < instance_info[j].attributeValueCount; k++)
							{
								int attributeInstanceIndex = ma.Reader.ReadInt32();

								MochaAttributeValue val = new MochaAttributeValue();
								val.AttributeInstanceID = _instanceGuids[attributeInstanceIndex];

								MochaAttributeType attributeType = (MochaAttributeType)ma.Reader.ReadInt32();
								switch (attributeType)
								{
									case MochaAttributeType.None: break;
									case MochaAttributeType.Text:
									{
										int stringTableIndex = ma.Reader.ReadInt32();
										string value = _stringTable[stringTableIndex];
										val.Value = value;
										break;
									}
									case MochaAttributeType.Boolean:
									{
										bool value = ma.Reader.ReadBoolean();
										val.Value = value;
										break;
									}
									case MochaAttributeType.Date:
									{
										DateTime value = ma.Reader.ReadDateTime();
										val.Value = value;
										break;
									}
									case MochaAttributeType.Unknown:
									{
										break;
									}
								}

								mcl.Libraries[i].Instances[j].AttributeValues.Add(val);
							}
						}
					}
				}
			}
			else
			{
				Console.Error.WriteLine("mocha: mcl: error: ignoring 'Attributes' section found in non-mcl file");
			}
			#endregion

			#region Relationships
			File fRelationships = fsom.FindFile("Relationships");
			if (mcl != null && fRelationships != null)
			{
				using (MemoryAccessor ma = new MemoryAccessor(fRelationships.GetData()))
				{
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						for (int j = 0; j < library_info[i].relationshipCount; j++)
						{
							int relationshipIndex = ma.Reader.ReadInt32();
							int sourceInstanceIndex = ma.Reader.ReadInt32();

							MochaRelationship rel = new MochaRelationship();
							rel.RelationshipInstanceID = _instanceGuids[relationshipIndex];
							rel.SourceInstanceID = _instanceGuids[sourceInstanceIndex];

							int targetInstanceCount = ma.Reader.ReadInt32();
							for (int k = 0; k < targetInstanceCount; k++)
							{
								int instanceIndex = ma.Reader.ReadInt32();
								rel.DestinationInstanceIDs.Add(_instanceGuids[instanceIndex]);
							}

							mcl.Libraries[i].Relationships.Add(rel);
						}
					}
				}
			}
			else
			{
				Console.Error.WriteLine("mocha: mcl: error: ignoring 'Relationships' section found in non-mcl file");
			}
			#endregion

			#region Tenants
			File fTenants = fsom.FindFile("Tenants");
			if (mcl != null && fTenants != null)
			{
				using (MemoryAccessor ma = new MemoryAccessor(fTenants.GetData()))
				{
					int tenantCount = ma.Reader.ReadInt32();
					for (int i = 0; i < tenantCount; i++)
					{
						int instanceIndex = ma.Reader.ReadInt32();
						Guid instanceGuid = _instanceGuids[instanceIndex];

						int tenantNameIndex = ma.Reader.ReadInt32();
						string tenantName = _stringTable[tenantNameIndex];

						MochaTenant tenant = new MochaTenant();
						tenant.ID = instanceGuid;
						tenant.Name = tenantName;

						int libraryReferenceCount = ma.Reader.ReadInt32();
						for (int j = 0; j < libraryReferenceCount; j++)
						{
							int libraryIndex = ma.Reader.ReadInt32();
							Guid libraryID = _instanceGuids[libraryIndex];
							tenant.LibraryReferences.Add(new MochaLibraryReference(libraryID));
						}

						int instanceCount = ma.Reader.ReadInt32();
						for (int j = 0; j < instanceCount; j++)
						{
							int instanceIndex2 = ma.Reader.ReadInt32();
							MochaInstance inst = new MochaInstance();
							inst.ID = _instanceGuids[instanceIndex2];
							tenant.Instances.Add(inst);
						}

						int relationshipCount = ma.Reader.ReadInt32();
						for (int j = 0; j < relationshipCount; j++)
						{
							int sourceInex = ma.Reader.ReadInt32();
							Guid ssource = _instanceGuids[sourceInex];
							int relationshipInex = ma.Reader.ReadInt32();
							Guid relati = _instanceGuids[relationshipInex];

							MochaRelationship rel = new MochaRelationship();
							rel.SourceInstanceID = ssource;
							rel.RelationshipInstanceID = relati;

							int count = ma.Reader.ReadInt32();
							for (int k = 0; k < count; k++)
							{
								int targetIndex = ma.Reader.ReadInt32();
								Guid targ = _instanceGuids[targetIndex];
								rel.DestinationInstanceIDs.Add(targ);
							}

							tenant.Relationships.Add(rel);
						}
						mcl.Tenants.Add(tenant);

					}
					ma.Close();
					fTenants.SetData(ma.ToArray());

				}
			}
			else
			{
				Console.Error.WriteLine("mocha: mcl: error: ignoring 'Tenants' section found in non-mcl file");
			}
			#endregion

			#region Journal
			// journal is present in MCX / MCD (application, data files)
			// is an opcode based format
			// eg. 0x01 = create instance, 0x02 = delete instance
			// 		0x04 = set attribute value, 0x05 = delete attribute value
			//		0x08 = create relationship, 0x09 = remove relationship

			File fJournal = fsom.FindFile("Journal");
			if (mcx != null && fJournal != null)
			{
				using (MemoryAccessor ma = new MemoryAccessor(fJournal.GetData()))
				{
					MochaSnapshotTransaction currentTransaction = null;

					while (!ma.Reader.EndOfStream)
					{
						MochaOpcode opcode = (MochaOpcode)ma.Reader.ReadByte();
						switch (opcode)
						{
							case MochaOpcode.BeginTransaction:
							{
								currentTransaction = new MochaSnapshotTransaction();
								currentTransaction.TenantName = _stringTable[ma.Reader.ReadInt32()];
								break;
							}
							case MochaOpcode.EndTransaction:
							{
								mcx.Transactions.Add(currentTransaction);
								currentTransaction = null;
								break;
							}
							case MochaOpcode.CreateInstance:
							{
								DateTime effectiveDate = ma.Reader.ReadDateTime();
								int guidIndex = ma.Reader.ReadInt32();
								int classGuidIndex = ma.Reader.ReadInt32();

								MochaSnapshotCreateInstanceTransactionOperation op = new MochaSnapshotCreateInstanceTransactionOperation();
								op.GlobalIdentifier = _instanceGuids[guidIndex];
								op.ClassGlobalIdentifier = _instanceGuids[classGuidIndex];
								op.EffectiveDate = effectiveDate;
								currentTransaction.Operations.Add(op);
								break;
							}
							case MochaOpcode.CreateRelationship:
							case MochaOpcode.RemoveRelationship:
							{
								DateTime effectiveDate = ma.Reader.ReadDateTime();
								int sourceInstanceIndex = ma.Reader.ReadInt32();
								int relationshipIndex = ma.Reader.ReadInt32();
								int targetInstanceCount = ma.Reader.ReadInt32();

								MochaSnapshotAssociateRelationshipTransactionOperation rel = new MochaSnapshotAssociateRelationshipTransactionOperation();
								rel.RelationshipInstanceID = _instanceGuids[relationshipIndex];
								rel.SourceInstanceID = _instanceGuids[sourceInstanceIndex];
								rel.EffectiveDate = effectiveDate;

								for (int k = 0; k < targetInstanceCount; k++)
								{
									int instanceIndex = ma.Reader.ReadInt32();
									rel.TargetInstanceIDs.Add(_instanceGuids[instanceIndex]);
								}

								if (opcode == MochaOpcode.RemoveRelationship)
								{
									rel.Remove = true;
								}

								currentTransaction.Operations.Add(rel);
								break;
							}
							case MochaOpcode.AssignAttribute:
							{
								DateTime effectiveDate = ma.Reader.ReadDateTime();
								int instanceIndex = ma.Reader.ReadInt32();
								Guid instanceID = _instanceGuids[instanceIndex];

								int attributeIndex = ma.Reader.ReadInt32();
								Guid attributeInstanceID = _instanceGuids[attributeIndex];

								object value = ReadMochaValue(ma.Reader, _stringTable);
								
								MochaSnapshotAssignAttributeTransactionOperation mav = new MochaSnapshotAssignAttributeTransactionOperation();
								mav.SourceInstanceID = instanceID;
								mav.AttributeInstanceID = attributeInstanceID;
								mav.EffectiveDate = effectiveDate;
								mav.Value = value;
								currentTransaction.Operations.Add(mav);
								break;
							}
						}
					}
				}
			}
			else
			{
				Console.Error.WriteLine("mocha: mcl: error: ignoring 'Journal' section found in non-mcx file");
			}
			#endregion
		}

		private object ReadMochaValue(Reader reader, List<string> stringTable)
		{
			object value = null;
			MochaAttributeType dataType = (MochaAttributeType)reader.ReadInt32();
			switch (dataType)
			{
				case MochaAttributeType.Boolean:
				{
					value = reader.ReadBoolean();
					break;
				}
				case MochaAttributeType.Date:
				{
					value = reader.ReadDateTime();
					break;
				}
				case MochaAttributeType.None:
				{
					value = null;
					break;
				}
				case MochaAttributeType.Numeric:
				{
					value = reader.ReadDecimal();
					break;
				}
				case MochaAttributeType.Text:
				{
					int valueIndex = reader.ReadInt32();
					value = stringTable[valueIndex];
					break;
				}
			}
			return value;
		}

		protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeSaveInternal(objectModels);

			FileNameSize = 32;

			ObjectModel om = objectModels.Pop();
			MochaClassLibraryObjectModel mcl = (om as MochaClassLibraryObjectModel);
			MochaSnapshotObjectModel mcx = (om as MochaSnapshotObjectModel);
			if (mcl == null && mcx == null) throw new ObjectModelNotSupportedException();

			FileSystemObjectModel fsom = new FileSystemObjectModel();

			List<Guid> _instanceIndices = new List<Guid>();
			List<string> _stringTable = new List<string>();

			if (mcl != null)
			{
				#region Libraries
				{
					File f = fsom.AddFile("Libraries");
					MemoryAccessor ma = new MemoryAccessor();
					ma.Writer.WriteInt32(mcl.Libraries.Count);
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						ma.Writer.WriteGuid(mcl.Libraries[i].ID);
						ma.Writer.WriteInt32(mcl.Libraries[i].Instances.Count);
						ma.Writer.WriteInt32(mcl.Libraries[i].Relationships.Count);
					}
					ma.Close();
					f.SetData(ma.ToArray());
				}
				#endregion
				#region Instances
				{
					File f = fsom.AddFile("Instances");
					MemoryAccessor ma = new MemoryAccessor();
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						ma.Writer.WriteInt32(mcl.Libraries[i].Instances.Count);
						for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
						{
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, mcl.Libraries[i].Instances[j].ID));
							ma.Writer.WriteInt32(mcl.Libraries[i].Instances[j].AttributeValues.Count);

							MochaInstanceFlags flags = MochaInstanceFlags.None;
							if (mcl.Libraries[i].Instances[j].Index != null)
							{
								flags |= MochaInstanceFlags.HasIndex;
							}
							ma.Writer.WriteInt32((int)flags);

							if ((flags & MochaInstanceFlags.HasIndex) == MochaInstanceFlags.HasIndex)
							{
								ma.Writer.WriteInt32(mcl.Libraries[i].Instances[j].Index.Value);
							}
						}
					}
					ma.Close();
					f.SetData(ma.ToArray());
				}
				#endregion
				#region Attributes
				{
					File f = fsom.AddFile("Attributes");
					MemoryAccessor ma = new MemoryAccessor();

					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						/*
						writer.WriteInt32(mcl.Libraries[i].Metadata.Count);
						for (int j = 0; j < mcl.Libraries[i].Metadata.Count; j++)
						{
							writer.WriteNullTerminatedString(mcl.Libraries[i].Metadata[j].Name);
							writer.WriteNullTerminatedString(mcl.Libraries[i].Metadata[j].Value);
						}
						*/
						for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
						{
							for (int k = 0; k < mcl.Libraries[i].Instances[j].AttributeValues.Count; k++)
							{
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, mcl.Libraries[i].Instances[j].AttributeValues[k].AttributeInstanceID));

								if (mcl.Libraries[i].Instances[j].AttributeValues[k].Value == null)
								{
									ma.Writer.WriteInt32((int)MochaAttributeType.None);
								}
								else if (mcl.Libraries[i].Instances[j].AttributeValues[k].Value is string)
								{
									ma.Writer.WriteInt32((int)MochaAttributeType.Text);
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, mcl.Libraries[i].Instances[j].AttributeValues[k].Value as string));
								}
								else
								{
									ma.Writer.WriteInt32((int)MochaAttributeType.Unknown);
									ma.Writer.WriteObject(mcl.Libraries[i].Instances[j].AttributeValues[k].Value);
								}
							}
						}
					}

					ma.Close();
					f.SetData(ma.ToArray());
				}
				#endregion
				#region Relationships
				{
					File f = fsom.AddFile("Relationships");
					MemoryAccessor ma = new MemoryAccessor();
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						for (int j = 0; j < mcl.Libraries[i].Relationships.Count; j++)
						{
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, mcl.Libraries[i].Relationships[j].RelationshipInstanceID));
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, mcl.Libraries[i].Relationships[j].SourceInstanceID));

							ma.Writer.WriteInt32(mcl.Libraries[i].Relationships[j].DestinationInstanceIDs.Count);
							for (int k = 0; k < mcl.Libraries[i].Relationships[j].DestinationInstanceIDs.Count; k++)
							{
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, mcl.Libraries[i].Relationships[j].DestinationInstanceIDs[k]));
							}
						}
					}
					ma.Close();
					f.SetData(ma.ToArray());
				}
				#endregion

				#region Tenants
				{
					if (mcl.Tenants.Count > 0)
					{
						File f = fsom.AddFile("Tenants");
						MemoryAccessor ma = new MemoryAccessor();
						ma.Writer.WriteInt32(mcl.Tenants.Count);
						foreach (MochaTenant tenant in mcl.Tenants)
						{
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.ID));
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, tenant.Name));

							ma.Writer.WriteInt32(tenant.LibraryReferences.Count);
							for (int i = 0; i < tenant.LibraryReferences.Count; i++)
							{
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.LibraryReferences[i].LibraryID));
							}

							ma.Writer.WriteInt32(tenant.Instances.Count);
							for (int i = 0; i < tenant.Instances.Count; i++)
							{
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.Instances[i].ID));
							}
							ma.Writer.WriteInt32(tenant.Relationships.Count);
							for (int i = 0; i < tenant.Relationships.Count; i++)
							{
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.Relationships[i].SourceInstanceID));
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.Relationships[i].RelationshipInstanceID));
								ma.Writer.WriteInt32(tenant.Relationships[i].DestinationInstanceIDs.Count);
								for (int j = 0; j < tenant.Relationships[i].DestinationInstanceIDs.Count; j++)
								{
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.Relationships[i].DestinationInstanceIDs[j]));
								}
							}
						}
						ma.Close();
						f.SetData(ma.ToArray());
					}
				}
				#endregion
			}
			else if (mcx != null)
			{
				#region Journal
				{
					if (mcx.Transactions.Count > 0)
					{
						File f = fsom.AddFile("Journal");
						MemoryAccessor ma = new MemoryAccessor();
						foreach (MochaSnapshotTransaction t in mcx.Transactions)
						{
							ma.Writer.WriteByte((byte)MochaOpcode.BeginTransaction);
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, t.TenantName));
							foreach (MochaSnapshotTransactionOperation op in t.Operations)
							{
								if (op is MochaSnapshotCreateInstanceTransactionOperation ci)
								{
									ma.Writer.WriteByte((byte)MochaOpcode.CreateInstance);
									ma.Writer.WriteDateTime(ci.EffectiveDate);
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, ci.GlobalIdentifier));
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, ci.ClassGlobalIdentifier));
								}
								else if (op is MochaSnapshotAssignAttributeTransactionOperation aa)
								{
									ma.Writer.WriteByte((byte)MochaOpcode.AssignAttribute);
									ma.Writer.WriteDateTime(aa.EffectiveDate);
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, aa.SourceInstanceID));
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, aa.AttributeInstanceID));
									WriteMochaValue(ma.Writer, aa.Value, _stringTable);
								}
								else if (op is MochaSnapshotAssociateRelationshipTransactionOperation ar)
								{
									ma.Writer.WriteByte((byte)MochaOpcode.CreateRelationship);
									ma.Writer.WriteDateTime(ar.EffectiveDate);
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, ar.SourceInstanceID));
									ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, ar.RelationshipInstanceID));
									ma.Writer.WriteInt32(ar.TargetInstanceIDs.Count);
									foreach (Guid id in ar.TargetInstanceIDs)
									{
										ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, id));
									}
								}
							}
							ma.Writer.WriteByte((byte)MochaOpcode.EndTransaction);
						}
						ma.Close();
						f.SetData(ma.ToArray());
					}
				}
				#endregion
			}
			#region Guid Table
			{
				File f = fsom.AddFile("GlobalIdentifiers");
				MemoryAccessor ma = new MemoryAccessor();
				ma.Writer.WriteInt32(_instanceIndices.Count);
				foreach (Guid id in _instanceIndices)
				{
					ma.Writer.WriteGuid(id);
				}
				ma.Close();
				f.SetData(ma.ToArray());
			}
			#endregion
			#region String Table
			{
				File f = fsom.AddFile("StringTable");
				MemoryAccessor ma = new MemoryAccessor();
				ma.Writer.WriteInt32(_stringTable.Count);
				foreach (string id in _stringTable)
				{
					ma.Writer.WriteNullTerminatedString(id);
				}
				ma.Close();
				f.SetData(ma.ToArray());
			}
			#endregion

			objectModels.Push(fsom);
		}

		private void WriteMochaValue(Writer writer, object value, List<string> _stringTable)
		{
			if (value == null)
			{
				writer.WriteInt32((int)MochaAttributeType.None);
			}
			else if (value is bool)
			{
				writer.WriteInt32((int)MochaAttributeType.Boolean);
				writer.WriteBoolean((bool)value);
			}
			else if (value is decimal)
			{
				writer.WriteInt32((int)MochaAttributeType.Numeric);
				writer.WriteDecimal((decimal)value);
			}
			else if (value is string)
			{
				writer.WriteInt32((int)MochaAttributeType.Text);
				writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, value.ToString()));
			}
			else
			{
				writer.WriteInt32((int)MochaAttributeType.Unknown);
				writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, value.ToString()));
			}
		}

		private int AddOrUpdateLookupTable<T1>(List<T1> dict, T1 id)
		{
			if (!dict.Contains(id))
			{
				dict.Add(id);
			}
			return dict.IndexOf(id);
		}
	}
}
