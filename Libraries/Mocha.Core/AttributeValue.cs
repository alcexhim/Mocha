//
//  Attribute.cs
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
	public struct AttributeValue : IEquatable<AttributeValue>
	{
		public InstanceHandle SourceInstance { get; set; }
		public InstanceHandle AttributeInstance { get; set; }
		public DateTime EffectiveDate { get; set; }
		public object Value { get; set; }

		private bool isNotEmpty;
		public bool IsEmpty { get { return !isNotEmpty; } }

		public static readonly AttributeValue Empty;

		public AttributeValue(InstanceHandle sourceInstance, InstanceHandle attributeInstance, DateTime effectiveDate, object value)
		{
			SourceInstance = sourceInstance;
			AttributeInstance = attributeInstance;
			EffectiveDate = effectiveDate;
			Value = value;
			isNotEmpty = true;
		}

		public bool Equals(AttributeValue other)
		{
			if (IsEmpty != other.IsEmpty || EffectiveDate != other.EffectiveDate
				|| SourceInstance != other.SourceInstance
				|| AttributeInstance != other.AttributeInstance)
				return false;

			if (Value != null && other.Value != null && Value.Equals(other.Value))
				return true;

			return false;
		}

		public static bool operator ==(AttributeValue left, AttributeValue right)
		{
			return left.Equals(right);
		}
		public static bool operator !=(AttributeValue left, AttributeValue right)
		{
			return !left.Equals(right);
		}
	}
}
