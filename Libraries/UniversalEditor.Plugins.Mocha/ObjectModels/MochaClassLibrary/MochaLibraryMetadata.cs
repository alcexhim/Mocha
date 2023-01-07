//
//  MochaLibraryMetadata.cs
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

namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary
{
	public class MochaLibraryMetadata : ICloneable
	{
		public class MochaLibraryMetadataCollection
			: System.Collections.ObjectModel.Collection<MochaLibraryMetadata>
		{
			private Dictionary<string, MochaLibraryMetadata> _itemsByName = new Dictionary<string, MochaLibraryMetadata>();
			public MochaLibraryMetadata this[string name]
			{
				get
				{
					if (_itemsByName.ContainsKey(name))
						return _itemsByName[name];
					return null;
				}
			}
			public bool Contains(string name)
			{
				return _itemsByName.ContainsKey(name);
			}

			protected override void ClearItems()
			{
				base.ClearItems();
				_itemsByName.Clear();
			}
			protected override void InsertItem(int index, MochaLibraryMetadata item)
			{
				base.InsertItem(index, item);
				_itemsByName[item.Name] = item;
			}
			protected override void RemoveItem(int index)
			{
				if (_itemsByName.ContainsKey(this[index].Name))
					_itemsByName.Remove(this[index].Name);
				base.RemoveItem(index);
			}
		}

		public string Name { get; set; }
		public string Value { get; set; }

		public MochaLibraryMetadata(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public object Clone()
		{
			MochaLibraryMetadata clone = new MochaLibraryMetadata(Name?.Clone() as string, Value?.Clone() as string);
			return clone;
		}
	}
}
