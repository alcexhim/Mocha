using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MBS.Web;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web.MasterPages
{
	public partial class Default : MasterPage
	{
		public string PageTitle
		{
			get
			{
				return lblTaskTitle.Text;
			}
			set
			{
				lblTaskTitle.Text = value;
				Page.Title = Page.FormatPageTitle(value);
			}
		}
		public InstanceKey PageTitleInstance
		{
			get
			{
				return ibTaskTitle.InstanceReferences.Count == 1 ? ibTaskTitle.InstanceReferences[0] : InstanceKey.Empty;
			}
			set
			{
				ibTaskTitle.InstanceReferences.Clear();
				if (value != InstanceKey.Empty)
				{
					ibTaskTitle.InstanceReferences.Add(value);
					ibTaskTitle.Visible = true;
					lblTaskTitle.Visible = false;
				}
				else
				{
					ibTaskTitle.Visible = false;
					lblTaskTitle.Visible = true;
				}
			}
		}
		public string PageSubtitle { get { return lblTaskSubtitle.Text; } set { lblTaskSubtitle.Text = value; } }
		public InstanceKey PageSubtitleInstance
		{
			get
			{
				return ibTaskSubtitle.InstanceReferences.Count == 1 ? ibTaskSubtitle.InstanceReferences[0] : InstanceKey.Empty;
			}
			set
			{
				ibTaskSubtitle.InstanceReferences.Clear();
				if (value != InstanceKey.Empty)
				{
					ibTaskSubtitle.InstanceReferences.Add(value);
					ibTaskSubtitle.Visible = true;
					lblTaskSubtitle.Visible = false;
				}
				else
				{
					ibTaskSubtitle.Visible = false;
					lblTaskSubtitle.Visible = true;
				}
			}
		}

		public bool PageFooterVisible { get { return pnlPageFooter.Visible; } set { pnlPageFooter.Visible = value; } }

		private void BuildUserMenu()
		{
			mnuUserMenu.Items.Add(new MBS.Web.Controls.MenuItem() { Text = "Log Out", TargetURL = Page.ExpandRelativePath("~/d/logout.htmld") });
		}
		private void BuildMegaMenuTenanted(Oms oms, Instance instTenant)
		{
			Instance instMenu = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has_mega__Menu);

			if (instMenu != null)
			{
				Instance[] instMenuSections = oms.GetRelatedInstances(instMenu, KnownRelationshipGuids.Menu__has__Menu_Section);
				for (int i = 0; i < instMenuSections.Length; i++)
				{
					HtmlGenericControl divUwtMenuSection = new HtmlGenericControl("div");
					divUwtMenuSection.AddCssClass("uwt-menu-section");

					HtmlGenericControl divUwtTitle = new HtmlGenericControl("div");
					divUwtTitle.AddCssClass("uwt-title");
					divUwtTitle.InnerHtml = oms.GetTranslationValue(instMenuSections[i], KnownRelationshipGuids.Menu_Item__has_title__Translatable_Text_Constant);
					divUwtMenuSection.Controls.Add(divUwtTitle);

					HtmlGenericControl divUwtContent = new HtmlGenericControl("div");
					divUwtContent.AddCssClass("uwt-content");

					HtmlGenericControl ul = new HtmlGenericControl("ul");
					ul.AddCssClass("uwt-menu");

					Instance[] instMenuItems = oms.GetRelatedInstances(instMenuSections[i], KnownRelationshipGuids.Menu_Section__has__Menu_Item);
					for (int j = 0; j < instMenuItems.Length; j++)
					{
						HtmlGenericControl li = BuildMenuItem(oms, instMenuItems[j]);
						if (li != null)
						{
							ul.Controls.Add(li);
						}
					}

					divUwtContent.Controls.Add(ul);

					divUwtMenuSection.Controls.Add(divUwtContent);
					/*
					divUwtTitle = new HtmlGenericControl("div");
					divUwtTitle.AddCssClass("uwt-title");
					divUwtTitle.InnerHtml = "Another section";
					divUwtMenuSection.Controls.Add(divUwtTitle);
					*/
					divMegaMenu.Controls.Add(divUwtMenuSection);
				}
			}
		}

		private HtmlGenericControl BuildMenuItem(Oms oms, Instance inst)
		{
			if (inst == null)
				return null;

			HtmlGenericControl li = new HtmlGenericControl("li");
			li.AddCssClass("uwt-visible");

			Instance instParent = oms.GetParentClass(inst);
			if (instParent.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemHeader)
			{

			}
			else if (instParent.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemInstance)
			{
				Instance relInst = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Instance_Menu_Item__has_target__Instance);
				if (relInst != null)
				{
					HtmlAnchor a = new HtmlAnchor();
					a.HRef = String.Format("~/{0}/d/inst/{1}.htmld", oms.TenantName, oms.GetInstanceKey(relInst));

					string instText = oms.GetTranslationValue(inst, KnownRelationshipGuids.Menu_Item__has_title__Translatable_Text_Constant);
					if (instText == null)
					{
						instText = oms.GetInstanceText(relInst);
					}
					a.InnerText = instText;
					li.Controls.Add(a);
				}
				else
				{
					li.AddCssClass("uwt-disabled");

					HtmlAnchor a = new HtmlAnchor();
					a.HRef = "#";

					string instText = oms.GetTranslationValue(inst, KnownRelationshipGuids.Menu_Item__has_title__Translatable_Text_Constant);
					a.InnerText = instText;
					li.Controls.Add(a);
				}
			}
			return li;
		}

		private void CreateMegaMenuSection(string title, System.Web.UI.WebControls.LinkButton[] linkButtons)
		{
			HtmlGenericControl ul = new HtmlGenericControl("ul");
			ul.AddCssClass("uwt-menu");

			foreach (System.Web.UI.WebControls.LinkButton button in linkButtons)
			{
				HtmlGenericControl li = new HtmlGenericControl("li");
				li.AddCssClass("uwt-visible");
				li.Controls.Add(button);
				ul.Controls.Add(li);
			}

			CreateMegaMenuSection(title, ul);
		}
		private void CreateMegaMenuSection(string title, System.Web.UI.Control control)
		{
			HtmlGenericControl divUwtMenuSection = new HtmlGenericControl("div");
			divUwtMenuSection.AddCssClass("uwt-menu-section");

			HtmlGenericControl divUwtTitle = new HtmlGenericControl("div");
			divUwtTitle.AddCssClass("uwt-title");
			divUwtTitle.InnerHtml = title;
			divUwtMenuSection.Controls.Add(divUwtTitle);

			HtmlGenericControl divUwtContent = new HtmlGenericControl("div");
			divUwtContent.AddCssClass("uwt-content");
			divUwtContent.Controls.Add(control);
			
			divUwtMenuSection.Controls.Add(divUwtContent);

			divMegaMenu.Controls.Add(divUwtMenuSection);
		}

		private string GetTenantTitle()
		{
			Oms oms = (this.GetTenantedVariable("OMS") as Oms);
			if (oms == null) return null;

			Instance instTenant = oms.GetTenantInstance();
			return oms.GetTranslationValue(instTenant, KnownRelationshipGuids.Tenant__has_application_title__Translation);
		}
		private string GetTenantType()
		{
			Oms oms = (this.GetTenantedVariable("OMS") as Oms);
			if (oms == null) return null;

			Instance instTenant = oms.GetTenantInstance();
			Instance instTenantType = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has__Tenant_Type);
			return oms.GetInstanceText(instTenantType);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lblLegalNoticeText.Text = String.Format(System.Configuration.ConfigurationManager.AppSettings["System.LegalNoticeText"], DateTime.Now.Year.ToString());

			string CurrentTenantName = this.GetCurrentTenantName();

			Oms oms = (this.GetTenantedVariable("OMS") as Oms);
			if (oms == null) return;

			Instance instTenant = oms.GetTenantInstance();
			if (instTenant == null)
			{
				lblTaskTitle.Text = "Tenant not found";
				lblTaskSubtitle.Text = this.GetCurrentTenantName();
				return;
			}
			BuildUserMenu();

			// Tenant.has mega Menu Section
			BuildMegaMenuTenanted(oms, instTenant);
			/*
			CreateMegaMenuSection("Tasks", new LinkButton[]
			{
				new LinkButton() { Text = "Manage businesses", PostBackUrl = "~/businesses" },
				new LinkButton() { Text = "Manage drivers", PostBackUrl = "~/drivers" },
				new LinkButton() { Text = "Manage vehicles", PostBackUrl = "~/vehicles" }
			});
			CreateMegaMenuSection("Configure", new LinkButton[]
			{
				new LinkButton() { Text = "Configure lookup tables", PostBackUrl = "~/lookup" }
			});
			CreateMegaMenuSection("Reports", new LinkButton[]
			{
				new LinkButton() { Text = "View expired businesses", PostBackUrl = "~/businesses/expired" }
			});
			*/

			// FIXME: this should all be PageBuilder components

			HtmlGenericControl divAboutApplication = new HtmlGenericControl("div");
			System.Web.UI.WebControls.Image image = new Image();

			Instance instCompanyLogoImage = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has_company_logo_image__File);
			if (instCompanyLogoImage != null)
			{
				image.ImageUrl = String.Format("~/Images/Uploads/{{{0}}}.png", instCompanyLogoImage.GlobalIdentifier);
			}
			else
			{
				image.Visible = false;
			}
			image.Width = new Unit(256, UnitType.Pixel);
			divAboutApplication.Controls.Add(image);
			divAboutApplication.Controls.Add(new HtmlGenericControl("br"));

			divAboutApplication.Controls.Add(new Label() { Text = String.Format("{0} Version 1.04M (Nightingale)", GetTenantTitle()) });
			divAboutApplication.Controls.Add(new HtmlGenericControl("br"));
			divAboutApplication.Controls.Add(new Label() { Text = String.Format("Running on {0}", GetTenantType()) });
			divAboutApplication.Controls.Add(new HtmlGenericControl("br"));
			divAboutApplication.Controls.Add(new Label() { Text = "Developed by Michael Becker" });

			CreateMegaMenuSection(String.Format("About {0}", GetTenantTitle()), divAboutApplication);

			if (this.HasTenantedVariable("LoginToken"))
			{
				if (DateTime.Now > ((LoginTokenInfo)this.GetTenantedVariable("LoginToken")).Expires)
				{
					// re-auth the expiry
					string sDuration = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginToken.Duration"];
					if (sDuration == null) sDuration = "30";
					int iDuration = 30;
					if (!Int32.TryParse(sDuration, out iDuration))
					{
						iDuration = 30;
					}

					LoginTokenInfo lti = ((LoginTokenInfo)this.GetTenantedVariable("LoginToken"));
					lti.Expires = DateTime.Now.AddMinutes(iDuration);
					this.SetTenantedVariable("LoginToken", lti);
				}
			}
			else
			{
				if (Request.Url.Segments.Length > 2 && Request.Url.Segments[2] == "inst/")
				{
					Response.Clear();
					Response.Status = "401 Unauthorized";
					Response.ContentType = "application/json";
					Response.Write("{ \"type\": \"error\", \"code\": 401, \"title\": \"Unauthorized\", \"description\": \"You are not logged in\" }");
					Response.End();
					return;
				}

				this.SetTenantedVariable("LoginRedirectURL", Request.Path);

				string loginURL = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginURL"];
				loginURL = loginURL.Replace("{tenant}", CurrentTenantName);
				Response.Redirect(loginURL);
				// Response.Redirect(loginURL.Replace("~/", String.Format("~/{0}/", CurrentTenantName)));
			}

			if (this.HasTenantedVariable("LoginToken"))
			{
				LoginTokenInfo lti = ((LoginTokenInfo)this.GetTenantedVariable("LoginToken"));
				btnUserName.Text = oms.GetInstanceText(lti.UserInstance);
			}

			ul = (HtmlGenericControl)this.Master.FindControl("aspcBeforeContent").FindControl("ul");

			Instance[] insts = oms.GetRelatedInstances(instTenant, KnownRelationshipGuids.Tenant__has_sidebar__Menu_Item);
			for (int i = 0; i < insts.Length; i++)
			{
				Instance parentClassInstance = oms.GetParentClass(insts[i]);
				if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemHeader)
				{
					HtmlGenericControl li1 = new HtmlGenericControl("li");
					li1.AddCssClass("uwt-visible");
					li1.AddCssClass("uwt-section");
					HtmlAnchor aHref1 = new HtmlAnchor();
					aHref1.InnerHtml = oms.GetTranslationValue(insts[i], KnownRelationshipGuids.Menu_Item__has_title__Translatable_Text_Constant);
					li1.Controls.Add(aHref1);
					ul.Controls.Add(li1);
				}
				else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemCommand || parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemInstance)
				{
					HtmlGenericControl li1 = new HtmlGenericControl("li");
					li1.AddCssClass("uwt-visible");

					HtmlAnchor aHref1 = new HtmlAnchor();

					Instance instIcon = oms.GetRelatedInstance(insts[i], KnownRelationshipGuids.Command_Menu_Item__has__Icon);
					if (instIcon != null)
					{
						string iconName = oms.GetAttributeValue<string>(instIcon, KnownAttributeGuids.Text.Name);

						HtmlGenericControl iIcon = new HtmlGenericControl("i");
						iIcon.AddCssClass("fa");
						iIcon.AddCssClass(String.Format("fa-{0}", iconName));
						aHref1.Controls.Add(iIcon);
					}

					HtmlGenericControl spanTitle = new HtmlGenericControl("span");
					spanTitle.AddCssClass("uwt-title");
					spanTitle.InnerHtml = oms.GetTranslationValue(insts[i], KnownRelationshipGuids.Menu_Item__has_title__Translatable_Text_Constant);
					aHref1.Controls.Add(spanTitle);

					if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemCommand)
					{
						string path = oms.GetAttributeValue<string>(insts[i], KnownAttributeGuids.Text.TargetURL);
						aHref1.HRef = Page.ExpandRelativePath(path);
					}
					else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.MenuItemInstance)
					{
						Instance instRel = oms.GetRelatedInstance(insts[i], KnownRelationshipGuids.Instance_Menu_Item__has_target__Instance);
						aHref1.HRef = Page.ExpandRelativePath(String.Format("~/d/inst/{0}.htmld", oms.GetInstanceKey(instRel).ToString()));
					}

					li1.Controls.Add(aHref1);
					ul.Controls.Add(li1);
				}
			}

			// BEGIN: this area loaded by Mocha
			// add in any tenant style sheets here
			this.RegisterStyleSheet("~/PhoenixSNS/phoenix.css");

		}
	}
}
