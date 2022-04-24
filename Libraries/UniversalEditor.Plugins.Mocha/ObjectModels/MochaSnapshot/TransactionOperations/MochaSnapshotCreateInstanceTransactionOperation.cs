//
//  MochaSnapshotCreateInstanceTransactionOperation.cs
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
namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot.TransactionOperations
{
	public class MochaSnapshotCreateInstanceTransactionOperation : MochaSnapshotTransactionOperation
	{
		public Guid GlobalIdentifier { get; set; } = Guid.Empty;
		public Guid ClassGlobalIdentifier { get; set; } = Guid.Empty;

		public override object Clone()
		{
			MochaSnapshotCreateInstanceTransactionOperation clone = new MochaSnapshotCreateInstanceTransactionOperation();
			clone.EffectiveDate = EffectiveDate;
			clone.GlobalIdentifier = GlobalIdentifier;
			clone.ClassGlobalIdentifier = ClassGlobalIdentifier;
			return clone;
		}

		public override string ToString()
		{
			return String.Format("CI: {0} : {1}", GlobalIdentifier, ClassGlobalIdentifier);
		}
	}
}
