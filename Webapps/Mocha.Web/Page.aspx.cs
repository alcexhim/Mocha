using System;
using System.Web;
using System.Web.UI;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web
{
	public partial class Page : System.Web.UI.Page
	{
		protected override void OnInitComplete(EventArgs e)
		{
			base.OnInitComplete(e);

			Oms oms = this.GetOMS();
			if (oms == null) return;

			object oCid = RouteData.Values["ClassID"];
			object oIid = RouteData.Values["InstanceID"];
			object rCid = RouteData.Values["RelatedClassID"];
			object rIid = RouteData.Values["RelatedInstanceID"];
			if (oCid == null && oIid == null)
			{
				// Master.PageTitle = "Welcome, New User!";
				return;
			}

			int cid = -1, iid = -1;
			if (!(Int32.TryParse(oCid.ToString(), out cid) && Int32.TryParse(oIid.ToString(), out iid)))
			{
				// label.Text = "parse failed";
				return;
			}

			Instance inst = oms.GetInstance(new InstanceKey(cid, iid));
			if (inst == null)
			{
				return;
			}

			OmsContext context = new OmsContext(); 
			context.CallStack.Push(new OmsCallStack(oms.GetInstanceKey(inst), new OmsVariable[]
			{
				new OmsVariable(oms.GetInstance(KnownInstanceGuids.PromptValueClasses.This_Instance), inst)
			}));

			// determine page security
			// Securable Object.secured to Method (returning an Instance Set):
			// 	Anyone | Authenticated Users | (specific user, group, or domain Instance Set)

			Instance instSecurityMethod = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Securable_Item__secured_by__Method);
			object rv = oms.ExecuteMethod(instSecurityMethod, context);
			if (rv is Instance[])
			{
				Instance[] secInsts = (Instance[])rv;
				for (int i = 0; i < secInsts.Length; i++)
				{
					if (secInsts[i].GlobalIdentifier == KnownInstanceGuids.SecurityDomains.Anyone)
					{
						break;
					}
					if (secInsts[i].GlobalIdentifier == KnownInstanceGuids.SecurityDomains.AuthenticatedUsers)
					{
						this.SetTenantedVariable("LoginRedirectURL", Request.Path);

						string loginURL = System.Configuration.ConfigurationManager.AppSettings["Authentication.LoginURL"];
						Response.Redirect(loginURL.Replace("~/", String.Format("~/{0}/", this.GetCurrentTenantName())));
						return;
					}
				}
			}

			// Security Domain@get Anyone instance					GSI - Get Specific Instance
			// Security Domain@get Authenticated Users instance		GSI - Get Specific Instance

			Instance instClass = oms.GetParentClass(inst);

			OmsVariable[] promptValues = null;

			IOmsResponse resp = oms.ExecuteInstance(context, inst, null);

			PageBuilder pb = new PageBuilder((SessionContext)Session["SessionContext"]);
			pb.RenderResponse(resp, PageBuilderPageContent);
		}
	}
}
