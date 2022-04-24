//
//  Program.cs
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
using Mocha.Core;
using Mocha.Core.OmsImplementations;
using UniversalEditor.Plugins.Mocha;

namespace Mocha.TestProject
{
	class Program
	{
		private static class LocalInstanceGuids
		{
			public static class Classes
			{
				public static Guid Character { get; } = new Guid("{50df54ef-a093-4761-aaf9-2bb926d22ce1}");
				public static Guid InventoryItem { get; } = new Guid("{ca8d01b6-166f-4913-8548-32bdbe677111}");
			}

			public static class Characters
			{
				public static Guid Ashlyn { get; } = new Guid("{13511137-c59e-44c5-814f-d4b7c21981d0}");
			}
		}
		private static class LocalRelationshipGuids
		{
			public static Guid Character__has__Gender { get; } = new Guid("{c1ceedf6-33ec-45b3-8a91-bafd17edd069}");
		}

		private static readonly Guid GUID_ZZZ = new Guid("{9d68a728-dc6a-49a3-9017-c9f53905b8ce}");
		private static readonly Guid GUID_CLASS_HAS_ZZZ = new Guid("{912505ae-cdef-4328-9d23-26a99e73299f}");
		private static readonly Guid GUID_ZZZ_FOR_CLASS = new Guid("{cc0cd358-e608-4f4c-8151-9e8194a52136}");

		private static void TestExisting(Oms oms)
		{
			InstanceHandle ihInventItem = oms.GetInstance(LocalInstanceGuids.Classes.InventoryItem);
			string title = oms.GetInstanceTitle(ihInventItem);
			string title2 = oms.GetTranslationValue(ihInventItem, oms.GetInstance(KnownRelationshipGuids.Class__has_title__Translatable_Text_Constant), oms.GetInstance(KnownInstanceGuids.Languages.English));

		}

		public static void Main(string[] args)
		{
			Oms oms = new MemoryOms();

			oms.InitializeTenants("System/Tenants");

			oms.Load("System/Snapshots");

			oms.DefaultTenant = oms.GetTenant("archalos1");

			TestExisting(oms);
			oms.BeginTransaction();

			InstanceHandle ihInventoryItem = oms.CreateClass(LocalInstanceGuids.Classes.InventoryItem, "InventoryItem", "Inventory Item");
			InstanceHandle ihCharacter = oms.CreateClass(LocalInstanceGuids.Classes.Character, "Character", "Character");
			InstanceHandle ihGender = oms.CreateClass(LocalInstanceGuids.Classes.Character, "Gender", "Gender");

			oms.CommitTransaction();

			oms.BeginTransaction();

			InstanceHandle ikCharacter__has__Gender = oms.CreateRelationshipInstance(ihCharacter, "has", ihGender, LocalRelationshipGuids.Character__has__Gender);
			InstanceHandle ihGender1 = oms.CreateInstance(ihGender);

			oms.CommitTransaction();
			oms.BeginTransaction();

			InstanceHandle ikCharacter__Ashlyn = oms.CreateInstance(LocalInstanceGuids.Characters.Ashlyn, LocalInstanceGuids.Classes.Character);
			oms.CreateRelationship(ikCharacter__Ashlyn, ikCharacter__has__Gender, ihGender1);

			oms.CommitTransaction();

			InstanceHandle ihClass = oms.GetInstance(InstanceKey.Parse("1$1"));
			ihClass = oms.GetInstance(KnownInstanceGuids.Classes.Class);

			Guid id = Guid.Empty;


			bool verified = oms.Transactions.Verify();

			InstanceHandle ihGndr = oms.GetRelatedInstance(ikCharacter__Ashlyn, ikCharacter__has__Gender);

			// InstanceKey key = oms.GetInstanceKey(ihGndr);

			InstanceHandle ihGnd2 = oms.GetRelatedInstance(ikCharacter__Ashlyn, ikCharacter__has__Gender);
			oms.Save("System/Snapshots");

			Console.WriteLine(ikCharacter__Ashlyn.ToString());
		}
	}
}
