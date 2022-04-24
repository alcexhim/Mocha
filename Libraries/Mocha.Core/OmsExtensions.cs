//
//  OmsExtensions.cs - high-level extension methods for the Mocha OMS
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
using System.Text;

namespace Mocha.Core
{
	/// <summary>
	/// Contains high-level extension methods for working with the Mocha OMS.
	/// </summary>
	public static class OmsExtensions
	{
		public static bool HasInstance(this Oms oms, Guid id)
		{
			return oms.GetInstance(id) != InstanceHandle.Empty;
		}
		public static bool IsInitialized(this Oms oms)
		{
			if (oms.DefaultTenant == TenantHandle.Empty)
				throw new InvalidOperationException("please specify a tenant");

			if (oms.HasInstance(KnownInstanceGuids.Classes.Class) && oms.HasInstance(KnownInstanceGuids.Classes.Attribute) && oms.HasInstance(KnownInstanceGuids.Classes.Relationship))
			{
				return true;
			}
			return false;
		}

		public static InstanceHandle CreateClass(this Oms oms, Guid classInstanceId, string name = null, string title = null)
		{
			return CreateClass(oms, classInstanceId, KnownInstanceGuids.Classes.Class, name, title);
		}
		/// <summary>
		/// Creates a class with the given <paramref name="classInstanceId" /> and optional name and title.
		/// </summary>
		/// <remarks>
		/// This method makes several calls to the underlying <see cref="Oms" /> to create the class instance and assign the
		/// Name attribute the value of the <paramref name="name" /> parameter. A Translation with relationship
		/// `Class.has title Translation` is associated with the newly-created Class. By default, the Translation Value is
		/// created with the given <paramref name="title" /> and Language set to `English (United Sstates)`. If
		/// <paramref name="title"/> is null, the <paramref name="name" /> parameter is used instead.
		/// </remarks>
		/// <returns>The <see cref="InstanceKey" /> of the newly-created class.</returns>
		/// <param name="oms">Oms.</param>
		/// <param name="classInstanceId">Class instance identifier.</param>
		/// <param name="name">The language-agnostic name to assign to the newly-created class.</param>
		/// <param name="title">The language-specific title to assign to the newly-created class.</param>
		public static InstanceHandle CreateClass(this Oms oms, Guid classInstanceId, Guid parentClassInstanceId, string name = null, string title = null)
		{
			// FIXME: this should take into account security policy for the currently-logged-in user
			InstanceHandle ih = oms.CreateInstance(classInstanceId, KnownInstanceGuids.Classes.Class);
			if (parentClassInstanceId != Guid.Empty)
			{
				oms.CreateRelationship(ih, oms.GetInstance(KnownRelationshipGuids.Class__has_super__Class), oms.GetInstance(parentClassInstanceId));
				oms.CreateRelationship(oms.GetInstance(parentClassInstanceId), oms.GetInstance(KnownRelationshipGuids.Class__has_sub__Class), ih);
			}
			if (name != null)
			{
				oms.SetAttributeValue(ih, oms.GetInstance(KnownAttributeGuids.Text.Name), name);
				if (title == null)
				{
					title = name;
				}

				oms.CreateTranslation(ih, oms.GetInstance(KnownRelationshipGuids.Class__has_title__Translatable_Text_Constant), oms.GetInstance(KnownInstanceGuids.Languages.English), title);
			}
			return ih;
		}

		public static InstanceHandle CreateTranslation(this Oms oms, InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle languageInstance, string value)
		{
			InstanceHandle keyTTC = oms.GetRelatedInstance(sourceInstance, relationshipInstance);
			if (keyTTC == InstanceHandle.Empty)
			{
				keyTTC = oms.CreateInstance(KnownInstanceGuids.Classes.Translation);
				oms.CreateRelationship(sourceInstance, relationshipInstance, keyTTC);
			}

			InstanceHandle keyTTCValue = oms.CreateInstance(KnownInstanceGuids.Classes.TranslatableTextConstantValue);
			oms.CreateRelationship(keyTTCValue, oms.GetInstance(KnownRelationshipGuids.Translatable_Text_Constant_Value__has__Language), languageInstance);

			oms.CreateRelationship(keyTTC, oms.GetInstance(KnownRelationshipGuids.Translatable_Text_Constant__has__Translatable_Text_Constant_Value), keyTTCValue);
			oms.SetAttributeValue(keyTTCValue, oms.GetInstance(KnownAttributeGuids.Text.Value), value);
			return keyTTC;
		}

		public static InstanceHandle GetParentClassInstance(this Oms oms, InstanceHandle inst)
		{
			InstanceHandle instParent = oms.GetRelatedInstance(inst, oms.GetInstance(KnownRelationshipGuids.Instance__for__Class));
			return instParent;
		}

		/// <summary>
		/// Gets all instances of the Class instance.
		/// </summary>
		/// <returns>The class instances.</returns>
		/// <param name="oms">The <see cref="Oms" /> to query.</param>
		public static InstanceHandle[] GetClassInstances(this Oms oms)
		{
			return oms.GetRelatedInstances(oms.GetInstance(KnownInstanceGuids.Classes.Class), oms.GetInstance(KnownRelationshipGuids.Class__has_sub__Class));
		}

