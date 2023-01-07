//
//  AttributeValue.cs
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
	internal struct AttributeValue
	{
		public DateTime EffectiveDateTime;
		public object Value;
		public Guid UserInstanceID;

		private bool isNotEmpty;
		public bool IsEmpty { get { return !isNotEmpty; } }

		public static readonly AttributeValue Empty = new AttributeValue();

		public AttributeValue(object value, DateTime effectiveDateTime, Guid userInstanceId)
		{
			Value = value;
			EffectiveDateTime = effectiveDateTime;
			UserInstanceID = userInstanceId;
			isNotEmpty = true;
		}
	}
}
