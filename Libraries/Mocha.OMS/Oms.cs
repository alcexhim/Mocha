//
//  IOmsProviderBase.cs
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

using Mocha.Core;

namespace Mocha.OMS
{
	public abstract class Oms
	{
		protected abstract bool IsConnectedInternal();
		public bool IsConnected { get { return IsConnectedInternal(); } }
		public OmsEnvironment Environment { get; set; } = null;
		public string TenantName { get { return Environment.StorageProvider.DefaultTenantName; } set { Environment.StorageProvider.DefaultTenantName = value; } }

		public int GetInstanceIndex(Instance instance)
		{
			if (_instanceIndices == null)
			{
				UpdateInstanceIndices();
			}
			if (!_instanceIndices.ContainsKey(TenantName))
			{
				UpdateInstanceIndices();
			}
			if (instance != null)
			{
				if (_instanceIndices[TenantName].ContainsKey(instance.GlobalIdentifier))
				{
					return _instanceIndices[TenantName][instance.GlobalIdentifier];
				}
			}
			return 0;
		}

		private Dictionary<string, Dictionary<Guid, int>> _instanceIndices = null;
		private void UpdateInstanceIndices()
		{
			if (_instanceIndices == null)
			{
				_instanceIndices = new Dictionary<string, Dictionary<Guid, int>>();
			}
			lock (_instanceIndices)
			{
				if (!_instanceIndices.ContainsKey(TenantName))
				{
					_instanceIndices[TenantName] = new Dictionary<Guid, int>();
				}
			}

			Instance[] instClasses = GetInstances(GetInstance(KnownInstanceGuids.Classes.Class));
			for (int i = 0; i < instClasses.Length; i++)
			{
				int index = i + 1;
				decimal? indexV = null; // (this.GetAttributeValue(instClasses[i], KnownAttributeGuids.Numeric.Index) as decimal?);
				if (indexV != null)
				{
					index = (int)indexV.GetValueOrDefault();
				}
				UpdateInstanceIndices(instClasses[i], index);
			}
		}

		private void UpdateInstanceIndices(Instance inst)
		{
			Instance instClass = GetRelatedInstance(inst, KnownRelationshipGuids.Instance__for__Class);
			if (!_instanceIndices[TenantName].ContainsKey(inst.GlobalIdentifier))
			{
				int index = 0;
				Instance[] instInstances = GetRelatedInstances(instClass, KnownRelationshipGuids.Class__has__Instance);
				index = instInstances.Length;

				decimal? indexV = null; // (this.GetAttributeValue(inst, KnownAttributeGuids.Numeric.Index) as decimal?);
				if (indexV != null)
				{
					index = (int)indexV.GetValueOrDefault();
				}

				_instanceIndices[TenantName][inst.GlobalIdentifier] = index;
			}
		}
		private void UpdateInstanceIndices(Instance inst, int index)
		{
			_instanceIndices[TenantName][inst.GlobalIdentifier] = index;

			if (inst.GlobalIdentifier == KnownInstanceGuids.Classes.Class)
				return;
			/*
			Instance[] insts2 = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Class__has_sub__Class);
			for (int j = 0; j < insts2.Length; j++)
			{
				UpdateInstanceIndices(oms, insts2[j], index + j + 1);
			}
			*/
			Instance[] insts2 = GetRelatedInstances(inst, KnownRelationshipGuids.Class__has__Instance);
			for (int j = 0; j < insts2.Length; j++)
			{
				int indexWV = j + 1;
				decimal? indexW = null; // (this.GetAttributeValue(insts2[j], KnownAttributeGuids.Numeric.Index) as decimal?);
				if (indexW != null)
				{
					indexWV = (int)indexW.GetValueOrDefault();
				}

				_instanceIndices[TenantName][insts2[j].GlobalIdentifier] = indexWV;
			}
		}