		public static string GetStringValue(this Oms oms, InstanceHandle[] stringComponents, InstanceHandle currentInstance)
		{
			if (stringComponents == null)
				return null;

			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < stringComponents.Length; i++)
			{
				InstanceHandle ihParent = oms.GetParentClassInstance(stringComponents[i]);
				if (oms.GetInstanceID(ihParent) == KnownInstanceGuids.Classes.ExtractSingleInstanceStringComponent)
				{
					InstanceHandle ihRelationship = oms.GetRelatedInstance(stringComponents[i], oms.GetInstance(KnownRelationshipGuids.Extract_Single_Instance_String_Component__has__Relationship));
					InstanceHandle relInst = oms.GetRelatedInstance(currentInstance, ihRelationship);
					if (relInst.IsEmpty)
					{
						// FIXME
					}
					sb.Append(GetInstanceTitle(oms, relInst));
				}
				else if (oms.GetInstanceID(ihParent) == KnownInstanceGuids.Classes.InstanceAttributeStringComponent)
				{
					InstanceHandle ihAttribute = oms.GetRelatedInstance(stringComponents[i], oms.GetInstance(KnownRelationshipGuids.Instance_Attribute_String_Component__has__Attribute));
					string value = oms.GetAttributeValue<string>(currentInstance, ihAttribute);
					sb.Append(value);
				}
				else if (oms.GetInstanceID(ihParent) == KnownInstanceGuids.Classes.TextConstantStringComponent)
				{
					string value = oms.GetAttributeValue<string>(stringComponents[i], oms.GetInstance(KnownAttributeGuids.Text.Value));
					sb.Append(value);
				}
			}
			return sb.ToString();
		}

		public static string GetInstanceTitle(this Oms oms, InstanceHandle inst)
		{
			InstanceHandle ihParentClass = oms.GetRelatedInstance(inst, oms.GetInstance(KnownRelationshipGuids.Instance__for__Class));
			if (ihParentClass.IsEmpty)
			{
				Console.WriteLine("[mocha warning]: GetInstanceTitle: parent class is empty for inst with UID {0}", oms.GetInstanceID(inst));
				Console.WriteLine("---------------- (using relationship with UID {0}, valid? {1} )", KnownRelationshipGuids.Instance__for__Class, oms.GetInstance(KnownRelationshipGuids.Instance__for__Class));
			}
			InstanceHandle ihTitleString = oms.GetRelatedInstance(ihParentClass, oms.GetInstance(KnownRelationshipGuids.Class__instance_labeled_by__String));
			InstanceHandle[] ihStringComponents = oms.GetRelatedInstances(ihTitleString, oms.GetInstance(KnownRelationshipGuids.String__has__String_Component));

			string value = oms.GetStringValue(ihStringComponents, inst);
			return value;
		}

		public static InstanceHandle CreateRelationshipInstance(this Oms oms, InstanceHandle sourceClassInstance, string name, InstanceHandle targetClassInstance, Guid globalIdentifier = default(Guid))
		{
			InstanceHandle key = InstanceHandle.Empty;
			if (globalIdentifier == default(Guid))
			{
				key = oms.CreateInstance(KnownInstanceGuids.Classes.Relationship);
			}
			else
			{
				key = oms.CreateInstance(globalIdentifier, KnownInstanceGuids.Classes.Relationship);
			}
			InstanceHandle keyAttributeName = oms.GetInstance(KnownAttributeGuids.Text.Name);

			oms.SetAttributeValue(key, keyAttributeName, name);
			oms.CreateRelationship(key, oms.GetInstance(KnownRelationshipGuids.Relationship__has_source__Class), sourceClassInstance);
			oms.CreateRelationship(key, oms.GetInstance(KnownRelationshipGuids.Relationship__has_destination__Class), targetClassInstance);

			return key;
		}

		public static string GetTranslationValue(this Oms oms, InstanceHandle sourceInstance, InstanceHandle relationshipInstance, InstanceHandle languageInstance, string defaultValue = null)
		{
			InstanceHandle keyTTC = oms.GetRelatedInstance(sourceInstance, relationshipInstance);
			InstanceHandle[] keyTTCValues = oms.GetRelatedInstances(keyTTC, oms.GetInstance(KnownRelationshipGuids.Translatable_Text_Constant__has__Translatable_Text_Constant_Value));
			for (int i = 0; i < keyTTCValues.Length; i++)
			{
				InstanceHandle ihLang = oms.GetRelatedInstance(keyTTCValues[i], oms.GetInstance(KnownRelationshipGuids.Translatable_Text_Constant_Value__has__Language));
				if (ihLang == languageInstance)
				{
					return oms.GetAttributeValue(keyTTCValues[i], oms.GetInstance(KnownAttributeGuids.Text.Value), defaultValue);
				}
			}
			return defaultValue;
		}
	}
}
