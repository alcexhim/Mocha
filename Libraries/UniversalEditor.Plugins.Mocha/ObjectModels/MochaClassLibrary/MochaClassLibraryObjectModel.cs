//
//  MochaClassLibrary.cs
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
using Mocha.Core;

namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary
{
	public class MochaClassLibraryObjectModel : ObjectModel
	{
		public MochaTenant.MochaTenantCollection Tenants { get; } = new MochaTenant.MochaTenantCollection();
		public MochaLibrary.MochaLibraryCollection Libraries { get; } = new MochaLibrary.MochaLibraryCollection();

		public override void Clear()
		{
			Libraries.Clear();
		}

		public override void CopyTo(ObjectModel where)
		{
			MochaClassLibraryObjectModel clone = (where as MochaClassLibraryObjectModel);
			if (clone == null) throw new ObjectModelNotSupportedException();

			for (int i = 0; i < Libraries.Count; i++)
			{
				clone.Libraries.Add(Libraries[i].Clone() as MochaLibrary);
			}
			for (int i = 0; i < Tenants.Count; i++)
			{
				clone.Tenants.Add(Tenants[i].Clone() as MochaTenant);
			}
		}
		public MochaInstance FindInstance(Guid id)
		{
			return FindInstance(id, out IMochaStore store);
		}
		public MochaInstance FindInstance(Guid id, out IMochaStore store)
		{
			for (int i = 0; i < Libraries.Count; i++)
			{
				MochaInstance inst = Libraries[i].Instances[id];
				if (inst != null)
				{
					store = Libraries[i];
					return inst;
				}
			}
			for (int i = 0; i < Tenants.Count; i++)
			{
				MochaInstance inst = Tenants[i].Instances[id];
				if (inst != null)
				{
					store = Tenants[i];
					return inst;
				}
			}

			store = null;
			return null;
		}

		public MochaRelationship FindRelationship(RelationshipKey relationshipKey)
		{
			return FindRelationship(relationshipKey, out IMochaStore store);
		}
		public MochaRelationship FindRelationship(RelationshipKey relationshipKey, out IMochaStore store)
		{
			for (int i = 0; i < Libraries.Count; i++)
			{
				MochaRelationship rel = Libraries[i].Relationships[relationshipKey];
				if (rel != null)
				{
					store = Libraries[i];
					return rel;
				}
			}
			for (int i = 0; i < Tenants.Count; i++)
			{
				MochaRelationship rel = Tenants[i].Relationships[relationshipKey];
				if (rel != null)
				{
					store = Tenants[i];
					return rel;
				}
			}

			store = null;
			return null;
		}
	}
}
