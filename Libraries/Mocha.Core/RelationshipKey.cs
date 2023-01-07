//
//  RelationshipKey.cs
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
namespace Mocha.Core
{
	public struct RelationshipKey
	{
		public Guid SourceInstanceID { get; }
		public Guid RelationshipID { get; }

		public RelationshipKey(Guid sourceInstanceId, Guid relationshipId)
		{
			SourceInstanceID = sourceInstanceId;
			RelationshipID = relationshipId;
		}

		public override bool Equals(object obj)
		{
			return (obj is RelationshipKey) && ((RelationshipKey)obj).SourceInstanceID == SourceInstanceID && ((RelationshipKey)obj).RelationshipID == RelationshipID;
		}
		public static bool operator ==(RelationshipKey left, RelationshipKey right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(RelationshipKey left, RelationshipKey right)
		{
			return !left.Equals(right);
		}

		public override int GetHashCode()
		{
			int hashcode = 0x51ed270b;
			hashcode += (SourceInstanceID.GetHashCode() * -1521134295);
			hashcode += (RelationshipID.GetHashCode() * -1521134295);
			return hashcode;
		}
	}
}
