using System;
using System.Collections.Generic;
using Mocha.Core;
using Mocha.Storage.Local.Internal;
using UniversalEditor;
using UniversalEditor.Accessors;
using UniversalEditor.ObjectModels.PropertyList;
using UniversalEditor.Plugins.Mocha.DataFormats.MochaBinary;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;

namespace Mocha.Storage.Local
{
	public class LocalStorageProvider : StorageProvider
	{
		public string BasePath { get; set; }
		public LocalStorageProvider()
		{
			BasePath = FindBasePath();
		}
		public LocalStorageProvider(string basePath)
		{
			BasePath = basePath;
		}

		private string FindBasePath()
		{
			// first check our .config file for a basepath
			string basePathConfig = System.Configuration.ConfigurationManager.AppSettings["BasePath"];
			if (basePathConfig != null)
			{
				return basePathConfig;
			}

			System.Reflection.Assembly entryAsm = System.Reflection.Assembly.GetEntryAssembly();
			if (entryAsm == null)
			{
				entryAsm = System.Reflection.Assembly.GetCallingAssembly();
			}
			if (entryAsm == null)
			{
				entryAsm = System.Reflection.Assembly.GetExecutingAssembly();
			}
			if (entryAsm == null)
			{
				Console.WriteLine("Error: no base path specified");
			}
			return System.IO.Path.GetDirectoryName(entryAsm.Location);
		}

		private string dbg_CurrentFileName = null;

		private static string MakeRelativePath(string absolutePath, string basePath)
		{
			string retval = String.Empty;
			if (absolutePath.StartsWith(basePath))
			{
				retval = absolutePath.Substring(basePath.Length);
			}
			if (retval.StartsWith("/")) retval = retval.Substring(1);
			return retval;
		}

		protected override Instance GetInstanceInternal(Guid id)
		{
			if (Tenants.ContainsKey(DefaultTenantName))
			{
				Instance inst = Tenants[DefaultTenantName].Instances[id];
				if (inst == null)
				{
					foreach (Guid lib in TenantLibraryReferences[DefaultTenantName])
					{
						inst = Libraries[lib].Instances[id];
						if (inst != null)
							return inst;
					}
				}
				return inst;
			}
			return null;
		}

		private Dictionary<string, LocalStorageTenant> Tenants = new Dictionary<string, LocalStorageTenant>();
		private Dictionary<string, List<Guid>> TenantLibraryReferences = new Dictionary<string, List<Guid>>();

		private Dictionary<Guid, LocalStorageTenant> Libraries = new Dictionary<Guid, LocalStorageTenant>();

		private MochaBinaryDataFormat mcldf = new MochaBinaryDataFormat();
		private MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();