		public string GetAttachmentUrl(Instance inst, System.Collections.Generic.Dictionary<InstanceKey, byte[]> entropy)
		{
			string accessKey = this.BuildAccessKeyForOmsAttachment(inst, entropy);
			return String.Format("/{0}/attachment/{1}/{2}", this.TenantName, this.GetInstanceKey(inst), accessKey);
		}

		protected abstract Instance CreateInstanceInternal();
		public Instance CreateInstance(Guid classInstanceId)
		{
			Instance inst = CreateInstanceInternal();

			InstanceKey ikClass = this.GetInstanceKey(GetInstance(classInstanceId));
			InstanceKey ikInst = this.GetInstanceKey(inst);

			AssignRelationship(inst, KnownRelationshipGuids.Instance__for__Class, new Guid[] { classInstanceId });
			AssignRelationship(GetInstance(classInstanceId), KnownRelationshipGuids.Class__has__Instance, new Guid[] { inst.GlobalIdentifier });

			UpdateInstanceIndices(inst);

			ikClass = this.GetInstanceKey(GetInstance(classInstanceId));
			ikInst = this.GetInstanceKey(inst);
			return inst;
		}

		protected abstract Instance GetInstanceInternal(Guid globalIdentifier);
		public Instance GetInstance(Guid globalIdentifier)
		{
			return GetInstanceInternal(globalIdentifier);
		}

		public Instance[] GetInstances(Instance parentClassInstance = null, bool searchSubclasses = false)
		{
			List<Instance> list = new List<Instance>();
			if (parentClassInstance == null)
			{
				return null;
				// we are NOT returning every single gddmn instance on the database. that would be HELL
				// return Environment.StorageProvider.GetInstances();
			}

			Instance[] insts = GetRelatedInstances(parentClassInstance, KnownRelationshipGuids.Class__has__Instance, OmsSearchOption.None);
			list.AddRange(insts);

			if (searchSubclasses)
			{
				Instance[] subClasses = GetRelatedInstances(parentClassInstance, KnownRelationshipGuids.Class__has_sub__Class, OmsSearchOption.None);
				for (int i = 0; i < subClasses.Length; i++)
				{
					if (subClasses[i].GlobalIdentifier == parentClassInstance.GlobalIdentifier)
					{
						continue;
					}

					Instance[] insts2 = GetInstances(subClasses[i], true);
					list.AddRange(insts2);
				}
			}

			return list.ToArray();
		}

		protected abstract Instance[] GetRelatedInstancesInternal(Instance target, Guid relationshipID, OmsSearchOption searchOption);
		public Instance[] GetRelatedInstances(Instance target, Guid relationshipID, OmsSearchOption searchOption = OmsSearchOption.None)
		{
			return GetRelatedInstancesInternal(target, relationshipID, searchOption);
		}
		public Instance GetRelatedInstance(Instance target, Guid relationshipID, OmsSearchOption searchOption = OmsSearchOption.None)
		{
			Instance[] rels = GetRelatedInstances(target, relationshipID, searchOption);
			if (rels.Length > 0)
			{
				return rels[0];
			}
			return null;
		}

		protected abstract object GetAttributeValueInternal(Instance instTarget, Guid instAttributeID, object defaultValue, DateTime effectiveDate);
		public object GetAttributeValue(Instance instTarget, Guid instAttributeID)
		{
			return GetAttributeValue(instTarget, instAttributeID, null);
		}
		public object GetAttributeValue(Instance instTarget, Guid instAttributeID, object defaultValue)
		{
			return GetAttributeValue(instTarget, instAttributeID, defaultValue, DateTime.Now);
		}
		public object GetAttributeValue(Instance instTarget, Guid instAttributeID, object defaultValue, DateTime effectiveDate)
		{
			object value = GetAttributeValueInternal(instTarget, instAttributeID, defaultValue, effectiveDate);
			value = this.CastAttributeValue(value, this.GetParentClass(this.GetInstance(instAttributeID)));
			return value;
		}

