using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using MBS.Web;
using MBS.Web.Controls;
using Mocha.Core;
using Mocha.OMS;
using Mocha.OMS.OMSComponents;
using Mocha.Web.Controls;
using UniversalEditor.ObjectModels.PropertyList;

namespace Mocha.Web
{
	/*
	 * Instance Security brainstorming:
	 * 
	 * An Instance's security is defined by its Class.
	 * For example:
	 * 		A Business may only be visible to people within that Business, but also
	 * 		(in the case of the City of Orlando) to City officials administering
	 * 		the application.
	 * 
	 * Security Policy
	 * 		.for Instance*
	 * 		.has Boolean Attribute `Visible`
	 * 		.has Boolean Attribute `Editable` 
	 * 		.has Security Group*
	 *  
	 * The Security Policy is applied to all associated Instances and Security
	 * Groups. To determine if a particular User is authorized to see an Instance:
	 * 
	 *		Instance@is visible by User parm (GRA)*P [rsmb]:
	 *
	 *			Returns Attribute : Visible
	 *
	 *			Loop on Instance Set: Instance.has Security Policy*
	 *			Get Attribute: Security Policy@get Visible to User parm (BA) *P[ramb]:
	 *				
	 *				if `Set`.`is Set A intersects with Set B`(User.has Security Group, Security Policy.has Security Group): return true
	 *
	 *						I have		[Security A]		You want	Security B
	 *									Security X						Security C
	 *									Security W						[Security A]
	 * 
	 *				else: return false
	 *
	 *
	 */ 

    public partial class InstancePage : System.Web.UI.Page
    {
		private const string PROMPT_VALUE_PREFIX = "ctl00$ctl00$aspcContent$aspcContent$SummaryComponent_0$0_";

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			pnlDashboardEmpty.Visible = true;
			pnlDashboardContent.Visible = false;

			object oCid = RouteData.Values["ClassID"];
			object oIid = RouteData.Values["InstanceID"];
			object rCid = RouteData.Values["RelatedClassID"];
			object rIid = RouteData.Values["RelatedInstanceID"];
			string paramzb64 = (string)RouteData.Values["Parameters"];

			Oms oms = this.GetOMS();
			if (oms == null) return;

			Page.Title = this.FormatPageTitle("Welcome");

			if (oCid == null && oIid == null)
			{
				Instance instUser = null;
				string userTitle = "New User";
				object oToken = this.GetTenantedVariable("LoginToken");
				if (oToken != null)
				{
					LoginTokenInfo token = (LoginTokenInfo)oToken;
					instUser = token.UserInstance;
					userTitle = oms.GetTTC(token.UserInstance, oms.GetInstance(KnownRelationshipGuids.User__has_display_name__Translatable_Text_Constant));
				}
				Master.PageTitle = String.Format("Welcome, {0}!", userTitle);

				// display dashboard
				if (instUser != null)
				{
					Instance instPage = oms.GetRelatedInstance(instUser, KnownRelationshipGuids.User__has_default__Page);
					if (instPage != null)
					{
						string pageTitle = oms.GetTTC(instPage, oms.GetInstance(KnownRelationshipGuids.Page__has_title__Translation), null);
						if (pageTitle == null)
						{
							pageTitle = String.Format("Welcome, {0}!", userTitle);
						}
						Master.PageTitle = pageTitle;

						PageBuilder pb = new PageBuilder((SessionContext)this.GetTenantedVariable("SessionContext"));
						pb.RenderPage(instPage, pnlDashboardContent);

						pnlDashboardEmpty.Visible = false;
						pnlDashboardContent.Visible = true;
						Master.PageFooterVisible = false;
					}
				}
				return;
			}

			int cid = -1, iid = -1;
			if (!(Int32.TryParse(oCid.ToString(), out cid) && Int32.TryParse(oIid.ToString(), out iid)))
			{
				label.Text = "parse failed";
				return;
			}

