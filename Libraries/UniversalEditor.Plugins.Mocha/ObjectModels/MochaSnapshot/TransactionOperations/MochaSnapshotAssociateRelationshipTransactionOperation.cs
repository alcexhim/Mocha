//
//  MochaSnapshotAssignAttributeTransactionOperation.cs
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
using System.Collections.Generic;

using MBS.Framework.Collections;

namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaSnapshot.TransactionOperations
{
	public class MochaSnapshotAssociateRelationshipTransactionOperation : MochaSnapshotTransactionOperation
	{
		public Guid SourceInstanceID { get; set; } = Guid.Empty;
		public Guid RelationshipInstanceID { get; set; } = Guid.Empty;
		public List<Guid> TargetInstanceIDs { get; } = new List<Guid>();
		public bool Remove { get; set; } = false;

		public override object Clone()
		{
			MochaSnapshotAssociateRelationshipTransactionOperation clone = new MochaSnapshotAssociateRelationshipTransactionOperation();
			clone.SourceInstanceID = SourceInstanceID;
			clone.RelationshipInstanceID = RelationshipInstanceID;
			clone.EffectiveDate = EffectiveDate;
			foreach (Guid id in TargetInstanceIDs)
			{
				clone.TargetInstanceIDs.Add(id);
			}
			return clone;
		}

		public override string ToString()
		{
			return String.Format("AR: {0} . {1} => {2}", SourceInstanceID, RelationshipInstanceID, TargetInstanceIDs.ToString(", "));
		}
	}
}
