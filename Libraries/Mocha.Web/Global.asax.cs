using System.Web;
using System.Web.Routing;
using MBS.Web.ConfigurationSections.Routing;

namespace Mocha.Web
{
    public class Global : HttpApplication
    {
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
