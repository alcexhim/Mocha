using System;
using System.Web;
using System.Web.UI;

using MBS.Web;

namespace Mocha.Web.MasterPages
{
    public partial class Default : System.Web.UI.MasterPage
    {
		public string PageTitle { get { return lblTaskTitle.Text; } set { lblTaskTitle.Text = value; } }
		public string PageSubtitle { get { return lblTaskSubtitle.Text; } set { lblTaskSubtitle.Text = value; } }

		protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			string CurrentTenantName = System.Configuration.ConfigurationManager.AppSettings["Tenant.Default.Name"];
			lblTenantName.Text = CurrentTenantName;
			lblSystemVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			object lt = Session["LoginToken"];
			if (lt == null || ((LoginTokenInfo)lt).IsEmpty || DateTime.Now > ((LoginTokenInfo)lt).Expires)
			{
				if (Request.Url.Segments.Length > 2 && Request.Url.Segments[2] == "inst/")
				{
					Response.Clear();
					Response.Status = "401 Unauthorized";
					Response.ContentType = "application/json";
					Response.Write("{ \"code\": 401, \"title\": \"Unauthorized\", \"description\": \"You are not logged in\" }");
					Response.End();
					return;
				}

				Session["LoginRedirectURL"] = Request.Path;
				Response.Redirect(String.Format("~/{0}/account/login", CurrentTenantName));
			}
			else
			{
				// re-auth the expiry
				string sDuration = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginToken.Duration"];
				if (sDuration == null) sDuration = "30";
				int iDuration = 30;
				if (!Int32.TryParse(sDuration, out iDuration))
				{
					iDuration = 30;
				}

				LoginTokenInfo lti = ((LoginTokenInfo)lt);
				lti.Expires = DateTime.Now.AddMinutes(iDuration);
				Session["LoginToken"] = lti;
			}

			// BEGIN: this area loaded by Mocha
			// add in any tenant style sheets here
			this.RegisterStyleSheet("~/PhoenixSNS/phoenix.css");

		}
	}
}
