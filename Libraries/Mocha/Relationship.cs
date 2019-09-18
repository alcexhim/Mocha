//
//  MochaRelationship.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2019 
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
using System.Text;
using UniversalEditor.ObjectModels.PropertyList;

namespace Mocha
{
	public class Relationship
	{
		public class RelationshipCollection
			: System.Collections.ObjectModel.Collection<Relationship>
		{
			public Relationship this[Instance sourceInstance, Instance relationshipInstance]
			{
				get
				{
					foreach (Relationship rel in this)
					{
						if (rel.RelationshipInstance == null) continue;

						if (rel.SourceInstance.Equals(sourceInstance) && rel.RelationshipInstance.Equals(relationshipInstance))
							return rel;
					}
					return null;
				}
			}
			public Relationship[] this[Instance sourceInstance]
			{
				get
				{
					List<Relationship> list = new List<Relationship>();
					foreach (Relationship rel in this)
					{
						if (rel.SourceInstance.Equals(sourceInstance))
							list.Add(rel);
					}
					return list.ToArray();
				}
			}
		}

		public Instance SourceInstance { get; set; }
		public Instance RelationshipInstance { get; set; }

		public Instance.InstanceCollection DestinationInstances { get; } = new Instance.InstanceCollection();


		public static Relationship[] FromJSONPropertyList(IPropertyListContainer plom)
		{
			if (plom.Groups.Count > 0)
			{
				// multiple instances ,in groups
				List<Relationship> list = new List<Relationship>();
				foreach (Group g in plom.Groups)
				{
					Relationship inst = FromJSONPropertyList(g) [0];
					list.Add(inst);
				}
				return list.ToArray();
			}
			else
			{
				// single instance
				Property propTargetInst = plom.Properties["TargetInstance"];
				Property propRelationshipInst = plom.Properties["RelationshipInstance"];
				// Property propInverseRelationshipInstID = plom.Properties["InverseRelationshipInstanceID"];

				Group grpTargetInst = (propTargetInst?.Value as Group);
				Group grpRelationshipInst = (propRelationshipInst?.Value as Group);

				if (grpTargetInst != null)
				{
					Relationship rel = new Relationship();
					rel.SourceInstance = Instance.FromJSONPropertyList(grpTargetInst) [0];
					rel.RelationshipInstance = Instance.FromJSONPropertyList(grpRelationshipInst) [0];

					if (plom.Groups["DestinationInstances"] != null)
					{
						foreach (Group grpDestinationInst in plom.Groups["DestinationInstances"].Groups)
						{
							rel.DestinationInstances.Add(Instance.FromJSONPropertyList(grpDestinationInst)[0]);
						}
					}

					return new Relationship[] { rel };
				}
			}
			return new Relationship[0];
		}

		public string ToJSONString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			sb.Append("\"TargetInstance\":");
			sb.Append(SourceInstance.ToJSONString());
			sb.Append(",\"RelationshipInstance\":");
			sb.Append(RelationshipInstance.ToJSONString());
			sb.Append(",\"DestinationInstances\":[");
			foreach (Instance inst in DestinationInstances)
			{
				sb.Append(inst.ToJSONString());
				if (DestinationInstances.IndexOf(inst) < DestinationInstances.Count - 1) sb.Append(',');
			}
			sb.Append("]}");
			return sb.ToString();
		}
	}
}
