//
//  OmsExtensions.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2021 Mike Becker's Software
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
using MBS.Framework;
using Mocha.Core;
using Mocha.Core.TransactionOperations;
using UniversalEditor.Accessors;

using UniversalEditor.Plugins.Mocha.DataFormats.MochaBinary;
using UniversalEditor.Plugins.Mocha.DataFormats.MochaTenantDefinition;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot.TransactionOperations;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaTenantDefinition;

namespace UniversalEditor.Plugins.Mocha
{
	public static class OmsExtensions
	{
		private static DataFormat mcldf = new MochaBinaryDataFormat();
		private static DataFormat tenantdf = new MochaTenantDefinitionDataFormat();

		public static void Load(this Oms oms, string path)
		{
			if (!System.IO.Directory.Exists(path))
			{
				Console.WriteLine("[mocha info]: snapshot dir nonexistent, skipping rehydration");
				return;
			}

			string[] files = System.IO.Directory.GetFiles(path, "*.mcx", System.IO.SearchOption.AllDirectories);
			foreach (string file in files)
			{
				oms.LoadMCXFile(file);
			}
		}
		public static void LoadFile(this Oms oms, string file)
		{
			MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();
			Document.Load(mcl, mcldf, new FileAccessor(file, false, false, true));

			oms.LoadObjectModel(mcl);
		}
		public static void LoadMCXFile(this Oms oms, string file)
		{
			MochaSnapshotObjectModel mcx = new MochaSnapshotObjectModel();
			Document.Load(mcx, mcldf, new FileAccessor(file, false, false, true));

			oms.LoadMCX(mcx);
		}
		public static void LoadStream(this Oms oms, System.IO.Stream stream)
		{
			MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();
			Document.Load(mcl, mcldf, new StreamAccessor(stream));

			oms.LoadObjectModel(mcl);
		}

		private static void LoadObjectModel(this Oms oms, MochaClassLibraryObjectModel mcl)
		{
			foreach (MochaTenant tenant in mcl.Tenants)
			{
				oms.DefaultTenant = oms.GetTenant(tenant.Name);
				if (oms.DefaultTenant.IsEmpty)
					throw new InvalidOperationException("attempted to load snapshot for nonexistent tenant");

				oms.BeginTransaction();
				for (int j = 0; j < tenant.Instances.Count; j++)
				{
					oms.CreateInstance(tenant.Instances[j].ID, Guid.Empty);
				}
				oms.CommitTransaction();
			}
		}
		private static void LoadMCX(this Oms oms, MochaSnapshotObjectModel mcx)
		{
			foreach (MochaSnapshotTransaction t in mcx.Transactions)
			{
				oms.DefaultTenant = oms.GetTenant(t.TenantName);
				if (oms.DefaultTenant.IsEmpty)
					throw new InvalidOperationException("attempted to load snapshot for nonexistent tenant");

				oms.BeginTransaction();
				for (int j = 0; j < t.Operations.Count; j++)
				{
					oms.LoadMCXOperation(t.Operations[j]);
				}
				oms.CommitTransaction();
			}
		}

		private static void LoadMCXOperation(this Oms oms, MochaSnapshotTransactionOperation op)
		{
			if (op is MochaSnapshotAssignAttributeTransactionOperation aa)
			{
				oms.SetAttributeValue(oms.GetInstance(aa.SourceInstanceID), oms.GetInstance(aa.AttributeInstanceID), aa.EffectiveDate, aa.Value);
			}
			else if (op is MochaSnapshotAssociateRelationshipTransactionOperation ar)
			{
				List<InstanceHandle> targetInstances = new List<InstanceHandle>();
				foreach (Guid id in ar.TargetInstanceIDs)
				{
					targetInstances.Add(oms.GetInstance(id));
				}
				oms.CreateRelationship(oms.GetInstance(ar.SourceInstanceID), oms.GetInstance(ar.RelationshipInstanceID), targetInstances.ToArray(), ar.EffectiveDate);
			}
			else if (op is MochaSnapshotCreateInstanceTransactionOperation ci)
			{
				oms.CreateInstance(ci.GlobalIdentifier, ci.ClassGlobalIdentifier);
			}
		}

