//
//  InstanceHandle.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2022 Mike Becker's Software
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
using MBS.Framework;

namespace Mocha.Core
{
	public struct InstanceHandle : IEquatable<InstanceHandle>
	{
		private NanoId _ID;
		private bool isNotEmpty;

		public static InstanceHandle Create()
		{
			InstanceHandle handle = new InstanceHandle();
			handle._ID = NanoId.Generate();
			handle.isNotEmpty = true;
			return handle;
		}

		public byte[] ToByteArray()
		{
			return System.Text.Encoding.ASCII.GetBytes(_ID.ToString());
		}

		public static readonly InstanceHandle Empty = new InstanceHandle();

		public bool IsEmpty { get { return !isNotEmpty; } }

		public bool Equals(InstanceHandle other)
		{
			if (IsEmpty && other.IsEmpty)
				return true;

			return _ID == other._ID && !IsEmpty;
		}

		public static bool operator ==(InstanceHandle left, InstanceHandle right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(InstanceHandle left, InstanceHandle right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			if (IsEmpty)
				return "(empty)  ";

			return _ID.ToString();
		}


		public static string IntToString(int value, char[] baseChars)
		{
			string result = string.Empty;
			int targetBase = baseChars.Length;

			do
			{
				result = baseChars[value % targetBase] + result;
				value = value / targetBase;
			}
			while (value > 0);

			return result;
		}

		/// <summary>
		/// An optimized method using an array as buffer instead of 
		/// string concatenation. This is faster for return values having 
		/// a length > 1.
		/// </summary>
		public static string IntToStringFast(int value, char[] baseChars)
		{
			// 32 is the worst cast buffer size for base 2 and int.MaxValue
			int i = 32;
			char[] buffer = new char[i];
			int targetBase = baseChars.Length;

			do
			{
				buffer[--i] = baseChars[value % targetBase];
				value = value / targetBase;
			}
			while (value > 0);

			char[] result = new char[32 - i];
			Array.Copy(buffer, i, result, 0, 32 - i);

			return new string(result);
		}
	}
}
