using System;
using System.Web;
using System.Web.Routing;
using MBS.Web.ConfigurationSections.Routing;
using Mocha.OMS;

namespace Mocha.Web
{
    public class Global : HttpApplication
    {
		private static object _OMS_Lock = new object();
		public static Oms OMS { get; private set; } = null;
		static Global()
		{
			lock (_OMS_Lock)
			{
				OMS = InitializeOms();
			}
		}

		public static Oms InitializeOms()
		{
			Oms oms = null;
			bool useLocalOms = true;
			if (useLocalOms)
			{
				oms = new LocalOms();
				(oms as LocalOms).Environment = new OmsEnvironment(new Storage.Local.LocalStorageProvider("/usr/share/mocha/system"));
				oms.Environment.Initialize();
				oms.Environment.StorageProvider.DefaultTenantName = oms.TenantName;
			}
			else
			{
				/*
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
						// OMSUnavailable.Visible = true;
					}
				}
				*/
			}
			return oms;
		}

		protected void Application_Start()
		{
			// this is so much easier with .NET than apache
			foreach (RouteConfigurationElement route in RoutingConfigurationSection.Settings.Routes)
			{
				RouteTable.Routes.MapPageRoute(route.RouteName, route.RouteUrl, route.VirtualPath);
			}
		}
	}
}
