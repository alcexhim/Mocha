using System;
using System.Web;
using System.Web.UI;

using MBS.Web;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web.MasterPages
{
	public partial class Blank : MasterPage
	{
		public bool AutoConnectOMS { get; set; } = true;
		public bool OMSAvailable { get; set; } = false;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			string ThemeName = System.Configuration.ConfigurationManager.AppSettings["Tenant.Default.Theme"] ?? "Slate";

			this.RegisterScript(String.Format("~/Themes/{0}/Theme.js", ThemeName));
			this.RegisterStyleSheet(String.Format("~/Themes/{0}/Theme.css", ThemeName));
			this.RegisterStyleSheet("~/StyleSheets/mcx.css");

			this.RegisterScript("~/Scripts/Mocha.js");
			this.RegisterScript("~/Scripts/Instance.js");
			this.RegisterScript("~/Scripts/InstanceKey.js");
			this.RegisterScript("~/Scripts/InstanceBrowser.js");
			this.RegisterScript("~/Scripts/Page.js");
			this.RegisterScript("~/Scripts/PageBuilder.js");
			this.RegisterScript("~/Scripts/SummaryPageComponent.js");

			// can we connect to the OMS?
			if (AutoConnectOMS)
			{
				SessionContext sess = (this.GetTenantedVariable("SessionContext") as SessionContext);
				if (sess == null)
				{
					sess = new SessionContext();
					sess.TenantName = this.GetCurrentTenantName();
					sess.ApplicationPath = Request.ApplicationPath;
					this.SetTenantedVariable("SessionContext", sess);
				}

				Oms oms = sess.GetOms();
				if (oms != null && oms.IsConnected)
				{
					oms.TenantName = this.GetCurrentTenantName();
					oms.Environment.StorageProvider.DefaultTenantName = oms.TenantName;
				}
				this.SetTenantedVariable("OMS", oms);

				OMSAvailable = (oms != null && oms.IsConnected);

				if (OMSAvailable)
				{
					aspcContent.Visible = true;

					Instance instTenant = oms.GetTenantInstance();
					if (instTenant != null)
					{
						Instance instTenantIconImage = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has_icon_image__File);
						if (instTenantIconImage != null)
						{
							System.Web.UI.HtmlControls.HtmlLink link = new System.Web.UI.HtmlControls.HtmlLink();
							link.Attributes.Add("rel", "shortcut icon");
							link.Attributes.Add("href", sess.GetAttachmentUrl(instTenantIconImage, this.Page.GetOmsAttachmentEntropy()));
							Page.Header.Controls.Add(link);
						}
					}
				}
				else
				{
					if (Request.Url.Segments.Length > 2 && Request.Url.Segments[2] == "inst/")
					{
						Response.Clear();
						Response.Status = "503 Service Unavailable";
						Response.ContentType = "application/json";
						Response.Write("{ \"code\": 503, \"title\": \"Service Unavailable\", \"description\": \"OMS service is not running\" }");
						Response.End();
					}
					aspcContent.Visible = false;
				}
			}
		}
	}
}
