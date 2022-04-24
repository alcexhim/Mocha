//
//  MochaXMLDataFormat.cs
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
using System.Linq;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Markup;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;

namespace UniversalEditor.Plugins.Mocha.DataFormats.MochaXML
{
	public class MochaXMLDataFormat : XMLDataFormat
	{
		protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeLoadInternal(objectModels);
			objectModels.Push(new MarkupObjectModel());
		}
		protected override void AfterLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.AfterLoadInternal(objectModels);

			MarkupObjectModel mom = (objectModels.Pop() as MarkupObjectModel);
			MochaClassLibraryObjectModel mcl = (objectModels.Pop() as MochaClassLibraryObjectModel);

			MarkupTagElement tagMocha = (mom.Elements["mocha"] as MarkupTagElement);
			if (tagMocha == null) throw new InvalidDataFormatException("file does not contain top-level 'mocha' tag");

			MarkupTagElement tagLibraries = (tagMocha.Elements["libraries"] as MarkupTagElement);
			if (tagLibraries != null)
			{
				for (int i = 0; i < tagLibraries.Elements.Count; i++)
				{
					MochaLibrary library = LoadLibrary(mcl, tagLibraries.Elements[i] as MarkupTagElement);
					if (library == null) continue;

					mcl.Libraries.Merge(library);
				}
			}

