//
//  LocalStorageTenant.cs
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

namespace Mocha.Storage.Local.Internal
{
	internal class LocalStorageTenant
	{
		public Guid ID { get; set; }
		public string Name { get; set; }

		public InstanceCollection Instances = new InstanceCollection();
		public AttributeCollection Attributes = new AttributeCollection();
		public RelationshipCollection Relationships = new RelationshipCollection();

		public LocalStorageTenant()
		{
		}

		public void SetAttributeValue(Guid instanceId, Guid attributeInstanceID, object value, DateTime effectiveDate = default(DateTime), Guid userInstanceID = default(Guid))
		{
			if (Attributes[new AttributeKey(instanceId, attributeInstanceID)] == null)
			{
				Attributes[new AttributeKey(instanceId, attributeInstanceID)] = new System.Collections.Generic.List<AttributeValue>();
			}
			Attributes[new AttributeKey(instanceId, attributeInstanceID)].Add(new AttributeValue(value, effectiveDate, userInstanceID));
		}

		public void AddRelationshipTargetInstances(Guid sourceInstanceID, Guid relationshipInstanceID, Guid[] ids)
		{
			for (int i = 0; i < ids.Length; i++)
			{
				if (Relationships[new Core.RelationshipKey(sourceInstanceID, relationshipInstanceID)] == null)
				{
					Relationships[new Core.RelationshipKey(sourceInstanceID, relationshipInstanceID)] = new RelationshipValue();
				}
				Relationships[new Core.RelationshipKey(sourceInstanceID, relationshipInstanceID)].TargetInstanceIDs.Add(ids[i]);
			}
		}
	}
}
