using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace Mocha.Web
{

	public partial class Logout : System.Web.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (this.HasTenantedVariable("LoginToken"))
			{
				this.ClearTenantedVariable("LoginToken");
				if (this.HasTenantedVariable("LoginRedirectURL"))
				{
					this.ClearTenantedVariable("LoginRedirectURL");
				}
			}
			this.ClearTenantedVariable("OMS");
			this.ClearTenantedVariable("oms_attachment_entropy");

			Response.Redirect(String.Format("~/{0}", Master.GetCurrentTenantName()));
		}
	}
}