		/// <summary>
		/// Persists all pending transactions in <see cref="Oms.PendingTransactions" /> to the backing store.
		/// </summary>
		/// <param name="oms">Oms.</param>
		/// <param name="path">Path.</param>
		public static void Save(this Oms oms, string path)
		{
			string filename = System.IO.Path.Combine(new string[] { path, String.Format("{0}.mcx", DateTime.Now.ToString("s").Replace(':', '-')) });
			string parentDirectory = System.IO.Path.GetDirectoryName(filename);
			if (!System.IO.Directory.Exists(parentDirectory))
			{
				System.IO.Directory.CreateDirectory(parentDirectory);
			}

			MochaSnapshotObjectModel mcx = new MochaSnapshotObjectModel();
			foreach (Transaction transaction in oms.PendingTransactions)
			{
				string tenantName = oms.GetTenantName(transaction.Tenant);
				MochaSnapshotTransaction snapshotTransaction = new MochaSnapshotTransaction();
				snapshotTransaction.TenantName = tenantName;

				foreach (TransactionOperation op in transaction.Operations)
				{
					if (op is AssignAttributeTransactionOperation aa)
					{
						MochaSnapshotAssignAttributeTransactionOperation ssaa = new MochaSnapshotAssignAttributeTransactionOperation();
						ssaa.SourceInstanceID = oms.GetInstanceID(aa.SourceInstance);
						ssaa.AttributeInstanceID = oms.GetInstanceID(aa.AttributeInstance);
						ssaa.EffectiveDate = aa.EffectiveDate;
						ssaa.Value = aa.Value;
						snapshotTransaction.Operations.Add(ssaa);
					}
					else if (op is AssociateRelationshipTransactionOperation ar)
					{
						MochaSnapshotAssociateRelationshipTransactionOperation ssar = new MochaSnapshotAssociateRelationshipTransactionOperation();
						ssar.SourceInstanceID = oms.GetInstanceID(ar.SourceInstance);
						ssar.RelationshipInstanceID = oms.GetInstanceID(ar.RelationshipInstance);
						ssar.EffectiveDate = ar.EffectiveDate;
						for (int i = 0; i < ar.TargetInstances.Length; i++)
						{
							ssar.TargetInstanceIDs.Add(oms.GetInstanceID(ar.TargetInstances[i]));
						}
						snapshotTransaction.Operations.Add(ssar);
					}
					else if (op is CreateInstanceTransactionOperation ci)
					{
						MochaSnapshotCreateInstanceTransactionOperation ssci = new MochaSnapshotCreateInstanceTransactionOperation();
						ssci.GlobalIdentifier = ci.GlobalIdentifier;
						ssci.ClassGlobalIdentifier = ci.ClassGlobalIdentifier;
						snapshotTransaction.Operations.Add(ssci);
					}
				}
				mcx.Transactions.Add(snapshotTransaction);
			}
			Document.Save(mcx, mcldf, new FileAccessor(filename, true, true));

			oms.PendingTransactions.Clear();
		}

		public static void InitializeTenants(this Oms oms, string path)
		{
			TenantHandle defaultTenant = oms.DefaultTenant;

			string[] files = { path };
			if (System.IO.Directory.Exists(path))
			{
				files = System.IO.Directory.GetFiles(path, "tenant.xml", System.IO.SearchOption.AllDirectories);
			}

			foreach (string file in files)
			{
				MochaTenantDefinitionObjectModel mcl1 = new MochaTenantDefinitionObjectModel();
				Document.Load(mcl1, tenantdf, new FileAccessor(file, false, false, true));

				foreach (Tenant tenant in mcl1.Tenants)
				{
					oms.DefaultTenant = oms.CreateTenant(tenant.Name);

					string[] libraryPathList = new string[tenant.LibraryReferences.Count];
					for (int i = 0;  i < tenant.LibraryReferences.Count; i++)
					{
						string libraryPath = tenant.LibraryReferences[i].Path.ReplaceVariables(new KeyValuePair<string, object>[]
						{
							new KeyValuePair<string, object>("MochaRoot", System.IO.Path.Combine(new string[] { System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "System" } ))
						});
						libraryPathList[i] = libraryPath;
					}
					oms.Initialize(libraryPathList);

					Console.WriteLine("[mocha debug]: initialized tenant `{0}`", tenant.Name);
				}
			}

			oms.DefaultTenant = defaultTenant;
			oms.PendingTransactions.Clear();
		}