			if (Request.Url.Segments.Length > 2 && Request.Url.Segments[2] == "inst/")
			{
				Response.Clear();
				Response.ContentType = "application/json";

				if (!this.HasTenantedVariable("LoginToken") || DateTime.Now > ((LoginTokenInfo)this.GetTenantedVariable("LoginToken")).Expires)
				{
					Response.Status = "401 Unauthorized";
					Response.Write("{ \"type\": \"error\",\"code\": 401, \"title\": \"Unauthorized\", \"description\": \"You are not logged in\" }");
					Response.End();
					return;
				}
				
				// send the request to the OMS
				Instance p_inst = oms.GetInstance(new InstanceKey(cid, iid));
				if (p_inst == null)
				{
					Response.Status = "404 Not Found";
					Response.Write("{ \"type\": \"error\", \"code\": 404, \"title\": \"Not Found\", \"description\": \"The requested resource was not found\" }");
					Response.End();
					return;
				}

				OmsContext v_context = new OmsContext();
				Instance rel_task = null;
				IOmsResponse v_resp = oms.ExecuteInstance(v_context, p_inst, rel_task);

				Response.Write(v_resp.ToJSONString());
				Response.End();
				return;
			}

			Instance inst = oms.GetInstance(new InstanceKey(cid, iid));
			Instance instModule = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Instance__for__Module);

			Instance instRel = null;
			if (rCid != null && rIid != null)
			{
				int rcid = -1, riid = -1;
				if (!(Int32.TryParse(rCid.ToString(), out rcid) && Int32.TryParse(rIid.ToString(), out riid)))
				{
					label.Text = "parse failed";
					return;
				}
				instRel = oms.GetInstance(new InstanceKey(rcid, riid));
			}

			Instance instClass = oms.GetParentClass(inst);

