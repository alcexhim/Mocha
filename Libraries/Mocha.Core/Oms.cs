//
//  Oms.cs
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
using Mocha.Core.TransactionOperations;

namespace Mocha.Core
{
	/// <summary>
	/// Represents the Mocha Object Management System, through which all database
	/// operations are performed.
	/// </summary>
	public abstract class Oms
	{
		public TenantHandle DefaultTenant { get; set; } = TenantHandle.Empty;

		protected abstract TenantHandle CreateTenantInternal(string name);
		public TenantHandle CreateTenant(string name)
		{
			return CreateTenantInternal(name);
		}
		public TenantHandle[] CreateTenants(string prefix, int count)
		{
			TenantHandle[] handles = new TenantHandle[count];
			for (int i = 1; i <= count; i++)
			{
				handles[i - 1] = CreateTenantInternal(String.Format("{0}{1}", prefix, i));
			}
			return handles;
		}
		protected abstract string GetTenantNameInternal(TenantHandle handle);
		public string GetTenantName(TenantHandle handle)
		{
			return GetTenantNameInternal(handle);
		}

		protected abstract TenantHandle GetTenantInternal(string name);

		public TenantHandle GetTenant(string name)
		{
			return GetTenantInternal(name);
		}

		protected abstract int CountInstancesInternal(InstanceKey classInstanceKey);
		/// <summary>
		/// Returns a count of all instances on this <see cref="Oms" />.
		/// </summary>
		/// <returns>The instances.</returns>
		/// <param name="classInstanceKey">Class instance key.</param>
		public int CountInstances(InstanceKey classInstanceKey)
		{
			return CountInstancesInternal(classInstanceKey);
		}
		public InstanceKey GetNextInstanceKey(InstanceKey parentClassKey)
		{
			return new InstanceKey(parentClassKey.InstanceIndex, CountInstances(parentClassKey) + 1);
		}

		protected abstract void RegisterInstanceInternal(InstanceHandle handle);
		protected void RegisterInstance(InstanceHandle handle)
		{
			RegisterInstanceInternal(handle);
		}

		protected abstract void SetInstanceKeyInternal(InstanceHandle handle, InstanceKey key);
		protected void SetInstanceKey(InstanceHandle handle, InstanceKey key)
		{
			SetInstanceKeyInternal(handle, key);
		}

		protected abstract InstanceKey GetInstanceKeyInternal(InstanceHandle handle);
		public InstanceKey GetInstanceKey(InstanceHandle handle)
		{
			return GetInstanceKeyInternal(handle);
		}
		protected abstract Guid GetInstanceIDInternal(InstanceHandle handle);
		public Guid GetInstanceID(InstanceHandle handle)
		{
			return GetInstanceIDInternal(handle);
		}

		protected abstract void SetInstanceIDInternal(InstanceHandle handle, Guid id);
		protected void SetInstanceID(InstanceHandle handle, Guid id)
		{
			SetInstanceIDInternal(handle, id);
		}

		protected InstanceHandle CreateInstanceInternal()
		{
			InstanceHandle ih = InstanceHandle.Create();
			RegisterInstance(ih);
			return ih;
		}

		// protected abstract InstanceKey CreateInstanceInternal(Guid instanceGuid, Guid parentClassGuid);
		public InstanceHandle CreateInstance(InstanceHandle parentClassInstance)
		{
			InstanceHandle ih = InstanceHandle.Create();
			CurrentTransaction.Operations.Add(new CreateInstanceTransactionOperation(ih, Guid.Empty, GetInstanceID(parentClassInstance)));
			return ih;
		}
		public InstanceHandle CreateInstance(Guid parentClassGuid)
		{
			return CreateInstance(Guid.NewGuid(), parentClassGuid);
		}

		public InstanceHandle CreateInstance(Guid instanceGuid, Guid parentClassGuid)
		{
			InstanceHandle ihNew = InstanceHandle.Create();

			CurrentTransaction.Operations.Add(new CreateInstanceTransactionOperation(ihNew, instanceGuid, parentClassGuid));

			return ihNew;
		}

