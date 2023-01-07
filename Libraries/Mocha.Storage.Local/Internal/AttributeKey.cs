//
//  AttributeKey.cs
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
namespace Mocha.Storage.Local.Internal
{
	internal struct AttributeKey
	{
		public Guid InstanceID { get; }
		public Guid AttributeID { get; }

		public AttributeKey(Guid instanceID, Guid attributeID)
		{
			InstanceID = instanceID;
			AttributeID = attributeID;
		}

		public override bool Equals(object obj)
		{
			return (obj is AttributeKey) && ((AttributeKey)obj).InstanceID == InstanceID && ((AttributeKey)obj).AttributeID == AttributeID;
		}
		public static bool operator ==(AttributeKey left, AttributeKey right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(AttributeKey left, AttributeKey right)
		{
			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			int hashcode = 0x51ed270b;
			hashcode += (InstanceID.GetHashCode() * -1521134295);
			hashcode += (AttributeID.GetHashCode() * -1521134295);
			return hashcode;
		}
	}
}
