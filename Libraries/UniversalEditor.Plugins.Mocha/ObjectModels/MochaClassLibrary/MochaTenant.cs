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
			public void Merge(MochaTenant tenant)
			{
				MochaTenant existing = null;  // this[tenant.Name];
				if (existing != null)
				{

				}

				Add(tenant);
			}
		}

		public Guid ID { get; set; }
		public Guid DefaultObjectSourceID { get; set; }

		public string Name { get; set; } = null;

		public MochaLibraryMetadata.MochaLibraryMetadataCollection Metadata { get; } = new MochaLibraryMetadata.MochaLibraryMetadataCollection();

		public MochaInstance.MochaInstanceCollection Instances { get; } = new MochaInstance.MochaInstanceCollection();
		public MochaRelationship.MochaRelationshipCollection Relationships { get; } = new MochaRelationship.MochaRelationshipCollection();
		public MochaLibraryReference.MochaLibraryReferenceCollection LibraryReferences { get; } = new MochaLibraryReference.MochaLibraryReferenceCollection();

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
			foreach (MochaLibraryReference item in LibraryReferences)
			{
				clone.LibraryReferences.Add(item.Clone() as MochaLibraryReference);
			}
			return clone;
		}
	}
}