		protected abstract InstanceHandle[] GetInstancesInternal();
		/// <summary>
		/// Gets the <see cref="InstanceHandle" />s for all instances on this <see cref="Oms" />.
		/// </summary>
		/// <returns>The instances.</returns>
		public InstanceHandle[] GetInstances()
		{
			return GetInstancesInternal();
		}

		protected abstract InstanceHandle GetInstanceInternal(Guid id);
		protected abstract InstanceHandle GetInstanceInternal(InstanceKey key);
		/// <summary>
		/// Gets the <see cref="InstanceHandle" /> for the instance with the given <paramref name="id" />.
		/// </summary>
		/// <returns>The <see cref="InstanceHandle" /> for the instance with the given <paramref name="id" />.</returns>
		/// <param name="id">The global identifier for the instance.</param>
		public InstanceHandle GetInstance(Guid id)
		{
			return GetInstanceInternal(id);
		}
		public InstanceHandle GetInstance(InstanceKey key)
		{
			return GetInstanceInternal(key);
		}

		protected abstract bool GetAttributeValueInternal(InstanceHandle sourceInstance, InstanceHandle attributeInstance, DateTime effectiveDate, object defaultValue, ref object value);

		/// <summary>
		/// Gets the value of the attribute with the given <paramref name="attributeKey" /> on the specified instance as of the current
		/// date and time. If the attribute has not been assigned, returns <paramref name="defaultValue" />.
		/// </summary>
		/// <returns>The attribute value.</returns>
		/// <param name="instanceKey">Instance identifier.</param>
		/// <param name="attributeKey">Attribute identifier.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetAttributeValue<T>(InstanceHandle sourceInstance, InstanceHandle attributeInstance, T defaultValue = default(T))
		{
			return GetAttributeValue(sourceInstance, attributeInstance, DateTime.Now, defaultValue);
		}
		/// <summary>
		/// Gets the value of the attribute with the given <paramref name="attributeKey" /> on the specified instance as of the given
		/// <paramref name="effectiveDate" />. If the attribute has not been assigned, returns <paramref name="defaultValue" />.
		/// </summary>
		/// <returns>The attribute value.</returns>
		/// <param name="instanceKey">Instance identifier.</param>
		/// <param name="attributeKey">Attribute identifier.</param>
		/// <param name="effectiveDate">Effective date.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T GetAttributeValue<T>(InstanceHandle sourceInstance, InstanceHandle attributeInstance, DateTime effectiveDate, T defaultValue = default(T))
		{
			object val = null;
			if (GetAttributeValueInternal(sourceInstance, attributeInstance, effectiveDate, defaultValue, ref val))
			{
				if (val is T)
				{
					return (T)val;
				}
				return defaultValue;
			}
			return defaultValue;
		}

		protected abstract void SetAttributeValueInternal(InstanceHandle sourceInstance, InstanceHandle attributeInstance, object value, DateTime effectiveDate);
		/// <summary>
		/// Sets the value of the attribute with the given <paramref name="attributeInstance" /> on the instance with the given
		/// <paramref name="instanceKey" /> to <paramref name="value" />.
		/// </summary>
		/// <param name="instanceKey">The <see cref="InstanceKey" /> of the target instance on which to set the attribute.</param>
		/// <param name="attributeKey">The <see cref="InstanceKey" /> of the attribute to set.</param>
		/// <param name="value">The value to set.</param>
		/// <typeparam name="T">The <see cref="Type" /> of the value to set.</typeparam>
		/// <remarks>
		/// Although this is referred to "setting" an attribute, in reality a new attribute value entry is added to the collection
		/// with the Effective Date set to <see cref="DateTime.Now" />. Maintaining the database as append-only allows us to go back
		/// at any time and read historical data.
		/// </remarks>
		public void SetAttributeValue<T>(InstanceHandle sourceInstance, InstanceHandle attributeInstance, T value)
		{
			SetAttributeValue(sourceInstance, attributeInstance, DateTime.Now, value);
		}
		public void SetAttributeValue<T>(InstanceHandle sourceInstance, InstanceHandle attributeInstance, DateTime effectiveDate, T value)
		{
			CurrentTransaction.Operations.Add(new AssignAttributeTransactionOperation(sourceInstance, attributeInstance, effectiveDate, value));
		}

