//
//  Test.cs
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
using NUnit.Framework;
using System;

using Mocha.Core;
using UniversalEditor.Plugins.Mocha;
using UniversalEditor.Accessors;
using Mocha.OMS;

namespace Mocha.Tests
{
	[TestFixture(Category = "OMS")]
	public class MemoryOms
	{
		private static readonly InstanceKey IK_CLASS = new InstanceKey(1, 1);

		private static readonly Type TYPE_OMS = typeof(OMS.LocalOms);

		private Oms oms = null;
		internal static Oms GetTestingOms()
		{
			Oms _oms = (Oms)TYPE_OMS.Assembly.CreateInstance(TYPE_OMS.FullName);
			// _oms.TenantName = _oms.CreateTenant("default");
			// _oms.Initialize(new MemoryAccessor(Properties.Resources.Mocha_Core_v1_0_mcl));
			return _oms;
		}

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

		[Test(Description = "Determines whether the Mocha OMS successfully initializes.")]
		public void Initialization()
		{
			InstanceHandle[] ihs = oms.GetInstances();
			Assert.Greater(ihs.Length, 0);

			InstanceHandle ihClass = oms.GetInstance(KnownInstanceGuids.Classes.Class);
			Assert.IsFalse(ihClass.IsEmpty);
			/*
			InstanceKey keyClass = oms.GetInstanceKey(ihClass);

			Assert.AreEqual(IK_CLASS, keyClass);
			*/		
		}

		/// <summary>
		/// The test value used for SetAttributeValue tests.
		/// </summary>
		private const string SAV_TEST_VALUE = "TeStVaLuE";

		[Test(Description = "Determines whether the SetAttributeValue method properly assigns an attribute value.")]
		public void SetAttributeValue()
		{
			InstanceHandle keyClass = oms.GetInstance(KnownInstanceGuids.Classes.Class);
			InstanceHandle keyAttributeName = oms.GetInstance(KnownAttributeGuids.Text.Name);

			oms.BeginTransaction();

			oms.SetAttributeValue(keyClass, keyAttributeName, SAV_TEST_VALUE);

			oms.CommitTransaction();

			string value = oms.GetAttributeValue<string>(keyClass, keyAttributeName);

			Assert.AreEqual(SAV_TEST_VALUE, value);
		}

		[Test(Description = "Tests whether the DiscardTransaction successfully rolls back the SetAttributeValue operation.")]
		public void SetAttributeValueDiscarded()
		{
			InstanceHandle keyClass = oms.GetInstance(KnownInstanceGuids.Classes.Class);
			InstanceHandle keyAttributeName = oms.GetInstance(KnownAttributeGuids.Text.Name);

			oms.BeginTransaction();

			oms.SetAttributeValue(keyClass, keyAttributeName, SAV_TEST_VALUE);

			oms.DiscardTransaction();

			string value = oms.GetAttributeValue<string>(keyClass, keyAttributeName);

			Assert.IsNull(value);
		}

		[Test(Description = "Determines whether the CreateRelationship method properly associates a relationship.")]
		public void CreateRelationship()
		{
			InstanceHandle keyClass = oms.GetInstance(KnownInstanceGuids.Classes.Class);
			InstanceHandle keyAttribute = oms.GetInstance(KnownInstanceGuids.Classes.Attribute);

			oms.BeginTransaction();

			InstanceHandle keyRelationship = oms.CreateRelationshipInstance(keyClass, "has test relationship", keyAttribute);

			// Class.has test relationship Attribute
			oms.CreateRelationship(keyClass, keyRelationship, keyAttribute);

			oms.CommitTransaction();

			InstanceHandle[] keys = oms.GetRelatedInstances(keyClass, keyRelationship);
			Assert.NotNull(keys);

			Assert.AreEqual(1, keys.Length);

			Assert.AreEqual(keyAttribute, keys[0]);
		}

		/// <summary>
		/// Throwaway GUID for a temporary class used to test relationships.
		/// </summary>
		private static readonly Guid GUID_ZZZ = new Guid("{9d68a728-dc6a-49a3-9017-c9f53905b8ce}");
		private static readonly Guid GUID_CLASS_HAS_ZZZ = new Guid("{912505ae-cdef-4328-9d23-26a99e73299f}");
		private static readonly Guid GUID_ZZZ_FOR_CLASS = new Guid("{cc0cd358-e608-4f4c-8151-9e8194a52136}");

		/// <summary>
		/// Determines whether the OMS properly identifies and generates a sibling relationship.
		/// </summary>
		[Test(Description = "Determines whether the OMS properly identifies and generates a sibling relationship.")]
		public void SiblingRelationshipAutogenerates()
		{
			oms.BeginTransaction();

			InstanceHandle ihClass = oms.GetInstance(KnownInstanceGuids.Classes.Class);
			InstanceHandle ihZzz = oms.CreateClass(GUID_ZZZ);

			oms.CommitTransaction();
			oms.BeginTransaction();

			InstanceHandle ihZzzInst1 = oms.CreateInstance(ihZzz);

			InstanceHandle ihClass_has_ZZZ = oms.CreateRelationshipInstance(ihClass, "has", ihZzz, GUID_CLASS_HAS_ZZZ);
			InstanceHandle ihZZZ_for_Class = oms.CreateRelationshipInstance(ihZzz, "for", ihClass, GUID_ZZZ_FOR_CLASS);

			oms.CommitTransaction();
			oms.BeginTransaction();
			oms.CreateRelationship(ihClass_has_ZZZ, oms.GetInstance(KnownRelationshipGuids.Relationship__has_sibling__Relationship), ihZZZ_for_Class);
			oms.CommitTransaction();

			oms.BeginTransaction();
			oms.CreateRelationship(ihClass, ihClass_has_ZZZ, ihZzzInst1);
			oms.CommitTransaction();

			InstanceHandle ihZzzClass = oms.GetRelatedInstance(ihZzzInst1, ihZZZ_for_Class);
			Assert.AreEqual(ihClass, ihZzzClass);
		}
	}
}
