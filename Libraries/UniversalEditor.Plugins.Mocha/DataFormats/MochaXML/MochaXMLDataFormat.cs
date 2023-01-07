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
using MBS.Framework;
using Mocha.Core;
using UniversalEditor.DataFormats.Markup.XML;
using UniversalEditor.ObjectModels.Markup;
using UniversalEditor.Plugins.Mocha.ObjectModels.MochaClassLibrary;

namespace UniversalEditor.Plugins.Mocha.DataFormats.MochaXML
{
	public class MochaXMLDataFormat : XMLDataFormat
	{
		public MochaXMLDataFormat()
		{
			/*
			this.Settings.Entities.Add("IDC_Class", KnownInstanceGuids.Classes.Class.ToString("B"));
			this.Settings.Entities.Add("IDC_Attribute", "{F9CD7751-EF62-4F7C-8A28-EBE90B8F46AA}");
			this.Settings.Entities.Add("IDC_BooleanAttribute", "{EA830448-A403-4ED9-A3D3-048D5D5C3A03}");
			this.Settings.Entities.Add("IDC_NumericAttribute", "{9DE86AF1-EFD6-4B71-9DCC-202F247C94CB}");
			this.Settings.Entities.Add("IDC_TextAttribute", "{C2F36542-60C3-4B9E-9A96-CA9B309C43AF}");
			this.Settings.Entities.Add("IDC_DateAttribute", "{0B7B1812-DFB4-4F25-BF6D-CEB0E1DF8744}");
			this.Settings.Entities.Add("IDC_Relationship", "{9B0A80F9-C325-4D36-997C-FB4106204648}");
			this.Settings.Entities.Add("IDC_Instance", "{263C4882-945F-4DE9-AED8-E0D6516D4903}");
			this.Settings.Entities.Add("IDC_InstanceSet", KnownInstanceGuids.Classes.InstanceSet.ToString("B"));
			this.Settings.Entities.Add("IDC_InstanceDefinition", "{ee26f146-0b89-4cfe-a1af-ae6ac3533eae}");
			this.Settings.Entities.Add("IDC_String", "");
			this.Settings.Entities.Add("IDC_Module", "");
			this.Settings.Entities.Add("IDC_Translation", "{04A53CC8-3206-4A97-99C5-464DB8CAA6E6}");
			this.Settings.Entities.Add("IDC_TranslationValue", "{6D38E757-EC18-43AD-9C35-D15BB446C0E1}");
			this.Settings.Entities.Add("IDC_Enumeration", "");
			this.Settings.Entities.Add("IDC_StringComponent", "{F9E2B671-13F5-4172-A568-725ACD8BBFAB}");
			this.Settings.Entities.Add("IDC_ExtractSingleInstanceStringComponent", "{FCECCE4E-8D05-485A-AE34-B1B45E766661}");
			this.Settings.Entities.Add("IDC_InstanceAttributeStringComponent", "{623565D5-5AEE-49ED-A5A9-0CFE670507BC}");

			this.Settings.Entities.Add("IDC_User", "{9C6871C1-9A7F-4A3A-900E-69D1D9E24486}");
			this.Settings.Entities.Add("IDC_Report", KnownInstanceGuids.Classes.Report.ToString("B"));
			this.Settings.Entities.Add("IDC_ReportColumn", KnownInstanceGuids.Classes.ReportColumn.ToString("B"));

			this.Settings.Entities.Add("IDC_Page", "{D9626359-48E3-4840-A089-CD8DA6731690}");
			this.Settings.Entities.Add("IDC_HeadingPageComponent", "{FD86551E-E4CE-4B8B-95CB-BEC1E6A0EE2B}");

			this.Settings.Entities.Add("IDC_Element", KnownInstanceGuids.Classes.Element.ToString("B"));
			this.Settings.Entities.Add("IDC_ElementContent", KnownInstanceGuids.Classes.ElementContent.ToString("B"));
			this.Settings.Entities.Add("IDC_ElementContentDisplayOption", "	");

			this.Settings.Entities.Add("IDC_Event", KnownInstanceGuids.Classes.Event.ToString("B"));

			this.Settings.Entities.Add("IDC_WorkData", KnownInstanceGuids.Classes.WorkData.ToString("B"));
			this.Settings.Entities.Add("IDC_WorkSet", KnownInstanceGuids.Classes.WorkSet.ToString("B"));

			this.Settings.Entities.Add("IDC_AccessModifier", "");
			this.Settings.Entities.Add("IDC_Method", KnownInstanceGuids.Classes.Method.ToString("B"));
			this.Settings.Entities.Add("IDC_CalculateDateMethod", KnownInstanceGuids.Classes.CalculateDateMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_BuildAttributeMethod", KnownInstanceGuids.Classes.BuildAttributeMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_GetAttributeMethod", KnownInstanceGuids.Classes.GetAttributeMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_SelectFromInstanceSetMethod", KnownInstanceGuids.Classes.SelectFromInstanceSetMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_GetReferencedAttributeMethod", KnownInstanceGuids.Classes.GetReferencedAttributeMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_InstanceOpMethod", KnownInstanceGuids.Classes.InstanceOpMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_GetSpecifiedInstancesMethod", KnownInstanceGuids.Classes.GetSpecificInstancesMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_GetReferencedInstanceSetMethod", KnownInstanceGuids.Classes.GetReferencedInstanceSetMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_EvaluateBooleanExpressionMethod", "");
			this.Settings.Entities.Add("IDC_GetRelationshipMethod", "{d53c9232-89ef-4cca-8520-261da6787450}");
			this.Settings.Entities.Add("IDC_ConditionalSelectAttributeMethod", KnownInstanceGuids.Classes.ConditionalSelectAttributeMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_GetInstanceSetBySystemRoutineMethod", KnownInstanceGuids.Classes.GetInstanceSetBySystemRoutineMethod.ToString("B"));
			this.Settings.Entities.Add("IDC_ConditionalMethodCase", KnownInstanceGuids.Classes.ConditionalMethodCase.ToString("B"));

			this.Settings.Entities.Add("IDC_Executable", KnownInstanceGuids.Classes.Executable.ToString("B"));
			this.Settings.Entities.Add("IDC_Parameter", global::Mocha.Core.KnownInstanceGuids.Classes.Parameter.ToString("B"));
			this.Settings.Entities.Add("IDC_ReportObject", global::Mocha.Core.KnownInstanceGuids.Classes.ReportObject.ToString("B"));

			this.Settings.Entities.Add("IDC_Validation", KnownInstanceGuids.Classes.Validation.ToString("B"));
			this.Settings.Entities.Add("IDC_ValidationClassification", KnownInstanceGuids.Classes.ValidationClassification.ToString("B"));

			this.Settings.Entities.Add("IDC_MethodBinding", "{CB36098E-B9BF-4D22-87FA-4186EC632C89}");
			this.Settings.Entities.Add("IDC_ParameterAssignment", "");

			this.Settings.Entities.Add("IDC_ReturnAttributeMethodBinding", "{30FB6BA6-2BBB-41D2-B91A-709C00A07790}");
			this.Settings.Entities.Add("IDC_ReturnInstanceSetMethodBinding", "{AADC20F9-7559-429B-AEF0-97E059295C76}");

			this.Settings.Entities.Add("IDC_PageComponent_TabContainerPageComponent", "");
			this.Settings.Entities.Add("IDC_DetailPageComponent", "");

			// FIXME: these should be `Report Field`.has source Method `GA - Get Attribute Method...` or `GSI - Get Specific Instance`... etc.
			this.Settings.Entities.Add("IDC_ReportField", "{655A04D9-FE35-4F89-9AAB-B8FA34989D03}");
			this.Settings.Entities.Add("IDC_AttributeReportField", "{C06E0461-A956-4599-9708-012C8FE04D94}");
			this.Settings.Entities.Add("IDC_RelationshipReportField", "{FC4E3BB5-1EA7-44FF-B828-2EA54CDD4ABB}");
			this.Settings.Entities.Add("IDC_StandardReport", "{FDF4A498-DE83-417D-BA01-707372125C8D}");

			this.Settings.Entities.Add("IDC_Tenant", "{703F9D65-C584-4D9F-A656-D0E3C247FF1F}");
			this.Settings.Entities.Add("IDC_AuditLine", "");

			this.Settings.Entities.Add("IDC_Dashboard", KnownInstanceGuids.Classes.Dashboard.ToString("B"));
			this.Settings.Entities.Add("IDC_DashboardContent", KnownInstanceGuids.Classes.DashboardContent.ToString("B"));
			this.Settings.Entities.Add("IDC_Theme", KnownInstanceGuids.Classes.Theme.ToString("B"));
			*/

			this.Settings.Entities.Add("IDA_Name", "{9153A637-992E-4712-ADF2-B03F0D9EDEA6}");
			this.Settings.Entities.Add("IDA_Singular", "{F1A06573-C447-4D85-B4E7-54A438C4A960}");
			this.Settings.Entities.Add("IDA_Abstract", "{A1C0255D-3E8D-47D7-A186-1C93C93D4667}");
			this.Settings.Entities.Add("IDA_Value", "{041DD7FD-2D9C-412B-8B9D-D7125C166FE0}");
			this.Settings.Entities.Add("IDA_DateAndTime", "{ea71cc92-a5e9-49c1-b487-8ad178b557d2}");
			this.Settings.Entities.Add("IDA_Order", "{49423f66-8837-430d-8cac-7892ebdcb1fe}");
			this.Settings.Entities.Add("IDA_Abbreviation", "{7ee78e44-e894-41a0-996b-d0f33c76e23b}");
			this.Settings.Entities.Add("IDA_StartDate", "{F23CD242-64DB-4115-AB57-E9C70B0A38DC}");
			this.Settings.Entities.Add("IDA_EndDate", "{B3AED59A-1977-4CE4-A2F9-891A1CACDEB8}");
			this.Settings.Entities.Add("IDA_Static", "{9A3A0719-64C2-484F-A55E-22CD4597D9FD}");
			this.Settings.Entities.Add("IDA_Verb", "{61345a5d-3397-4a96-8797-8863f03a476c}");
			this.Settings.Entities.Add("IDA_Final", "{4028ec9a-7d7b-4bfb-8883-e450e797ca0e}");
			this.Settings.Entities.Add("IDA_Required", KnownAttributeGuids.Boolean.Required.ToString("B"));
			this.Settings.Entities.Add("IDA_Null", KnownAttributeGuids.Boolean.Null.ToString("B"));
			this.Settings.Entities.Add("IDA_Visible", KnownAttributeGuids.Boolean.Visible.ToString("B"));
			this.Settings.Entities.Add("IDA_UseAnyCondition", KnownAttributeGuids.Boolean.UseAnyCondition.ToString("B"));
			this.Settings.Entities.Add("IDA_ValidateOnlyOnSubmit", KnownAttributeGuids.Boolean.ValidateOnlyOnSubmit.ToString("B"));

			// class GUID loader
			// first load top-level properties
			Type[] types = typeof(KnownInstanceGuids).Assembly.GetTypes();
			foreach (Type t in types)
			{
				object[] attrs = t.GetCustomAttributes(typeof(ExportEntitiesAttribute), false);
				if (attrs.Length > 0)
				{
					ExportEntitiesAttribute attr = (ExportEntitiesAttribute)attrs[0];
					if (attr.Prefix != null || attr.Suffix != null)
					{
						InitGuids(t, attr.Prefix, attr.Suffix, false);
					}
				}
			}

			// InitGuids(typeof(KnownInstanceGuids.Classes), "IDC");
			// InitGuids(typeof(KnownInstanceGuids.MethodClasses), "IDC");

			// InitGuids(typeof(KnownRelationshipGuids), "IDR", false);

			/*

			// {085bd706-eece-4604-ac04-b7af114d1d21}

			// 6fb6534c-2a46-4d6d-b9df-fd581f19efed
			*/		

			this.Settings.Entities.Add("IDI_Module_MochaBaseSystem", "{3ffd3a31-208c-49c9-905d-2a69362902ca}");

			this.Settings.Entities.Add("IDI_Language_English", "{68BB6038-A4B5-4EE1-AAE9-326494942062}");
			this.Settings.Entities.Add("IDI_Language_Spanish", "{6dc357cb-37c3-43ed-ae13-6259fb109213}");
			this.Settings.Entities.Add("IDI_Language_French", "{6bf0cf09-87c9-4e21-b360-7eb5a1c279de}");
			this.Settings.Entities.Add("IDI_Language_German", "{c7c1d740-0d3c-493f-ab0b-fe1b42546d0a}");
			this.Settings.Entities.Add("IDI_Language_Italian", "{cf165170-0680-4a41-8f88-88f34b2b1986}");
			this.Settings.Entities.Add("IDI_Language_Chinese", "{6f908a9b-7464-4a16-aed9-7eccb8d39032}");
			this.Settings.Entities.Add("IDI_Language_Japanese", "{1e16de9d-0e49-4a79-b690-4905c46a94cc}");
			this.Settings.Entities.Add("IDI_Language_Korean", "{d03a795e-906b-49ee-87ea-c1bef4b8ee9a}");
		}

