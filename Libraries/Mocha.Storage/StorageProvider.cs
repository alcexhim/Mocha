using System;
using Mocha.Core;

namespace Mocha.Storage
{
	public abstract class StorageProvider
	{
		public StorageProvider()
		{
		}

		protected abstract void InitializeInternal();
		public void Initialize()
		{
			InitializeInternal();
		}

		public string DefaultTenantName { get; set; }

		private System.Collections.Generic.Dictionary<Guid, Tenant> _tenantsByID = new System.Collections.Generic.Dictionary<Guid, Tenant>();
		private System.Collections.Generic.Dictionary<string, Tenant> _tenantsByName = new System.Collections.Generic.Dictionary<string, Tenant>();
		private void RegisterTenant(Tenant tenant, Guid id)
		{
			_tenantsByID[id] = tenant;
			_tenantsByName[tenant.Name] = tenant;
		}

		protected abstract Tenant CreateTenantInternal(string name, Guid id);
		public Tenant CreateTenant(string name, Guid id)
		{
			Tenant tenant = CreateTenantInternal(name, id);
			RegisterTenant(tenant, id);
			return tenant;
		}

		protected abstract Instance[] GetInstancesInternal();

		/// <summary>
		/// Gets all <see cref="Instance" />s on this <see cref="StorageProvider" />.
		/// </summary>
		/// <returns>The instances.</returns>
		public Instance[] GetInstances()
		{
			return GetInstancesInternal();
		}

		public Tenant GetTenantByID(Guid id)
		{
			return _tenantsByID[id];
		}
		public Tenant GetTenantByName(string name)
		{
			return _tenantsByName[name];
		}

		protected abstract void WriteInstanceInternal(Instance instance);
		public void WriteInstance(Instance instance)
		{
			WriteInstanceInternal(instance);
		}

		protected abstract Instance GetInstanceInternal(Guid id);
		public Instance GetInstance(Guid id) { return GetInstanceInternal(id); }

		protected abstract void SetAttributeValueInternal(Guid instanceId, Guid attributeId, object value, DateTime effectiveDate, Instance userInstance);
		public void SetAttributeValue(Guid instanceId, Guid attributeId, object value, DateTime effectiveDate, Instance userInstance = null)
		{
			SetAttributeValueInternal(instanceId, attributeId, value, effectiveDate, userInstance);
		}
		public void SetAttributeValue(Guid instanceId, Guid attributeId, object value, Instance userInstance = null)
		{
			SetAttributeValue(instanceId, attributeId, value, DateTime.Now, userInstance);
		}

		protected abstract object GetAttributeValueInternal(Instance instance, Guid attributeGuid, DateTime effectiveDate, object defaultValue);
		public object GetAttributeValue(Instance instance, Guid attributeGuid, DateTime effectiveDate, object defaultValue)
		{
			return GetAttributeValueInternal(instance, attributeGuid, effectiveDate, defaultValue);
		}

		protected abstract Instance CreateInstanceInternal(Guid id);
		public Instance CreateInstance()
		{
			return CreateInstanceInternal(Guid.NewGuid());
		}
		public Instance CreateInstance(Instance parentClassInstance, Guid id)
		{
			Instance inst = CreateInstanceInternal(id);
			SetParentClass(inst, parentClassInstance);
			return inst;
		}
		public Instance CreateInstance(Instance parentClassInstance)
		{
			Guid id = Guid.NewGuid();
			return CreateInstance(parentClassInstance, id);
		}

		private void SetParentClass(Instance sourceInstance, Instance parentClassInstance)
		{
			AddRelationshipTargetInstance(sourceInstance.GlobalIdentifier, KnownRelationshipGuids.Instance__for__Class, parentClassInstance.GlobalIdentifier);
			AddRelationshipTargetInstance(parentClassInstance.GlobalIdentifier, KnownRelationshipGuids.Class__has__Instance, sourceInstance.GlobalIdentifier);
		}

		protected abstract Relationship GetRelationshipInternal(Guid sourceInstanceId, Guid relationshipInstanceId);
		public Relationship GetRelationship(Instance sourceInstance, Instance instRelationship)
		{
			return GetRelationshipInternal(sourceInstance.GlobalIdentifier, instRelationship.GlobalIdentifier);
		}

		protected abstract Relationship CreateRelationshipInternal(Guid sourceInstanceId, Guid relationshipInstanceId);
		public Relationship CreateRelationship(Guid sourceInstanceId, Guid relationshipInstanceId)
		{
			return CreateRelationshipInternal(sourceInstanceId, relationshipInstanceId);
		}


		protected abstract void AddRelationshipTargetInstancesInternal(Guid sourceInstanceId, Guid relationshipInstanceId, Guid[] targetInstanceIds);
		public void AddRelationshipTargetInstances(Guid sourceInstanceId, Guid relationshipInstanceId, Guid[] targetInstanceIds)
		{
			AddRelationshipTargetInstancesInternal(sourceInstanceId, relationshipInstanceId, targetInstanceIds);
		}
		public void AddRelationshipTargetInstance(Instance sourceInstance, Instance relationshipInstance, Instance targetInstance)
		{
			AddRelationshipTargetInstances(sourceInstance, relationshipInstance, new Instance[] { targetInstance });
		}
		public void AddRelationshipTargetInstances(Instance sourceInstance, Instance relationshipInstance, Instance[] targetInstances)
		{
			Guid[] targetInstanceIDs = new Guid[targetInstances.Length];
			for (int i = 0; i < targetInstances.Length; i++)
			{
				targetInstanceIDs[i] = targetInstances[i].GlobalIdentifier;
			}
			AddRelationshipTargetInstances(sourceInstance.GlobalIdentifier, relationshipInstance.GlobalIdentifier, targetInstanceIDs);
		}
		public void AddRelationshipTargetInstance(Guid sourceInstanceId, Guid relationshipInstanceId, Guid targetInstanceId)
		{
			AddRelationshipTargetInstances(sourceInstanceId, relationshipInstanceId, new Guid[] { targetInstanceId });
		}

		protected abstract Guid[] GetRelationshipTargetInstancesInternal(Guid sourceInstanceId, Guid relationshipInstanceId);
		public Guid[] GetRelationshipTargetInstances(Guid sourceInstanceId, Guid relationshipInstanceId)
		{
			return GetRelationshipTargetInstancesInternal(sourceInstanceId, relationshipInstanceId);
		}
		public Instance[] GetRelationshipTargetInstances(Instance sourceInstance, Instance relationshipInstance)
		{
			if (sourceInstance == null)
			{
				Console.Error.WriteLine("empty Source Instance for Get Relationship Target Instances");
				return new Instance[0];
				// throw new OmsException("empty Source Instance for Get Relationship Target Instances");
			}
			if (relationshipInstance == null)
				throw new OmsException("empty Relationship Instance for Get Relationship Target Instances");

			Guid[] ids = GetRelationshipTargetInstancesInternal(sourceInstance.GlobalIdentifier, relationshipInstance.GlobalIdentifier);
			if (ids == null)
				return null;

			Instance[] insts = new Instance[ids.Length];
			for (int i = 0; i < ids.Length; i++)
			{
				insts[i] = GetInstance(ids[i]);
			}
			return insts;
		}
	}
}