		public static void Initialize(this Oms oms, string path)
		{
			string[] files = new string[] { path };
			if (System.IO.Directory.Exists(path))
			{
				files = System.IO.Directory.GetFiles(path, "*.mcl");
			}
			oms.Initialize(files);
		}
		public static void Initialize(this Oms oms, string[] path)
		{
			string[] files = path;

			MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();
			for (int i = 0; i < files.Length; i++)
			{
				MochaClassLibraryObjectModel mcl1 = new MochaClassLibraryObjectModel();
				Document.Load(mcl1, mcldf, new FileAccessor(files[i], false, false, true));
				mcl1.CopyTo(mcl);
			}

			oms.Initialize(mcl);
		}

		public static void Initialize(this Oms oms, Accessor accessor)
		{
			MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();
			Document.Load(mcl, mcldf, accessor);

			oms.Initialize(mcl);
		}

		private static void Initialize(this Oms oms, MochaClassLibraryObjectModel mcl)
		{
			oms.BeginTransaction();

			for (int i = 0; i < mcl.Libraries.Count; i++)
			{
				for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
				{
					oms.CreateInstance(mcl.Libraries[i].Instances[j].ID, Guid.Empty);
				}
			}

			oms.CommitTransaction();

			oms.BeginTransaction();
			for (int i = 0; i < mcl.Libraries.Count; i++)
			{
				for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
				{
					for (int k = 0; k < mcl.Libraries[i].Instances[j].AttributeValues.Count; k++)
					{
						oms.SetAttributeValue(oms.GetInstance(mcl.Libraries[i].Instances[j].ID), oms.GetInstance(mcl.Libraries[i].Instances[j].AttributeValues[k].AttributeInstanceID), mcl.Libraries[i].Instances[j].AttributeValues[k].Value);
					}
				}
				for (int j = 0; j < mcl.Libraries[i].Relationships.Count; j++)
				{
					Guid[] ids = new Guid[mcl.Libraries[i].Relationships[j].DestinationInstanceIDs.Count];
					for (int k = 0; k < mcl.Libraries[i].Relationships[j].DestinationInstanceIDs.Count; k++)
					{
						ids[k] = mcl.Libraries[i].Relationships[j].DestinationInstanceIDs[k];
					}
					oms.CreateRelationship(oms.GetInstance(mcl.Libraries[i].Relationships[j].SourceInstanceID), oms.GetInstance(mcl.Libraries[i].Relationships[j].RelationshipInstanceID), GuidsToInstances(oms, ids));
				}
			}
			oms.CommitTransaction();

			oms.BeginTransaction();


			// create the Class::Instance.has parent Class relationship
			oms.CreateRelationship(oms.GetInstance(KnownInstanceGuids.Classes.Class), oms.GetInstance(KnownRelationshipGuids.Class__has__Instance), oms.GetInstance(KnownInstanceGuids.Classes.Class));
			oms.CreateRelationship(oms.GetInstance(KnownInstanceGuids.Classes.Class), oms.GetInstance(KnownRelationshipGuids.Instance__for__Class), oms.GetInstance(KnownInstanceGuids.Classes.Class));

			oms.CommitTransaction();
		}

		private static InstanceHandle[] GuidsToInstances(Oms oms, Guid[] guids)
		{
			List<InstanceHandle> list = new List<InstanceHandle>();
			foreach (Guid id in guids)
			{
				InstanceHandle ih = oms.GetInstance(id);
				list.Add(ih);
			}
			return list.ToArray();
		}
	}
}
