//
//  MemoryOms.cs
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
using System.Diagnostics.Contracts;

namespace Mocha.Core.OmsImplementations
{
	public class MemoryOms : Oms
	{
		private class MemoryOmsTenant
		{
			public string Name { get; }
			public TenantHandle Handle { get; }

			public List<InstanceHandle> handles { get; } = new List<InstanceHandle>();

			public Dictionary<InstanceKey, InstanceHandle> _instsByKey { get; } = new Dictionary<InstanceKey, InstanceHandle>();
			public Dictionary<Guid, InstanceHandle> _instsByGuid { get; } = new Dictionary<Guid, InstanceHandle>();

			public Dictionary<InstanceHandle, InstanceKey> _keysByInst { get; } = new Dictionary<InstanceHandle, InstanceKey>();
			public Dictionary<InstanceHandle, Guid> _guidsByInst { get; } = new Dictionary<InstanceHandle, Guid>();

			public Dictionary<InstanceHandle, Dictionary<InstanceHandle, List<RelationshipValue>>> _rels = new Dictionary<InstanceHandle, Dictionary<InstanceHandle, List<RelationshipValue>>>();
			public Dictionary<InstanceHandle, Dictionary<InstanceHandle, List<AttributeValue>>> _attrs = new Dictionary<InstanceHandle, Dictionary<InstanceHandle, List<AttributeValue>>>();

			public MemoryOmsTenant(TenantHandle handle, string name)
			{
				Handle = handle;
				Name = name;
			}
		}

		private Dictionary<string, MemoryOmsTenant> _tenantsByName = new Dictionary<string, MemoryOmsTenant>();
		private Dictionary<TenantHandle, MemoryOmsTenant> _tenantsByHandle = new Dictionary<TenantHandle, MemoryOmsTenant>();

		protected override TenantHandle CreateTenantInternal(string name)
		{
			TenantHandle handle = TenantHandle.Create();
			MemoryOmsTenant tenant = new MemoryOmsTenant(handle, name);
			_tenantsByName[name] = tenant;
			_tenantsByHandle[handle] = tenant;
			return handle;
		}

		protected override TenantHandle GetTenantInternal(string name)
		{
			if (_tenantsByName.ContainsKey(name))
				return _tenantsByName[name].Handle;
			return TenantHandle.Empty;
		}
		protected override string GetTenantNameInternal(TenantHandle handle)
		{
			if (_tenantsByHandle.ContainsKey(handle))
			{
				return _tenantsByHandle[handle].Name;
			}
			return null;
		}

		private MemoryOmsTenant GetDefaultTenant()
		{
			return _tenantsByHandle[DefaultTenant];
		}

		protected override void SetInstanceKeyInternal(InstanceHandle handle, InstanceKey key)
		{
			GetDefaultTenant()._instsByKey[key] = handle;
			GetDefaultTenant()._keysByInst[handle] = key;
		}
		protected override void SetInstanceIDInternal(InstanceHandle handle, Guid globalIdentifier)
		{
			GetDefaultTenant()._instsByGuid[globalIdentifier] = handle;
			GetDefaultTenant()._guidsByInst[handle] = globalIdentifier;
		}

		protected override int CountInstancesInternal(InstanceKey classInstanceKey)
		{
			InstanceHandle ihClass = GetInstance(classInstanceKey);

			InstanceHandle[] insts = GetRelatedInstances(GetInstance(classInstanceKey), GetInstance(KnownRelationshipGuids.Class__has__Instance));
			return insts.Length;
		}

		protected override void RegisterInstanceInternal(InstanceHandle handle)
		{
			GetDefaultTenant().handles.Add(handle);
		}

		/*
		protected override InstanceKey CreateInstanceInternal(Guid instanceGuid, Guid parentClassGuid)
		{
			InstanceKey parentClassKey = GetInstanceKey(GetInstance(parentClassGuid));
			if (parentClassKey.IsEmpty && parentClassGuid == KnownInstanceGuids.Classes.Class)
			{
				parentClassKey = new InstanceKey(1, 1);
				_instsByGuid[KnownInstanceGuids.Classes.Class] = parentClassKey;
			}

			InstanceKey key = GetNextInstanceKey(parentClassKey);

			_insts[key] = instanceGuid;
			_instsByGuid[instanceGuid] = key;

			if (!_instsByParentClassKey.ContainsKey(parentClassKey))
			{
				_instsByParentClassKey[parentClassKey] = new List<InstanceKey>();
			}
			_instsByParentClassKey[parentClassKey].Add(key);

			return key;
		}
		*/

