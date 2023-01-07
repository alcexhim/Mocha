using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using MBS.Web;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web
{
    public partial class Default : System.Web.UI.Page
    {
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			string[] pathParts = Request.Path.Split(new char[] { '/' });
			if (pathParts.Length >= 2)
			{
				string tenantName = pathParts[1];

				// check to see if the tenant exists
				if (tenantName == "Default.aspx")
				{
					Response.Redirect(String.Format("~/{0}", ConfigurationManager.AppSettings["Tenant.Default.Name"]));
				}

				if (!this.HasTenantedVariable("LoginToken"))
				{
					// otherwise, redirect to login page

					string loginRedirectURL = ConfigurationManager.AppSettings["Authentication.LoginURL"] ?? "~/Login.aspx";

					string loginURL = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginURL"];
					loginURL = loginURL.Replace("{tenant}", tenantName);
					this.Redirect(loginURL);

					/*// not implemented yet
					Oms oms = this.GetOMS();
					Instance instTenant = oms.GetTenantInstance();

					Instance instSSOProvider = oms.GetRelatedInstances(instTenant, KnownRelationshipGuids.Tenant__has__Single_Sign_On_Provider);
					loginRedirectURL = oms.GetAttributeValue<string>(instSSOProvider[i], KnownAttributeGuids.Text.TargetURL);
					*/
					// this.Redirect(loginRedirectURL);
				}
				else
				{
					if (ConfigurationManager.AppSettings["DefaultHomePageUrl"] != null)
					{
						this.Redirect(ConfigurationManager.AppSettings["DefaultHomePageUrl"]);
					}
				}
			}
		}
	}
}
