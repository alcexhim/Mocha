//
//  InstanceID.cs
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
using System.Diagnostics;

namespace Mocha
{
	[DebuggerNonUserCode] // don't bitch about the internals
	public struct InstanceClassIDPair
	{
		public static InstanceClassIDPair Empty;

		public int ClassID;
		public int InstanceID;

		public InstanceClassIDPair(string classAndInstanceID)
		{
			string[] parts = classAndInstanceID.Split('$');
			ClassID = Int32.Parse(parts[0]);
			InstanceID = Int32.Parse(parts[1]);
		}
		public InstanceClassIDPair(int classID, int instanceID)
		{
			ClassID = classID;
			InstanceID = instanceID;
		}

		public override string ToString()
		{
			return String.Join("$", new object[] { ClassID, InstanceID });
		}

		public static bool operator ==(InstanceClassIDPair left, InstanceClassIDPair right)
		{
			return (left.ClassID == right.ClassID && left.InstanceID == right.InstanceID);
		}
		public static bool operator !=(InstanceClassIDPair left, InstanceClassIDPair right)
		{
			return (left.ClassID != right.ClassID && left.InstanceID != right.InstanceID);
		}
	}
}