		protected abstract void AssignRelationshipInternal(Instance sourceInstance, Guid relationshipInstanceId, Guid[] targetInstanceIds, DateTime effectiveDate);
		public void AssignRelationship(Instance sourceInstance, Guid relationshipInstanceId, Guid[] targetInstanceIds, DateTime effectiveDate)
		{
			AssignRelationshipInternal(sourceInstance, relationshipInstanceId, targetInstanceIds, effectiveDate);
		}
		public void AssignRelationship(Instance sourceInstance, Guid relationshipInstanceId, Guid[] targetInstanceIds)
		{
			AssignRelationship(sourceInstance, relationshipInstanceId, targetInstanceIds, DateTime.Now);
		}

		internal Instance GetTranslation(Instance inst, Instance instRelationshipOrTTC)
		{
			throw new NotImplementedException();
		}

		protected abstract void SetAttributeValueInternal(Instance sourceInstance, Guid attributeInstanceId, object value, DateTime effectiveDate);
		public void SetAttributeValue(Instance sourceInstance, Guid attributeInstanceId, object value, DateTime effectiveDate)
		{
			SetAttributeValueInternal(sourceInstance, attributeInstanceId, value, effectiveDate);
		}
		public void SetAttributeValue(Instance sourceInstance, Guid attributeInstanceId, object value)
		{
			SetAttributeValue(sourceInstance, attributeInstanceId, value, DateTime.Now);
		}

		private string const_co = "SSBkb24ndCBrbm93IHdoeSB0aGlzIHBhcmFtZXRlciBpcyBoZXJlLiBJZiB5b3Uga25vdyB3aGF0\nIHRoaXMgaXMgdXNlZCBmb3IsIHBsZWFzZSB0ZWxsIG1lIHNvIEkgY2FuIGZpeCBpdC4gSGVsbG8g\nZnJvbSBYcHJlc3NPIQo=";

		public bool VerifyAccessKeyForOmsAttachment(Instance inst, string accessKey, System.Collections.Generic.Dictionary<InstanceKey, byte[]> entropy)
		{
			if (!entropy.ContainsKey(this.GetInstanceKey(inst)))
			{
				byte[] entropyData = new byte[256];
				random.NextBytes(entropyData);

				entropy[this.GetInstanceKey(inst)] = entropyData;
			}

			string entropy_co = Convert.ToBase64String(entropy[this.GetInstanceKey(inst)]);
			string base64 = MBS.Framework.Conversion.UrlSafeBase64ToBase64(accessKey);
			string ft_co = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(base64));
			string originalFileName = this.GetAttributeValue<string>(inst, KnownAttributeGuids.Text.Name);

			string[] ft_parms = ft_co.Split(new char[] { '?' });
			if (ft_parms.Length == 5 && ft_parms[0] == entropy_co /* && ft_parms[4] == originalFileName */)
				return true;

			return false;
		}

		private Random random = new Random();
		public string BuildAccessKeyForOmsAttachment(Instance inst, System.Collections.Generic.Dictionary<InstanceKey, byte[]> entropy)
		{
			if (!entropy.ContainsKey(this.GetInstanceKey(inst)))
			{
				byte[] entropyData = new byte[256];
				random.NextBytes(entropyData);

				entropy[this.GetInstanceKey(inst)] = entropyData;
			}

			string entropy_co = Convert.ToBase64String(entropy[this.GetInstanceKey(inst)]);
			string guid = inst.GlobalIdentifier.ToString();
			long timestamp = DateTime.Now.Ticks;
			string originalFileName = this.GetAttributeValue<string>(inst, KnownAttributeGuids.Text.Name);
			string ft_co = String.Format("{0}?oms-attachments/{1}?{2}??{3}", entropy_co, guid, timestamp, originalFileName);
			string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(ft_co));
			string safeBase64 = MBS.Framework.Conversion.Base64ToUrlSafeBase64(base64);
			return safeBase64;
		}
	}
}