			MarkupTagElement tagTenants = (tagMocha.Elements["tenants"] as MarkupTagElement);
			if (tagTenants != null)
			{
				foreach (MarkupTagElement tagTenant in tagTenants.Elements.OfType<MarkupTagElement>())
				{
					if (tagTenant == null) continue;
					if (tagTenant.FullName != "tenant") continue;

					MarkupAttribute attTenantID = tagTenant.Attributes["id"];
					MarkupAttribute attTenantName = tagTenant.Attributes["name"];

					if (attTenantID == null || attTenantName == null) continue;

					MochaTenant tenant = new MochaTenant();
					tenant.ID = new Guid(attTenantID.Value);
					tenant.Name = attTenantName.Value;

					MarkupTagElement tagLibraryReferences = tagTenant.Elements["libraryReferences"] as MarkupTagElement;
					if (tagLibraryReferences != null)
					{
						foreach (MarkupTagElement tagLibraryReference in tagLibraryReferences.Elements.OfType<MarkupTagElement>())
						{
							MarkupAttribute attLibraryId = tagLibraryReference.Attributes["libraryId"];
							if (attLibraryId == null) continue;

							tenant.LibraryReferences.Add(new MochaLibraryReference(new Guid(attLibraryId.Value)));
						}
					}
					MarkupTagElement tagInstances = tagTenant.Elements["instances"] as MarkupTagElement;
					if (tagInstances != null)
					{
						foreach (MarkupTagElement tagInstance in tagInstances.Elements.OfType<MarkupTagElement>())
						{
							MochaInstance inst = LoadInstance(tenant, tagInstance);
							tenant.Instances.Add(inst);
						}
					}

					mcl.Tenants.Add(tenant);
				}
			}
		}

		private MochaLibrary LoadLibrary(MochaClassLibraryObjectModel parent, MarkupTagElement tag)
		{
			if (tag == null) return null;
			if (tag.FullName != "library") return null;

			MarkupAttribute attGuid = tag.Attributes["id"];
			if (attGuid == null) return null;

			Guid id = new Guid(attGuid.Value);

			MochaLibrary library = parent.Libraries[id];
			if (library == null)
			{
				library = new MochaLibrary();
				library.ID = id;
			}

			MarkupAttribute attDefaultObjectSourceId = tag.Attributes["defaultObjectSourceId"];
			if (attDefaultObjectSourceId != null)
			{
				library.DefaultObjectSourceID = new Guid(attDefaultObjectSourceId.Value);
			}
			else
			{
				library.DefaultObjectSourceID = global::Mocha.Core.KnownInstanceGuids.ObjectSources.System;
			}

			MarkupTagElement tagMetadata = tag.Elements["metadata"] as MarkupTagElement;
			if (tagMetadata != null)
			{
				for (int i = 0; i < tagMetadata.Elements.Count; i++)
				{
					MarkupTagElement tagMetadataItem = tagMetadata.Elements[i] as MarkupTagElement;
					if (tagMetadataItem == null) continue;

					library.Metadata.Add(new MochaLibraryMetadata(tagMetadataItem.Name, tagMetadataItem.Value));
				}
			}

			MarkupTagElement tagInstances = tag.Elements["instances"] as MarkupTagElement;
			if (tagInstances != null)
			{
				for (int i = 0; i < tagInstances.Elements.Count; i++)
				{
					MochaInstance inst = LoadInstance(library, tagInstances.Elements[i] as MarkupTagElement);
					if (inst == null) continue;

					library.Instances.Add(inst);
				}
			}

			return library;
		}

		private MochaInstance LoadInstance(IMochaStore library, MarkupTagElement tag)
		{
			if (tag == null) return null;
			if (tag.FullName != "instance") return null;

			MarkupAttribute attID = tag.Attributes["id"];
			if (attID == null) return null;

			MarkupAttribute attIndex = tag.Attributes["index"];
			int? index = null;
			if (attIndex != null)
			{
				if (Int32.TryParse(attIndex.Value, out int index2))
					index = index2;
			}

			MarkupAttribute attClassInstanceId = tag.Attributes["classInstanceId"];
			MarkupAttribute attSuperClassId = tag.Attributes["superClassId"];

			Guid instanceId = new Guid(attID.Value);
			MochaInstance inst = library.Instances[instanceId];
			if (inst == null)
			{
				inst = new MochaInstance();
				inst.ID = instanceId;
			}
			inst.Index = index;

			if (inst.Index != null)
			{
				// SetIndex(library, inst);
			}

			if (attClassInstanceId != null)
			{
				if (Guid.TryParse(attClassInstanceId.Value, out Guid classInstanceId))
				{
					SetParentClass(library, inst.ID, classInstanceId);
				}
				else
				{
					Console.Error.WriteLine("bad guid for classInstanceId: {0}", attClassInstanceId.Value);
				}
			}
			if (attSuperClassId != null)
			{
				if (Guid.TryParse(attSuperClassId.Value, out Guid superClassId))
				{
					SetClass(library, inst.ID);
					SetOwner(library, inst.ID, global::Mocha.Core.KnownInstanceGuids.Users.XQEnvironments);
					SetSource(library, inst.ID, library.DefaultObjectSourceID); // global::Mocha.Core.KnownInstanceGuids.ObjectSources.System);
					SetSuperClass(library, inst.ID, superClassId);
				}
				else
				{
					Console.Error.WriteLine("bad guid for superClassId: {0}", attSuperClassId.Value);
				}
			}

			MarkupTagElement tagAttributeValues = tag.Elements["attributeValues"] as MarkupTagElement;
			if (tagAttributeValues != null)
			{
				for (int i = 0; i < tagAttributeValues.Elements.Count; i++)
				{
					MochaAttributeValue attv = LoadAttributeValue(tagAttributeValues.Elements[i] as MarkupTagElement);
					if (attv == null) continue;

					inst.AttributeValues.Add(attv);
				}
			}


			MarkupTagElement tagRelationships = tag.Elements["relationships"] as MarkupTagElement;
			if (tagRelationships != null)
			{
				for (int i = 0; i < tagRelationships.Elements.Count; i++)
				{
					MarkupTagElement tagRelationship = tagRelationships.Elements[i] as MarkupTagElement;
					if (tagRelationship == null) continue;
					if (tagRelationship.FullName != "relationship") continue;

					MarkupAttribute attRelationshipInstanceId = tagRelationship.Attributes["relationshipInstanceId"];
					if (attRelationshipInstanceId == null) continue;

					if (String.IsNullOrEmpty(attRelationshipInstanceId.Value))
					{
						Console.Error.WriteLine("relationshipInstanceId not specified for relationship");
						continue;
					}

					MochaRelationship rel = new MochaRelationship();
					rel.SourceInstanceID = inst.ID;

					if (Guid.TryParse(attRelationshipInstanceId.Value, out Guid id))
					{
						rel.RelationshipInstanceID = id;
					}
					else
					{
						Console.Error.WriteLine("bad guid for relationship: relationshipInstanceId='{0}'", attRelationshipInstanceId.Value);
					}

					MarkupTagElement tagTargetInstances = tagRelationship.Elements["targetInstances"] as MarkupTagElement;
					if (tagTargetInstances != null)
					{
						for (int j = 0; j < tagTargetInstances.Elements.Count; j++)
						{
							MarkupTagElement tagInstanceReference = tagTargetInstances.Elements[j] as MarkupTagElement;
							if (tagInstanceReference == null) continue;
							if (tagInstanceReference.FullName != "instanceReference") continue;

							MarkupAttribute attInstanceId = tagInstanceReference.Attributes["instanceId"];
							if (attInstanceId == null) continue;

							if (Guid.TryParse(attInstanceId.Value, out Guid instId))
							{
								rel.DestinationInstanceIDs.Add(instId);
							}
							else
							{
								Console.Error.WriteLine("bad guid for instanceReference: instanceId='{0}'", attInstanceId.Value);
							}
						}
					}

					library.Relationships.Add(rel);
				}
			}

			MarkupTagElement tagTranslations = tag.Elements["translations"] as MarkupTagElement;
			if (tagTranslations != null)
			{
				for (int i = 0; i < tagTranslations.Elements.Count; i++)
				{
					MarkupTagElement tagTranslation = (tagTranslations.Elements[i] as MarkupTagElement);
					if (tagTranslation == null) continue;
					if (tagTranslation.FullName != "translation") continue;

					MarkupAttribute attRelationshipId = tagTranslation.Attributes["relationshipInstanceId"];
					if (attRelationshipId == null) continue;

					MarkupTagElement tagTranslationValues = (tagTranslation.Elements["translationValues"] as MarkupTagElement);
					if (tagTranslationValues == null) continue;

					MochaInstance instTTC = new MochaInstance();
					instTTC.ID = Guid.NewGuid();
					SetParentClass(library, instTTC.ID, global::Mocha.Core.KnownInstanceGuids.Classes.Translation);

					MochaRelationship relInstance__has__Translatable_Text_Constant = new MochaRelationship();
					relInstance__has__Translatable_Text_Constant.SourceInstanceID = inst.ID;
					relInstance__has__Translatable_Text_Constant.RelationshipInstanceID = new Guid(attRelationshipId.Value);
					relInstance__has__Translatable_Text_Constant.DestinationInstanceIDs.Add(instTTC.ID);
					library.Relationships.Add(relInstance__has__Translatable_Text_Constant);

					MochaRelationship relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value = new MochaRelationship();
					relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value.SourceInstanceID = instTTC.ID;
					relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Translatable_Text_Constant__has__Translatable_Text_Constant_Value;

					for (int j = 0; j < tagTranslationValues.Elements.Count; j++)
					{
						MarkupTagElement tagTranslationValue = (tagTranslationValues.Elements[j] as MarkupTagElement);
						if (tagTranslationValue == null) continue;
						if (tagTranslationValue.FullName != "translationValue") continue;

						MarkupAttribute attLanguageInstanceID = tagTranslationValue.Attributes["languageInstanceId"];
						MarkupAttribute attValue = tagTranslationValue.Attributes["value"];
						if (attLanguageInstanceID == null || attValue == null) continue;

						// create a new TTCValue instance
						MochaInstance instTranslationValue = new MochaInstance();
						instTranslationValue.ID = Guid.NewGuid();
						SetParentClass(library, instTranslationValue.ID, global::Mocha.Core.KnownInstanceGuids.Classes.TranslatableTextConstantValue);

						// associate the TTCValue with the Language
						MochaRelationship relTranslatable_Text_Constant_Value__has__Language = new MochaRelationship();
						relTranslatable_Text_Constant_Value__has__Language.SourceInstanceID = instTranslationValue.ID;
						relTranslatable_Text_Constant_Value__has__Language.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Translatable_Text_Constant_Value__has__Language;
						relTranslatable_Text_Constant_Value__has__Language.DestinationInstanceIDs.Add(new Guid(attLanguageInstanceID.Value));
						library.Relationships.Add(relTranslatable_Text_Constant_Value__has__Language);

						// set the Value attribute of the TTCValue
						MochaAttributeValue mavValue = new MochaAttributeValue();
						mavValue.AttributeInstanceID = global::Mocha.Core.KnownAttributeGuids.Text.Value;
						mavValue.Value = attValue.Value;
						instTranslationValue.AttributeValues.Add(mavValue);

						// add the TTCValue to the instance list
						library.Instances.Add(instTranslationValue);

						// add the TTCValue to the TTC.has TTC Value relationship
						relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value.DestinationInstanceIDs.Add(instTranslationValue.ID);
					}

					library.Relationships.Add(relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value);

					library.Instances.Add(instTTC);
				}
			}

			return inst;
		}

		private void SetIndex(IMochaStore library, MochaInstance instance)
		{
			if (instance.Index == null)
				return;

			MochaAttributeValue mav = new MochaAttributeValue();
			mav.AttributeInstanceID = global::Mocha.Core.KnownAttributeGuids.Numeric.Index;
			mav.Value = instance.Index.Value;
			instance.AttributeValues.Add(mav);
		}

		private void SetSource(IMochaStore library, Guid instanceId, Guid sourceTypeId)
		{
			MochaRelationship relClass__has__Instance = new MochaRelationship();
			relClass__has__Instance.SourceInstanceID = instanceId;
			relClass__has__Instance.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Class__has__Object_Source;
			relClass__has__Instance.DestinationInstanceIDs.Add(sourceTypeId);
			library.Relationships.Add(relClass__has__Instance);

			MochaRelationship relInstance__for__Class = new MochaRelationship();
			relInstance__for__Class.SourceInstanceID = sourceTypeId;
			relInstance__for__Class.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Object_Source__for__Class;
			relInstance__for__Class.DestinationInstanceIDs.Add(instanceId);
			library.Relationships.Add(relInstance__for__Class);
		}

		private void SetOwner(IMochaStore library, Guid instanceId, Guid ownerId)
		{
			MochaRelationship relClass__has__Instance = new MochaRelationship();
			relClass__has__Instance.SourceInstanceID = instanceId;
			relClass__has__Instance.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Class__has_owner__User;
			relClass__has__Instance.DestinationInstanceIDs.Add(ownerId);
			library.Relationships.Add(relClass__has__Instance);

			MochaRelationship relInstance__for__Class = new MochaRelationship();
			relInstance__for__Class.SourceInstanceID = ownerId;
			relInstance__for__Class.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.User__owner_for__Class;
			relInstance__for__Class.DestinationInstanceIDs.Add(instanceId);
			library.Relationships.Add(relInstance__for__Class);
		}

		private void SetClass(IMochaStore library, Guid id)
		{
			MochaRelationship relClass__has__Instance = new MochaRelationship();
			relClass__has__Instance.SourceInstanceID = global::Mocha.Core.KnownInstanceGuids.Classes.Class;
			relClass__has__Instance.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Class__has__Instance;
			relClass__has__Instance.DestinationInstanceIDs.Add(id);
			library.Relationships.Add(relClass__has__Instance);

			MochaRelationship relInstance__for__Class = new MochaRelationship();
			relInstance__for__Class.SourceInstanceID = id;
			relInstance__for__Class.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Instance__for__Class;
			relInstance__for__Class.DestinationInstanceIDs.Add(global::Mocha.Core.KnownInstanceGuids.Classes.Class);
			library.Relationships.Add(relInstance__for__Class);
		}

		private void SetSuperClass(IMochaStore library, Guid sourceInstanceID, Guid parentClassID)
		{
			MochaRelationship relHasSuperClass = new MochaRelationship();
			relHasSuperClass.SourceInstanceID = sourceInstanceID;
			relHasSuperClass.DestinationInstanceIDs.Add(parentClassID);
			relHasSuperClass.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Class__has_super__Class;
			library.Relationships.Add(relHasSuperClass);

			MochaRelationship relHasSubClass = new MochaRelationship();
			relHasSubClass.SourceInstanceID = parentClassID;
			relHasSubClass.DestinationInstanceIDs.Add(sourceInstanceID);
			relHasSubClass.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Class__has_sub__Class;
			library.Relationships.Add(relHasSubClass);
		}
		private void SetParentClass(IMochaStore library, Guid sourceInstanceID, Guid parentClassID)
		{
			MochaRelationship relHasSuperClass = new MochaRelationship();
			relHasSuperClass.SourceInstanceID = sourceInstanceID;
			relHasSuperClass.DestinationInstanceIDs.Add(parentClassID);
			relHasSuperClass.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Instance__for__Class;
			library.Relationships.Add(relHasSuperClass);

			MochaRelationship relHasSubClass = new MochaRelationship();
			relHasSubClass.SourceInstanceID = parentClassID;
			relHasSubClass.DestinationInstanceIDs.Add(sourceInstanceID);
			relHasSubClass.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Class__has__Instance;
			library.Relationships.Add(relHasSubClass);
		}

		private MochaAttributeValue LoadAttributeValue(MarkupTagElement tag)
		{
			if (tag == null) return null;
			if (tag.FullName != "attributeValue") return null;

			MarkupAttribute attInstanceID = tag.Attributes["attributeInstanceId"];
			MarkupAttribute attValue = tag.Attributes["value"];

			if (attInstanceID == null || attValue == null)
				return null;

			MochaAttributeValue item = new MochaAttributeValue();
			item.AttributeInstanceID = new Guid(attInstanceID.Value);
			item.Value = attValue.Value;
			return item;
		}

		protected override void BeforeSaveInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeSaveInternal(objectModels);

			MochaClassLibraryObjectModel mcl = (objectModels.Pop() as MochaClassLibraryObjectModel);
			MarkupObjectModel mom = new MarkupObjectModel();

			objectModels.Push(mom);
		}
	}
}
