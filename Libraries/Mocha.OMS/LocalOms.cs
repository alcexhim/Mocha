using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Mocha.Core;
using Mocha.Storage.Local;

namespace Mocha.OMS
{
	public class LocalOms : Oms
	{
		protected override bool IsConnectedInternal()
		{
			return true;
		}

		protected override Instance GetInstanceInternal(Guid globalIdentifier)
		{
			return Environment.StorageProvider.GetInstance(globalIdentifier);
		}

		protected override object GetAttributeValueInternal(Instance instTarget, Guid instAttributeID, object defaultValue, DateTime effectiveDate)
		{
			return Environment.StorageProvider.GetAttributeValue(instTarget, instAttributeID, effectiveDate, defaultValue);
		}

		protected override Instance CreateInstanceInternal()
		{
			Instance inst = Environment.StorageProvider.CreateInstance();
			return inst;
		}
		protected override void AssignRelationshipInternal(Instance sourceInstance, Guid relationshipInstanceId, Guid[] targetInstanceIds, DateTime effectiveDate)
		{
			Environment.StorageProvider.AddRelationshipTargetInstances(sourceInstance.GlobalIdentifier, relationshipInstanceId, targetInstanceIds);
		}
		protected override void SetAttributeValueInternal(Instance sourceInstance, Guid attributeInstanceId, object value, DateTime effectiveDate)
		{
			Environment.StorageProvider.SetAttributeValue(sourceInstance.GlobalIdentifier, attributeInstanceId, value);
		}

		protected override Instance[] GetRelatedInstancesInternal(Instance target, Guid relationshipID, OmsSearchOption searchOption)
		{
			Instance instRelationship = GetInstance(relationshipID);
			if (instRelationship == null)
			{
				Console.Error.WriteLine("invalid relationship '{0}'", relationshipID.ToString("B"));
			}
			Instance[] insts = Environment.StorageProvider.GetRelationshipTargetInstances(target, instRelationship);
			if (insts == null)
			{
				insts = new Instance[0];
			}

			List<Instance> list = new List<Instance>();
			list.AddRange(insts);

			if (((searchOption & OmsSearchOption.SuperclassesAlways) == OmsSearchOption.SuperclassesAlways) || (((searchOption & OmsSearchOption.SuperclassesIfEmpty) == OmsSearchOption.SuperclassesIfEmpty) && insts.Length == 0))
			{
				Instance[] instSuperclasses = GetRelatedInstancesInternal(target, KnownRelationshipGuids.Class__has_super__Class, OmsSearchOption.None);
				for (int i = 0; i < instSuperclasses.Length; i++)
				{
					insts = GetRelatedInstancesInternal(instSuperclasses[i], relationshipID, OmsSearchOption.None);
					list.AddRange(insts);
				}
			}
			return list.ToArray();
		}
	}
}

