//
//  MochaRelationship.cs
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

namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary
{
	public class MochaRelationship : ICloneable
	{
		public class MochaRelationshipCollection
			: System.Collections.ObjectModel.Collection<MochaRelationship>
		{
			private Dictionary<RelationshipKey, MochaRelationship> _itemsByKey = new Dictionary<RelationshipKey, MochaRelationship>();
			public MochaRelationship this[RelationshipKey key]
			{
				get
				{
					if (_itemsByKey.ContainsKey(key))
						return _itemsByKey[key];
					return null;
				}
			}

			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByKey.Clear();
			}
			protected override void InsertItem(int index, MochaRelationship item)
			{
				base.InsertItem(index, item);
				_itemsByKey[new RelationshipKey(item.SourceInstanceID, item.RelationshipInstanceID)] = item;
			}
			protected override void RemoveItem(int index)
			{
				_itemsByKey.Remove(new RelationshipKey(this[index].SourceInstanceID, this[index].RelationshipInstanceID));
				base.RemoveItem(index);
			}

			public void Merge(MochaRelationship item)
			{
				MochaRelationship orig = this[new RelationshipKey(item.SourceInstanceID, item.RelationshipInstanceID)];
				if (orig != null)
				{
					orig.Merge(item);
					return;
				}
				Add(item);
			}
		}

		public Guid RelationshipInstanceID { get; set; }
		public Guid SourceInstanceID { get; set; }
		public List<Guid> DestinationInstanceIDs { get; set; } = new List<Guid>();

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="MochaRelationship"/> should be removed from the tenant.
		/// </summary>
		/// <value><c>true</c> if remove; otherwise, <c>false</c>.</value>
		public bool Remove { get; set; } = false;

		private void Merge(MochaRelationship item)
		{
			if (!(SourceInstanceID == item.SourceInstanceID && RelationshipInstanceID == item.RelationshipInstanceID))
				throw new InvalidOperationException("cannot merge two instances with different (source and relationship) identifiers");

			foreach (Guid id in item.DestinationInstanceIDs)
				DestinationInstanceIDs.Add(id);
		}

		public object Clone()
		{
			MochaRelationship clone = new MochaRelationship();
			clone.RelationshipInstanceID = RelationshipInstanceID;
			clone.SourceInstanceID = SourceInstanceID;
			for (int i = 0; i < DestinationInstanceIDs.Count; i++)
			{
				clone.DestinationInstanceIDs.Add(DestinationInstanceIDs[i]);
			}
			return clone;
		}
	}
}