			OmsContext context = new OmsContext();
			context.GlobalVariables.Add(new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst));
			context.GlobalVariables.Add(new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.Related_Instance), instRel));
			context.CallStack.Push(new OmsCallStack(oms.GetInstanceKey(inst), new OmsVariable[]
			{
				new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst)
			}));

			if (inst != null)
			{
				if (inst.GlobalIdentifier == KnownInstanceGuids.Tasks.OpenDefinitionInCodeEditor)
				{
					Response.Redirect(String.Format("http://localhost:63320/node?ref={0}&tenant={1}", instRel.GlobalIdentifier.ToString(), oms.TenantName));
					return;
				}
				else if (inst.GlobalIdentifier == new Guid("{c1aafaf7-835b-4bc4-9fd8-cc57055cc3d1}"))
				{
					if (instRel != null)
					{
						ibTestMethodBinding.InstanceReferences.Add(oms.GetInstanceKey(instRel));

						Instance[] instParms = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Method__has__Method_Parameter);
						for (int i = 0; i < instParms.Length; i++)
						{
							FormViewItemInstance fvi1 = new FormViewItemInstance();
							fvi1.Title = oms.GetInstanceText(instParms[i]);
							// fvi1.ValidClassIDs.Add(oms.GetInstanceKey(oms.GetInstance(KnownInstanceGuids.Classes.Class)));
							fvTestMethodBindingParameters.Items.Add(fvi1);
						}

						pnlDashboard.Visible = false;
						pnlTestMethodBinding.Visible = true;

						if (this.IsPostBack)
						{
							object value = oms.ExecuteMethod(instRel, context, new OmsVariable[]
							{
								new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst)
							});

							Instance instAttrClass = null;
							if (value is bool)
							{
								instAttrClass = oms.GetInstance(KnownInstanceGuids.Classes.BooleanAttribute);
							}
							else if (value is DateTime)
							{
								instAttrClass = oms.GetInstance(KnownInstanceGuids.Classes.DateAttribute);
							}
							else if (value is string)
							{
								instAttrClass = oms.GetInstance(KnownInstanceGuids.Classes.TextAttribute);
							}
							else if (value is decimal)
							{
								instAttrClass = oms.GetInstance(KnownInstanceGuids.Classes.NumericAttribute);
							}
							else if (value is Instance)
							{
								instAttrClass = oms.GetParentClass((Instance)value);
							}
							else if (value is Instance[])
							{
								instAttrClass = oms.GetParentClass(((Instance[])value)[0]);
							}
							else
							{
								instAttrClass = null;
							}

							fvTestMethodBinding.Items["fviTestMethodBindingID"].Value = instRel.GlobalIdentifier.ToString("B");
							if (instAttrClass != null)
							{
								((FormViewItemInstance)fvTestMethodBinding.Items["fviTestMethodBindingReturnType"]).SelectedInstances.Add(oms.GetInstanceKey(instAttrClass));
							}
							pnlTestMethodBindingReturned.Visible = true;

							if (value is Instance[] || value is Instance)
							{
								FormViewItemInstance fvii = new FormViewItemInstance() { Title = "Value" };
								List<InstanceKey> keyz = new List<InstanceKey>();
								if (value is Instance)
								{
									fvii.SelectedInstances.Add(oms.GetInstanceKey((Instance)value));
								}
								else if (value is Instance[])
								{
									for (int i = 0; i < ((Instance[])value).Length; i++)
									{
										fvii.SelectedInstances.Add(oms.GetInstanceKey(((Instance[])value)[i]));
									}
								}
								fvTestMethodBinding.Items.Add(fvii);
							}
							else
							{
								fvTestMethodBinding.Items.Add(new FormViewItemText() { Title = "Value", Value = value?.ToString() });
							}
						}
						return;
					}
				}
			}

			string taskTitle = null;
			if (inst == null)
			{
				taskTitle = "not found";
				return;
			}
			else
			{
				taskTitle = oms.GetInstanceText(inst);
			}
			label.Text = taskTitle;
			Page.Title = this.FormatPageTitle(taskTitle);

			pnlTask.Visible = true;
			pnlDashboard.Visible = false;
			pnlDashboardContent.Visible = false;

			if (instClass.GlobalIdentifier == KnownInstanceGuids.Classes.Class)
			{
				Master.PageTitle = "View Class";
				Master.PageSubtitleInstance = oms.GetInstanceKey(inst);

				pnlTask.Visible = false;
				pnlClass.Visible = true;

				fvStructure.Items["fvStructure_Name"].Value = oms.GetInstanceText(inst);
				fvStructure.Items["fvStructure_GlobalIdentifier"].Value = inst.GlobalIdentifier.ToString("B");

				if (instModule != null)
				{
					((FormViewItemInstance)fvStructure.Items["fvStructure_Module"]).SelectedInstances.Add(oms.GetInstanceKey(instModule));
				}

				Instance[] instAttrs = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Class__has__Attribute);
				for (int i = 0; i < instAttrs.Length; i++)
				{
					// attributes
					ListViewItem lvi = new ListViewItem();
					ListViewItemColumnInstance lvc = new ListViewItemColumnInstance();
					lvc.ColumnID = "lvcAttribute";
					lvc.InstanceIDs.Add(oms.GetInstanceKey(instAttrs[i]));
					lvi.Columns.Add(lvc);

					lvc = new ListViewItemColumnInstance();
					lvc.ColumnID = "lvcAttributeType";
					lvc.InstanceIDs.Add(oms.GetInstanceKey(oms.GetParentClass(instAttrs[i])));
					lvi.Columns.Add(lvc);
					lvAttributes.Items.Add(lvi);

					ListViewColumn lvcAttribute = new ListViewColumn() { Title = oms.GetInstanceText(instAttrs[i]), ID = String.Format("lvcAttribute{0}", i) };
					lvInstances.Columns.Add(lvcAttribute);
				}

				Instance[] instRelationships = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Class__has__Relationship);
				for (int i = 0; i < instRelationships.Length; i++)
				{
					ListViewItem lvi = new ListViewItem();
					ListViewItemColumnInstance lvic1 = new ListViewItemColumnInstance() { ColumnID = "lvcRelationship" };
					lvic1.InstanceIDs.Add(oms.GetInstanceKey(instRelationships[i]));
					lvi.Columns.Add(lvic1);
					lvRelationships.Items.Add(lvi);
				}

				Instance[] instSuperclasses = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Class__has_super__Class);
				for (int i = 0; i < instSuperclasses.Length; i++)
				{
					if (instSuperclasses[i] != null)
					{
						((FormViewItemInstance)fvInheritance.Items["fvInheritance_Superclasses"]).SelectedInstances.Add(oms.GetInstanceKey(instSuperclasses[i]));
					}
				}
				Instance[] instSubclasses = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Class__has_sub__Class);
				for (int i = 0; i < instSubclasses.Length; i++)
				{
					((FormViewItemInstance)fvInheritance.Items["fvInheritance_Subclasses"]).SelectedInstances.Add(oms.GetInstanceKey(instSubclasses[i]));
				}

				Instance[] insts = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Class__has__Instance);
				for (int i = 0; i < insts.Length; i++)
				{
					ListViewItem lvi = new ListViewItem();
					ListViewItemColumnInstance lvc = new ListViewItemColumnInstance();
					lvc.ColumnID = "lvcInstance";
					lvc.InstanceIDs.Add(oms.GetInstanceKey(insts[i]));
					lvi.Columns.Add(lvc);

					for (int j = 0; j < instAttrs.Length; j++)
					{
						lvi.Columns.Add(new ListViewItemColumn() { ColumnID = String.Format("lvcAttribute{0}", j), Value = oms.GetAttributeValue(insts[i], instAttrs[j])?.ToString() });
					}

					lvInstances.Items.Add(lvi);
				}
			}
			else if (oms.IsClassSubclassOf(instClass, KnownInstanceGuids.Classes.Method)
			/* instClass.GlobalIdentifier == KnownInstanceGuids.Classes.BuildAttributeMethod
			|| instClass.GlobalIdentifier == KnownInstanceGuids.Classes.GetAttributeMethod
			|| instClass.GlobalIdentifier == KnownInstanceGuids.Classes.GetSpecificInstancesMethod
			|| instClass.GlobalIdentifier == KnownInstanceGuids.Classes.GetReferencedAttributeMethod
			|| instClass.GlobalIdentifier == KnownInstanceGuids.Classes.GetReferencedInstanceSetMethod
			*/
			)
			{
				pnlTask.Visible = false;
				pnlMethod.Visible = true;
				if (instModule != null)
				{
					((FormViewItemInstance)fvMethodDefinition.Items["fviModule"]).SelectedInstances.Add(oms.GetInstanceKey(instModule));
				}

				if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.GetReferencedAttributeMethod)
				{
					// SAC - Conditional Select Attribute Method
					pnlMethodImplementationGRA.Visible = true;
				}
				else if (instClass.GlobalIdentifier == KnownInstanceGuids.MethodClasses.ConditionalSelectAttributeMethod)
				{
					// SAC - Conditional Select Attribute Method
					pnlMethodImplementationSAC.Visible = true;

					Instance[] instCases = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Conditional_Method__has__Conditional_Method_Case);
					foreach (Instance instCase in instCases)
					{
						Instance instExecutableReturningAttribute = oms.GetRelatedInstance(instCase, KnownRelationshipGuids.Conditional_Select_Attribute_Case__invokes__Executable_returning_Attribute);
					}
				}

				Master.PageTitle = String.Format("View {0}", oms.GetInstanceText(instClass));
				Master.PageSubtitle = oms.GetInstanceText(inst);
				Master.PageSubtitleInstance = oms.GetInstanceKey(inst);

				Instance instMethod_for_Class = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Method__for__Class);
				((FormViewItemInstance)fvMethodDefinition.Items["fviForClass"]).SelectedInstances.Add(oms.GetInstanceKey(instMethod_for_Class));

				fvMethodDefinition.Items["fviVerb"].Value = oms.GetAttributeValue(inst, KnownAttributeGuids.Text.Verb)?.ToString();
				fvMethodDefinition.Items["fviName"].Value = oms.GetAttributeValue(inst, KnownAttributeGuids.Text.Name)?.ToString();
				// fvMethodDefinition.Items["fviAccess"].Value = oms.GetAttributeValue(inst, KnownAttributeGuids.Text.Verb)?.ToString();

				// Instance instMethod_for_Module = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Method__for__Module);
				// ((FormViewItemInstance)fvMethodDefinition.Items["fviModule"]).SelectedInstances.Add(oms.GetInstanceKey(instMethod_for_Module));

				Instance[] instMethodParms = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Method__has__Method_Parameter);
				for (int i = 0; i < instMethodParms.Length; i++)
				{
					bool required = false, nullAllowed = false;
					required = oms.GetAttributeValue<bool>(instMethodParms[i], KnownAttributeGuids.Boolean.Required);
					nullAllowed = oms.GetAttributeValue<bool>(instMethodParms[i], KnownAttributeGuids.Boolean.Null);

					if (required && nullAllowed)
					{
						((FormViewItemInstance)fvMethodParms.Items["fviRequiredParmsNullAllowed"]).SelectedInstances.Add(oms.GetInstanceKey(instMethodParms[i]));
					}
					else if (required)
					{
						((FormViewItemInstance)fvMethodParms.Items["fviRequiredParmsNotNull"]).SelectedInstances.Add(oms.GetInstanceKey(instMethodParms[i]));
					}
					else
					{
						((FormViewItemInstance)fvMethodParms.Items["fviOptionalParms"]).SelectedInstances.Add(oms.GetInstanceKey(instMethodParms[i]));
					}
				}

				Instance instReturnAttribute = null;
				instReturnAttribute = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Method__returns__Attribute);
				if (instReturnAttribute == null)
				{
					instReturnAttribute = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Get_Attribute_Method__has__Attribute);
				}
				if (instReturnAttribute != null)
				{
					(fvMethodReturn.Items["fviReturnAttribute"] as FormViewItemInstance).SelectedInstances.Add(oms.GetInstanceKey(instReturnAttribute));
				}
			}
			else if (oms.IsClassSubclassOf(instClass, KnownInstanceGuids.Classes.MethodBinding))
			{
				pnlTask.Visible = false;
				pnlMethodBinding.Visible = true;

				Master.PageTitle = String.Format("View {0}", oms.GetInstanceText(instClass));
				Master.PageSubtitleInstance = oms.GetInstanceKey(inst);

				Instance instExecutesMethod = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Method_Binding__for__Method);
				((FormViewItemInstance)fvMethodBindingDefinition.Items["fviMethodBindingExecutesMethod"]).SelectedInstances.Add(oms.GetInstanceKey(instExecutesMethod));

				Instance[] instMethodParms = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Method_Binding__has__Parameter_Assignment);
				for (int i = 0; i < instMethodParms.Length; i++)
				{
					ListViewItem lvi = new ListViewItem();

					Instance instAssignsFrom = oms.GetRelatedInstance(instMethodParms[i], KnownRelationshipGuids.Parameter_Assignment__assigns_from__Method_Binding);
					Instance instAssignsTo = oms.GetRelatedInstance(instMethodParms[i], KnownRelationshipGuids.Parameter_Assignment__assigns_to__Parameter);

					// FIXME: the loop requires these to be in the same order as defined on the ListView, this should NOT be a requirement
					ListViewItemColumnInstance lvicAssignsTo = new ListViewItemColumnInstance() { ColumnID = "colMethodBindingParmAssignsTo" };
					lvicAssignsTo.InstanceIDs.Add(oms.GetInstanceKey(instAssignsTo));
					lvi.Columns.Add(lvicAssignsTo);

					ListViewItemColumnInstance lvicAssignsFrom = new ListViewItemColumnInstance() { ColumnID = "colMethodBindingParmAssignsFrom" };
					lvicAssignsFrom.InstanceIDs.Add(oms.GetInstanceKey(instAssignsFrom));
					lvi.Columns.Add(lvicAssignsFrom);

					lvMethodBindingParameters.Items.Add(lvi);
				}

				return;
			}
			else if (instClass.GlobalIdentifier == KnownInstanceGuids.Classes.ElementContent)
			{
				pnlTask.Visible = false;
				pnlElementContent.Visible = true;

				Master.PageTitle = String.Format("View {0}", oms.GetInstanceText(instClass));
				Master.PageSubtitleInstance = oms.GetInstanceKey(inst);

				Instance instElement = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Element_Content__for__Element);
				InstanceKey ikElement = oms.GetInstanceKey(instElement);
				(fvElementContent.Items["fvElementContent_ForElement"] as FormViewItemInstance).SelectedInstances.Add(ikElement);

				Instance instInstance = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Element_Content__has__Instance);
				if (instInstance != null)
				{
					InstanceKey ikInstance = oms.GetInstanceKey(instInstance);
					(fvElementContent.Items["fvElementContent_DefaultDataType"] as FormViewItemInstance).SelectedInstances.Add(ikInstance);
					(fvElementContent.Items["fvElementContent_DataTypeOverride"] as FormViewItemInstance).SelectedInstances.Add(ikInstance);
				}

				Instance[] instValidations = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Element_Content__has__Validation);
				foreach (Instance instValidation in instValidations)
				{
					Instance instValidationClassification = oms.GetRelatedInstance(instValidation, KnownRelationshipGuids.Validation__has__Validation_Classification);

					ListViewItemColumnInstance lvicTrueConditions = new ListViewItemColumnInstance() { ColumnID = "lvcElementContentDerivedValidationsTrueConditions" };
					Instance[] trueConditions = oms.GetRelatedInstances(instValidation, KnownRelationshipGuids.Validation__has_true_condition__Executable);
					foreach (Instance trueCondition in trueConditions)
					{
						lvicTrueConditions.InstanceIDs.Add(oms.GetInstanceKey(trueCondition));
					}

					ListViewItemColumnInstance lvicFalseConditions = new ListViewItemColumnInstance() { ColumnID = "lvcElementContentDerivedValidationsFalseConditions" };
					Instance[] falseConditions = oms.GetRelatedInstances(instValidation, KnownRelationshipGuids.Validation__has_false_condition__Executable);
					foreach (Instance falseCondition in falseConditions)
					{
						lvicFalseConditions.InstanceIDs.Add(oms.GetInstanceKey(falseCondition));
					}

					lvElementContentDerivedValidations.Items.Add(new ListViewItem(new ListViewItemColumn[]
					{
						new ListViewItemColumn() { ColumnID = "lvcElementContentDerivedValidationsValidation" },
						new ListViewItemColumnInstance() { ColumnID = "lvcElementContentDerivedValidationsValidationClassification", InstanceIDs = new List<InstanceKey>(new InstanceKey[] { oms.GetInstanceKey(instValidationClassification) })},
						lvicTrueConditions,
						lvicFalseConditions,
						new ListViewItemColumn() { ColumnID="lvcElementContentDerivedValidationsUseAnyCondition", Value = ""},
						new ListViewItemColumn() { ColumnID="lvcElementContentDerivedValidationsOnlyOnSubmit", Value = (oms.GetAttributeValue<bool>(instValidation, KnownAttributeGuids.Boolean.ValidateOnlyOnSubmit) ? "Yes" : String.Empty)},
						new ListViewItemColumnInstance() { ColumnID = "lvcElementContentDerivedValidationsErrorWordBucket", InstanceIDs = new List<InstanceKey>(new InstanceKey[] { oms.GetInstanceKey(oms.GetTranslation(instValidation, KnownRelationshipGuids.Validation__has_failure_message__Translation)) }) }
					}));
				}
			}

			if (IsPostBack)
			{
				// fill in the prompt values from the posted back page
				foreach (string key in Request.Form)
				{
					if (key.StartsWith(PROMPT_VALUE_PREFIX))
					{
						string qiid = key.Substring(PROMPT_VALUE_PREFIX.Length);
						string value = Request.Form[key];
						if (qiid.Contains("_"))
						{
							int index = qiid.IndexOf("_");
							string type = qiid.Substring(index);
							if (type == "Hidden")
							{
							}
							else
							{
								continue;
							}
							qiid = qiid.Substring(0, index);
						}
						context.GlobalVariables.Add(new OmsVariable(oms.GetInstance(InstanceKey.Parse(qiid)), value));
					}
				}
			}

			context.IsPostback = this.IsPostBack;
			context.RelatedInstance = instRel;

			IOmsResponse resp = oms.ExecuteInstance(context, inst, null);
			if (resp != null)
			{
				Page.Title = this.FormatPageTitle(resp.Title);
				this.RegisterScript("~/Scripts/Task.js");

				Master.PageTitleInstance = oms.GetInstanceKey(inst);
				// Master.PageTitle = resp.Title;
				if (resp.DescriptionInstance != InstanceKey.Empty)
				{
					Master.PageSubtitleInstance = resp.DescriptionInstance;
				}
				else
				{
					Master.PageSubtitle = resp.Description;
				}

				PageBuilder pb = new PageBuilder((SessionContext)this.GetTenantedVariable("SessionContext"));

				foreach (OMSComponent comp in resp.Components)
				{
					Control ctl = pb.RenderOMSComponent(comp);
					ctl.ID = comp.Name;
					if (ctl != null)
						pnlTask.Controls.Add(ctl);
				}


				if (paramzb64 != null)
				{
					string paramz = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(paramzb64));
					PropertyListObjectModel plom = new PropertyListObjectModel();
					UniversalEditor.DataFormats.PropertyList.JavaScriptObjectNotation.JSONDataFormat json = new UniversalEditor.DataFormats.PropertyList.JavaScriptObjectNotation.JSONDataFormat();
					UniversalEditor.Document.Load(plom, json, new UniversalEditor.Accessors.StringAccessor(paramz));

					for (int i = 0; i < plom.Items.Count; i++)
					{
						string val = (string)((plom.Items[i] as Group).Items[1] as Property).Value;
						(pnlTask.Controls[1] as FormView).Items[i].Value = val;
					}
				}
				else
				{
					if (IsPostBack)
					{
						if (pnlTask.Controls[1] is FormView)
						{
							if ((pnlTask.Controls[1] as FormView).Items.Count > 0)
							{
								foreach (string key in Request.Form)
								{
									if (key.StartsWith(PROMPT_VALUE_PREFIX))
									{
										string qiid = key.Substring(PROMPT_VALUE_PREFIX.Length);

										string fvid = String.Format("SummaryComponent_0$0_{0}", qiid);
										(pnlTask.Controls[1] as FormView).Items[fvid].Value = Request.Form[key];
									}
								}

								Instance[] instMethodCalls = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Task__executes__Method_Call);
								// start calling methods associated with this Task
								foreach (Instance instMethodCall in instMethodCalls)
								{
									oms.ExecuteInstance(context, instMethodCall);
								}
							}
						}
					}
				}
			}
		}



		/*
		protected override void Render(HtmlTextWriter writer)
		{
			// if (otask == null) return;
			writer.Write("<!DOCTYPE html>");
			writer.Write("<html>");
			writer.Write("<head>");
			writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + Request.ApplicationPath + "StyleSheets/uwt.css\" />");
			writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + Request.ApplicationPath + "StyleSheets/Slate/uwt.css\" />");
			writer.Write("</head>");
			writer.Write("<body>");
			writer.Write("<div class=\"uwt-header\">&nbsp;</div>");
			writer.Write("<div class=\"uwt-sidebar\">&nbsp;</div>");
			writer.Write("<div class=\"uwt-page\">");


			// page content goes here
			writer.Write("<h1>View Instance <a href=\"#\">" + oname + "</a></h1>");

			writer.Write("</div>");
			writer.Write("<div class=\"uwt-footer\">&nbsp;</div>");
			writer.Write("</body>");
			writer.Write("</html>");
			writer.Flush();
		}
		*/
	}
	}
