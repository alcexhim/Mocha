using System;
using System.Web;
using System.Web.UI;

using MBS.Web;
using MBS.Framework.Security.Cryptography;
using Mocha.OMS;

namespace Mocha.Web
{
    public partial class LoginPage : System.Web.UI.Page
    {
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			this.AddCssClass("LoginPage");

			this.AddCssClass("uwt-hide-sidebar");
			this.AddCssClass("uwt-hide-header");

			this.Page.Title = "Login";
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			lblLegalNoticeText.Text = String.Format(System.Configuration.ConfigurationManager.AppSettings["System.LegalNoticeText"], DateTime.Now.Year.ToString());

            OMSClient oms = (Session["OMS"] as OMSClient);
			if (oms == null) return;

			string username = Request.Form["user_name"];
			string userpass = Request.Form["user_pass"];
			string tenant = Request.Url.Segments[1];

			if (username != null)
			{
				if (!String.IsNullOrEmpty(userpass))
				{

					Instance clsUser = oms.GetInstance(KnownInstanceGUIDs.User);
					Instance[] instUsers = oms.GetInstances(clsUser);

					Instance instAttUserName = oms.GetInstance(KnownAttributeGUIDs.UserName);
					Instance instAttPasswordHash = oms.GetInstance(KnownAttributeGUIDs.PasswordHash);
					Instance instAttPasswordSalt = oms.GetInstance(KnownAttributeGUIDs.PasswordSalt);

					bool userOK = false;
					foreach (Instance instUser in instUsers)
					{
						string userName = oms.GetAttributeValue<string>(instUser, instAttUserName);
						if (userName == username)
						{
							string passHash = oms.GetAttributeValue<string>(instUser, instAttPasswordHash);
							string passSalt = oms.GetAttributeValue<string>(instUser, instAttPasswordSalt);

							string salt = "7e893ba949b041bab73c6f4f0bcb9413";  // RandomSalt();
							string hash = Authentication.HashPass(userpass, salt);
							if (passHash == hash)
							{
								userOK = true;
								break;
							}
						}
					}

					if (userOK)
					{
						TimeSpan ts = new TimeSpan(0, 1, 0);

						string sDuration = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginToken.Duration"];
						if (sDuration == null) sDuration = "30";
						int iDuration = 30;
						if (!Int32.TryParse(sDuration, out iDuration))
						{
							iDuration = 30;
						}

						// we need to update the LoginToken in the OMS now...

						LoginTokenInfo userToken = new LoginTokenInfo(Guid.NewGuid(), DateTime.Now.AddMinutes(iDuration), username, tenant);
						Session["LoginToken"] = userToken;

						if (Session["LoginRedirectURL"] != null)
						{
							Response.Redirect(Session["LoginRedirectURL"].ToString());
						}
						else
						{
							this.Redirect("~/");
						}
					}
				}
				else
				{
				}
			}
		}
	}
}