		private void InitGuids(Type type, string prefix, string suffix, bool nested)
		{
			Console.Error.WriteLine("debug: loading type {0}", type);
			System.Reflection.PropertyInfo[] pis = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
			foreach (System.Reflection.PropertyInfo pi in pis)
			{
				if (pi.PropertyType == typeof(Guid))
				{
					Guid value = (Guid)pi.GetValue(null, null);

					string name = String.Format("{0}{1}{2}", prefix, pi.Name, suffix);
					Console.Error.WriteLine("debug: mapped entity &{0}; to {1}", name, value.ToString("B"));
					this.Settings.Entities.Add(name, value.ToString("B"));
				}
			}

			if (nested)
			{
				Type[] subtypes = type.GetNestedTypes();
				foreach (Type subtype in subtypes)
				{
					InitGuids(subtype, prefix, suffix, nested);
				}
			}
		}

		protected override void BeforeLoadInternal(Stack<ObjectModel> objectModels)
		{
			base.BeforeLoadInternal(objectModels);
			objectModels.Push(new MarkupObjectModel() { Accessor = this.Accessor });
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

					if (attTenantID == null) continue;

					Guid id = new Guid(attTenantID.Value);
					MochaTenant tenant = mcl.Tenants[id];
					if (tenant == null)
					{
						tenant = new MochaTenant();
						tenant.ID = id;
						mcl.Tenants.Add(tenant);
					}
					if (tenant.Name == null && attTenantName != null)
					{
						tenant.Name = attTenantName.Value;
					}

					MarkupTagElement tagLibraryReferences = tagTenant.Elements["libraryReferences"] as MarkupTagElement;
					if (tagLibraryReferences != null)
					{
						foreach (MarkupTagElement tagLibraryReference in tagLibraryReferences.Elements.OfType<MarkupTagElement>())
						{
							MarkupAttribute attLibraryId = tagLibraryReference.Attributes["libraryId"];
							if (attLibraryId == null) continue;

							tenant.LibraryReferences.Add(new Guid(attLibraryId.Value));
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
				}
			}
		}

