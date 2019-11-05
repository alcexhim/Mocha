using System;
using System.Web;
using System.Web.UI;

using MBS.Web;
using Mocha.OMS;

namespace Mocha.Web.MasterPages
{
	public partial class Blank : System.Web.UI.MasterPage
	{
		public bool AutoConnectOMS { get; set; } = true;
		public bool OMSAvailable { get; set; } = false;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.RegisterScript("~/Themes/Slate/Theme.js");
			this.RegisterStyleSheet("~/Themes/Slate/Theme.css");
			this.RegisterStyleSheet("~/StyleSheets/mcx.css");

			// can we connect to the OMS?
			if (AutoConnectOMS)
			{
				OMSClient oms = (Session["OMS"] as OMSClient);
				if (oms == null)
				{
					// attempt to connect
					oms = new OMSClient();

					string serverName = Mocha.Web.Configuration.ConfigurationSections.OMS.OMSConfigurationSection.Settings.Server.HostName;
					int portNumber = Mocha.Web.Configuration.ConfigurationSections.OMS.OMSConfigurationSection.Settings.Server.PortNumber;

					System.Net.IPHostEntry entry = System.Net.Dns.GetHostEntry(serverName);
					if (entry != null)
					{
						try
						{
							oms.Connect(entry.AddressList[0], portNumber);
						}
						catch (System.Net.Sockets.SocketException ex)
						{
							OMSUnavailable.Visible = true;
						}
					}

					if (oms.IsConnected)
						Session["OMS"] = oms;
				}

				OMSAvailable = (Session["OMS"] != null && oms.IsConnected);

				if (OMSAvailable)
				{
					aspcContent.Visible = true;
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