		protected override void InitializeInternal()
		{
			string[] fileNames = System.IO.Directory.GetFiles(BasePath, "*.mcl", System.IO.SearchOption.AllDirectories);

			List<string> list = new List<string>(fileNames);
			// SORTING IS REQUIRED !!! DO NOT REMOVE
			list.Sort();

			for (int i = 0; i < fileNames.Length; i++)
			{
				MochaClassLibraryObjectModel mcl1 = new MochaClassLibraryObjectModel();
				Document.Load(mcl1, mcldf, new FileAccessor(fileNames[i], false, false, true));
				mcl1.CopyTo(mcl);
			}

			for (int i = 0; i < mcl.Libraries.Count; i++)
			{
				Libraries[mcl.Libraries[i].ID] = new LocalStorageTenant();

				for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
				{
					Libraries[mcl.Libraries[i].ID].Instances[mcl.Libraries[i].Instances[j].ID] = new Instance(mcl.Libraries[i].Instances[j].ID, mcl.Libraries[i].Instances[j].Index);
				}
				for (int j = 0; j < mcl.Libraries[i].Instances.Count; j++)
				{
					for (int k = 0; k < mcl.Libraries[i].Instances[j].AttributeValues.Count; k++)
					{
						Libraries[mcl.Libraries[i].ID].SetAttributeValue(mcl.Libraries[i].Instances[j].ID, mcl.Libraries[i].Instances[j].AttributeValues[k].AttributeInstanceID, mcl.Libraries[i].Instances[j].AttributeValues[k].Value);
					}
				}
				for (int j = 0; j < mcl.Libraries[i].Relationships.Count; j++)
				{
					Guid[] ids = new Guid[mcl.Libraries[i].Relationships[j].DestinationInstanceIDs.Count];
					for (int k = 0; k < mcl.Libraries[i].Relationships[j].DestinationInstanceIDs.Count; k++)
					{
						ids[k] = mcl.Libraries[i].Relationships[j].DestinationInstanceIDs[k];
					}

					Libraries[mcl.Libraries[i].ID].AddRelationshipTargetInstances(mcl.Libraries[i].Relationships[j].SourceInstanceID, mcl.Libraries[i].Relationships[j].RelationshipInstanceID, ids);
				}
				Libraries[mcl.Libraries[i].ID].Relationships.ApplySiblingRelationships();
			}
			for (int i = 0; i < mcl.Tenants.Count; i++)
			{
				if (!Tenants.ContainsKey(mcl.Tenants[i].Name))
				{
					CreateTenant(mcl.Tenants[i].Name, mcl.Tenants[i].ID);
				}

				TenantLibraryReferences[mcl.Tenants[i].Name] = new List<Guid>();
				for (int j = 0; j < mcl.Tenants[i].LibraryReferences.Count; j++)
				{
					TenantLibraryReferences[mcl.Tenants[i].Name].Add(mcl.Tenants[i].LibraryReferences[j]);
				}
				for (int j = 0; j < mcl.Tenants[i].Instances.Count; j++)
				{
					Tenants[mcl.Tenants[i].Name].Instances[mcl.Tenants[i].Instances[j].ID] = new Instance(mcl.Tenants[i].Instances[j].ID, mcl.Tenants[i].Instances[j].Index);
				}
				for (int j = 0; j < mcl.Tenants[i].Instances.Count; j++)
				{
					for (int k = 0; k < mcl.Tenants[i].Instances[j].AttributeValues.Count; k++)
					{
						Tenants[mcl.Tenants[i].Name].SetAttributeValue(mcl.Tenants[i].Instances[j].ID, mcl.Tenants[i].Instances[j].AttributeValues[k].AttributeInstanceID, mcl.Tenants[i].Instances[j].AttributeValues[k].Value);
					}
				}
				for (int j = 0; j < mcl.Tenants[i].Relationships.Count; j++)
				{
					Guid[] ids = new Guid[mcl.Tenants[i].Relationships[j].DestinationInstanceIDs.Count];
					for (int k = 0; k < mcl.Tenants[i].Relationships[j].DestinationInstanceIDs.Count; k++)
					{
						ids[k] = mcl.Tenants[i].Relationships[j].DestinationInstanceIDs[k];
					}
					Tenants[mcl.Tenants[i].Name].AddRelationshipTargetInstances(mcl.Tenants[i].Relationships[j].SourceInstanceID, mcl.Tenants[i].Relationships[j].RelationshipInstanceID, ids);
				}
				Tenants[mcl.Tenants[i].Name].Relationships.ApplySiblingRelationships();
			}
		}

		protected override void WriteInstanceInternal(Instance instance)
		{
		}

		protected override Tenant CreateTenantInternal(string name, Guid id)
		{
			Tenant tenant = new Tenant(name, id);
			Tenants.Add(name, new LocalStorageTenant());
			return tenant;
		}

