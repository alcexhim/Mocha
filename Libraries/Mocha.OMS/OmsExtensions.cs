using System;
using System.Collections.Generic;
using System.Text;
using MBS.Framework;
using Mocha.Core;
using Mocha.OMS.OMSComponents;

namespace Mocha.OMS
{
	public static class OmsExtensions
	{
		public static string ToJSONString(this Instance inst)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			sb.Append('}');
			return sb.ToString();
		}
		public static string ToJSONString(this Relationship rel)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			sb.Append('}');
			return sb.ToString();
		}
		public static string ToJSONString(this OMSComponent component)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			if (component is OMSComponents.OMSCommandComponent)
			{
				sb.Append("\"type\": \"command\"");
			}
			else if (component is OMSComponents.OMSDetailComponent)
			{
				OMSComponents.OMSDetailComponent comp = (OMSComponents.OMSDetailComponent)component;
				sb.Append("\"type\": \"detail\", ");
				sb.Append("\"columns\": [ ");
				for (int i = 0; i < comp.Columns.Count; i++)
				{
					sb.Append("{ ");
					sb.Append(String.Format("\"instanceId\": \"{0}\", ", comp.Columns[i].InstanceID.ToString()));
					sb.Append(String.Format("\"title\": \"{0}\" ", comp.Columns[i].Title));
					sb.Append(" }");
					if (i < comp.Columns.Count - 1)
					{
						sb.Append(", ");
					}
				}
				sb.Append(" ], ");
				sb.Append("\"rows\": [ ");
				for (int i = 0; i < comp.Rows.Count; i++)
				{
					sb.Append("{ ");
					sb.Append(String.Format("\"instanceId\": \"{0}\", ", comp.Rows[i].InstanceID.ToString()));
					sb.Append("\"columns\": [ ");
					for (int j = 0; j < comp.Rows[i].Columns.Count; j++)
					{
						sb.Append("{ ");
						sb.Append(String.Format("\"columnInstanceId\": \"{0}\", ", comp.Rows[i].Columns[j].ColumnInstanceID.ToString()));
						sb.Append(String.Format("\"displayAsCount\": \"{0}\", ", comp.Rows[i].Columns[j].DisplayAsCount.ToString()));
						sb.Append(String.Format("\"value\": {0} ", ValueToJSONString(comp.Rows[i].Columns[j])));
						sb.Append(" }");
						if (j < comp.Rows[i].Columns.Count - 1)
						{
							sb.Append(", ");
						}
					}
					sb.Append(" ]");
					sb.Append(" }");
					if (i < comp.Rows.Count - 1)
					{
						sb.Append(", ");
					}
				}
				sb.Append(" ]");
			}
			else if (component is OMSComponents.OMSHeaderComponent)
			{
				sb.Append("\"type\": \"header\"");
			}
			else if (component is OMSComponents.OMSImageComponent)
			{
				sb.Append("\"type\": \"image\"");
			}
			else if (component is OMSComponents.OMSPanelComponent)
			{
				sb.Append("\"type\": \"panel\"");
			}
			else if (component is OMSComponents.OMSParagraphComponent)
			{
				sb.Append("\"type\": \"paragraph\"");
			}
			else if (component is OMSComponents.OMSSequentialContainerComponent)
			{
				sb.Append("\"type\": \"sequential\"");
			}
			else if (component is OMSComponents.OMSSummaryComponent)
			{
				sb.Append("\"type\": \"summary\"");
			}
			else if (component is OMSComponents.OMSTabContainerComponent)
			{
				sb.Append("\"type\": \"tabcontainer\"");
			}
			sb.Append('}');
			return sb.ToString();
		}

		private static string ValueToJSONString(OMSDetailComponent.OMSDetailRowColumn rc)
		{
			if (rc.InstanceIDs == null)
			{
				return String.Format("\"{0}\"", rc.Value);
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("[ ");
			for (int i = 0; i < rc.InstanceIDs.Length; i++)
			{
				sb.Append(String.Format("\"{0}\"", rc.InstanceIDs[i].ToString()));
				if (i < rc.InstanceIDs.Length - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append(" ]");
			return sb.ToString();
		}

		public static string ToJSONString(this IOmsResponse resp)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append('{');
			if (resp is OmsPageResponse)
			{
				OmsPageResponse page = (resp as OmsPageResponse);
				sb.Append("\"type\": \"page\", ");
				sb.Append(String.Format("\"title\": \"{0}\", ", page.Title));
				sb.Append(String.Format("\"subtitle\": \"{0}\", ", page.Description));
				sb.Append(String.Format("\"relatedInstance\": {0}, ", page.DescriptionInstance == InstanceKey.Empty ? "null" : String.Format("\"{0}\"", page.DescriptionInstance.ToString())));

				sb.Append("\"components\": [ ");
				for (int i = 0; i < page.Components.Count; i++)
				{
					sb.Append(page.Components[i].ToJSONString());
					if (i < page.Components.Count - 1)
					{
						sb.Append(", ");
					}
				}
				sb.Append("] ");
			}
			sb.Append('}');
			return sb.ToString();
		}

		/// <summary>
		/// Gets the <see cref="Instance" /> representing the class of which the given <see cref="Instance" /> is an instance.
		/// </summary>
		/// <returns>The parent class for the specified <see cref="Instance" />.</returns>
		/// <param name="inst">The <see cref="Instance" /> for which to return the parent class.</param>
		public static Instance GetParentClass(this Oms oms, Instance inst)
		{
			return oms.GetRelatedInstance(inst, KnownRelationshipGuids.Instance__for__Class);
		}

		public static object GetAttributeValue(this Oms oms, Instance instTarget, Guid instAttributeID, object defaultValue = null)
		{
			return oms.GetAttributeValue(instTarget, instAttributeID, defaultValue, DateTime.Now);
		}
		public static object GetAttributeValue(this Oms oms, Instance instTarget, Instance instAttribute, object defaultValue = null)
		{
			return oms.GetAttributeValue(instTarget, instAttribute.GlobalIdentifier, defaultValue);
		}
		public static T GetAttributeValue<T>(this Oms oms, Instance instTarget, Instance instAttribute, T defaultValue = default(T))
		{
			object value = oms.GetAttributeValue(instTarget, instAttribute, (object)defaultValue);
			if (value is T)
				return (T)value;
			return defaultValue;
		}
		public static T GetAttributeValue<T>(this Oms oms, Instance instTarget, Guid instAttributeID, T defaultValue = default(T))
		{
			object value = oms.GetAttributeValue(instTarget, instAttributeID, (object)defaultValue);
			if (value is T)
				return (T)value;

			if (value is string && ((string)value).TryParse<T>(out T val))
			{
				return val;
			}
			return defaultValue;
		}

		public static Instance GetRelatedInstance(this Oms oms, Instance target, Instance relationship)
		{
			if (relationship == null)
			{
				Console.Error.WriteLine("mocha: oms: ERROR: relationship is null for Get Related Instance [Singular]");
				return null;
			}
			return oms.GetRelatedInstance(target, relationship.GlobalIdentifier);
		}
		public static Instance[] GetRelatedInstances(this Oms oms, Instance target, Instance relationship)
		{
			if (relationship == null)
			{
				Console.Error.WriteLine("mocha: oms: ERROR: relationship is null for Get Related Instances [Nonsingular]");
				return null;
			}
			return oms.GetRelatedInstances(target, relationship.GlobalIdentifier);
		}



		public static string GetTTC(this Oms oms, Instance instTTC, string defaultValue = "")
		{
			return oms.GetTTC(null, instTTC, defaultValue);
		}

		/// <summary>
		/// Gets the value of the Translatable Text Constant related to the specified <see cref="Instance" /> via the relationship with the given Relationship <see cref="Guid" />.
		/// </summary>
		/// <returns><see cref="String"/> value of the Translatable Text Constant related to the specified <see cref="Instance" /> via the given Relationship <see cref="Guid" />.</returns>
		/// <param name="inst">The <see cref="Instance" /> for which to query a value.</param>
		/// <param name="guidRelationship">The <see cref="Guid" /> of the relationship for which to query a value.</param>
		/// <param name="defaultValue">The default value returned if the Translatable Text Constant is not defined on the specified <see cref="Instance" /> for the given Relationship.</param>
		public static string GetTranslationValue(this Oms oms, Instance inst, Guid guidRelationship, string defaultValue = "")
		{
			return oms.GetTTC(inst, oms.GetInstance(guidRelationship), defaultValue);
		}
		public static string GetTTC(this Oms oms, Instance inst, Instance instRelationshipOrTTC, string defaultValue = "")
		{
			string value = defaultValue;
			if (inst != null)
			{
				instRelationshipOrTTC = oms.GetRelatedInstance(inst, instRelationshipOrTTC);
			}
			if (instRelationshipOrTTC != null)
			{
				Instance instTTCValue = oms.GetRelatedInstance(instRelationshipOrTTC, KnownRelationshipGuids.Translation__has__Translation_Value);
				if (instTTCValue == null)
					return null;

				Instance Attribute___Value = oms.GetInstance(KnownAttributeGuids.Text.Value);
				value = oms.GetAttributeValue<string>(instTTCValue, Attribute___Value);
			}
			return value;
		}

		public static Instance GetTranslation(this Oms oms, Instance inst, Guid guidRelationship)
		{
			if (inst != null)
			{
				return oms.GetRelatedInstance(inst, guidRelationship);
			}
			return null;
		}

		public static Instance GetInstance(this Oms oms, InstanceKey key)
		{
			Instance[] instClasses = oms.GetInstances(oms.GetInstance(KnownInstanceGuids.Classes.Class));

			Instance[] instClasses2 = oms.InstanceSetFilter(instClasses, new OmsFilterCondition[]
			{


			});

			if (key.ClassIndex > 0 && key.ClassIndex - 1 < instClasses.Length)
			{
				Instance instClass = instClasses[key.ClassIndex - 1];
				if (instClass == null) return null;

				Instance[] instInstances = oms.GetInstances(instClass);
				if (key.InstanceIndex > 0 && key.InstanceIndex - 1 < instInstances.Length)
				{
					Instance instInstance = instInstances[key.InstanceIndex - 1];
					return instInstance;
				}
			}
			return null;
		}

		public static string FormatString(this Oms oms, Instance instString, Instance instRelated)
		{
			OmsContext context = new OmsContext();

			// String Component.has method returning Text Attribute `...`
			// Translation:
			// `Translation`.instance labeled by String
			//		[ String Component: extract Text Attribute `Value` from related instance `Translation`.has `Translation Value` for language My Language
			//

			StringBuilder sb = new StringBuilder();
			Instance[] instStringComponents = oms.GetRelatedInstances(instString, KnownRelationshipGuids.String__has__String_Component);
			for (int i = 0; i < instStringComponents.Length; i++)
			{
				// Instance instStringComponentRAMB = oms.GetRelatedInstance(instStringComponents[i]);
				if (instStringComponents[i] == null)
				{
					continue;
				}

				Instance instStringComponentClass = oms.GetParentClass(instStringComponents[i]);

				if (instStringComponentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ExtractSingleInstanceStringComponent)
				{
					Instance instESIRelationship = oms.GetRelatedInstance(instStringComponents[i], KnownRelationshipGuids.Extract_Single_Instance_String_Component__has__Relationship);
					if (instESIRelationship != null)
					{
						Instance related = oms.GetRelatedInstance(instRelated, instESIRelationship);
						if (related != null)
						{
							string value = oms.GetTTC(related); // oms.GetInstanceText(related); // oms.GetTTC(related);
							if (String.IsNullOrEmpty(value))
							{
								// HACK: not sure why we need one vs the other (does NOT work for TTC for some reason)
								value = oms.GetInstanceText(related);

								if (String.IsNullOrEmpty(value))
								{
									// HACK: for Translation Values
									value = oms.GetAttributeValue<string>(related, KnownAttributeGuids.Text.Value);
								}
							}
							if (value == null)
							{
								value = String.Format("[ ESI broken: {0} ]", instESIRelationship.GlobalIdentifier);
							}
							sb.Append(value);
						}
					}
					else
					{
						sb.Append(String.Format("[missing ESI relationship {0} ]", instStringComponents[i].GlobalIdentifier));
					}
				}
				else if (instStringComponentClass.GlobalIdentifier == KnownInstanceGuids.Classes.InstanceAttributeStringComponent)
				{
					Instance instIAAttribute = oms.GetRelatedInstance(instStringComponents[i], KnownRelationshipGuids.Instance_Attribute_String_Component__has__Attribute);

					bool isStatic = oms.GetAttributeValue<bool>(instStringComponents[i], KnownAttributeGuids.Boolean.Static);
					if (instIAAttribute != null)
					{
						if (isStatic)
						{
							Instance instRelatedClass = oms.GetParentClass(instRelated);
							sb.Append(oms.GetAttributeValue<string>(instRelatedClass, instIAAttribute));
						}
						else
						{
							sb.Append(oms.GetAttributeValue<string>(instRelated, instIAAttribute));
						}
					}
					else
					{
						sb.Append(String.Format("[missing IA attribute {0} ]", instStringComponents[i].GlobalIdentifier));
					}
				}
				else if (instStringComponentClass.GlobalIdentifier == KnownInstanceGuids.Classes.StringComponent)
				{
					Instance instSourceMethod = oms.GetRelatedInstance(instStringComponents[i], KnownRelationshipGuids.String_Component__has_source__Method);
					object value = null;
					try
					{
						value = oms.ExecuteMethod(instSourceMethod, context, new OmsVariable[]
						{
							new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), instRelated)
						});
					}
					catch (Exception ex)
					{
						context.Messages.Add(new OmsMessage(ex.Message, OmsMessageSeverity.Error));
					}

					if (value is Instance)
					{
						sb.Append(oms.GetInstanceText((value as Instance)));
					}
					else
					{
						sb.Append(value);
					}
				}
				else
				{
					throw new Exception("string component error");
				}
			}
			return sb.ToString();
		}

		public static Instance GetTenantInstance(this Oms oms)
		{
			Instance clsTenant = oms.GetInstance(KnownInstanceGuids.Classes.Tenant);
			if (clsTenant == null)
				return null;

			Instance[] instTenants = oms.GetInstances(clsTenant);
			if (instTenants == null)
				return null;

			return instTenants[0];
		}

		public static string GetInstanceText(this Oms oms, Instance inst)
		{
			Instance instClass = oms.GetParentClass(inst);
			Instance instLabeledByString = oms.GetRelatedInstance(instClass, KnownRelationshipGuids.Class__instance_labeled_by__String, OmsSearchOption.SuperclassesAlways);
			if (instLabeledByString != null)
			{
				return oms.FormatString(instLabeledByString, inst);
			}
			return null;
		}

		public static object GetReportFieldEC(this Oms oms, Instance inst, Instance reportFieldInst)
		{
			Instance instParentClassForReportField = oms.GetParentClass(reportFieldInst);
			if (instParentClassForReportField.GlobalIdentifier == KnownInstanceGuids.Classes.AttributeReportField)
			{
				Instance instRel = oms.GetRelatedInstance(reportFieldInst, KnownRelationshipGuids.Attribute_Report_Field__has_target__Attribute);
				object value = oms.GetAttributeValue(inst, instRel);
				return value;
			}
			else if (instParentClassForReportField.GlobalIdentifier == KnownInstanceGuids.Classes.RelationshipReportField)
			{
				Instance instRel = oms.GetRelatedInstance(reportFieldInst, KnownRelationshipGuids.Relationship_Report_Field__has_target__Relationship);
				Instance relatedInst = oms.GetRelatedInstance(inst, instRel);
				return relatedInst;
			}
			else if (instParentClassForReportField.GlobalIdentifier == KnownInstanceGuids.Classes.ReportField)
			{
				return oms.ExecuteReportField(reportFieldInst, inst);
			}
			return null;
		}

		public static object ExecuteReportField(this Oms oms, Instance instReportField, Instance inst)
		{
			OmsContext context = new OmsContext();
			context.GlobalVariables.Add(new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst));
			OmsVariable[] promptValues = new OmsVariable[]
			{
				new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst),
				new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.Report_Field_Instance), instReportField)
			};
			context.CallStack.Push(new OmsCallStack(oms.GetInstanceKey(instReportField), promptValues));
			Instance instSourceMethod = oms.GetRelatedInstance(instReportField, KnownRelationshipGuids.Report_Field__has_source__Method);

			object value = oms.ExecuteMethod(instSourceMethod, context, promptValues);

			Instance instValue = null;
			if (value is Instance)
			{
				instValue = (Instance)value;
			}
			else if (value is Instance[])
			{
				Instance[] vals = (Instance[])value;
				if (vals.Length == 1)
				{
					instValue = vals[0];
				}
			}
			if (instValue != null)
			{
				bool renderAsText = oms.GetAttributeValue<bool>(instReportField, KnownAttributeGuids.Boolean.RenderAsText);
				if (renderAsText)
				{
					return oms.GetInstanceText(instValue);
				}
			}
			return value;
		}

		public static InstanceKey[] GetInstanceKeys(this Oms oms, Instance[] inst)
		{
			if (inst == null)
				return null;

			InstanceKey[] keys = new InstanceKey[inst.Length];
			for (int i = 0; i < inst.Length; i++)
			{
				keys[i] = oms.GetInstanceKey(inst[i]);
			}
			return keys;
		}
		public static InstanceKey GetInstanceKey(this Oms oms, Instance inst)
		{
			Instance instClass = oms.GetParentClass(inst);
			int iClassIndex = 0;
			if (instClass != null)
				iClassIndex = oms.GetInstanceIndex(instClass);

			int iInstanceIndex = 0;
			iInstanceIndex = oms.GetInstanceIndex(inst);

			return new InstanceKey(iClassIndex, iInstanceIndex);
		}
		public static string ExecuteMethodReturningTextOrTranslation(this Oms oms, Instance inst, OmsContext context)
		{
			object textval = oms.ExecuteMethod(inst, context);
			if (textval is Instance)
			{
				return oms.GetTTC((Instance)textval);
			}
			else if (textval is Instance[])
			{
				Instance[] ary = (Instance[])textval;
				if (ary.Length > 0)
				{
					return oms.GetTTC(ary[0]);
				}
			}
			else if (textval is string)
			{
				return (string)textval;
			}
			return null;
		}

		/// <summary>
		/// Executes the method specified by the <paramref name="inst" /> param.
		/// </summary>
		/// <returns>The instance.</returns>
		/// <param name="oms">Oms.</param>
		/// <param name="inst">Instance.</param>
		/// <param name="promptValues">Prompt values.</param>
		/// <remarks>
		/// <para>
		/// Method Execution starts here.
		/// Ideally, we should call ExecuteMethod() with a Method Binding, which
		/// sets up the appropriate parameter passing into the associated Method.
		/// </para>
		/// <para>
		/// Parameters specified in promptValues will be assigned to the Method
		/// Binding. The Method Binding is responsible for taking these inputs
		/// and mapping them to the appropriate parameters on the associated Method.
		/// </para>
		/// <para>
		/// When the parameters are set up, the Method is then called. Some
		/// Methods take no parameters; if any are supplied, they can be safely
		/// ignored. If a Method requires a parameter that is not mapped in the
		/// Method Binding or present in promptValues, the OMS responds with an
		/// appropriate error message.
		/// </para>
		/// <para>
		/// Eventually, control returns to the caller of the ExecuteMethod()
		/// function, in the form of: a null reference, representing null; a
		/// single Instance value, for single instance methods; an array of
		/// Instances, for multiple instance methods; a string, for
		/// Text Attribute; a decimal, for Numeric Attribute; a bool, for Boolean
		/// Attribute; or a DateTime, for Date Attribute.
		/// </para>
		/// </remarks>
		public static object ExecuteMethod(this Oms oms, Instance inst, OmsContext context, OmsVariable[] promptValues = null)
		{
			if (inst == null)
			{
				// MADI error: method is empty
				context.Messages.Add(new OmsMessage("Empty Method for Execute Method system call"));
				return null;
			}

			context.CallStack.Push(new OmsCallStack(oms.GetInstanceKey(inst), promptValues));
			object value = __ExecuteMethodInternal(oms, inst, context);
			context.CallStack.Pop();

			return value;
		}

		private static object __ExecuteMethodInternal(Oms oms, Instance inst, OmsContext context)
		{
			// example: BA - Build Attribute Method `Common Text@get '@'`
			Instance instClass = oms.GetParentClass(inst);

			List<OmsVariable> listPromptValues = new List<OmsVariable>();

			if (instClass.GlobalIdentifier == KnownInstanceGuids.Classes.ReturnAttributeMethodBinding ||
				instClass.GlobalIdentifier == KnownInstanceGuids.Classes.ReturnInstanceSetMethodBinding)
			{
				Instance instMethod = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Method_Binding__for__Method);
				Instance[] instMethodParameters = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Method_Binding__has__Parameter_Assignment);
				for (int i = 0; i < instMethodParameters.Length; i++)
				{
					Instance assignsToMethodParm = oms.GetRelatedInstance(instMethodParameters[i], KnownRelationshipGuids.Parameter_Assignment__assigns_to__Parameter);
					Instance assignsFromMethodBinding = oms.GetRelatedInstance(instMethodParameters[i], KnownRelationshipGuids.Parameter_Assignment__assigns_from__Method_Binding);

					object value = oms.ExecuteMethod(assignsFromMethodBinding, context);
					listPromptValues.Add(new OmsVariable(assignsToMethodParm, value));
				}

				object val = oms.ExecuteMethod(instMethod, context, listPromptValues.ToArray());
				return val;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.BuildAttributeMethod)
			{
				object value = oms.GetAttributeValue(inst, KnownAttributeGuids.Text.Value);

				Instance instReturnType = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Method__has_return_type__Class);
				value = oms.CastAttributeValue(value, instReturnType);
				return value;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.SelectFromInstanceSetMethod)
			{
				Instance classInstance2 = (context.LocalVariables[KnownInstanceGuids.PromptValueClasses.Class_Instance_for_SS_Method]?.Value as Instance);
				if (classInstance2 == null)
				{
					context.Messages.Add(new OmsMessage("missing required parm 'Class' for SS - Select from Instance Set method"));
					return null;
				}
				return oms.GetInstances(classInstance2);
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetSpecifiedInstancesMethod)
			{
				if (oms.GetAttributeValue<bool>(inst, KnownAttributeGuids.Boolean.Singular))
				{
					Instance value = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Specific_Instances_Method__has__Instance);
					return value;
				}
				else
				{
					Instance[] value = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Get_Specific_Instances_Method__has__Instance);
					return value;
				}
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetInstanceSetBySystemRoutineMethod)
			{
				OmsCallStack csMethod = context.CallStack.Pop();
				OmsCallStack csMethodBinding = context.CallStack.Pop();

				object retval = null;
				if (inst.GlobalIdentifier == KnownInstanceGuids.Methods.GetInstanceSetBySystemRoutine.Tenant__get__Current_Tenant)
				{
					retval = oms.GetTenantInstance();
				}
				else if (inst.GlobalIdentifier == KnownInstanceGuids.Methods.GetInstanceSetBySystemRoutine.Instance__get__This_Instance)
				{
					retval = context.GlobalVariables[KnownInstanceGuids.PromptValueClasses.This_Instance]?.Value;
				}

				context.CallStack.Push(csMethodBinding);
				context.CallStack.Push(csMethod);
				return retval;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetReferencedInstanceSetMethod)
			{
				Instance instRelated = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Referenced_Instance_Set_Method__has_relationship__Method);
				Instance instLoopOnInstanceSet = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Referenced_Instance_Set_Method__loop_on__Instance_Set);

				object loopOnInstanceSet = oms.ExecuteMethod(instLoopOnInstanceSet, context);

				// get the instance set referenced by the specified Relationship using the specified Prompt Value 
				// loop on instance set: prompt value
				// get relationship :  relationship

				// PromptValues here is not correct, the PromptValues being assigned are the ones from the entry point Method, not the Method Binding
				// here we need to have the Method Binding assign parameters to the entry point Method, then call the Method with those paramters

				// e.g. Method Binding.assign parameter `This Instance` to `User Login`

				// OmsCallStack temp2 = context.CallStack.Pop();

				// HACK HACK HACK: THIS IS WRONG. REMOVE WHEN WE CAN APPROPRIATELY ASSIGN PARAMETERS FROM METHOD BINDINGS TO METHODS
				// OmsVariable pval = context.LocalVariables[instLoopOnInstanceSet.GlobalIdentifier];

				// context.CallStack.Push(temp2);
				// context.CallStack.Push(temp);

				if (loopOnInstanceSet != null)
				{
					Instance pinst = (Instance)loopOnInstanceSet;

					Instance instRelationship = (Instance)oms.ExecuteMethod(instRelated, context);

					object value = null;
					if (oms.GetAttributeValue<bool>(inst, KnownAttributeGuids.Boolean.Singular))
					{
						value = oms.GetRelatedInstance(pinst, instRelationship);
					}
					else
					{
						value = oms.GetRelatedInstances(pinst, instRelationship);
					}
					return value;
				}
				else
				{
					context.Messages.Add(new OmsMessage("Empty Required Parm `Loop on Instance Set`", OmsMessageSeverity.Error));
				}
				return null;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetAttributeMethod)
			{
				// FIXME: do we have to pass prompt values through the context?
				// or if this is a method binding, that's what method binding parameter assignments are for!
				Instance instAttribute = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Attribute_Method__has__Attribute);

				OmsVariable pval = context.LocalVariables[KnownInstanceGuids.PromptValueClasses.This_Instance];
				if (pval == null)
				{
					context.Messages.Add(new OmsMessage(String.Format("empty required parm {0}", KnownInstanceGuids.PromptValueClasses.This_Instance), OmsMessageSeverity.Error));
					return null;
				}

				Instance pinst = (Instance)pval.Value;

				object value = oms.GetAttributeValue(pinst, instAttribute);
				return value;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetRelationshipMethod)
			{
				// FIXME: do we have to pass prompt values through the context?
				// or if this is a method binding, that's what method binding parameter assignments are for!
				Instance instRelationship = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Relationship_Method__has__Relationship);
				return instRelationship;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetReferencedAttributeMethod)
			{
				Instance instRelated = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Referenced_Attribute_Method__has__Attribute);
				Instance instLoopOnInstanceSet = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Referenced_Attribute_Method__loop_on__Instance_Set);
				// get the instance set referenced by the specified Relationship using the specified Prompt Value 
				// loop on instance set: prompt value
				// get relationship :  relationship

				OmsVariable pval = context.GlobalVariables[instLoopOnInstanceSet.GlobalIdentifier];

				if (pval == null)
				{
					context.Messages.Add(new OmsMessage("Empty Required Parm `Loop on Instance Set`"));
					return null;
				}
				Instance pinst = (Instance)pval.Value;

				object value = oms.GetAttributeValue(pinst, instRelated);
				return value;
			}
			return null;
		}

		/// <summary>
		/// Casts the given value to the appropriate system data type represented by the given attribute class instance.
		/// </summary>
		/// <returns>The casted value.</returns>
		/// <param name="oms">Oms.</param>
		/// <param name="value">The value to cast.</param>
		/// <param name="instAttributeClass">The instance representing the class of attribute that should be returned.</param>
		public static object CastAttributeValue(this Oms oms, object value, Instance instAttributeClass)
		{
			if (value == null)
				return null;

			if (instAttributeClass != null)
			{
				if (instAttributeClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
				{
					return value;
				}
				else if (instAttributeClass.GlobalIdentifier == KnownInstanceGuids.Classes.DateAttribute)
				{
					return DateTime.Parse(value.ToString());
				}
				else if (instAttributeClass.GlobalIdentifier == KnownInstanceGuids.Classes.BooleanAttribute)
				{
					return Boolean.Parse(value.ToString());
				}
				else if (instAttributeClass.GlobalIdentifier == KnownInstanceGuids.Classes.NumericAttribute)
				{
					return Decimal.Parse(value.ToString());
				}
			}
			return value;
		}

		public static IOmsResponse ExecuteInstance(this Oms oms, OmsContext context, Instance instance, Instance rel_task = null)
		{
			if (instance == null)
			{
				return null;
			}

			Instance classInstance = oms.GetParentClass(instance);
			if (rel_task == null)
			{
				rel_task = oms.GetRelatedInstance(classInstance, KnownRelationshipGuids.Class__has_default__Task);
			}
			if (rel_task == null)
			{
				// still do not have a 'has default Task' for this Class
			}

			/*
			if (classInstance.GlobalIdentifier == KnownInstanceGuids.Classes.MethodCall)
			{
				Instance instMethod = oms.GetRelatedInstance(instance, KnownRelationshipGuids.Method_Call__has__Method);
				Instance[] instPromptValues = oms.GetRelatedInstances(instance, KnownRelationshipGuids.Method_Call__has__Prompt_Value);

				for (int i = 0; i < instPromptValues.Length; i++)
				{
					Instance instPromptValueType = oms.GetParentClass(instPromptValues[i]);
					Instance instPrompt = oms.GetRelatedInstance(instPromptValues[i], KnownRelationshipGuids.Prompt_Value__has__Prompt);

					object value = null;
					if (instPromptValueType.GlobalIdentifier == KnownInstanceGuids.PromptValueClasses.InstancePromptValue)
					{
						value = oms.GetRelatedInstance(instPromptValues[i], KnownRelationshipGuids.Instance_Prompt_Value__has__Instance);
					}
					context.GlobalVariables.Add(new OmsVariable(instPrompt, value));
				}
				IOmsResponse ret = oms.ExecuteInstance(context, instMethod, null);
				for (int i = 0; i < instPromptValues.Length; i++)
				{
					context.GlobalVariables.RemoveAt(context.GlobalVariables.Count - 1);
				}
				return ret;
			}
			*/

			if (classInstance.GlobalIdentifier == KnownInstanceGuids.Classes.StandardReport)
			{
				// FIXME: directly executing the instance should present the standard report summary view
				// -----> to actually run the report, use .../d/inst/69$1/rel-task/2993$104.htmld (Run Report task)

				OmsPageResponse resp = new OmsPageResponse();
				// FIXME: don't hardcode this
				resp.Title = oms.GetTranslationValue(instance, KnownRelationshipGuids.Report__has_title__Translatable_Text_Constant);
				resp.Description = oms.GetTranslationValue(instance, KnownRelationshipGuids.Report__has_description__Translatable_Text_Constant);

				if (context.GlobalVariables.Count == 1)
				{
					IOmsResponse respPrompts = oms.ExecuteInstancePrompts(instance, KnownRelationshipGuids.Report__has__Prompt);
					if (respPrompts != null)
						return respPrompts;
				}
				else
				{

				}

				Instance instReportDataSource = oms.GetRelatedInstance(instance, KnownRelationshipGuids.Report__has__Report_Data_Source);
				if (instReportDataSource == null)
				{
					throw new OmsException(String.Format("empty Report Data Source for Report {0} ({1})", oms.GetInstanceKey(instance), instance.GlobalIdentifier));
				}

				Instance instRDSSourceMethod = oms.GetRelatedInstance(instReportDataSource, KnownRelationshipGuids.Report_Data_Source__has_source__Method);
				if (instRDSSourceMethod == null)
				{
					throw new OmsException(String.Format("empty Source Method for Report Data Source {0} ({1})", oms.GetInstanceKey(instReportDataSource), instReportDataSource.GlobalIdentifier));
				}

				object respMethodResult = oms.ExecuteMethod(instRDSSourceMethod, context, new OmsVariable[]
				{
					new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), instance)
				});

				OMSComponents.OMSSummaryComponent summary = new OMSComponents.OMSSummaryComponent();
				resp.Components.Add(summary);

				OMSComponents.OMSDetailComponent detail = new OMSComponents.OMSDetailComponent();
				// FIXME: Detail Page Component should get its columns from a .has column source Method
				// FIXME: Detail Page Component should get its rows from a .has row source Method

				// The column source Method should return an Instance Set of Report Field instances. This defaults
				// to the GRS - Get Related Instance Set method for the relationship `Detail Page Component.has Report Field`
				Instance[] instReportColumns = oms.GetRelatedInstances(instance, KnownRelationshipGuids.Report__has__Report_Column);

				// FIXME: this doesn't work as written because it assumes This Instance is a Detail Page Component, but This Instance here is a Report
				// For a report, we may need to create another prompt value (say `Report Instance`) and use that instead of `This Instance`

				// SELECT * FROM `Sample@get References` [524$221] WHERE ...
				// object instReportColumns2 = oms.ExecuteMethod(oms.GetInstance(new Guid("{2a8186fe-9341-430f-9988-f541f2295eab}")), new PromptValue[] { new PromptValue(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), instance) });

				Instance[] instReportFields = new Instance[instReportColumns.Length];
				for (int i = 0; i < instReportColumns.Length; i++)
				{
					instReportFields[i] = oms.GetRelatedInstance(instReportColumns[i], KnownRelationshipGuids.Report_Column__has__Report_Field);
					// string title = oms.GetTranslationValue(instReportColumns[i], KnownRelationshipGuids.Report_Column__has_title__Translatable_Text_Constant);
					string title = oms.GetTranslationValue(instReportFields[i], KnownRelationshipGuids.Report_Field__has_title__Translatable_Text_Constant);

					Instance instReportFieldClass = oms.GetParentClass(instReportFields[i]);
					if (instReportFieldClass != null)
					{
						if (instReportFieldClass.GlobalIdentifier == KnownInstanceGuids.Classes.PrimaryObjectReportField)
						{
							if (respMethodResult is Instance[])
							{
								if (((Instance[])respMethodResult).Length > 0)
								{
									// hack ugly hack
									title = oms.GetInstanceText(oms.GetParentClass(((Instance[])respMethodResult)[0]));
								}
							}
						}
					}

					detail.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailColumn(oms.GetInstanceKey(instReportColumns[i]), title));
				}

				if (respMethodResult is Instance[])
				{
					Instance[] insts = (respMethodResult as Instance[]);
					for (int i = 0; i < insts.Length; i++)
					{
						OMSComponents.OMSDetailComponent.OMSDetailRow row = new OMSComponents.OMSDetailComponent.OMSDetailRow();
						row.InstanceID = oms.GetInstanceKey(insts[i]);
						for (int j = 0; j < instReportColumns.Length; j++)
						{
							InstanceKey rcKey = oms.GetInstanceKey(instReportColumns[j]);
							Instance instReportFieldClass = oms.GetParentClass(instReportFields[j]);

							Instance[] instReportColumnOptions = oms.GetRelatedInstances(instReportColumns[j], KnownRelationshipGuids.Report_Column__has__Report_Column_Option);
							bool displayAsCount = oms.InstanceSetContains(instReportColumnOptions, KnownInstanceGuids.ReportColumnOptions.DisplayAsCount);

							if (instReportFieldClass != null)
							{
								if (instReportFieldClass.GlobalIdentifier == KnownInstanceGuids.Classes.PrimaryObjectReportField)
								{
									row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(oms.GetInstanceKey(instReportColumns[j]), oms.GetInstanceText(insts[i]), oms.GetInstanceKey(insts[i])));
								}
								else if (instReportFieldClass.GlobalIdentifier == KnownInstanceGuids.Classes.RelationshipReportField)
								{
									Instance instRelationshipTarget = oms.GetRelatedInstance(instReportFields[j], KnownRelationshipGuids.Relationship_Report_Field__has_target__Relationship);
									if (instRelationshipTarget != null)
									{
										Instance[] instRelated = oms.GetRelatedInstances(insts[i], instRelationshipTarget);
										if (instRelated == null)
										{
											row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, String.Format("UNKNOWN REL INST for {0} and {1}", insts[i].GlobalIdentifier, instRelationshipTarget.GlobalIdentifier)));
										}
										else
										{
											InstanceKey[] instanceKeys = new InstanceKey[instRelated.Length];
											for (int k = 0; k < instRelated.Length; k++)
											{
												instanceKeys[k] = oms.GetInstanceKey(instRelated[k]);
											}
											row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, null, instanceKeys) { DisplayAsCount = displayAsCount });
										}
									}
									else
									{
										row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, "ERR#!`Relationship Report Field.has target Relationship` is empty"));
									}
								}
								else if (instReportFieldClass.GlobalIdentifier == KnownInstanceGuids.Classes.AttributeReportField)
								{
									Instance instAttributeTarget = oms.GetRelatedInstance(instReportFields[j], KnownRelationshipGuids.Attribute_Report_Field__has_target__Attribute);
									row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, oms.GetAttributeValue<string>(insts[i], instAttributeTarget)));
								}
								else if (instReportFieldClass.GlobalIdentifier == KnownInstanceGuids.Classes.ReportField)
								{
									string valstr = null;
									object value = oms.ExecuteReportField(instReportFields[j], insts[i]);
									InstanceKey[] ik = null;
									if (value is Instance)
									{
										valstr = oms.GetInstanceText((value as Instance));
										ik = new InstanceKey[] { oms.GetInstanceKey((value as Instance)) };
									}
									else if (value is Instance[])
									{
										valstr = null;
										ik = new InstanceKey[(value as Instance[]).Length];
										for (int k = 0; k < ((Instance[])value).Length; k++)
										{
											ik[k] = oms.GetInstanceKey(((Instance[])value)[k]);
										}
									}
									else if (value != null)
									{
										valstr = value.ToString();
									}
									if (ik == null)
									{
										row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, valstr));
									}
									else
									{
										row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, valstr, ik));
									}
								}
								else
								{
									row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, "ERR#!"));
								}
							}
							else
							{
								row.Columns.Add(new OMSComponents.OMSDetailComponent.OMSDetailRowColumn(rcKey, "ERR#!`Report Field . for Class` should not be empty!"));
							}
						}
						detail.Rows.Add(row);
					}
				}

				resp.Components.Add(detail);
				return resp;
			}
			else if (classInstance.GlobalIdentifier == KnownInstanceGuids.Classes.UITask)
			{
				if (!context.IsPostback)
				{
					Instance instInitiatingElement = oms.GetRelatedInstance(instance, KnownRelationshipGuids.Task__has_initiating__Element);
					IOmsResponse respPrompts = oms.ExecuteInstanceElement(context, instance, instInitiatingElement);
					if (respPrompts != null)
						return respPrompts;
				}
				else
				{
					// FIXME: here is where we actually execute the task processes using promptValues
					Instance[] methodCalls = oms.GetRelatedInstances(instance, KnownRelationshipGuids.Task__executes__Method_Call);
					foreach (Instance inst in methodCalls)
					{
						oms.ExecuteInstance(context, inst, null);
					}
				}
			}
			else if (classInstance.GlobalIdentifier == KnownInstanceGuids.Classes.Page)
			{
				OmsPageResponse page = new OmsPageResponse();
				Instance[] instComponents = oms.GetRelatedInstances(instance, KnownRelationshipGuids.Page__has__Page_Component);
				for (int i = 0; i < instComponents.Length; i++)
				{
					OMSComponent component = oms.CreatePageComponent(instComponents[i], context);
					if (component == null)
						continue;

					page.Components.Add(component);
				}
				return page;
			}
			return null;
		}

		private static OMSComponent CreatePageComponent(this Oms oms, Instance instance, OmsContext context)
		{
			Instance instParentClass = oms.GetParentClass(instance);
			return OmsPageBuilder.CreatePageComponent(oms, instance, context);
		}

		private static void InsertElement(Oms oms, OmsPageResponse resp, Instance[] elementContents)
		{
			OMSComponents.OMSSummaryComponent summaryPrompts = null;
			for (int i = 0; i < elementContents.Length; i++)
			{
				Instance instInstance = oms.GetRelatedInstance(elementContents[i], KnownRelationshipGuids.Element_Content__has__Instance);
				Instance[] paramAssignments = oms.GetRelatedInstances(elementContents[i], KnownRelationshipGuids.Element_Content__has__Parameter_Assignment);

				for (int j = 0; j < paramAssignments.Length; j++)
				{
					Instance parmAssignsFromMB = oms.GetRelatedInstance(paramAssignments[j], KnownRelationshipGuids.Parameter_Assignment__assigns_from__Method_Binding);
					Instance parmAssignsToParameter = oms.GetRelatedInstance(paramAssignments[j], KnownRelationshipGuids.Parameter_Assignment__assigns_to__Parameter);
					Instance parameterHasDataTypeInstance = oms.GetRelatedInstance(parmAssignsToParameter, KnownRelationshipGuids.Parameter__has_data_type__Instance);

					// TODO: set up the assignments for this Element Content
					OmsContext context = new OmsContext();
					object value = oms.ExecuteMethod(parmAssignsFromMB, context);

					// bool checkdatatype = oms.CheckDataType(value, parameterHasDataTypeInstance);
				}

				string title = oms.GetInstanceText(instInstance);

				Instance[] instDisplayOptions = oms.GetRelatedInstances(elementContents[i], KnownRelationshipGuids.Element_Content__has__Element_Content_Display_Option);
				bool displayAsPageTitle = oms.InstanceSetContains(instDisplayOptions, KnownInstanceGuids.ElementContentDisplayOptions.DisplayAsPageTitle);
				bool notEnterable = oms.InstanceSetContains(instDisplayOptions, KnownInstanceGuids.ElementContentDisplayOptions.NotEnterable);
				bool required = oms.InstanceSetContains(instDisplayOptions, KnownInstanceGuids.ElementContentDisplayOptions.Required);

				if (displayAsPageTitle && resp.DescriptionInstance == InstanceKey.Empty)
				{
					// fixme: get instance value for this element content
					resp.DescriptionInstance = oms.GetInstanceKey(instInstance);
					continue;
				}

				Instance instInstanceClass = oms.GetParentClass(instInstance);

				OMSComponents.OMSSummaryComponent.OMSSummaryField field = null;
				if (instInstanceClass != null)
				{
					if (instInstanceClass.GlobalIdentifier == KnownInstanceGuids.Classes.Element)
					{
						if (summaryPrompts != null)
						{
							resp.Components.Add(summaryPrompts);
							summaryPrompts = null;
						}

						Instance[] instSubElementContents = oms.GetRelatedInstances(instInstance, KnownRelationshipGuids.Element__has__Element_Content);
						InsertElement(oms, resp, instSubElementContents);
					}
					else if (instInstanceClass.GlobalIdentifier == KnownInstanceGuids.Classes.BooleanAttribute)
					{
						if (summaryPrompts == null)
						{
							summaryPrompts = new OMSComponents.OMSSummaryComponent();
							summaryPrompts.Name = "Prompts";
						}
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldBoolean(oms.GetInstanceKey(elementContents[i]), title, null);
					}
					else if (instInstanceClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
					{
						if (summaryPrompts == null)
						{
							summaryPrompts = new OMSComponents.OMSSummaryComponent();
							summaryPrompts.Name = "Prompts";
						}
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(elementContents[i]), title, null);
					}
					else if (instInstanceClass.GlobalIdentifier == KnownInstanceGuids.Classes.NumericAttribute)
					{
						if (summaryPrompts == null)
						{
							summaryPrompts = new OMSComponents.OMSSummaryComponent();
							summaryPrompts.Name = "Prompts";
						}
						// OMSComponents.OMSSummaryComponent.OMSSummaryFieldNumeric field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldNumeric(oms.GetInstanceKey(prompts[i]), title, null);
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(elementContents[i]), title, null);
					}
					else if (instInstanceClass.GlobalIdentifier == KnownInstanceGuids.Classes.DateAttribute)
					{
						if (summaryPrompts == null)
						{
							summaryPrompts = new OMSComponents.OMSSummaryComponent();
							summaryPrompts.Name = "Prompts";
						}
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldDateTime(oms.GetInstanceKey(elementContents[i]), title, null);
					}
					else
					{
						if (summaryPrompts == null)
						{
							summaryPrompts = new OMSComponents.OMSSummaryComponent();
							summaryPrompts.Name = "Prompts";
						}
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance(oms.GetInstanceKey(elementContents[i]), title, null);
						((OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance)field).ValidClassIDs.Add(oms.GetInstanceKey(instInstanceClass));
					}
				}
				else
				{
					field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(elementContents[i]), title, null);
				}

				/*
				Instance parentClass = oms.GetParentClass(prompts[i]);
				if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextPrompt)
				{
					summaryPrompts.Fields.Add(new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), String.Empty));
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.InstancePrompt)
				{
					OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), null);
					Instance[] instValidClasses = oms.GetRelatedInstances(prompts[i], KnownRelationshipGuids.Instance_Prompt__has_valid__Class);
					for (int j = 0; j < instValidClasses.Length; j++)
					{
						field.ValidClassIDs.Add(oms.GetInstanceKey(instValidClasses[j]));
					}
					summaryPrompts.Fields.Add(field);
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ChoicePrompt)
				{
					summaryPrompts.Fields.Add(new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), String.Empty));
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.DatePrompt)
				{
					OMSComponents.OMSSummaryComponent.OMSSummaryFieldDateTime field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldDateTime(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), null);
					summaryPrompts.Fields.Add(field);
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.BooleanPrompt)
				{
					OMSComponents.OMSSummaryComponent.OMSSummaryFieldBoolean field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldBoolean(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant));
					summaryPrompts.Fields.Add(field);
				}
				*/

				if (summaryPrompts != null)
				{
					if (field != null)
					{
						field.Name = "field" + i.ToString();
						field.Required = required;
						field.ReadOnly = notEnterable;
						summaryPrompts.Fields.Add(field);
					}
				}
			}
			if (summaryPrompts != null)
			{
				resp.Components.Add(summaryPrompts);
			}
		}

		private static IOmsResponse ExecuteInstanceElement(this Oms oms, OmsContext context, Instance instance, Instance instElement)
		{
			Instance relInst = context.RelatedInstance;
			context.SetWorkData(KnownInstanceGuids.PromptValueClasses.Related_Instance, relInst);

			OmsPageResponse resp = new OmsPageResponse();
			// Task Layout Page - Gets Page Title from RAMB
			// Get Page Title from RAMB: Task@get Title
			resp.Title = oms.GetTranslationValue(instance, KnownRelationshipGuids.Task__has_title__Translation);
			// Task Layout Page - Gets Page Subtitle from RAMB
			// Get Task Directive / Conclusion Note from RAMB
			resp.Description = oms.GetTranslationValue(instance, KnownRelationshipGuids.Task__has_instructions__Translation);

			Instance[] elementContents = oms.GetRelatedInstances(instElement, KnownRelationshipGuids.Element__has__Element_Content);
			InsertElement(oms, resp, elementContents);
			return resp;
		}
		private static IOmsResponse ExecuteInstancePrompts(this Oms oms, Instance instance, Guid promptRelationshipId)
		{
			OmsPageResponse resp = new OmsPageResponse();

			Instance classInstance = oms.GetParentClass(instance);
			if (classInstance.GlobalIdentifier == KnownInstanceGuids.Classes.StandardReport)
			{
				resp.Title = oms.GetTranslationValue(instance, KnownRelationshipGuids.Report__has_title__Translatable_Text_Constant);
				resp.Description = oms.GetTranslationValue(instance, KnownRelationshipGuids.Report__has_description__Translatable_Text_Constant);
			}
			else if (classInstance.GlobalIdentifier == KnownInstanceGuids.Classes.UITask)
			{
				resp.Title = oms.GetTranslationValue(instance, KnownRelationshipGuids.Task__has_title__Translation);
				resp.Description = oms.GetTranslationValue(instance, KnownRelationshipGuids.Task__has_instructions__Translation);
			}

			OMSComponents.OMSSummaryComponent summaryPrompts = new OMSComponents.OMSSummaryComponent();
			summaryPrompts.Name = "Prompts";

			Instance[] prompts = oms.GetRelatedInstances(instance, promptRelationshipId);
			if (prompts.Length == 0)
				return null;

			for (int i = 0; i < prompts.Length; i++)
			{
				Instance instReportField = oms.GetRelatedInstance(prompts[i], KnownRelationshipGuids.Prompt__has__Report_Field);
				Instance instSourceMethodBinding = oms.GetRelatedInstance(instReportField, KnownRelationshipGuids.Report_Field__has_source__Method);
				Instance instSourceMethod = oms.GetRelatedInstance(instSourceMethodBinding, KnownRelationshipGuids.Method_Binding__for__Method);
				Instance instMethodReturnDataType = oms.GetRelatedInstance(instSourceMethod, KnownRelationshipGuids.Method__has_return_type__Class);

				string title = oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant);

				OMSComponents.OMSSummaryComponent.OMSSummaryField field = null;
				if (instMethodReturnDataType != null)
				{
					if (instMethodReturnDataType.GlobalIdentifier == KnownInstanceGuids.Classes.BooleanAttribute)
					{
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldBoolean(oms.GetInstanceKey(prompts[i]), title, null);
					}
					else if (instMethodReturnDataType.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
					{
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), title, null);
					}
					else if (instMethodReturnDataType.GlobalIdentifier == KnownInstanceGuids.Classes.NumericAttribute)
					{
						// OMSComponents.OMSSummaryComponent.OMSSummaryFieldNumeric field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldNumeric(oms.GetInstanceKey(prompts[i]), title, null);
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), title, null);
					}
					else if (instMethodReturnDataType.GlobalIdentifier == KnownInstanceGuids.Classes.DateAttribute)
					{
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldDateTime(oms.GetInstanceKey(prompts[i]), title, null);
					}
					else
					{
						field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance(oms.GetInstanceKey(prompts[i]), title, null);
						((OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance)field).ValidClassIDs.Add(oms.GetInstanceKey(instMethodReturnDataType));
					}
				}
				else
				{
					field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), title, null);
					field.ReadOnly = true;
				}

				/*
				Instance parentClass = oms.GetParentClass(prompts[i]);
				if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextPrompt)
				{
					summaryPrompts.Fields.Add(new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), String.Empty));
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.InstancePrompt)
				{
					OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldInstance(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), null);
					Instance[] instValidClasses = oms.GetRelatedInstances(prompts[i], KnownRelationshipGuids.Instance_Prompt__has_valid__Class);
					for (int j = 0; j < instValidClasses.Length; j++)
					{
						field.ValidClassIDs.Add(oms.GetInstanceKey(instValidClasses[j]));
					}
					summaryPrompts.Fields.Add(field);
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ChoicePrompt)
				{
					summaryPrompts.Fields.Add(new OMSComponents.OMSSummaryComponent.OMSSummaryFieldText(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), String.Empty));
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.DatePrompt)
				{
					OMSComponents.OMSSummaryComponent.OMSSummaryFieldDateTime field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldDateTime(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant), null);
					summaryPrompts.Fields.Add(field);
				}
				else if (parentClass.GlobalIdentifier == KnownInstanceGuids.Classes.BooleanPrompt)
				{
					OMSComponents.OMSSummaryComponent.OMSSummaryFieldBoolean field = new OMSComponents.OMSSummaryComponent.OMSSummaryFieldBoolean(oms.GetInstanceKey(prompts[i]), oms.GetTranslationValue(prompts[i], KnownRelationshipGuids.Prompt__has_title__Translatable_Text_Constant));
					summaryPrompts.Fields.Add(field);
				}
				*/

				if (field != null)
				{
					field.Name = "field" + i.ToString();
					summaryPrompts.Fields.Add(field);
				}
			}
			resp.Components.Add(summaryPrompts);
			return resp;
		}

		public static Instance[] InstanceSetFilter(this Oms oms, Instance[] instanceSet, OmsFilterCondition[] filterConditions)
		{
			return null;
		}

		public static bool InstanceSetContains(this Oms oms, Instance[] instanceSet, Instance instance)
		{
			return InstanceSetContains(oms, instanceSet, instance.GlobalIdentifier);
		}
		public static bool InstanceSetContains(this Oms oms, Instance[] instanceSet, Guid globalIdentifier)
		{
			for (int i = 0; i < instanceSet.Length; i++)
			{
				if (instanceSet[i].GlobalIdentifier == globalIdentifier)
					return true;
			}
			return false;
		}

		public static bool IsClassSubclassOf(this Oms oms, Instance instClass, Instance instParentClass)
		{
			return oms.IsClassSubclassOf(instClass, instParentClass.GlobalIdentifier);
		}
		public static bool IsClassSubclassOf(this Oms oms, Instance instClass, Guid guidParentClass)
		{
			Instance[] insts = oms.GetRelatedInstances(instClass, KnownRelationshipGuids.Class__has_super__Class);
			if (oms.InstanceSetContains(insts, guidParentClass))
			{
				return true;
			}
			return false;
		}

	}
}