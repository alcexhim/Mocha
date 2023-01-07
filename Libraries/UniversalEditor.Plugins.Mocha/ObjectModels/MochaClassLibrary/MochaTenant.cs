//
//  MochaTenant.cs
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
namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary
{
	public class MochaTenant : ICloneable, IMochaStore
	{
		public class MochaTenantCollection
			: System.Collections.ObjectModel.Collection<MochaTenant>
		{
			public MochaTenant this[Guid id]
			{
				get
				{
					foreach (MochaTenant tenant in this)
					{
						if (tenant.ID == id)
							return tenant;
					}
					return null;
				}
			}
			public MochaTenant this[string name]
			{
				get
				{
					foreach (MochaTenant tenant in this)
					{
						if (tenant.Name == name)
							return tenant;
					}
					return null;
				}
			}
			public void Merge(MochaTenant tenant)
			{
				MochaTenant existing = this[tenant.ID];
				if (existing != null)
				{
					if (existing.Name == null && tenant.Name != null)
					{
						existing.Name = tenant.Name;
					}
					if (existing.DefaultObjectSourceID == Guid.Empty)
						existing.DefaultObjectSourceID = tenant.DefaultObjectSourceID;

					foreach (Guid lref in tenant.LibraryReferences)
					{
						if (!existing.LibraryReferences.Contains(lref))
							existing.LibraryReferences.Add(lref);
					}

					foreach (MochaInstance inst in tenant.Instances)
					{
						if (!existing.Instances.Contains(inst))
						{
							existing.Instances.Add(inst);
						}
					}
					foreach (MochaRelationship rel in tenant.Relationships)
					{
						if (!existing.Relationships.Contains(rel))
						{
							existing.Relationships.Add(rel);
						}
					}
				}
				else
				{
					Add(tenant);
				}
			}
		}

		public Guid ID { get; set; }
		public Guid DefaultObjectSourceID { get; set; }

		public string Name { get; set; } = null;

		public MochaLibraryMetadata.MochaLibraryMetadataCollection Metadata { get; } = new MochaLibraryMetadata.MochaLibraryMetadataCollection();

		public MochaInstance.MochaInstanceCollection Instances { get; } = new MochaInstance.MochaInstanceCollection();
		public MochaRelationship.MochaRelationshipCollection Relationships { get; } = new MochaRelationship.MochaRelationshipCollection();
		public System.Collections.Generic.List<Guid> LibraryReferences { get; } = new System.Collections.Generic.List<Guid>();

		public object Clone()
		{
			MochaTenant clone = new MochaTenant();
			clone.ID = ID;
			clone.DefaultObjectSourceID = DefaultObjectSourceID;
			clone.Name = (Name.Clone() as string);

			foreach (MochaLibraryMetadata item in Metadata)
			{
				clone.Metadata.Add(item.Clone() as MochaLibraryMetadata);
			}

			foreach (MochaInstance item in Instances)
			{
				clone.Instances.Add(item.Clone() as MochaInstance);
			}
			foreach (MochaRelationship item in Relationships)
			{
				clone.Relationships.Add(item.Clone() as MochaRelationship);
			}
			foreach (Guid item in LibraryReferences)
			{
				clone.LibraryReferences.Add((Guid)item);
			}
			return clone;
		}

		public MochaInstance FindInstance(Guid id)
		{
			MochaInstance inst = Instances[id];
			return inst;
		}
	}
}