		private MochaInstance ZqCompileMethod(IMochaStore library, MarkupTagElement tag)
		{
			MochaInstance inst = null;
			MarkupAttribute attID = tag.Attributes["id"];
			if (attID == null)
				return null;

			Guid id = new Guid(attID.Value);

			Console.Error.WriteLine("zq: compiling method {0}", id);

			MarkupAttribute attType = tag.Attributes["type"];
			MarkupAttribute attName = tag.Attributes["name"];
			MarkupAttribute attVerb = tag.Attributes["verb"];

			if (attName == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "Missing 'name' attribute for method definition", "MCX0101", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber, "Method Compiler");
			}
			if (attVerb == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "Missing 'verb' attribute for method definition", "MCX0101", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber, "Method Compiler");
			}

			if (attType != null)
			{
				switch (attType.Value)
				{
					case "getAttribute":
					{
						inst = ZqCreateInstance(library, id, global::Mocha.Core.KnownInstanceGuids.MethodClasses.GetAttributeMethod);
						break;
					}
					case "getInstanceSetBySystemRoutine":
					{
						inst = ZqCreateInstance(library, id, global::Mocha.Core.KnownInstanceGuids.MethodClasses.GetInstanceSetBySystemRoutineMethod);
						break;
					}
					case "conditionalSelectAttribute":
					{
						inst = ZqCreateInstance(library, id, global::Mocha.Core.KnownInstanceGuids.MethodClasses.ConditionalSelectAttributeMethod);

						if (!(tag.Elements["cases"] is MarkupTagElement tagCases))
						{
							ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "Missing 'cases' element for SAC - Conditional Select Attribute Method", "MCX0101", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber, "Method Compiler");
						}
						else
						{
							foreach (MarkupElement elCase in tagCases.Elements)
							{
								MarkupTagElement tagCase = (elCase as MarkupTagElement);
								if (tagCase == null) continue;
								if (tagCase.FullName != "case") continue;

								Guid caseId;
								MarkupAttribute attCaseId = tagCase.Attributes["id"];
								if (attCaseId != null)
								{
									caseId = Guid.Parse(attCaseId.Value);
								}

								MochaInstance instCase = ZqCreateInstance(library, caseId, KnownInstanceGuids.Classes.ConditionalEvaluationCase);

								MarkupAttribute attUseAnyCondition = tagCase.Attributes["useAnyCondition"];
								bool useAnyCondition = false;
								if (attUseAnyCondition != null)
								{
									useAnyCondition = Boolean.Parse(attUseAnyCondition.Value);
								}

								MarkupAttribute attReturnAttributeMethodBindingId = tagCase.Attributes["returnAttributeMethodBindingId"];

								MarkupTagElement tagTrueConditions = tagCase.Elements["trueConditions"] as MarkupTagElement;
								MarkupTagElement tagFalseConditions = tagCase.Elements["falseConditions"] as MarkupTagElement;

								ZqAssignAttribute(instCase, KnownAttributeGuids.Boolean.UseAnyCondition, useAnyCondition);
								if (attReturnAttributeMethodBindingId != null)
								{
									Guid returnAttributeMethodId = Guid.Parse(attReturnAttributeMethodBindingId.Value);
									ZqAssignRelationship(library, instCase.ID, KnownRelationshipGuids.Conditional_Method_Case__has_return_attribute__Method_Binding, new Guid[] { returnAttributeMethodId });
								}

								List<Guid> listTrueConditionsGuids = new List<Guid>();
								List<Guid> listFalseConditionsGuids = new List<Guid>();

								if (tagTrueConditions != null)
								{
									for (int j = 0; j < tagTrueConditions.Elements.Count; j++)
									{
										MarkupTagElement tagInstanceReference = tagTrueConditions.Elements[j] as MarkupTagElement;
										if (tagInstanceReference == null) continue;
										if (tagInstanceReference.FullName != "instanceReference") continue;

										MarkupAttribute attInstanceId = tagInstanceReference.Attributes["instanceId"];
										if (attInstanceId == null)
										{
											ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "instance ID not specified for relationship target instance", "MCX0107", tag.ParentObjectModel.Accessor.GetFileName(), tagInstanceReference.Definition.LineNumber, tagInstanceReference.Definition.ColumnNumber);
											continue;
										}

										if (Guid.TryParse(attInstanceId.Value, out Guid instId))
										{
											listTrueConditionsGuids.Add(instId);
										}
									}
								}
								if (tagFalseConditions != null)
								{
									for (int j = 0; j < tagFalseConditions.Elements.Count; j++)
									{
										MarkupTagElement tagInstanceReference = tagFalseConditions.Elements[j] as MarkupTagElement;
										if (tagInstanceReference == null) continue;
										if (tagInstanceReference.FullName != "instanceReference") continue;

										MarkupAttribute attInstanceId = tagInstanceReference.Attributes["instanceId"];
										if (attInstanceId == null)
										{
											ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "instance ID not specified for relationship target instance", "MCX0107", tag.ParentObjectModel.Accessor.GetFileName(), tagInstanceReference.Definition.LineNumber, tagInstanceReference.Definition.ColumnNumber);
											continue;
										}

										if (Guid.TryParse(attInstanceId.Value, out Guid instId))
										{
											listFalseConditionsGuids.Add(instId);
										}
									}
								}

								ZqAssignRelationship(library, instCase.ID, KnownRelationshipGuids.Conditional_Method_Case__has_true_condition__Method_Binding, listTrueConditionsGuids.ToArray());
								ZqAssignRelationship(library, instCase.ID, KnownRelationshipGuids.Conditional_Method_Case__has_false_condition__Method_Binding, listFalseConditionsGuids.ToArray());

								ZqAssignRelationship(library, inst.ID, KnownRelationshipGuids.Conditional_Method__has__Conditional_Method_Case, new Guid[] { instCase.ID });
								ZqAssignRelationship(library, instCase.ID, KnownRelationshipGuids.Conditional_Method_Case__for__Conditional_Method, new Guid[] { inst.ID });
							}
						}
						break;
					}
					default:
					{
						Console.Error.WriteLine("unknown ZQ method type {0}; ignoring", attType.Value);
						break;
					}
				}
				if (inst != null)
				{
					if (attName != null)
					{
						ZqAssignAttribute(inst, global::Mocha.Core.KnownAttributeGuids.Text.Name, attName.Value);
					}
					if (attVerb != null)
					{
						ZqAssignAttribute(inst, global::Mocha.Core.KnownAttributeGuids.Text.Verb, attVerb.Value);
					}

					SetupInstanceWithXML(library, inst, tag);
				}
			}

			return inst;
		}

		private void ZqAssignAttribute(MochaInstance inst, Guid attributeInstanceId, object value)
		{
			inst.AttributeValues.Add(new MochaAttributeValue() { AttributeInstanceID = attributeInstanceId, Value = value });
		}

		private MochaInstance ZqCompileRelationship(IMochaStore library, MarkupTagElement tag)
		{
			MarkupAttribute attID = tag.Attributes["id"];
			Guid instanceId = new Guid(attID.Value);
			Console.Error.WriteLine("zq: compiling relationship {0}", instanceId);

			MochaInstance inst = ZqCreateInstance(library, instanceId);

			MarkupAttribute attSourceClassId = tag.Attributes["sourceClassId"];
			MarkupAttribute attType = tag.Attributes["type"];
			MarkupAttribute attDestinationClassId = tag.Attributes["destinationClassId"];
			MarkupAttribute attSiblingRelationshipId = tag.Attributes["siblingRelationshipId"];
			MarkupAttribute attSingular = tag.Attributes["singular"];

			bool singular = false;
			if (attSingular != null)
			{
				if (!Boolean.TryParse(attSingular.Value, out singular))
				{
					return null;
				}
				ZqAssignAttribute(inst, global::Mocha.Core.KnownAttributeGuids.Boolean.Singular, singular);
			}
			if (attType != null)
			{
				ZqAssignAttribute(inst, global::Mocha.Core.KnownAttributeGuids.Text.RelationshipType, attType.Value);
			}

			Guid sourceClassId, destinationClassId, siblingRelationshipId;
			if (attSourceClassId != null)
			{
				if (Guid.TryParse(attSourceClassId.Value, out sourceClassId))
				{
					ZqAssignRelationship(library, instanceId, global::Mocha.Core.KnownRelationshipGuids.Relationship__has_source__Class, new Guid[] { sourceClassId });
				}
				else
				{

				}
			}
			if (attDestinationClassId != null)
			{
				if (Guid.TryParse(attDestinationClassId.Value, out destinationClassId))
				{
					ZqAssignRelationship(library, instanceId, global::Mocha.Core.KnownRelationshipGuids.Relationship__has_destination__Class, new Guid[] { destinationClassId });
				}
				else
				{

				}
			}

			if (attSiblingRelationshipId != null)
			{
				if (Guid.TryParse(attSiblingRelationshipId.Value, out siblingRelationshipId))
				{
					ZqAssignRelationship(library, instanceId, global::Mocha.Core.KnownRelationshipGuids.Relationship__has_sibling__Relationship, new Guid[] { siblingRelationshipId });
				}
				else
				{

				}
			}

			library.Relationships.Add(new MochaRelationship() { SourceInstanceID = sourceClassId, RelationshipInstanceID = KnownRelationshipGuids.Class__has__Relationship, DestinationInstanceIDs = new List<Guid>(new Guid[] { inst.ID }) });

			SetParentClass(library, instanceId, global::Mocha.Core.KnownInstanceGuids.Classes.Relationship);
			return inst;
		}

		private MochaInstance ZqCompileClass(IMochaStore library, MarkupTagElement tag)
		{
			MarkupAttribute attID = tag.Attributes["id"];
			Guid instanceId = new Guid(attID.Value);
			Console.Error.WriteLine("zq: compiling class {0}", instanceId);

			MochaInstance inst = ZqCreateInstance(library, instanceId);

			MarkupTagElement tagMethods = tag.Elements["methods"] as MarkupTagElement;
			if (tagMethods != null)
			{
				foreach (MarkupElement elMethod in tagMethods.Elements)
				{
					MochaInstance instMethod = ZqCompileMethod(library, elMethod as MarkupTagElement);
					if (instMethod != null)
					{
						ZqAssignRelationship(library, inst.ID, global::Mocha.Core.KnownRelationshipGuids.Class__has__Method, new Guid[] { instMethod.ID });
						ZqAssignRelationship(library, instMethod.ID, global::Mocha.Core.KnownRelationshipGuids.Method__for__Class, new Guid[] { inst.ID });
					}
				}
			}

			SetParentClass(library, instanceId, global::Mocha.Core.KnownInstanceGuids.Classes.Class);

			MarkupAttribute attSuperClassId = tag.Attributes["superClassId"];
			if (attSuperClassId != null)
			{
				if (Guid.TryParse(tag.Attributes["superClassId"].Value, out Guid superClassId))
				{
					SetSuperClass(library, instanceId, superClassId);
				}
			}
			else
			{
				SetSuperClass(library, instanceId, global::Mocha.Core.KnownInstanceGuids.Classes.Class);
			}

			SetOwner(library, inst.ID, global::Mocha.Core.KnownInstanceGuids.Users.XQEnvironments);
			SetSource(library, inst.ID, library.DefaultObjectSourceID); // global::Mocha.Core.KnownInstanceGuids.ObjectSources.System);

			SetupInstanceWithXML(library, inst, tag);
			return inst;
		}

		private MochaInstance ZqCreateInstance(IMochaStore library, Guid instanceId, Guid classInstanceId = default(Guid))
		{
			MochaInstance inst = library.Instances[instanceId];
			if (inst == null)
			{
				inst = new MochaInstance();
				inst.ID = instanceId;
				library.Instances.Add(inst);
			}
			if (classInstanceId != default(Guid))
			{
				SetParentClass(library, instanceId, classInstanceId);
			}
			return inst;
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

			MarkupTagElement tagLibraryReferences = tag.Elements["libraryReferences"] as MarkupTagElement;
			if (tagLibraryReferences != null)
			{
				foreach (MarkupTagElement tagLibraryReference in tagLibraryReferences.Elements.OfType<MarkupTagElement>())
				{
					MarkupAttribute attLibraryId = tagLibraryReference.Attributes["libraryId"];
					if (attLibraryId == null) continue;

					library.LibraryReferences.Add(new Guid(attLibraryId.Value));
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

			MarkupAttribute attID = tag.Attributes["id"];
			string strInstanceId = attID?.Value;
			if (strInstanceId == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "Missing GUID for instance declaration", "MCX0101", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber, "Instance Loader");
				return null;
			}

			Guid instanceId;
			if (!Guid.TryParse(strInstanceId, out instanceId))
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("Invalid GUID for instance declaration '{0}'", strInstanceId), "MCX0101", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber, "Instance Loader");
				return null;
			}

			MochaInstance inst = library.Instances[instanceId];

			if (tag.FullName == "class")
			{
				inst = ZqCompileClass(library, tag);
				return inst;
			}
			else if (tag.FullName == "method")
			{
				inst = ZqCompileMethod(library, tag);
				return inst;
			}
			else if (tag.FullName == "relationship")
			{
				inst = ZqCompileRelationship(library, tag);
				return inst;
			}
			else if (tag.FullName == "instanceSet")
			{
				inst = ZqCompileInstanceSet(library, tag);
				return inst;
			}

			if (inst == null)
			{
				inst = new MochaInstance();
				inst.ID = instanceId;
			}

			if (tag.FullName != "instance")
			{
				Console.Error.WriteLine("unhandled preprocess instance tag type '{0}'", tag.FullName);
				return null;
			}
			MarkupAttribute attIndex = tag.Attributes["index"];
			int? index = null;
			if (attIndex != null)
			{
				if (Int32.TryParse(attIndex.Value, out int index2))
					index = index2;
			}

			MarkupAttribute attClassInstanceId = tag.Attributes["classInstanceId"];
			MarkupAttribute attSuperClassId = tag.Attributes["superClassId"];

			inst.Index = index;

			if (inst.Index != null)
			{
				SetIndex(library, inst);
			}

			if (attClassInstanceId != null)
			{
				if (Guid.TryParse(attClassInstanceId.Value, out Guid classInstanceId))
				{
					SetParentClass(library, inst.ID, classInstanceId);
				}
				else
				{
					ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("Bad GUID for classInstanceId: {0}", attClassInstanceId.Value), "MCX0102", tag.ParentObjectModel.Accessor.GetFileName(), attClassInstanceId.Definition.LineNumber, attClassInstanceId.Definition.ColumnNumber);
				}
			}
			if (attSuperClassId != null)
			{
				if (Guid.TryParse(attSuperClassId.Value, out Guid superClassId))
				{
					Console.Error.WriteLine("creating class {0} inherits {1}", instanceId, superClassId);
					SetClass(library, inst.ID);
					SetOwner(library, inst.ID, global::Mocha.Core.KnownInstanceGuids.Users.XQEnvironments);
					SetSource(library, inst.ID, library.DefaultObjectSourceID); // global::Mocha.Core.KnownInstanceGuids.ObjectSources.System);
					SetSuperClass(library, inst.ID, superClassId);
				}
				else
				{
					ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("Bad GUID for superClassId '{0}'", attSuperClassId.Value), "MCX0902", tag.ParentObjectModel.Accessor.GetFileName(), attSuperClassId.Definition.LineNumber, attSuperClassId.Definition.ColumnNumber);
				}
			}
			if (attClassInstanceId == null && attSuperClassId == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, String.Format("neither classInstanceId nor superClassId specified for instance: {0}", inst.ID), "MCX0302", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber);
			}

			SetupInstanceWithXML(library, inst, tag);
			return inst;
		}

		private MochaInstance ZqCompileInstanceSet(IMochaStore library, MarkupTagElement tag)
		{
			MarkupAttribute attID = tag.Attributes["id"];
			Guid instanceId = new Guid(attID.Value);
			MochaInstance instSet = ZqCreateInstance(library, instanceId, KnownInstanceGuids.Classes.InstanceSet);

			MarkupTagElement tagInstances = (tag.Elements["instances"] as MarkupTagElement);
			if (tagInstances != null)
			{
				MochaRelationship relInstanceSetHasInstance = new MochaRelationship() { SourceInstanceID = instSet.ID, RelationshipInstanceID = KnownRelationshipGuids.Instance_Set__has__Instance };
				foreach (MarkupElement el in tagInstances.Elements)
				{
					MarkupTagElement tag1 = (el as MarkupTagElement);
					if (tag1 == null) continue;
					if (tag1.FullName != "instanceReference") continue;

					MarkupAttribute attInstRefId = tag1.Attributes["instanceId"];
					if (attInstRefId == null) continue;
					if (Guid.TryParse(attInstRefId.Value, out Guid instRefId))
					{
						relInstanceSetHasInstance.DestinationInstanceIDs.Add(instRefId);
					}
				}
				library.Relationships.Add(relInstanceSetHasInstance);
			}
			return instSet;
		}

		public bool WriteDebugInfo { get; set; } = true;

		private void SetupInstanceWithXML(IMochaStore library, MochaInstance inst, MarkupTagElement tag)
		{
			if (WriteDebugInfo)
			{
				SetupInstanceWithXML__DebugInfo(library, inst, tag);
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
			if (tagRelationships != null && tagRelationships.Elements.Count > 0)
			{
				for (int i = 0; i < tagRelationships.Elements.Count; i++)
				{
					MarkupTagElement tagRelationship = tagRelationships.Elements[i] as MarkupTagElement;
					if (tagRelationship == null) continue;
					if (tagRelationship.FullName != "relationship") continue;

					MarkupAttribute attRelationshipInstanceId = tagRelationship.Attributes["relationshipInstanceId"];
					string relationshipInstanceId = attRelationshipInstanceId?.Value;

					if (String.IsNullOrEmpty(relationshipInstanceId))
					{
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "relationshipInstanceId not specified for relationship", "MCX0902", tagRelationship.ParentObjectModel.Accessor.GetFileName(), tagRelationship.Definition.LineNumber, tagRelationship.Definition.ColumnNumber);
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
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("invalid GUID for relationship with instanceID '{0}'", attRelationshipInstanceId.Value), "MCX0903", tagRelationship.ParentObjectModel.Accessor.GetFileName(), tagRelationship.Definition.LineNumber, tagRelationship.Definition.ColumnNumber);
					}

					MarkupTagElement tagTargetInstances = tagRelationship.Elements["targetInstances"] as MarkupTagElement;
					if (tagTargetInstances != null)
					{
						MarkupTagElement[] tagTargetInstancesElems = tagTargetInstances.GetElementsByTagName("instanceReference");
						if (tagTargetInstancesElems.Length == 0)
						{
							ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, "empty targetInstances for relationship", "MCX0901", tag.ParentObjectModel.Accessor.GetFileName(), tagTargetInstances.Definition.LineNumber, tagTargetInstances.Definition.ColumnNumber);
						}
						for (int j = 0; j < tagTargetInstancesElems.Length; j++)
						{
							MarkupTagElement tagInstanceReference = tagTargetInstancesElems[j] as MarkupTagElement;
							if (tagInstanceReference == null) continue;
							if (tagInstanceReference.FullName != "instanceReference") continue;

							MarkupAttribute attInstanceId = tagInstanceReference.Attributes["instanceId"];
							if (attInstanceId == null)
							{
								ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "instance ID not specified for relationship target instance", "MCX0107", tag.ParentObjectModel.Accessor.GetFileName(), tagInstanceReference.Definition.LineNumber, tagInstanceReference.Definition.ColumnNumber);
								continue;
							}

							global::Mocha.Core.RelationshipKey thisRelKey = new global::Mocha.Core.RelationshipKey(rel.RelationshipInstanceID, global::Mocha.Core.KnownRelationshipGuids.Relationship__has_sibling__Relationship);

							MochaRelationship siblingRelationship = library.Relationships[thisRelKey]; // FindRelationship(library, thisRelKey);
							// FIXME: in order to link sibling relationships, we must be able to read all referenced libraries in at once
							// this might be a job for the compiler...

							if (Guid.TryParse(attInstanceId.Value, out Guid instId))
							{
								if (siblingRelationship != null)
								{
									global::Mocha.Core.RelationshipKey keysib = new global::Mocha.Core.RelationshipKey(instId, siblingRelationship.DestinationInstanceIDs[0]);
									MochaRelationship relsib = library.Relationships[keysib];
									if (relsib == null)
									{
										relsib = new MochaRelationship();
										relsib.SourceInstanceID = keysib.SourceInstanceID;
										relsib.RelationshipInstanceID = keysib.RelationshipID;
										relsib.DestinationInstanceIDs.Add(inst.ID);

										library.Relationships.Add(relsib);

										ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Message, String.Format("sibling relationship found empty {0} : {1}", siblingRelationship.SourceInstanceID, siblingRelationship.DestinationInstanceIDs[0]), "MCX0712", tagInstanceReference.ParentObjectModel.Accessor.GetFileName(), tagInstanceReference.Definition.LineNumber, tagInstanceReference.Definition.ColumnNumber);
									}
									else
									{
										Console.Error.WriteLine("sibling relationship found with targets {0} : {1} ", siblingRelationship.SourceInstanceID, siblingRelationship.DestinationInstanceIDs[0]);

									}
								}
								else
								{
									Console.Error.WriteLine("no sibling relationship found for relationship {0}", rel.RelationshipInstanceID.ToString("B"));
								}
								rel.DestinationInstanceIDs.Add(instId);
							}
							else
							{
								ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("Bad GUID for instanceReference: {0}", attInstanceId.Value), "MCX0102", tag.ParentObjectModel.Accessor.GetFileName(), attInstanceId.Definition.LineNumber, attInstanceId.Definition.ColumnNumber);
							}
						}
					}
					else
					{
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, "missing targetInstances for relationship", "MCX0903", tagRelationship.ParentObjectModel.Accessor.GetFileName(), tagRelationship.Definition.LineNumber, tagRelationship.Definition.ColumnNumber);
					}

					library.Relationships.Add(rel);
				}
			}
			else if (tagRelationships != null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, "no 'relationship' tags specified in 'relationships' element", "MCX0909", tagRelationships.ParentObjectModel.Accessor.GetFileName(), tagRelationships.Definition.LineNumber, tagRelationships.Definition.ColumnNumber);
			}

			SetupInstanceWithXML__Translations(library, tag, inst);
			SetupInstanceWithXML__IntegrationIDs(library, tag, inst);

			if (tagAttributeValues == null && tagRelationships == null && (tag.Elements["translations"] as MarkupTagElement) == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, String.Format("no attributes, relationships, or translations specified for instance '{0}'", inst.ID), "MCX0301", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber);
			}
		}

		private void SetupInstanceWithXML__DebugInfo(IMochaStore library, MochaInstance inst, MarkupTagElement tag)
		{
			Guid guidDebugDefinition = Guid.NewGuid();
			MochaInstance instDebugDefinition = ZqCreateInstance(library, guidDebugDefinition, KnownInstanceGuids.Classes.InstanceDefinition);

			ZqAssignAttribute(instDebugDefinition, KnownAttributeGuids.Text.DebugDefinitionFileName, tag.ParentObjectModel.Accessor.GetFileName());
			ZqAssignAttribute(instDebugDefinition, KnownAttributeGuids.Numeric.DebugDefinitionLineNumber, tag.Definition.LineNumber);
			ZqAssignAttribute(instDebugDefinition, KnownAttributeGuids.Numeric.DebugDefinitionColumnNumber, tag.Definition.ColumnNumber);

			ZqAssignRelationship(library, inst.ID, KnownRelationshipGuids.Instance__has__Instance_Definition, new Guid[] { instDebugDefinition.ID });
		}

		private void SetupInstanceWithXML__Translations(IMochaStore library, MarkupTagElement tag, MochaInstance inst)
		{
			MarkupTagElement tagTranslations = tag.Elements["translations"] as MarkupTagElement;
			if (tagTranslations != null)
			{
				for (int i = 0; i < tagTranslations.Elements.Count; i++)
				{
					MarkupTagElement tagTranslation = (tagTranslations.Elements[i] as MarkupTagElement);
					if (tagTranslation == null) continue;
					if (tagTranslation.FullName != "translation") continue;

					MarkupAttribute attRelationshipId = tagTranslation.Attributes["relationshipInstanceId"];
					if (attRelationshipId == null)
					{
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "missing 'relationshipInstanceId' attribute for 'translation' element", "MCX0701", tagTranslation.ParentObjectModel.Accessor.GetFileName(), tagTranslation.Definition.LineNumber, tagTranslation.Definition.ColumnNumber);
						continue;
					}

					MarkupTagElement tagTranslationValues = (tagTranslation.Elements["translationValues"] as MarkupTagElement);
					if (tagTranslationValues == null)
					{
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "missing 'translationValues' subelement for 'translation' element", "MCX0702", tagTranslation.ParentObjectModel.Accessor.GetFileName(), tagTranslation.Definition.LineNumber, tagTranslation.Definition.ColumnNumber);
						continue;
					}

					Guid relationshipInstanceId;
					if (!Guid.TryParse(attRelationshipId.Value, out relationshipInstanceId))
					{
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "invalid 'relationshipInstanceId' attribute for 'translation' element", "MCX0703", tagTranslation.ParentObjectModel.Accessor.GetFileName(), attRelationshipId.Definition.LineNumber, attRelationshipId.Definition.ColumnNumber);
						continue;
					}

					MochaInstance instTTC = new MochaInstance();
					instTTC.ID = Guid.NewGuid();
					SetParentClass(library, instTTC.ID, global::Mocha.Core.KnownInstanceGuids.Classes.Translation);

					MochaRelationship relInstance__has__Translatable_Text_Constant = new MochaRelationship();
					relInstance__has__Translatable_Text_Constant.SourceInstanceID = inst.ID;
					relInstance__has__Translatable_Text_Constant.RelationshipInstanceID = relationshipInstanceId;
					relInstance__has__Translatable_Text_Constant.DestinationInstanceIDs.Add(instTTC.ID);
					library.Relationships.Add(relInstance__has__Translatable_Text_Constant);

					MochaRelationship relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value = new MochaRelationship();
					relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value.SourceInstanceID = instTTC.ID;
					relTranslatable_Text_Constant__has__Translatable_Text_Constant_Value.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Translation__has__Translation_Value;

					if (tagTranslationValues.Elements.Count == 0)
					{
						ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, "no translation values defined for translation element", "MCX0702", tagTranslationValues.ParentObjectModel.Accessor.GetFileName(), tagTranslationValues.Definition.LineNumber, tagTranslationValues.Definition.ColumnNumber);
					}
					for (int j = 0; j < tagTranslationValues.Elements.Count; j++)
					{
						MarkupTagElement tagTranslationValue = (tagTranslationValues.Elements[j] as MarkupTagElement);
						if (tagTranslationValue?.FullName != "translationValue")
						{
							continue;
						}

						MarkupAttribute attLanguageInstanceID = tagTranslationValue.Attributes["languageInstanceId"];
						MarkupAttribute attValue = tagTranslationValue.Attributes["value"];
						if (attLanguageInstanceID == null)
						{
							ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "missing 'languageInstanceId' attribute for translation value", "MCX0702", tagTranslationValue.ParentObjectModel.Accessor.GetFileName(), tagTranslationValue.Definition.LineNumber, tagTranslationValue.Definition.ColumnNumber);
							continue;
						}
						if (attValue == null)
						{
							ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "missing 'value' attribute for translation value", "MCX0702", tagTranslationValue.ParentObjectModel.Accessor.GetFileName(), tagTranslationValue.Definition.LineNumber, tagTranslationValue.Definition.ColumnNumber);
							continue;
						}

						Guid languageInstanceId;
						if (!Guid.TryParse(attLanguageInstanceID?.Value, out languageInstanceId))
						{
							ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "invalid language instance ID for translation value", "MCX0705", tagTranslationValue.ParentObjectModel.Accessor.GetFileName(), tagTranslationValue.Definition.LineNumber, tagTranslationValue.Definition.ColumnNumber);
							continue;
						}

						// create a new TTCValue instance
						MochaInstance instTranslationValue = new MochaInstance();
						instTranslationValue.ID = Guid.NewGuid();
						SetParentClass(library, instTranslationValue.ID, global::Mocha.Core.KnownInstanceGuids.Classes.TranslationValue);

						// associate the TTCValue with the Language
						MochaRelationship relTranslatable_Text_Constant_Value__has__Language = new MochaRelationship();
						relTranslatable_Text_Constant_Value__has__Language.SourceInstanceID = instTranslationValue.ID;
						relTranslatable_Text_Constant_Value__has__Language.RelationshipInstanceID = global::Mocha.Core.KnownRelationshipGuids.Translation_Value__has__Language;
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
		}

		private void SetupInstanceWithXML__IntegrationIDs(IMochaStore library, MarkupTagElement tag, MochaInstance inst)
		{
			MarkupTagElement tagIntegrationIDs = (tag.Elements["integrationIds"] as MarkupTagElement);
			if (tagIntegrationIDs != null)
			{
				IEnumerable<MarkupTagElement> tagIntegrationIDsElems = tagIntegrationIDs.Elements.OfType<MarkupTagElement>();
				if (!tagIntegrationIDsElems.Any())
				{
					ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, String.Format("no 'integrationId' elements specified in 'integrationIds' for instance '{0}'", inst.ID.ToString("B")), "MCX0706", tagIntegrationIDs.ParentObjectModel.Accessor.GetFileName(), tagIntegrationIDs.Definition.LineNumber, tagIntegrationIDs.Definition.ColumnNumber);
				}
				else
				{
					foreach (MarkupTagElement tagIntegrationID in tagIntegrationIDsElems)
					{
						Guid id, typeId;

						MarkupAttribute attValue = tagIntegrationID.Attributes["value"];
						if (attValue == null)
						{
							ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "no 'value' attribute specified for integrationId", "MCX0713", tagIntegrationID.ParentObjectModel.Accessor.GetFileName(), tagIntegrationID.Definition.LineNumber, tagIntegrationID.Definition.ColumnNumber);
							continue;
						}

						MarkupAttribute attIntegrationIDTypeID = tagIntegrationID.Attributes["typeId"];
						if (!Guid.TryParse(attIntegrationIDTypeID?.Value, out typeId))
						{
							if (attIntegrationIDTypeID != null)
							{
								ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("bad GUID '{0}' for integrationId 'typeId' attribute", attIntegrationIDTypeID.Value), "MCX0712", tag.ParentObjectModel.Accessor.GetFileName(), attIntegrationIDTypeID.Definition.LineNumber, attIntegrationIDTypeID.Definition.ColumnNumber);
								continue;
							}
							typeId = Guid.NewGuid();
						}

						MarkupAttribute attIntegrationIDID = tagIntegrationID.Attributes["id"];
						if (!Guid.TryParse(attIntegrationIDID?.Value, out id))
						{
							if (attIntegrationIDID != null)
							{
								ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Warning, String.Format("bad GUID '{0}' for integrationId 'id' attribute", attIntegrationIDID.Value), "MCX0712", tag.ParentObjectModel.Accessor.GetFileName(), attIntegrationIDID.Definition.LineNumber, attIntegrationIDID.Definition.ColumnNumber);
							}
							id = Guid.NewGuid();
						}

						MochaInstance instIntegrationID = ZqCreateInstance(library, id, global::Mocha.Core.KnownInstanceGuids.Classes.IntegrationID);
						ZqAssignAttribute(instIntegrationID, global::Mocha.Core.KnownAttributeGuids.Text.Value, attValue?.Value);
						ZqAssignRelationship(library, id, global::Mocha.Core.KnownRelationshipGuids.Integration_ID__has__Integration_ID_Usage, new Guid[] { typeId });
						ZqAssignRelationship(library, typeId, global::Mocha.Core.KnownRelationshipGuids.Integration_ID_Usage__for__Integration_ID, new Guid[] { id });

						MarkupAttribute attSingular = tagIntegrationID.Attributes["singular"];
						if (attSingular != null)
						{
							ZqAssignAttribute(instIntegrationID, global::Mocha.Core.KnownAttributeGuids.Text.Value, Boolean.Parse(attSingular?.Value));
						}
					}
				}
			}
		}

		private void SetIndex(IMochaStore library, MochaInstance instance)
		{
			if (instance.Index == null)
				return;

			MochaAttributeValue mav = new MochaAttributeValue();
			mav.AttributeInstanceID = global::Mocha.Core.KnownAttributeGuids.Numeric.Index;
			mav.Value = (decimal) instance.Index.Value;
			instance.AttributeValues.Add(mav);
		}

		private void SetSource(IMochaStore library, Guid instanceId, Guid sourceTypeId)
		{
			ZqAssignRelationship(library, instanceId, global::Mocha.Core.KnownRelationshipGuids.Class__has__Object_Source, new Guid[] { sourceTypeId });
			ZqAssignRelationship(library, sourceTypeId, global::Mocha.Core.KnownRelationshipGuids.Object_Source__for__Class, new Guid[] { instanceId });
		}

		private void SetOwner(IMochaStore library, Guid instanceId, Guid ownerId)
		{
			ZqAssignRelationship(library, instanceId, global::Mocha.Core.KnownRelationshipGuids.Class__has_owner__User, new Guid[] { ownerId });
			ZqAssignRelationship(library, ownerId, global::Mocha.Core.KnownRelationshipGuids.User__owner_for__Class, new Guid[] { instanceId });
		}

		private void SetClass(IMochaStore library, Guid id)
		{
			SetParentClass(library, id, global::Mocha.Core.KnownInstanceGuids.Classes.Class);
		}

		private void SetSuperClass(IMochaStore library, Guid sourceInstanceID, Guid parentClassID)
		{
			ZqAssignRelationship(library, sourceInstanceID, global::Mocha.Core.KnownRelationshipGuids.Class__has_super__Class, new Guid[] { parentClassID });
			ZqAssignRelationship(library, parentClassID, global::Mocha.Core.KnownRelationshipGuids.Class__has_sub__Class, new Guid[] { sourceInstanceID });
		}
		private void SetParentClass(IMochaStore library, Guid sourceInstanceID, Guid parentClassID)
		{
			ZqAssignRelationship(library, sourceInstanceID, global::Mocha.Core.KnownRelationshipGuids.Instance__for__Class, new Guid[] { parentClassID });
			ZqAssignRelationship(library, parentClassID, global::Mocha.Core.KnownRelationshipGuids.Class__has__Instance, new Guid[] { sourceInstanceID });
		}

		private void ZqAssignRelationship(IMochaStore library, Guid sourceInstanceID, Guid relationshipInstanceID, Guid[] destinationInstanceIds)
		{
			MochaRelationship rel = new MochaRelationship();
			rel.SourceInstanceID = sourceInstanceID;
			for (int i = 0; i < destinationInstanceIds.Length; i++)
			{
				rel.DestinationInstanceIDs.Add(destinationInstanceIds[i]);
			}

			if (!library.Instances.Contains(relationshipInstanceID))
			{
				// FIXME: this doesn't work because we don't know exactly what libraries we've referenced
				// Console.Error.WriteLine("zq: assignRelationship: relationship not found {0}", relationshipInstanceID.ToString("B"));
			}
			rel.RelationshipInstanceID = relationshipInstanceID;
			library.Relationships.Add(rel);
		}

		private MochaAttributeValue LoadAttributeValue(MarkupTagElement tag)
		{
			if (tag == null) return null;
			if (tag.FullName != "attributeValue") return null;

			MarkupAttribute attInstanceID = tag.Attributes["attributeInstanceId"];
			MarkupAttribute attValue = tag.Attributes["value"];

			if (attInstanceID == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "attributeInstanceId not specified for attributeValue", "MCX0302", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber);
				return null;
			}

			Guid instanceId;
			if (!Guid.TryParse(attInstanceID.Value, out instanceId))
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, String.Format("invalid GUID for attributeInstanceId: '{0}'", attInstanceID.Value), "MCX0302", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber);
				return null;
			}

			if (attValue == null)
			{
				ConsoleExtensions.LogMSBuildMessage(MessageSeverity.Error, "value not specified for attributeValue", "MCX0303", tag.ParentObjectModel.Accessor.GetFileName(), tag.Definition.LineNumber, tag.Definition.ColumnNumber);
				return null;
			}

			MochaAttributeValue item = new MochaAttributeValue();
			item.AttributeInstanceID = instanceId;
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
