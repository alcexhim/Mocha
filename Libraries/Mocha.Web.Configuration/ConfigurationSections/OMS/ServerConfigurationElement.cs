using System;
using System.Configuration;

namespace Mocha.Web.Configuration.ConfigurationSections.OMS
{
	public class ServerConfigurationElement : ConfigurationElement
	{
		[ConfigurationProperty("hostName", IsRequired = true)]
		public string HostName { get { return (string)base["hostName"]; } set { base["hostName"] = value; } }

		[ConfigurationProperty("portNumber", DefaultValue = 28015)]
		public int PortNumber { get { return (int)base["portNumber"]; } set { base["portNumber"] = value; } }
	}
}