		protected abstract void CreateRelationshipInternal(InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle[] targetInstances, DateTime effectiveDate);
		/// <summary>
		/// Associates a one-to-one relationship of instance <paramref name="relationshipInstanceKey" /> from the instance
		/// <paramref name="sourceInstanceKey" /> to the instance <paramref name="targetInstanceKey" />.
		/// </summary>
		/// <param name="sourceInstanceKey">The instance key of the source instance.</param>
		/// <param name="relationshipInstanceKey">The instance key of the relationship.</param>
		/// <param name="targetInstanceKey">The instance key of the target instance.</param>
		public void CreateRelationship(InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle targetInstance)
		{
			CreateRelationship(sourceInstance, relationshipInstance, new InstanceHandle[] { targetInstance });
		}
		/// <summary>
		/// Associates a one-to-one relationship of instance <paramref name="relationshipInstanceKey" /> from the instance
		/// <paramref name="sourceInstanceKey" /> to the instance <paramref name="targetInstanceKey" />, to take effect on
		/// the given <paramref name="effectiveDate" />.
		/// </summary>
		/// <param name="sourceInstanceKey">The instance key of the source instance.</param>
		/// <param name="relationshipInstanceKey">The instance key of the relationship.</param>
		/// <param name="targetInstanceKey">The instance key of the target instance.</param>
		/// <param name="effectiveDate">The date upon which the relationship is to take effect.</param>
		public void CreateRelationship(InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle targetInstance, DateTime effectiveDate)
		{
			CreateRelationship(sourceInstance, relationshipInstance, new InstanceHandle[] { targetInstance }, effectiveDate);
		}
		/// <summary>
		/// Associates a one-to-many relationship of instance <paramref name="relationshipInstanceKey" /> from the instance
		/// <paramref name="sourceInstanceKey" /> to the instances specified in the <paramref name="targetInstanceKeys" /> array.
		/// </summary>
		/// <param name="sourceInstanceKey">The instance key of the source instance.</param>
		/// <param name="relationshipInstanceKey">The instance key of the relationship.</param>
		/// <param name="targetInstanceKeys">The instance keys of the target instances.</param>
		public void CreateRelationship(InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle[] targetInstances)
		{
			CreateRelationship(sourceInstance, relationshipInstance, targetInstances, DateTime.Now);
		}
		/// <summary>
		/// Associates a one-to-many relationship of instance <paramref name="relationshipInstanceKey" /> from the instance
		/// <paramref name="sourceInstanceKey" /> to the instances specified in the <paramref name="targetInstanceKeys" /> array,
		/// to take effect on the given <paramref name="effectiveDate" />.
		/// </summary>
		/// <param name="sourceInstanceKey">The instance key of the source instance.</param>
		/// <param name="relationshipInstanceKey">The instance key of the relationship.</param>
		/// <param name="targetInstanceKeys">The instance keys of the target instances.</param>
		/// <param name="effectiveDate">The date upon which the relationship is to take effect.</param>
		public void CreateRelationship(InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle[] targetInstances, DateTime effectiveDate)
		{
			// FIXME: deferred fixing known bug in design - if we associate sibling relationship with both relationship instances, we WILL
			// end up going into an infinite loop as there is no way to escape bouncing continuously between the two relationships
			// the fix should be to make sure that the requested target instances are not already associated in the relationship before wasting our time
			InstanceHandle[] targetInstances2 = GetRelatedInstances(sourceInstance, relationshipInstance);
			if (targetInstances2 != null)
			{
				// the relationship exists
				foreach (InstanceHandle ih in targetInstances2)
				{
				}
			}

			CurrentTransaction.Operations.Add(new AssociateRelationshipTransactionOperation(sourceInstance, relationshipInstance, targetInstances, effectiveDate));

			InstanceHandle ihSiblingRelationship = GetRelatedInstance(relationshipInstance, GetInstance(KnownRelationshipGuids.Relationship__has_sibling__Relationship));
			if (!ihSiblingRelationship.IsEmpty)
			{
				foreach (InstanceHandle ih in targetInstances)
				{
					CurrentTransaction.Operations.Add(new AssociateRelationshipTransactionOperation(ih, ihSiblingRelationship, new InstanceHandle[] { sourceInstance }, effectiveDate));
				}
			}
		}