		protected override void SetAttributeValueInternal(Guid instanceId, Guid attributeId, object value, DateTime effectiveDate, Instance userInstance)
		{
			AttributeKey key = new AttributeKey(instanceId, attributeId);
			if (Tenants[DefaultTenantName].Attributes[key] == null)
			{
				Tenants[DefaultTenantName].Attributes[key] = new List<AttributeValue>();
			}
			Tenants[DefaultTenantName].Attributes[key].Add(new AttributeValue(value, effectiveDate, userInstance == null ? Guid.Empty : userInstance.GlobalIdentifier));
		}
		protected override object GetAttributeValueInternal(Instance instance, Guid attributeGuid, DateTime effectiveDate, object defaultValue)
		{
			AttributeKey attkey = new AttributeKey(instance.GlobalIdentifier, attributeGuid);
			List<AttributeValue> list = Tenants[DefaultTenantName].Attributes[attkey];
			if (list == null)
			{
				foreach (Guid lib in TenantLibraryReferences[DefaultTenantName])
				{
					list = Libraries[lib].Attributes[attkey];
					if (list != null)
						break;
				}
			}

			if (list == null)
				return defaultValue;

			list.Sort(new Comparison<AttributeValue>((x, y) => x.EffectiveDateTime.CompareTo(y.EffectiveDateTime)));
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].EffectiveDateTime > effectiveDate)
					continue;

				return list[i].Value;
			}
			return defaultValue;
		}

		protected override Instance CreateInstanceInternal(Guid id)
		{
			Instance inst = new Instance(id);
			Tenants[DefaultTenantName].Instances[id] = inst;
			// Instances[inst.GetInstanceKey()] = inst;
			return inst;
		}

		protected override Instance[] GetInstancesInternal()
		{
			if (Tenants.ContainsKey(DefaultTenantName))
			{
				return Tenants[DefaultTenantName].Instances.ToArray();
			}
			return null;
		}

		protected override Relationship CreateRelationshipInternal(Guid sourceInstanceId, Guid relationshipInstanceId)
		{
			Instance sourceInstance = GetInstance(sourceInstanceId);
			Instance relationshipInstance = GetInstance(relationshipInstanceId);
			return new Relationship(sourceInstance, relationshipInstance);
		}

		protected override void AddRelationshipTargetInstancesInternal(Guid sourceInstanceId, Guid relationshipInstanceId, Guid[] targetInstanceIds)
		{
			RelationshipKey key = new RelationshipKey(sourceInstanceId, relationshipInstanceId);
			RelationshipValue rela = Tenants[DefaultTenantName].Relationships[key];
			if (rela == null)
			{
				rela = new RelationshipValue(targetInstanceIds);
				Tenants[DefaultTenantName].Relationships[key] = rela;
			}
			else
			{
				for (int i = 0; i < targetInstanceIds.Length; i++)
				{
					rela.TargetInstanceIDs.Add(targetInstanceIds[i]);
				}
			}
		}

		protected override Guid[] GetRelationshipTargetInstancesInternal(Guid sourceInstanceId, Guid relationshipInstanceId)
		{
			List<Guid> list = new List<Guid>();
			RelationshipValue rels = Tenants[DefaultTenantName].Relationships[new RelationshipKey(sourceInstanceId, relationshipInstanceId)];
			bool found = false;
			if (rels != null)
			{
				list.AddRange(rels.TargetInstanceIDs);
				found = true;
			}

			foreach (Guid lib in TenantLibraryReferences[DefaultTenantName])
			{
				rels = Libraries[lib].Relationships[new RelationshipKey(sourceInstanceId, relationshipInstanceId)];
				if (rels != null)
				{
					found = true;
					list.AddRange(rels.TargetInstanceIDs);
					continue;
				}
			}

			if (!found)
			{
				return null;
			}

			return list.ToArray();
		}

		protected override Relationship GetRelationshipInternal(Guid sourceInstanceId, Guid relationshipInstanceId)
		{
			Instance sourceInstance = GetInstance(sourceInstanceId);
			Instance relationshipInstance = GetInstance(relationshipInstanceId);
			return new Relationship(sourceInstance, relationshipInstance);
		}
	}
}
