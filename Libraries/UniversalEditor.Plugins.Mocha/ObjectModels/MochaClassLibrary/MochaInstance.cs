//
//  MochaInstance.cs
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
namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary
{
	public class MochaInstance : ICloneable
	{
		public class MochaInstanceCollection
			: System.Collections.ObjectModel.Collection<MochaInstance>
		{
			public MochaInstance this[Guid id]
			{
				get
				{
					for (int i = 0; i < Count; i++)
					{
						if (this[i].ID == id)
							return this[i];
					}
					return null;
				}
			}
			public void Merge(MochaInstance item)
			{
				MochaInstance orig = this[item.ID];
				if (orig != null)
				{
					orig.Merge(item);
					return;
				}
				Add(item);
			}

			public bool Contains(Guid id)
			{
				for (int i = 0; i < Count;i ++)
				{
					if (this[i].ID == id)
						return true;
				}
				return false;
			}
		}

		private void Merge(MochaInstance item)
		{
			if (ID != item.ID)
				throw new InvalidOperationException("cannot merge two instances with different global identifiers");


		}

		public Guid ID { get; set; } = Guid.Empty;
		public int? Index { get; set; } = null;
		public MochaAttributeValue.MochaAttributeValueCollection AttributeValues { get; } = new MochaAttributeValue.MochaAttributeValueCollection();

		public object Clone()
		{
			MochaInstance clone = new MochaInstance();
			clone.ID = ID;
			clone.Index = Index;
			for (int i = 0; i < AttributeValues.Count; i++)
			{
				clone.AttributeValues.Add(AttributeValues[i].Clone() as MochaAttributeValue);
			}
			return clone;
		}
	}
}
