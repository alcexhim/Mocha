//
//  MochaSnapshotTransaction.cs
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
namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot
{
	public class MochaSnapshotTransaction : ICloneable
	{
		public class MochaSnapshotTransactionCollection
			: System.Collections.ObjectModel.Collection<MochaSnapshotTransaction>
		{

		}

		public string TenantName { get; set; } = null;
		public MochaSnapshotTransactionOperation.MochaSnapshotTransactionOperationCollection Operations { get; } = new MochaSnapshotTransactionOperation.MochaSnapshotTransactionOperationCollection();

		public object Clone()
		{
			MochaSnapshotTransaction clone = new MochaSnapshotTransaction();
			foreach (MochaSnapshotTransactionOperation op in Operations)
			{
				clone.Operations.Add(op.Clone() as MochaSnapshotTransactionOperation);
			}
			return clone;
		}
	}
}
