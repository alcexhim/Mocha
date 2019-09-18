using System;
using System.Configuration;

using MBS.Web;

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
					Response.Redirect("~/default");
				}

				object token = Session["LoginToken"];
				if (token == null)
				{
					// otherwise, redirect to login page
					this.Redirect(ConfigurationManager.AppSettings["Authentication.LoginURL"]);
				}
				else
				{
					this.Redirect(ConfigurationManager.AppSettings["DefaultHomePageUrl"]);
				}
			}
		}
	}
}
