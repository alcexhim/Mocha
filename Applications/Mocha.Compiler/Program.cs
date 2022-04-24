//
//  MyClass.cs
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

			MochaClassLibraryObjectModel mcl = new MochaClassLibraryObjectModel();
			MochaBinaryDataFormat mcx = new MochaBinaryDataFormat();
			MochaXMLDataFormat xml = new MochaXMLDataFormat();

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			int totalInstances = 0, totalRelationships = 0;
			for (int i = 0; i < listFileNames.Count; i++)
			{
				Console.Error.WriteLine("reading {0}", listFileNames[i]);

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

			Console.Error.WriteLine("wrote {0} libraries with {1} instances and {2} relationships total", mcl.Libraries.Count, totalInstances, totalRelationships);

			string outputDir = System.IO.Path.GetDirectoryName(outputFileName);
			if (!System.IO.Directory.Exists(outputDir))
			{
				System.IO.Directory.CreateDirectory(outputDir);
			}

			FileAccessor faout = new FileAccessor(outputFileName, true, true);
			Document.Save(mcl, mcx, faout);
		}
	}
}
