//
//  RelationshipValue.cs
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

namespace Mocha.Storage.Local.Internal
{
	internal class RelationshipValue
	{
		public readonly List<Guid> TargetInstanceIDs;
		public static readonly RelationshipValue Empty = new RelationshipValue(new Guid[0]);

		public override bool Equals(object obj)
		{
			if (obj is RelationshipValue)
			{
				RelationshipValue other = (RelationshipValue)obj;
				for (int i = 0; i < TargetInstanceIDs.Count; i++)
				{
					if (!other.TargetInstanceIDs.Contains(TargetInstanceIDs[i]))
						return false;
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int hashcode = 0x51ed270b;
			hashcode += base.GetHashCode();
			for (int i = 0; i < TargetInstanceIDs.Count; i++)
			{
				hashcode += TargetInstanceIDs[i].GetHashCode();
			}
			return hashcode;
		}

		public static bool operator ==(RelationshipValue left, RelationshipValue right)
		{
			if (left is null && right is null)
				return true;
			if (left is null && !(right is null))
				return false;
			if (!(left is null) && (right is null))
				return false;

			return left.Equals(right);
		}
		public static bool operator !=(RelationshipValue left, RelationshipValue right)
		{
			if (left is null && right is null)
				return false;
			if (left is null && !(right is null))
				return true;
			if (!(left is null) && (right is null))
				return true;

			return !left.Equals(right);
		}

		public RelationshipValue() : this(new Guid[0])
		{
		}
		public RelationshipValue(Guid targetInstanceID) : this(new Guid[] { targetInstanceID })
		{
		}
		public RelationshipValue(Guid[] targetInstanceIDs)
		{
			TargetInstanceIDs = new List<Guid>(targetInstanceIDs);
		}
	}
}
