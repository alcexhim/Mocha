//
//  RelationshipCollection.cs
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
using Mocha.Core;

namespace Mocha.Storage.Local.Internal
{
	internal class RelationshipCollection
	{
		private Dictionary<RelationshipKey, RelationshipValue> _ItemsByID = new Dictionary<RelationshipKey, RelationshipValue>();

		public void ApplySiblingRelationships()
		{
			Dictionary<RelationshipKey, RelationshipValue> newValues = new Dictionary<RelationshipKey, RelationshipValue>();
			List<Guid> siblingsApplied = new List<Guid>();
			foreach (KeyValuePair<RelationshipKey, RelationshipValue> kvp in _ItemsByID)
			{
				RelationshipKey siblingKey = new RelationshipKey(kvp.Key.RelationshipID, KnownRelationshipGuids.Relationship__has_sibling__Relationship);
				if (_ItemsByID.ContainsKey(siblingKey))
				{
					RelationshipValue siblingRelationship = _ItemsByID[siblingKey];

					foreach (Guid targetInstanceId in kvp.Value.TargetInstanceIDs)
					{
						RelationshipKey newSiblingKey = new RelationshipKey(targetInstanceId, siblingRelationship.TargetInstanceIDs[0]);
						if (!_ItemsByID.ContainsKey(newSiblingKey))
						{
							newValues[newSiblingKey] = new RelationshipValue(kvp.Key.SourceInstanceID);
						}
					}
				}
			}
			foreach (KeyValuePair<RelationshipKey, RelationshipValue> kvp in newValues)
			{
				_ItemsByID[kvp.Key] = kvp.Value;
			}
		}

		public RelationshipValue this[RelationshipKey key]
		{
			get
			{
				if (!_ItemsByID.ContainsKey(key))
					return null;
				return _ItemsByID[key];
			}
			set
			{
				_ItemsByID[key] = value;
			}
		}
	}
}
