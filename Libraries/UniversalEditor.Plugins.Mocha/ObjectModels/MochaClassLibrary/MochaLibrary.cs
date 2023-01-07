//
//  MochaLibrary.cs
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
namespace UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary
{
	public class MochaLibrary : ICloneable, IMochaStore
	{
		public class MochaLibraryCollection
			: System.Collections.ObjectModel.Collection<MochaLibrary>
		{
			public MochaLibrary this[Guid id]
			{
				get
				{
					for (int i = 0; i < Count; i++)
					{
						if (this[i].ID == id)
							return this[i];
					}
					return null;
				}
			}

			public void Merge(MochaLibrary item)
			{
				MochaLibrary orig = this[item.ID];
				if (orig != null)
				{
					orig.Merge(item);
					return;
				}
				Add(item);
			}
		}

		private void Merge(MochaLibrary item)
		{
			for (int i = 0; i < item.Metadata.Count; i++)
			{
				if (!Metadata.Contains(item.Metadata[i].Name))
				{
					Metadata.Add(item.Metadata[i]);
				}
			}
			for (int i = 0; i < item.Instances.Count; i++)
			{
				Instances.Merge(item.Instances[i]);
			}
			for (int i = 0; i < item.Relationships.Count; i++)
			{
				Relationships.Merge(item.Relationships[i]);
			}
			for (int i = 0; i < item.LibraryReferences.Count; i++)
			{
				LibraryReferences.Add(item.LibraryReferences[i]);
			}
		}

		public Guid ID { get; set; }
		public MochaLibraryMetadata.MochaLibraryMetadataCollection Metadata { get; } = new MochaLibraryMetadata.MochaLibraryMetadataCollection();

		public MochaInstance.MochaInstanceCollection Instances { get; } = new MochaInstance.MochaInstanceCollection();
		public MochaRelationship.MochaRelationshipCollection Relationships { get; } = new MochaRelationship.MochaRelationshipCollection();
		public System.Collections.Generic.List<Guid> LibraryReferences { get; } = new System.Collections.Generic.List<Guid>();

		public Guid DefaultObjectSourceID { get; set; }

		public object Clone()
		{
			MochaLibrary clone = new MochaLibrary();
			clone.ID = ID;
			clone.DefaultObjectSourceID = DefaultObjectSourceID;
			for (int i = 0; i < Metadata.Count; i++)
			{
				clone.Metadata.Add(Metadata[i].Clone() as MochaLibraryMetadata);
			}
			for (int i = 0; i < Instances.Count; i++)
			{
				clone.Instances.Add(Instances[i].Clone() as MochaInstance);
			}
			for (int i = 0; i < Relationships.Count; i++)
			{
				clone.Relationships.Add(Relationships[i].Clone() as MochaRelationship);
			}
			for (int i = 0; i < LibraryReferences.Count; i++)
			{
				clone.LibraryReferences.Add(LibraryReferences[i]);
			}
			return clone;
		}

		public MochaInstance FindInstance(Guid id)
		{
			if (Instances[id] != null)
				return Instances[id];
			return null;
		}
	}
}
