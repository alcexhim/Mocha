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
namespace Mocha.Core
{
	public struct TenantHandle : IEquatable<TenantHandle>
	{
		private Guid _ID1;
		private Guid _ID2;

		private DateTime _date;
		private bool isNotEmpty;

		public static TenantHandle Create()
		{
			TenantHandle handle = new TenantHandle();
			handle._ID1 = Guid.NewGuid();
			handle._ID2 = Guid.NewGuid();
			handle._date = DateTime.Now;
			handle.isNotEmpty = true;
			return handle;
		}

		public byte[] ToByteArray()
		{
			byte[] data = new byte[32];
			Array.Copy(_ID1.ToByteArray(), 0, data, 0, 16);
			Array.Copy(_ID2.ToByteArray(), 0, data, 16, 16);
			return data;
		}

		public static readonly TenantHandle Empty = new TenantHandle();

		public bool IsEmpty { get { return !isNotEmpty; } }

		public bool Equals(TenantHandle other)
		{
			if (IsEmpty && other.IsEmpty)
				return true;

			return _ID1 == other._ID1 && _ID2 == other._ID2 && _date == other._date;
		}

		public static bool operator ==(TenantHandle left, TenantHandle right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(TenantHandle left, TenantHandle right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			if (IsEmpty)
				return null;

			byte[] data = ToByteArray();
			return Convert.ToBase64String(data);
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
