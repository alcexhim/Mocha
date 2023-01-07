//
//  WebApplication.cs
//
//  Author:
//       beckermj <>
//
//  Copyright (c) 2023 ${CopyrightHolder}
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
using System.Configuration;

namespace Mocha.Web.Server.Sys
{
	public class WebApplication : MBS.Framework.Application
	{
		private System.Net.Sockets.TcpListener listener = null;
		protected override int StartInternal()
		{
			string strport = ConfigurationManager.AppSettings["Mocha.Web.Server.Port"];
			if (Int32.TryParse(strport, out int port))
			{
				listener = new System.Net.Sockets.TcpListener(new System.Net.IPEndPoint(System.Net.IPAddress.Any, port));
				listener.Start();

				while (true)
				{
					System.Net.Sockets.TcpClient client = listener.AcceptTcpClient();
					System.Threading.Thread tClient = new System.Threading.Thread(tClient_ParameterizedThreadStart);
					tClient.Start(client);
				}
			}
			else
			{
				Console.Error.WriteLine("can't start listener: appSetting 'Mocha.Web.Server.Port' must be an integer (got '{0}')", strport);
				return -1;
			}
		}

		private void tClient_ParameterizedThreadStart(object parm)
		{
			try
			{
				System.Net.Sockets.TcpClient client = (System.Net.Sockets.TcpClient)parm;

				System.IO.Stream stream = client.GetStream();
				System.IO.StreamReader sr = new System.IO.StreamReader(stream);

				string line = sr.ReadLine();
				if (String.IsNullOrEmpty(line))
				{
					return;
				}

				Sys.HttpRequest req = Sys.HttpRequest.Parse(line);
				Sys.HttpResponse resp = new HttpResponse(stream);


				System.Collections.Generic.Dictionary<string, string> headers = new System.Collections.Generic.Dictionary<string, string>();

				while (true)
				{
					string line2 = sr.ReadLine();
					if (line2.Contains(":"))
					{
						string[] kvp1 = line2.Split(new char[] { ':' }, 2);
						if (kvp1.Length == 2)
						{
							headers[kvp1[0]] = kvp1[1];
						}
					}
					if (String.IsNullOrEmpty(line2))
						break;
				}

				OnRequestReceived(new RequestEventArgs(req, resp, stream, headers));
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				// silently drop it
			}
			catch (System.IO.IOException ex)
			{
				// silently drop it
			}
		}

		public event EventHandler<RequestEventArgs> RequestReceived;
		protected virtual void OnRequestReceived(RequestEventArgs e)
		{
			RequestReceived?.Invoke(this, e);
		}
	}
}
