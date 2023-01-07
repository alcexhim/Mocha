//
//  Program.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2022 Mike Becker's Software
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Net.Sockets;
using System.Text;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Debugger
{
	class MainClass
	{
		private static Oms _CurrentOms = null;

		public static void Main(string[] args)
		{
			_CurrentOms = InitializeOms();
			_CurrentOms.TenantName = "default";

			System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 63320);
			listener.Start();

			while (true)
			{
				System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();

				System.Threading.Thread t = new System.Threading.Thread(t_ParameterizedThreadStart);
				t.Start(client);
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

		private static void t_ParameterizedThreadStart(object parm)
		{
			System.Net.Sockets.TcpClient client = (System.Net.Sockets.TcpClient)parm;

			System.IO.StreamReader sr = new System.IO.StreamReader(client.GetStream());
			if (sr.EndOfStream)
				return;


			string firstline = sr.ReadLine();
			if (String.IsNullOrEmpty(firstline))
				return;

			MBS.Networking.Protocols.HyperTextTransfer.Request req = MBS.Networking.Protocols.HyperTextTransfer.Request.Parse(firstline);
			if (req.Method == "GET")
			{
				Uri uri = new Uri(String.Format("http://localhost:63320{0}", req.Path));
				System.Collections.Specialized.NameValueCollection nvc = System.Web.HttpUtility.ParseQueryString(uri.Query);

				string reff = nvc["ref"];
				if (!String.IsNullOrEmpty(reff))
				{
					if (Guid.TryParse(reff, out Guid guid))
					{

						lock (_CurrentOms)
						{
							if (nvc["tenant"] != null)
							{
								_CurrentOms.TenantName = nvc["tenant"];
							}
							else
							{
								_CurrentOms.TenantName = "default";
							}

							Instance inst = _CurrentOms.GetInstance(guid);
							if (inst != null)
							{
								Instance instDefinition = _CurrentOms.GetRelatedInstance(inst, KnownRelationshipGuids.Instance__has__Instance_Definition);

								// FIXME: we don't know how to pass the project name into this
								string rootpath = "/home/beckermj/Documents/Projects/Mocha.0/Content/Mocha.System";
								string filename = _CurrentOms.GetAttributeValue<string>(instDefinition, KnownAttributeGuids.Text.DebugDefinitionFileName);
								decimal linenum = _CurrentOms.GetAttributeValue<decimal>(instDefinition, KnownAttributeGuids.Numeric.DebugDefinitionLineNumber);
								decimal colnum = _CurrentOms.GetAttributeValue<decimal>(instDefinition, KnownAttributeGuids.Numeric.DebugDefinitionColumnNumber);

								string args = String.Format("\"{0}/{1}\";{2};{3}", rootpath, filename, linenum, colnum);
								Console.WriteLine("mcxdebug: launching monodevelop {0}", args);
								System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("monodevelop", args);
								psi.UseShellExecute = true;
								System.Diagnostics.Process.Start(psi);

								RespondWith(client, _CurrentOms.GetInstanceText(inst), _CurrentOms.GetInstanceText(_CurrentOms.GetRelatedInstance(inst, KnownRelationshipGuids.Instance__for__Module)));
								return;
							}
						}
					}
				}
			}

			while (!sr.EndOfStream)
			{ 
				string nextline = sr.ReadLine();
				if (String.IsNullOrEmpty(nextline))
					break;

				string[] nextparts = nextline.Split(new char[] { ':' }, 2);
			}
		}

		private static void RespondWith(TcpClient client, string nodename, string projname)
		{
			StringBuilder resp = new StringBuilder();
			resp.AppendLine("HTTP/1.1 200 OK");
			resp.AppendLine();
			resp.AppendLine("<html><head><title>Mocha HTTP Debugger Plugin</title></head><body>");
			resp.AppendLine("<strong>Mocha HTTP Debugger Plugin</strong>");
			resp.AppendLine("<p>The requested node has been opened in your IDE</p>");
			resp.AppendLine(String.Format("<p><strong>Node:</strong> {0}</p><p><strong>Project:</strong> {1}</p>", nodename, projname));
			resp.AppendLine("</body></html>");
			resp.AppendLine();
			byte[] respdata = Encoding.UTF8.GetBytes(resp.ToString());
			client.GetStream().Write(respdata, 0, respdata.Length);
			client.Close();
		}
	}
}
