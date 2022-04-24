//
//  AssignAttributeTransactionOperation.cs
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
namespace Mocha.Core.TransactionOperations
{
	public class AssignAttributeTransactionOperation : TransactionOperation
	{
		public InstanceHandle SourceInstance { get; } = InstanceHandle.Empty;
		public InstanceHandle AttributeInstance { get; } = InstanceHandle.Empty;
		public DateTime EffectiveDate { get; } = DateTime.Now;
		public object Value { get; } = null;

		protected override byte[] GetDataInternal()
		{
			byte[] data = new byte[8 + 8 + 8 + 8];
			/*
			Array.Copy(BitConverter.GetBytes(InstanceKey.ClassIndex), 0, data, 0, 4);
			Array.Copy(BitConverter.GetBytes(InstanceKey.InstanceIndex), 0, data, 4, 4);
			Array.Copy(BitConverter.GetBytes(InstanceKey.ClassIndex), 0, data, 8, 4);
			Array.Copy(BitConverter.GetBytes(InstanceKey.InstanceIndex), 0, data, 16, 4);
			Array.Copy(BitConverter.GetBytes(EffectiveDate.ToBinary()), 0, data, 20, 8);
			*/
			// FIXME: write value
			return data;
		}

		public AssignAttributeTransactionOperation(InstanceHandle sourceInstance, InstanceHandle attributeInstance, DateTime effectiveDate, object value)
		{
			SourceInstance = sourceInstance;
			AttributeInstance = attributeInstance;
			EffectiveDate = effectiveDate;
			Value = value;
		}
	}
}
