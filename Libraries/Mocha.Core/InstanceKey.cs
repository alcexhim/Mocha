//
//  InstanceKey.cs
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
namespace Mocha.Core
{
	public struct InstanceKey
	{
		public int ClassIndex { get; }
		public int InstanceIndex { get; }

		private bool _isNotEmpty;
		public bool IsEmpty { get { return !_isNotEmpty; } }

		public static readonly InstanceKey Empty = new InstanceKey();

		public static InstanceKey Parse(string instanceKey)
		{
			return new InstanceKey(instanceKey);
		}

		public InstanceKey(string instanceKey)
		{
			if (instanceKey == null)
			{
				ClassIndex = 0;
				InstanceIndex = 0;
				_isNotEmpty = false;
			}
			else
			{
				if (instanceKey.Contains("$"))
				{
					string[] split = instanceKey.Split(new char[] { '$' });
					if (split.Length == 2)
					{
						if (Int32.TryParse(split[0], out int ci) && Int32.TryParse(split[1], out int ii))
						{
							ClassIndex = ci;
							InstanceIndex = ii;
							_isNotEmpty = true;
							return;
						}
					}
				}
			}
			throw new ArgumentException("must be a string containing two integers separated by a '$'");
		}
		public InstanceKey(int classIndex, int instanceIndex)
		{
			ClassIndex = classIndex;
			InstanceIndex = instanceIndex;
			_isNotEmpty = true;
		}

		public override bool Equals(object obj)
		{
			return (obj is InstanceKey) && ((InstanceKey)obj).ClassIndex == ClassIndex && ((InstanceKey)obj).InstanceIndex == InstanceIndex && ((InstanceKey)obj).IsEmpty == IsEmpty;
		}
		public static bool operator ==(InstanceKey left, InstanceKey right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(InstanceKey left, InstanceKey right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return String.Format("{0}${1}", ClassIndex, InstanceIndex);
		}
	}
}
