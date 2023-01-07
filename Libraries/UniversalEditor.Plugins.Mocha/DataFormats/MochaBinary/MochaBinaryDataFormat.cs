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
using UniversalEditor.Accessors;
using UniversalEditor.IO;
using UniversalEditor.ObjectModels.FileSystem;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;

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
			public int libraryReferenceCount;
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

			MochaClassLibraryObjectModel mcl = (objectModels.Pop() as MochaClassLibraryObjectModel);
			if (mcl == null) throw new ObjectModelNotSupportedException();

			List<Guid> _instanceGuids = new List<Guid>();
			List<string> _stringTable = new List<string>();

			LIBRARY_INFO[] library_info = null;

			#region Libraries
			{
				File fLibraries = fsom.FindFile("Libraries");
				using (MemoryAccessor ma = new MemoryAccessor(fLibraries.GetData()))
				{
					int libraryCount = ma.Reader.ReadInt32();
					library_info = new LIBRARY_INFO[libraryCount];

					for (int i = 0; i < libraryCount; i++)
					{
						MochaLibrary library = new MochaLibrary();
						library.ID = ma.Reader.ReadGuid();
						library_info[i].libraryReferenceCount = ma.Reader.ReadInt32();
						library_info[i].instanceCount = ma.Reader.ReadInt32();
						// library_info[i].attributeValueCount = ma.Reader.ReadInt32();
						library_info[i].relationshipCount = ma.Reader.ReadInt32();

						mcl.Libraries.Add(library);
					}
				}
			}
			#endregion
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

			INSTANCE_INFO[] instance_info = null;
			#region Instances
			{
				File f = fsom.FindFile("Instances");
				using (MemoryAccessor ma = new MemoryAccessor(f.GetData()))
				{
					for (int i = 0; i < mcl.Libraries.Count; i++)
					{
						int instance_info_offset = 0; // fixme: this should not be needed
						int libraryReferenceCount = ma.Reader.ReadInt32();
						for (int j = 0; j < libraryReferenceCount; j++)
						{
							int libraryIndex = ma.Reader.ReadInt32();
							Guid libraryID = _instanceGuids[libraryIndex];
							mcl.Libraries[i].LibraryReferences.Add(libraryID);
						}
						int instanceCount = ma.Reader.ReadInt32();
						// if (instance_info == null)
						{
							instance_info = new INSTANCE_INFO[instanceCount];
						}
						/*
						else
						{
							instance_info_offset = instance_info.Length;
							Array.Resize<INSTANCE_INFO>(ref instance_info, instance_info.Length + instanceCount);
						}
						*/
						for (int j = 0; j < instanceCount; j++)
						{
							instance_info[j + instance_info_offset].instanceIndex = ma.Reader.ReadInt32();
							instance_info[j + instance_info_offset].attributeValueCount = ma.Reader.ReadInt32();
							MochaInstanceFlags flags = (MochaInstanceFlags)ma.Reader.ReadInt32();

							int? index = null;
							if ((flags & MochaInstanceFlags.HasIndex) == MochaInstanceFlags.HasIndex)
							{
								index = ma.Reader.ReadInt32();
							}

							MochaInstance inst = new MochaInstance();
							inst.ID = _instanceGuids[instance_info[j + instance_info_offset].instanceIndex];
							inst.Index = index;
							mcl.Libraries[i].Instances.Add(inst);
						}
					}
				}
			}
			#endregion
			#region Attributes
			{
				File f = fsom.FindFile("Attributes");
				using (MemoryAccessor ma = new MemoryAccessor(f.GetData()))
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

								val.Value = ReadMochaAttributeValue(ma.Reader, _stringTable);
								mcl.Libraries[i].Instances[j].AttributeValues.Add(val);
							}
						}
					}
				}
			}
			#endregion
			#region Relationships
			{
				File f = fsom.FindFile("Relationships");
				using (MemoryAccessor ma = new MemoryAccessor(f.GetData()))
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
			#endregion

			#region Tenants
			{
				File f = fsom.FindFile("Tenants");
				if (f != null)
				{
					using (MemoryAccessor ma = new MemoryAccessor(f.GetData()))
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
								tenant.LibraryReferences.Add(libraryID);
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
						f.SetData(ma.ToArray());

					}
				}
			}
			#endregion

			#region Journal
			{
				// journal is present in MCX / MCD (application, data files)
				// is an opcode based format
				// eg. 0x01 = create instance, 0x02 = delete instance
				// 		0x04 = set attribute value, 0x05 = delete attribute value
				//		0x08 = create relationship, 0x09 = remove relationship

				File f = fsom.FindFile("Journal");
				if (f != null)
				{
					using (MemoryAccessor ma = new MemoryAccessor(f.GetData()))
					{
						for (int i = 0; i < mcl.Tenants.Count; i++)
						{
							bool readingTenant = true;
							while (readingTenant)
							{
								MochaOpcode opcode = (MochaOpcode)ma.Reader.ReadByte();
								switch (opcode)
								{
									case MochaOpcode.BeginTenant:
									{
										int tenantNameIndex = ma.Reader.ReadInt32();
										break;
									}
									case MochaOpcode.EndTenant:
									{
										readingTenant = false; // exit outer loop
										break;
									}
									case MochaOpcode.CreateInstance:
									{
										int guidIndex = ma.Reader.ReadInt32();

										MochaInstance inst = new MochaInstance();
										inst.ID = _instanceGuids[guidIndex];
										mcl.Tenants[i].Instances.Add(inst);
										break;
									}
									case MochaOpcode.CreateRelationship:
									case MochaOpcode.RemoveRelationship:
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

										if (opcode == MochaOpcode.RemoveRelationship)
										{
											rel.Remove = true;
										}

										mcl.Tenants[i].Relationships.Add(rel);
										break;
									}
									case MochaOpcode.SetAttributeValue:
									{
										int instanceIndex = ma.Reader.ReadInt32();
										Guid instanceID = _instanceGuids[instanceIndex];

										int attributeIndex = ma.Reader.ReadInt32();
										Guid attributeInstanceID = _instanceGuids[attributeIndex];

										object value = ReadMochaAttributeValue(ma.Reader, _stringTable);

										MochaAttributeValue mav = new MochaAttributeValue();
										mav.AttributeInstanceID = attributeInstanceID;
										mav.Value = value;
										mcl.Tenants[i].Instances[instanceID].AttributeValues.Add(mav);
										break;
									}
								}
							}
						}
					}
				}
			}
			#endregion
		}

		private object ReadMochaAttributeValue(Reader reader, List<string>_stringTable)
		{
			MochaAttributeType attributeType = (MochaAttributeType)reader.ReadInt32();
			switch (attributeType)
			{
				case MochaAttributeType.None:
				{
					return null;
				}
				case MochaAttributeType.Text:
				{
					int stringTableIndex = reader.ReadInt32();
					string value = _stringTable[stringTableIndex];
					return value;
				}
				case MochaAttributeType.Boolean:
				{
					bool value = reader.ReadBoolean();
					return value;
				}
				case MochaAttributeType.Date:
				{
					DateTime value = reader.ReadDateTime();
					return value;
				}
				case MochaAttributeType.Numeric:
				{
					decimal value = reader.ReadDecimal();
					return value;
				}
				case MochaAttributeType.Unknown:
				{
					break;
				}
			}
			throw new NotSupportedException();
		}

		protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeSaveInternal(objectModels);

			FileNameSize = 32;

			MochaClassLibraryObjectModel mcl = (objectModels.Pop() as MochaClassLibraryObjectModel);
			if (mcl == null) throw new ObjectModelNotSupportedException();

			FileSystemObjectModel fsom = new FileSystemObjectModel();

			List<Guid> _instanceIndices = new List<Guid>();
			List<string> _stringTable = new List<string>();

			#region Libraries
			{
				File f = fsom.AddFile("Libraries");
				MemoryAccessor ma = new MemoryAccessor();
				ma.Writer.WriteInt32(mcl.Libraries.Count);
				for (int i = 0; i < mcl.Libraries.Count; i++)
				{
					ma.Writer.WriteGuid(mcl.Libraries[i].ID);
					ma.Writer.WriteInt32(mcl.Libraries[i].LibraryReferences.Count);
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
					ma.Writer.WriteInt32(mcl.Libraries[i].LibraryReferences.Count);
					for (int j = 0; j < mcl.Libraries[i].LibraryReferences.Count; j++)
					{
						ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, mcl.Libraries[i].LibraryReferences[j]));
					}
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

							WriteMochaAttributeValue(ma.Writer, _stringTable, mcl.Libraries[i].Instances[j].AttributeValues[k].Value);
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
							ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.LibraryReferences[i]));
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

			#region Journal
			{
				if (mcl.Tenants.Count > 0)
				{
					File f = fsom.AddFile("Journal");
					MemoryAccessor ma = new MemoryAccessor();
					foreach (MochaTenant tenant in mcl.Tenants)
					{
						ma.Writer.WriteByte((byte)MochaOpcode.BeginTenant);
						ma.Writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, tenant.Name));
						for (int i = 0; i < tenant.Instances.Count; i++)
						{
							for (int j = 0; j < tenant.Instances[i].AttributeValues.Count; j++)
							{
								ma.Writer.WriteByte((byte)MochaOpcode.SetAttributeValue);
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.Instances[i].ID));
								ma.Writer.WriteInt32(AddOrUpdateLookupTable<Guid>(_instanceIndices, tenant.Instances[i].AttributeValues[j].AttributeInstanceID));

								WriteMochaAttributeValue(ma.Writer, _stringTable, tenant.Instances[i].AttributeValues[j].Value);
							}
						}
						ma.Writer.WriteByte((byte)MochaOpcode.EndTenant);
					}
					ma.Close();
					f.SetData(ma.ToArray());
				}
			}
			#endregion

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

		private void WriteMochaAttributeValue(Writer writer, List<string> _stringTable, object value)
		{
			if (value == null)
			{
				writer.WriteInt32((int)MochaAttributeType.None);
			}
			else if (value is string)
			{
				writer.WriteInt32((int)MochaAttributeType.Text);
				writer.WriteInt32(AddOrUpdateLookupTable<string>(_stringTable, value as string));
			}
			else if (value is bool)
			{
				writer.WriteInt32((int)MochaAttributeType.Boolean);
				writer.WriteBoolean((bool)value);
			}
			else if (value is DateTime)
			{
				writer.WriteInt32((int)MochaAttributeType.Date);
				writer.WriteDateTime((DateTime)value);
			}
			else if (value is decimal || value is int)
			{
				writer.WriteInt32((int)MochaAttributeType.Numeric);

				// ...... eww.
				writer.WriteDecimal(Decimal.Parse(value.ToString()));
			}
			else
			{
				writer.WriteInt32((int)MochaAttributeType.Unknown);
				writer.WriteObject(value);
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
