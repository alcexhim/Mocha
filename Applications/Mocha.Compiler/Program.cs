//
//  Program.cs - main entry point for the ZeQuaL compiler (zq)
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
using System.Collections.Generic;
using Mocha.Core;
using UniversalEditor;
using UniversalEditor.Accessors;
using UniversalEditor.Plugins.Mocha.DataFormats.MochaBinary;
using UniversalEditor.Plugins.Mocha.DataFormats.MochaXML;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;

namespace Mocha.Compiler
{
	public class Program
	{
		public static void Main(string[] args)
		{
			List<string> listFileNames = new List<string>();

			string outputFileName = "output.mcx";
			bool foundFileName = false;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].StartsWith("/") && !foundFileName)
				{
					if (args[i].StartsWith("/out:"))
					{
						outputFileName = args[i].Substring(5);
					}
				}
				else
				{
					// is file name
					foundFileName = true;

					listFileNames.Add(args[i]);
				}
			}

			listFileNames.Sort();

			MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();
			MochaBinaryDataFormat mcx = new MochaBinaryDataFormat();
			MochaXMLDataFormat xml = new MochaXMLDataFormat();

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			int totalInstances = 0, totalRelationships = 0;
			for (int i = 0; i < listFileNames.Count; i++)
			{
				Console.Error.WriteLine("reading {0}", listFileNames[i]);

				if (!System.IO.File.Exists(listFileNames[i]))
				{
					MBS.Framework.ConsoleExtensions.LogMSBuildMessage(MBS.Framework.MessageSeverity.Error, "File not found", "MCX0003", listFileNames[i]);
					continue;
				}

				MochaClassLibraryObjectModel mcl1 = new MochaClassLibraryObjectModel();
				FileAccessor fain = new FileAccessor(listFileNames[i]);
				Document.Load(mcl1, xml, fain);

				for (int j = 0; j < mcl1.Libraries.Count; j++)
				{
					mcl.Libraries.Merge(mcl1.Libraries[j]);
					totalInstances += mcl1.Libraries[j].Instances.Count;
					totalRelationships += mcl1.Libraries[j].Relationships.Count;
				}
				for (int j = 0; j < mcl1.Tenants.Count; j++)
				{
					Console.Error.WriteLine("registered tenant {0}", mcl1.Tenants[j].Name);
					mcl.Tenants.Merge(mcl1.Tenants[j]);
				}
			}

			ZqLinkRelationships(mcl);

			Console.Error.WriteLine("wrote {0} libraries with {1} instances and {2} relationships total", mcl.Libraries.Count, totalInstances, totalRelationships);

			/*
			// LINKER SANITY CHECK
			// we need to load all the referenced MCX libraries in a testing
			// environment and check MCX relationships for existing instances
			foreach (MochaLibrary library in mcl.Libraries)
			{
				CheckInstancesForMochaStore(mcl, library);
			}
			foreach (MochaTenant tenant in mcl.Tenants)
			{
				CheckInstancesForMochaStore(mcl, tenant);
			}
			*/

			FileAccessor faout = new FileAccessor(outputFileName, true, true);
			Document.Save(mcl, mcx, faout);
		}

		private static void ZqLinkRelationships(MochaClassLibraryObjectModel mcl)
		{
			for (int i = 0; i < mcl.Libraries.Count; i++)
			{
				ZqLinkRelationships(mcl, mcl.Libraries[i]);
			}
			for (int i = 0; i < mcl.Tenants.Count; i++)
			{
				ZqLinkRelationships(mcl, mcl.Tenants[i]);
			}
		}
		private static void ZqLinkRelationships(MochaClassLibraryObjectModel mcl, IMochaStore store)
		{
			for (int j = 0; j < store.Relationships.Count; j++)
			{
				MochaInstance instRel = mcl.FindInstance(store.Relationships[j].RelationshipInstanceID);
				if (instRel != null)
				{
					MochaRelationship relSiblingRel = mcl.FindRelationship(new RelationshipKey(instRel.ID, KnownRelationshipGuids.Relationship__has_sibling__Relationship));
					if (relSiblingRel != null)
					{
						MochaInstance instSibling = mcl.FindInstance(relSiblingRel.DestinationInstanceIDs[0]);
						if (instSibling != null)
						{
							for (int k = 0; k < store.Relationships[j].DestinationInstanceIDs.Count; k++)
							{
								RelationshipKey rkSibling = new RelationshipKey(store.Relationships[j].DestinationInstanceIDs[k], instSibling.ID);
								MochaRelationship relSibling = store.Relationships[rkSibling];
								if (relSibling == null)
								{
									Console.Error.WriteLine("zq2: linking relationship {0}", instSibling.ID.ToString("B"));
									relSibling = new MochaRelationship() { SourceInstanceID = store.Relationships[j].DestinationInstanceIDs[k], RelationshipInstanceID = instSibling.ID };
									store.Relationships.Add(relSibling);
								}

								if (!relSibling.DestinationInstanceIDs.Contains(store.Relationships[j].SourceInstanceID))
								{
									relSibling.DestinationInstanceIDs.Add(store.Relationships[j].SourceInstanceID);
								}
							}
						}
						else
						{
							Console.Error.WriteLine("zq2: no sibling relationship '{0}' found", relSiblingRel.DestinationInstanceIDs[0].ToString("B"));
						}
					}
				}
			}
		}

		private static void CheckInstancesForMochaStore(MochaClassLibraryObjectModel mcl, IMochaStore library)
		{
			foreach (MochaRelationship rel in library.Relationships)
			{
				if (FindInstance(mcl, library, rel.SourceInstanceID) == null)
				{
					MBS.Framework.ConsoleExtensions.LogMSBuildMessage(MBS.Framework.MessageSeverity.Error, String.Format("relationship references nonexistent sourceInstanceId '{0}'", rel.SourceInstanceID));
				}
				if (FindInstance(mcl, library, rel.RelationshipInstanceID) == null)
				{
					MBS.Framework.ConsoleExtensions.LogMSBuildMessage(MBS.Framework.MessageSeverity.Error, String.Format("relationship references nonexistent relationshipInstanceId '{0}'", rel.RelationshipInstanceID));
				}
				foreach (Guid id in rel.DestinationInstanceIDs)
				{
					if (FindInstance(mcl, library, id) == null)
					{
						MBS.Framework.ConsoleExtensions.LogMSBuildMessage(MBS.Framework.MessageSeverity.Error, String.Format("relationship references nonexistent target instanceReference '{0}'", id));
					}
				}
			}
		}

		private static MochaInstance FindInstance(MochaClassLibraryObjectModel mcl, IMochaStore store, Guid id)
		{
			MochaInstance inst = store.FindInstance(id);
			if (inst != null)
			{
				return inst;
			}
			foreach (Guid libraryId in store.LibraryReferences)
			{
				inst = mcl.Libraries[libraryId].FindInstance(id);
				if (inst != null)
					return inst;
			}
			return null;
		}
	}
}
