using System;
using System.Web;
using System.Web.UI;

using MBS.Web;
using MBS.Framework.Security.Cryptography;
using Mocha.OMS;
using Mocha.Core;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Mocha.Web
{
    public partial class LoginPage : System.Web.UI.Page
	{

		private Oms oms = null;

		private System.Security.Cryptography.HashAlgorithm hasher = new System.Security.Cryptography.SHA512Managed();

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			this.AddCssClass("LoginPage");

			this.AddCssClass("uwt-hide-sidebar");
			this.AddCssClass("uwt-hide-header");

			this.Page.Title = "Login";

			this.Response.Headers.Remove("Server");
			this.Response.Headers["Server"] = "Mocha User Interface Service";
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			oms = (Global.OMS as Oms);
			if (oms == null)
			{
				/*
				fv.Visible = false;

				lblPageHeaderText.Text = "System Error";
				lblLoginHeaderText.Text = "Cannot Connect to OMS";
				lblLoginFooterText.Text = "Cannot connect to the OMS. If you are a system administrator, please check your configuration. Otherwise, contact your organization's support center for assistance.";

				pnl.FooterControls.Visible = false;
				*/			
				return;
			}

			oms.TenantName = Master.GetCurrentTenantName();

			Instance instTenant = oms.GetTenantInstance();
			if (instTenant == null)
			{
				if (Request.CurrentExecutionFilePathExtension == ".json")
				{
					Response.Clear();
					Response.ContentType = "application/json";

					Response.Write("{ \"result\": \"error\", \"title\": \"System Error\", \"subtitle\": \"Tenant Not Found\", \"message\": \"The specified tenant '{0}' was not found on this server. Please check the spelling and try again, or contact your organization's support center for assistance.\" }");
					Response.End();
					return;
				}
				else
				{
					// lblTaskTitle.Text = "Tenant not found";
					// lblTaskSubtitle.Text = Master.GetCurrentTenantName();
					/*
					fv.Visible = false;

					lblPageHeaderText.Text = "System Error";
					lblLoginHeaderText.Text = "Tenant Not Found";
					lblLoginFooterText.Text = String.Format("The specified tenant '{0}' was not found on this server. Please check the spelling and try again, or contact your organization's support center for assistance.", oms.TenantName);

					pnl.FooterControls.Visible = false;

					HardcodedPageContent.Visible = true;
					PageBuilderPageContent.Visible = false;
					*/				
					return;
				}
			}

			PageBuilder pb = new PageBuilder((SessionContext)this.GetTenantedVariable("SessionContext"));
			Instance instPage = oms.GetInstance(KnownInstanceGuids.Pages.LoginPage);
			Title = this.FormatPageTitle(oms.GetTranslationValue(instPage, KnownRelationshipGuids.Page__has_title__Translation));

			System.Web.UI.HtmlControls.HtmlGenericControl ctlStyle = new System.Web.UI.HtmlControls.HtmlGenericControl("style");
			ctlStyle.Attributes.Add("type", "text/css");
			ctlStyle.InnerHtml = pb.GetPageStyle(instPage);
			this.Header.Controls.Add(ctlStyle);

			Instance[] instStyles = oms.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Style);
			foreach (Instance instStyle in instStyles)
			{

			}

			System.Web.UI.HtmlControls.HtmlGenericControl lbl = new System.Web.UI.HtmlControls.HtmlGenericControl("p");
			lbl.InnerHtml = "app path: " + Request.ApplicationPath + " ; path : " + Request.Path + " ; url: " + Request.RawUrl + " ; pi: " + Request.PathInfo;
			PageBuilderPageContent.Controls.Add(lbl);

			OmsContext context = new OmsContext();
			context.GlobalVariables.Add(new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), instPage));
			context.CallStack.Push(new OmsCallStack(oms.GetInstanceKey(instPage), new OmsVariable[]
			{
				new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), instPage)
			}));

			Instance[] instComponents = oms.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Page_Component);
			for (int i = 0; i < instComponents.Length; i++)
			{
				Control comp = pb.CreatePageComponent(instComponents[i], context);
				pb.ApplyInstIdAndCssClass(comp, instComponents[i]);

				if (comp == null) continue;

				PageBuilderPageContent.Controls.Add(comp);
			}

			if (IsPostBack)
			{
				List<Instance> list = new List<Instance>();
				FindElements(new Instance[] { instPage }, list);

				Dictionary<InstanceKey, object> _ecvals = new Dictionary<InstanceKey, object>();

				foreach (Instance instElem in list)
				{
					Instance[] instElemContents = oms.GetRelatedInstances(instElem, KnownRelationshipGuids.Element__has__Element_Content);
					foreach (Instance instElemContent in instElemContents)
					{
						// Update Condition 
						// Executable returning Work Data
						// Do Not Update on Construct , on UI Change , on Submit
						// if (!`Do Not Update on Submit`) { }
						InstanceKey ikElemContent = oms.GetInstanceKey(instElemContent);
						string val = GetECValue(ikElemContent);
						_ecvals[ikElemContent] = val;
					}
				}

				// FIXME: these are hardcoded values for the ECs
				// we need to actually update the proper element values in context with the correct page
				string username = _ecvals[new InstanceKey(82, 8)]?.ToString();
				string userpass = _ecvals[new InstanceKey(82, 9)]?.ToString();
				string tenant = Master.GetCurrentTenantName();

				string errorTitle = null, errorDescription = null;

				if (username != null)
				{
					// here we go.
					// from here on out, this should be a call to authenticate user method
					// condition:
					// 	User Name is not null and Password is not null

					if (!String.IsNullOrEmpty(userpass))
					{
						Instance clsUser = oms.GetInstance(KnownInstanceGuids.Classes.User);
						Instance[] instUsers = oms.GetInstances(clsUser);

						Instance instAttUserName = oms.GetInstance(KnownAttributeGuids.Text.UserName);
						Instance instAttPasswordHash = oms.GetInstance(KnownAttributeGuids.Text.PasswordHash);
						Instance instAttPasswordSalt = oms.GetInstance(KnownAttributeGuids.Text.PasswordSalt);

						Instance userInstance = null;
						foreach (Instance instUser in instUsers)
						{
							string userName = oms.GetAttributeValue<string>(instUser, instAttUserName);
							if (userName == username)
							{
								string passHash = oms.GetAttributeValue<string>(instUser, instAttPasswordHash);
								string passSalt = oms.GetAttributeValue<string>(instUser, instAttPasswordSalt);

								string hash = hasher.ComputeHash(userpass + passSalt);
								if (passHash == hash)
								{
									userInstance = instUser;
									break;
								}	
							}
						}

						if (userInstance != null)
						{
							TimeSpan ts = new TimeSpan(0, 1, 0);
							DateTime dt = DateTime.Now;

							// this should be stored as an Attribute on the Tenant
							string sDuration = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginToken.Duration"];
							if (sDuration == null) sDuration = "30";
							int iDuration = 30;
							if (!Int32.TryParse(sDuration, out iDuration))
							{
								iDuration = 30;
							}

							// we need to update the LoginToken in the OMS now...
							LoginTokenInfo token = new LoginTokenInfo(Guid.NewGuid(), dt.AddMinutes(iDuration), username, tenant, userInstance);
							this.SetTenantedVariable("LoginToken", token);

							// ... and write an Audit Line for the user login
							WriteUserLoginAuditLine(token, dt, Request.UserHostAddress);


							if (this.HasTenantedVariable("LoginRedirectURL"))
							{
								Response.Redirect(this.GetTenantedVariable("LoginRedirectURL").ToString());
							}
							else
							{
								// remember, this.Redirect is an extension method that automatically adds the proper tenant to the path
								this.Redirect("~/");
							}
						}
						else
						{
							errorTitle = "Incorrect user name or password";
							errorDescription = String.Empty;
						}
					}
					else
					{
						errorTitle = "Please enter a password";
						errorDescription = String.Empty;
					}
				}
				else
				{
					errorTitle = "Please enter a user name";
					errorDescription = String.Empty;
				}

				if (this.IsPostBack)
				{
					if (errorTitle != null || errorDescription != null)
					{
						/*
						pnlError.Visible = true;
						lblErrorTitle.Text = errorTitle;
						lblErrorDescription.Text = errorDescription;
						*/					
					}
				}

			}

		}

		private void FindElements(Instance[] instances, List<Instance> list)
		{
			foreach (Instance inst in instances)
			{
				Instance instPClass = oms.GetParentClass(inst);
				Guid guidRel = Guid.Empty;
				if (instPClass.GlobalIdentifier == KnownInstanceGuids.Classes.Page)
				{
					Instance[] instComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Page__has__Page_Component);
					FindElements(instComponents, list);
				}
				else if (instPClass.GlobalIdentifier == KnownInstanceGuids.Classes.ElementPageComponent)
				{
					Instance instElem = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Element_Page_Component__has__Element);
					list.Add(instElem);
				}
				else if (instPClass.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
				{
					Instance[] instHeaderComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_header__Page_Component);
					FindElements(instHeaderComponents, list);
					Instance[] instContentComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
					FindElements(instContentComponents, list);
					Instance[] instFooterComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
					FindElements(instFooterComponents, list);
				}
				else if (oms.IsClassSubclassOf(instPClass, KnownInstanceGuids.Classes.ContainerPageComponent))
				{
					Instance[] instChildComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Container_Page_Component__has__Page_Component);
					FindElements(instChildComponents, list);
				}
			}
		}

		private string GetECValue(InstanceKey instanceKey)
		{
			return Request.Form[String.Format("ctl00$aspcContent$ec__{0}${1}", instanceKey.ClassIndex, instanceKey.InstanceIndex)];
		}

		private void WriteUserLoginAuditLine(LoginTokenInfo token, DateTime effectiveDate, string ipAddress)
		{
			Instance instUserLogin = oms.CreateInstance(KnownInstanceGuids.Classes.UserLogin);
			oms.AssignRelationship(instUserLogin, KnownRelationshipGuids.User_Login__has__User, new Guid[] { token.UserInstance.GlobalIdentifier }, effectiveDate);
			oms.SetAttributeValue(instUserLogin, KnownAttributeGuids.Text.IPAddress, ipAddress, effectiveDate);
			oms.SetAttributeValue(instUserLogin, KnownAttributeGuids.Text.Token, token.LoginToken.ToString("B"));
		}
	}
}
