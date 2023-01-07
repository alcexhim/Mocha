//
//  InstanceCollection.cs
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
	internal class InstanceCollection
	{
		private Dictionary<Guid, Instance> _ItemsByID = new Dictionary<Guid, Instance>();

		/*
		private Dictionary<InstanceKey, Instance> _ItemsByKey = new Dictionary<InstanceKey, Instance>();

		public Instance this[InstanceKey key]
		{
			get
			{
				if (_ItemsByKey.ContainsKey(key))
					return _ItemsByKey[key];
				return null;
			}
			set
			{
				_ItemsByKey[key] = value;
			}
		}
		*/
		public Instance this[Guid id]
		{
			get
			{
				if (_ItemsByID.ContainsKey(id))
					return _ItemsByID[id];
				return null;
			}
			set
			{
				_ItemsByID[id] = value;
			}
		}

		public Instance[] ToArray()
		{
			Instance[] array = new Instance[_ItemsByID.Count];
			_ItemsByID.Values.CopyTo(array, 0);
			return array;
		}
	}
}