		protected override InstanceHandle GetInstanceInternal(Guid id)
		{
			if (GetDefaultTenant()._instsByGuid.ContainsKey(id))
				return GetDefaultTenant()._instsByGuid[id];
			return InstanceHandle.Empty;
		}
		protected override InstanceHandle GetInstanceInternal(InstanceKey key)
		{
			if (GetDefaultTenant()._instsByKey.ContainsKey(key))
				return GetDefaultTenant()._instsByKey[key];
			return InstanceHandle.Empty;
		}

		protected override InstanceHandle[] GetInstancesInternal()
		{
			return GetDefaultTenant().handles.ToArray();
		}

		protected override bool GetAttributeValueInternal(InstanceHandle instanceId, InstanceHandle attributeId, DateTime effectiveDate, object defaultValue, ref object value)
		{
			if (!GetDefaultTenant()._attrs.ContainsKey(instanceId))
			{
				GetDefaultTenant()._attrs[instanceId] = new Dictionary<InstanceHandle, List<AttributeValue>>();
			}
			if (GetDefaultTenant()._attrs[instanceId].ContainsKey(attributeId))
			{
				List<AttributeValue> list = GetDefaultTenant()._attrs[instanceId][attributeId];
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].EffectiveDate <= effectiveDate)
					{
						value = list[i].Value;
						return true;
					}
				}
			}
			value = defaultValue;
			return false;
		}
		protected override void SetAttributeValueInternal(InstanceHandle sourceInstance, InstanceHandle attributeInstance, object value, DateTime effectiveDate)
		{
			if (!GetDefaultTenant()._attrs.ContainsKey(sourceInstance))
			{
				GetDefaultTenant()._attrs[sourceInstance] = new Dictionary<InstanceHandle, List<AttributeValue>>();
			}
			if (!GetDefaultTenant()._attrs[sourceInstance].ContainsKey(attributeInstance))
			{
				GetDefaultTenant()._attrs[sourceInstance][attributeInstance] = new List<AttributeValue>();
			}

			GetDefaultTenant()._attrs[sourceInstance][attributeInstance].Add(new AttributeValue(sourceInstance, attributeInstance, effectiveDate, value));
		}

		protected override void CreateRelationshipInternal(InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle[] targetInstances, DateTime effectiveDate)
		{
			if (!GetDefaultTenant()._rels.ContainsKey(sourceInstance))
			{
				GetDefaultTenant()._rels[sourceInstance] = new Dictionary<InstanceHandle, List<RelationshipValue>>();
			}
			if (!GetDefaultTenant()._rels[sourceInstance].ContainsKey(relationshipInstance))
			{
				GetDefaultTenant()._rels[sourceInstance][relationshipInstance] = new List<RelationshipValue>();
			}

			// FIXME: determine (by the Singular attribute on relationshipInstance)  if this is a Singular relationship, and throw exception if there is already a relationship assigned
			RelationshipValue relval = new RelationshipValue(sourceInstance, relationshipInstance, targetInstances, effectiveDate);
			if (!GetDefaultTenant()._rels[sourceInstance][relationshipInstance].Contains(relval))
			{
				GetDefaultTenant()._rels[sourceInstance][relationshipInstance].Add(relval);
			}
		}

		protected override InstanceHandle[] GetRelatedInstancesInternal(InstanceHandle sourceInstanceKey, InstanceHandle relationshipInstanceKey)
		{
			if (GetDefaultTenant()._rels.ContainsKey(sourceInstanceKey))
			{
				if (GetDefaultTenant()._rels[sourceInstanceKey].ContainsKey(relationshipInstanceKey))
				{
					List<RelationshipValue> list = GetDefaultTenant()._rels[sourceInstanceKey][relationshipInstanceKey];
					List<InstanceHandle> list2 = new List<InstanceHandle>();
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].EffectiveDate <= DateTime.Now)
						{
							list2.AddRange(list[i].TargetInstances);
						}
					}
					return list2.ToArray();
				}
			}
			return null;
		}

		protected override InstanceKey GetInstanceKeyInternal(InstanceHandle handle)
		{
			return GetDefaultTenant()._keysByInst[handle];
		}

		protected override Guid GetInstanceIDInternal(InstanceHandle handle)
		{
			Contract.Requires(!handle.IsEmpty);
			return GetDefaultTenant()._guidsByInst[handle];
		}
	}
}
