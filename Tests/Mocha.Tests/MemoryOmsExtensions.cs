//
//  MemoryOmsExtensions.cs
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
using NUnit.Framework;

namespace Mocha.Tests
{
	[TestFixture(Category = "OMS")]
	public class MemoryOmsExtensions
	{
		public static readonly Type TYPE_OMS = typeof(Core.OmsImplementations.MemoryOms);
		public static readonly Guid TEST_CLASS_GUID = new Guid("{380447a5-4174-414b-a80f-844997308358}");
		public const string TEST_CLASS_NAME = "TEST_CLASS_NAME";
		public const string TEST_CLASS_TITLE = "TEST CLASS TITLE";

		Oms oms = null;

		[SetUp()]
		public void SetUp()
		{
			oms = MemoryOms.GetTestingOms();
		}

		[TearDown()]
		public void TearDown()
		{
			oms = null;
		}

		/// <summary>
		/// Tests the functionality of the <see cref="OmsExtensions.CreateClass(Oms, Guid, string, string)" /> method using a known
		/// test class GUID and checks that the name attribute is properly set.
		/// </summary>
		[Test(Description = "Creates a class and tests the CreateTranslation functionality simultaneously.")]
		public void CreateClassWithTranslation()
		{
			oms.BeginTransaction();

			InstanceHandle key = oms.CreateClass(TEST_CLASS_GUID, TEST_CLASS_NAME, TEST_CLASS_TITLE);

			oms.CommitTransaction();


			string name = oms.GetAttributeValue<string>(key, oms.GetInstance(KnownAttributeGuids.Text.Name));
			Assert.AreEqual(TEST_CLASS_NAME, name);

			string title = oms.GetTranslationValue(key, oms.GetInstance(KnownRelationshipGuids.Class__has_title__Translatable_Text_Constant), oms.GetInstance(KnownInstanceGuids.Languages.English));
			Assert.AreEqual(TEST_CLASS_TITLE, title);
		}
	}
}
