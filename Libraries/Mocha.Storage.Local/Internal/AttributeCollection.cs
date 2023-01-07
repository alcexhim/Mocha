//
//  AttributeCollection.cs
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

using MochaAttribute = Mocha.Core.Attribute;

namespace Mocha.Storage.Local.Internal
{
	internal class AttributeCollection
	{
		private Dictionary<AttributeKey, List<AttributeValue>> _ItemsByID = new Dictionary<AttributeKey, List<AttributeValue>>();

		public List<AttributeValue> this[AttributeKey key]
		{
			get
			{
				if (!_ItemsByID.ContainsKey(key))
				{
					return null;
				}
				return _ItemsByID[key];
			}
			set
			{
				_ItemsByID[key] = value;
			}
		}
	}
}
