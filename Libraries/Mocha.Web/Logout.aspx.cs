using System;
using System.Web;
using System.Web.UI;

namespace Mocha.Web
{

	public partial class Logout : System.Web.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Session["LoginToken"] = null;

			Response.Redirect("~/");
		}
	}
}
