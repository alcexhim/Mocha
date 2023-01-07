using System;
using System.Web;
using System.Web.UI;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web
{

	public partial class Upload : System.Web.UI.Page
	{
		protected override void OnPreInit(EventArgs e)
		{
			SessionContext sess = (SessionContext)(Page.GetTenantedVariable("SessionContext"));
			if (sess == null)
			{
				sess = new SessionContext();
				sess.TenantName = this.GetCurrentTenantName();
				sess.ApplicationPath = Request.ApplicationPath;
				this.SetTenantedVariable("SessionContext", sess);
			}

			Oms oms = sess.GetOms();

			Instance instTenant = oms.GetTenantInstance();
			if (instTenant == null)
			{
				Response.Clear();
				Response.StatusCode = 404;
				Response.End();
				return;
			}

			Response.Clear();

			Instance inst = null;
			string[] path = Request.FilePath.Split(new char[] { '/' });
			if (Request.QueryString["vp"] != null)
			{
				path = Request.QueryString["vp"].Split(new char[] { '/' });
			}

			if (path.Length >= 3)
			{
				if (path[2] == "attachment")
				{
					if (path.Length == 5)
					{
						string instId = path[3];
						inst = oms.GetInstance(InstanceKey.Parse(instId));
						string key = path[4];
						if (oms.VerifyAccessKeyForOmsAttachment(inst, key, Page.GetOmsAttachmentEntropy()))
						{
							Response.ClearHeaders();
							Response.Clear();

							string physicalFilename = String.Format("{0}Images/Uploads/{1}.png", Request.PhysicalApplicationPath, inst.GlobalIdentifier.ToString("b"));
							if (System.IO.File.Exists(physicalFilename))
							{
								string mimetype = oms.GetAttributeValue<string>(inst, KnownAttributeGuids.Text.ContentType);
								Response.ContentType = mimetype;
								Response.WriteFile(physicalFilename);
								Response.End();
							}
							else
							{
								Response.StatusCode = 404;
								Response.End();
							}
						}
						else
						{
							Response.ClearHeaders();
							Response.Clear();
							Response.StatusCode = 401;
							Response.End();
						}
					}
					else
					{
						// FIXME: respond with error
					}
					return;
				}
			}

			Response.ClearHeaders();
			Response.Clear();
			Response.StatusCode = 404;
			Response.End();
		}
	}
}
