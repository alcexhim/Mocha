using System;
using System.Configuration;

namespace Mocha.Web.Configuration.ConfigurationSections.OMS
{
	public class OMSConfigurationSection : ConfigurationSection
	{
		private static OMSConfigurationSection settings = ConfigurationManager.GetSection("oms") as OMSConfigurationSection;
		public static OMSConfigurationSection Settings { get { return settings; } }

		[ConfigurationProperty("server", IsDefaultCollection = false)]
		public ServerConfigurationElement Server { get { return (ServerConfigurationElement)base["server"]; } set { base["server"] = value; } }

		// [ConfigurationProperty("userName")]
		// public string UserName { get { return (string)base["userName"]; } set { base["userName"] = value; } }
		// [ConfigurationProperty("password")]
		// public string Password { get { return (string)base["password"]; } set { base["password"] = value; } }
	}
}