		protected abstract InstanceHandle[] GetRelatedInstancesInternal(InstanceHandle sourceInstanceKey, InstanceHandle relationshipInstanceKey);
		public InstanceHandle GetRelatedInstance(InstanceHandle sourceInstance, InstanceHandle relationshipInstance)
		{
			InstanceHandle[] insts = GetRelatedInstances(sourceInstance, relationshipInstance);
			if (insts != null)
			{
				if (insts.Length > 0)
					return insts[insts.Length - 1];
			}
			return InstanceHandle.Empty;
		}
		public InstanceHandle[] GetRelatedInstances(InstanceHandle sourceInstance, InstanceHandle relationshipInstance)
		{
			return GetRelatedInstancesInternal(sourceInstance, relationshipInstance);
		}

		private Transaction CurrentTransaction { get; set; } = null;

		public Transaction.TransactionCollection Transactions { get; } = new Transaction.TransactionCollection();
		public List<Transaction> PendingTransactions { get; } = new List<Transaction>();

		public void BeginTransaction(TenantHandle tenant = default(TenantHandle))
		{
			if (tenant == default(TenantHandle))
			{
				tenant = DefaultTenant;
			}
			if (DefaultTenant == TenantHandle.Empty)
			{
				throw new InvalidOperationException("please specify a tenant on which to begin a transaction");
			}

			if (CurrentTransaction != null)
				throw new InvalidOperationException("please commit or discard the current transaction before beginning a new one");

			CurrentTransaction = new Transaction(DefaultTenant);
		}
		public void CommitTransaction()
		{
			if (CurrentTransaction == null)
			{
				throw new InvalidOperationException("please begin a transaction before committing it");
			}

			Transactions.Add(CurrentTransaction);
			PendingTransactions.Add(CurrentTransaction);

			foreach (TransactionOperation op in CurrentTransaction.Operations)
			{
				if (op is CreateInstanceTransactionOperation)
				{
					CreateInstanceTransactionOperation aa = (CreateInstanceTransactionOperation)op;

					InstanceHandle ihNew = aa.Handle;
					if (aa.GlobalIdentifier != Guid.Empty)
					{
						SetInstanceID(ihNew, aa.GlobalIdentifier);
					}
					else
					{
						SetInstanceID(ihNew, Guid.NewGuid());
					}

					InstanceHandle ihParent = InstanceHandle.Empty;
					if (aa.ClassGlobalIdentifier != Guid.Empty)
					{
						ihParent = GetInstance(aa.ClassGlobalIdentifier);
						if (ihParent == InstanceHandle.Empty)
						{
							ihParent = CreateInstanceInternal();
							SetInstanceID(ihParent, aa.ClassGlobalIdentifier);
						}
					}

					// InstanceKey key = CreateInstanceInternal(instanceGuid, parentClassGuid);
					if (ihParent != InstanceHandle.Empty)
					{
						CreateRelationshipInternal(ihParent, GetInstance(KnownRelationshipGuids.Class__has__Instance), new InstanceHandle[] { ihNew }, DateTime.Now);
						CreateRelationshipInternal(ihNew, GetInstance(KnownRelationshipGuids.Instance__for__Class), new InstanceHandle[] { ihParent }, DateTime.Now);
					}

					RegisterInstanceInternal(aa.Handle);
				}
				else if (op is AssignAttributeTransactionOperation)
				{
					AssignAttributeTransactionOperation aa = (AssignAttributeTransactionOperation)op;
					SetAttributeValueInternal(aa.SourceInstance, aa.AttributeInstance, aa.Value, aa.EffectiveDate);
				}
				else if (op is AssociateRelationshipTransactionOperation)
				{
					AssociateRelationshipTransactionOperation aa = (AssociateRelationshipTransactionOperation)op;
					CreateRelationshipInternal(aa.SourceInstance, aa.RelationshipInstance, aa.TargetInstances, aa.EffectiveDate);
				}
			}
			CurrentTransaction = null;
		}
		public void DiscardTransaction()
		{
			if (CurrentTransaction == null)
			{
				throw new InvalidOperationException("please begin a transaction before discarding it");
			}
			CurrentTransaction = null;
		}
	}
}
