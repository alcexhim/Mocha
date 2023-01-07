//
//  Program.cs
//
//  Author:
//       beckermj <>
//
//  Copyright (c) 2022 ${CopyrightHolder}
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
using System.IO;

using Mocha.Core;
using Mocha.OMS;
using Mocha.Web.Server.Sys;
using UniversalEditor.IO;

namespace Mocha.Web.Server
{
	class MainClass : Sys.WebApplication
	{
		public MainClass()
		{
			ShortName = "mocha-web";
		}

		public static void Main(string[] args)
		{
			(new MainClass()).Start();
		}

		protected override void OnRequestReceived(RequestEventArgs e)
		{
			base.OnRequestReceived(e);

			System.IO.StreamReader sr = new System.IO.StreamReader(e.Stream);
			System.IO.StreamWriter sw = new System.IO.StreamWriter(e.Stream);

			SessionContext ctx = new SessionContext();

			if (((HttpRequest)e.Request).Method == "POST")
			{
				System.Collections.Generic.Dictionary<string, string> postData = new System.Collections.Generic.Dictionary<string, string>();

				if (e.Headers.ContainsKey("Content-Length"))
				{
					string strContentLength = e.Headers["Content-Length"];
					int intContentLength = Int32.Parse(strContentLength);
					char[] buffer = new char[intContentLength];
					sr.ReadBlock(buffer, 0, intContentLength);

					string buf = new string(buffer);
					string[] values = buf.Split(new char[] { '&' });

					foreach (string val in values)
					{
						string[] kvp1 = val.Split(new char[] { '=' });
						string name = kvp1[0];
						string value = kvp1[1];

						postData[name] = FormDecode(value);
					}
				}

				OnPost(new PostEventArgs(e.Request, e.Response, postData));
			}
			else if (((HttpRequest)e.Request).Method == "GET")
			{
				string relativePath = ((HttpRequest)e.Request).Path;
				string[] pathSplit = relativePath.Split(new char[] { '/' }, StringSplitOptions.None);
				if (pathSplit.Length >= 2)
				{
					if (pathSplit[1] == String.Empty)
					{
						Write404(sw, ctx);
					}
					else if (pathSplit.Length >= 2)
					{
						if (pathSplit.Length >= 3)
						{
							if (pathSplit[2] == "attachment")
							{
								string tenantName = pathSplit[1];
								ctx.TenantName = tenantName;

								string instanceId = pathSplit[3];
								string key = pathSplit[4];

								InstanceKey ik = InstanceKey.Parse(instanceId);

								Mocha.OMS.Oms oms = ctx.GetOms();
								Instance instImage = oms.GetInstance(ik);
								if (oms.VerifyAccessKeyForOmsAttachment(instImage, key, ctx.GetOmsAttachmentEntropy()))
								{
									string physicalFilename = String.Format("{0}Images/Uploads/{1}.png", String.Empty, instImage.GlobalIdentifier.ToString("b"));
									if (System.IO.File.Exists(physicalFilename))
									{

										byte[] data = System.IO.File.ReadAllBytes(physicalFilename);
										string mimetype = oms.GetAttributeValue<string>(instImage, KnownAttributeGuids.Text.ContentType);
										sw.WriteLine("HTTP/1.1 200 OK");
										sw.WriteLine(String.Format("Content-Type: {0}", mimetype));
										sw.WriteLine(String.Format("Content-Length: {0}", data.Length));
										sw.WriteLine();
										sw.Flush();

										sw.BaseStream.Write(data, 0, data.Length);
										sw.BaseStream.Flush();

										// FIXME: !!!!!! figure out how to properly deal with large files and "connection reset"
										System.Threading.Thread.Sleep(1000);

										sw.Close();
										return;
									}
									else
									{
										Write404(sw, ctx);
										return;
									}
								}
								else
								{
									Write401(sw, ctx);
									return;
								}
							}
						}
						if (pathSplit[1] == "css")
						{
							if (pathSplit[2] == "main.css")
							{
								sw.WriteLine("HTTP/1.1 200 OK");
								sw.WriteLine("Content-Type: text/css");
								sw.WriteLine("Server: Mocha User Interface Service");
								sw.WriteLine();
								sw.Flush();

								System.IO.Stream st = typeof(MainClass).Assembly.GetManifestResourceStream("Mocha.Web.Server.Pages.css.css");
								st.CopyTo(e.Stream);
								sw.Close();
								return;
							}
							else if (pathSplit[2] == "uwt.css" || pathSplit[2] == "uwt.less")
							{
								sw.WriteLine("HTTP/1.1 200 OK");
								sw.WriteLine("Content-Type: text/css");
								sw.WriteLine("Server: Mocha User Interface Service");
								sw.WriteLine();
								sw.Flush();

								string DefaultThemeName = ConfigurationManager.AppSettings["Mocha.Default.Theme"] ?? "Pleasanton";
								string[] themeFiles = System.IO.Directory.GetFiles("Themes", "*", SearchOption.AllDirectories);

								System.Text.StringBuilder sb = new System.Text.StringBuilder();
								foreach (string themeFile in themeFiles)
								{
									string[] pathParts = themeFile.Split(new char[] { System.IO.Path.DirectorySeparatorChar }, StringSplitOptions.None);
									if (pathParts.Length > 2)
									{
										string themeName = pathParts[1];
										if (themeName == "Mobile" || themeName == DefaultThemeName)
										{
											sb.AppendLine(System.IO.File.ReadAllText(themeFile));
										}
									}
									else
									{
										sb.AppendLine(System.IO.File.ReadAllText(themeFile));
									}
								}

								if (pathSplit[2].EndsWith(".less"))
								{
									sw.WriteLine(sb.ToString());
								}
								else
								{
									dotless.Core.configuration.DotlessConfiguration config = new dotless.Core.configuration.DotlessConfiguration();
									config.MinifyOutput = true;
									sw.WriteLine(dotless.Core.Less.Parse(sb.ToString(), config).Replace('\n', ' ').Replace('\t', ' ').Replace("  ", " "));
								}

								sw.Close();
								return;
							}
							else if (pathSplit[2] == "Fonts")
							{
								string fontName = pathSplit[3];
								string fontFile = pathSplit[4];

								int indexOfQ = fontFile.IndexOf('?');
								if (indexOfQ != -1)
								{
									fontFile = fontFile.Substring(0, indexOfQ);
								}

								sw.WriteLine("HTTP/1.1 200 OK");
								sw.WriteLine("Content-Type: application/x-font-opentype");
								sw.WriteLine();
								sw.Flush();

								e.Response.WriteFile("Fonts/" + fontName + "/" + fontFile);

								e.Stream.Close();
								return;
							}
						}
						else
						{
							// assume tenant name
							ctx.TenantName = pathSplit[1];

							if (pathSplit.Length >= 2)
							{
								if (pathSplit.Length == 5 && pathSplit[1] == "madi" && pathSplit[2] == "authgwy")
								{
									ctx.TenantName = pathSplit[3];
									if (pathSplit[4] == "tenant-config.xml")
									{
										sw.WriteLine("HTTP/1.1 200 OK");
										sw.WriteLine("Content-Type: application/xml");
										sw.WriteLine("Server: Mocha User Interface Service");
										sw.WriteLine();

										sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
										sw.WriteLine("<mul:Tenant_Config xmlns:mml=\"urn:net.alcetech.schemas.Mocha.Model!1.0\" xmlns:wul=\"urn:net.alcetech.schemas.Mocha.UserInterface!1.0\" xmlns:mcl=\"urn:net.alcetech.schemas.Mocha.UserInterface!2.0\" xmlns:mc=\"urn:net.alcetech.schemas.Mocha/bsvc\" xmlns:nyw=\"urn:com.netyourwork/aod\" ");

										System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();
										dict["Allow_Attachments_And_Documents_To_Be_Shared_With_Other_Mobile_Apps"] = "0";
										dict["Apns_Allowed"] = "0";
										dict["Canvas_Is_Enabled"] = "1";
										dict["Canvas_Hex_Code"] = "#005cb9,#0875e1";
										dict["Clear_Mobile_SSO_Webview_Cookies_on_Login"] = "0";
										dict["Deter_Screenshots"] = "0";
										dict["Disable_Add_To_Contact"] = "0";
										dict["Disable_Importing_Attachments_From_Third-Party_Cloud_Services"] = "0";
										dict["Disable_Location_Service"] = "0";
										dict["Disable_Mail_To"] = "0";
										dict["Disable_Voice_in_Assistant_on_Mobile"] = "1";
										dict["Enable_Blue_Primary_Buttons"] = "0";
										dict["Enable_Certificate_Based_SSO"] = "0";
										dict["Enable_DOM_Storage"] = "0";
										dict["Enable_Email_Annotations"] = "1";
										dict["Enable_export_to_Worksheets"] = "1";
										dict["Enable_Fingerprint_Authentication"] = "1";
										dict["Enable_Geospace_Visualization"] = "0";
										dict["Enable_Mobile_Browser_SSO_for_Native_Apps"] = "0";
										dict["Enable_New_Profile"] = "0";
										dict["Error_Message_for_Browser"] = "This Browser is not supported by Workday. Please contact your IT department.";
										dict["Is_FedRAMP_Tenant"] = "0";
										dict["Google_Cloud_Messaging_Project_Number"] = "739632724832";
										dict["Hide_Links_to_Web_from_Deep_Linking_and_Inbox"] = "0";
										dict["Login_URL"] = "https://wd5-impl.workday.com/wday/authgwy/cityoforlando1/login-saml2.htmld";
										dict["Logout_URL"] = "https://cityoforlando.okta.com";
										dict["OMS_Note"] = "IMPL - Powered by Workday";
										dict["Pin_Auth_Enabled"] = "0";
										dict["Pin_Max_Attempts"] = "0";
										dict["Pin_Max_Length"] = "0";
										dict["Pin_Min_Length"] = "0";
										dict["Reset_Password_Online"] = "0";
										dict["Show_Change_Password_Link"] = "1";
										dict["Show_Forgotten_Password_Link"] = "0";
										dict["SignOn_Custom_Message"] = "&lt;p&gt;City of Orlando - Production&lt;/p&gt;";
										dict["SignOn_Message_Locale"] = "en_US";
										dict["SignOn_Note"] = "&lt;p&gt;City of Orlando - Production&lt;/p&gt;";
										dict["SignOn_Tenant_Logo_Url"] = "/cityoforlando1/images/signon.xml";
										dict["System_Confidence_Level"] = "PROD";
										dict["System_Note"] = "Your Implementation tenant will be unavailable for a maximum of 12 hours during the next Weekly Service Update; starting on Friday, December 30, 2022 at 6:00 PM Pacific Time (Los Angeles) (GMT-8) until Saturday, December 31, 2022 at 6:00 AM Pacific Time (Los Angeles) (GMT-8).";
										dict["Use_One_Time_Use_Link"] = "0";
										foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in dict)
										{
											sw.WriteLine(String.Format("\t{0}=\"{1}\"", kvp.Key, kvp.Value));
										}
										sw.WriteLine(" />");
										sw.Close();
										return;
									}
									if (pathSplit[4] == "login.htmld")
									{
										// we are at the login page
										// here we find the current tenant, and ask it to retrieve the PageBuilder login page

										//FIXME: make this better
										Mocha.OMS.Oms oms = ctx.GetOms();

										Instance instTenant = oms.GetTenantInstance();
										if (instTenant == null)
										{
											WriteGenericMessage(sw, ctx, "Invalid Tenant", "The URL you have provided is invalid. Please contact your system administrator if you require assistance.");
											return;
										}

										// Tenant@get login Page for Tenant and Application parm

										sw.WriteLine("HTTP/1.1 200 OK");
										sw.WriteLine("Content-Type: text/html");
										sw.WriteLine();

										PageBuilder2 pb = new PageBuilder2(oms, ctx);

										Instance instLoginPage = oms.GetInstance(KnownInstanceGuids.Pages.LoginPage);

										pb.RenderPage(instLoginPage, sw);
										sw.Close();
										return;
									}
									else
									{
										Write404(sw, ctx);
										return;
									}
								}

								object instLoginUser = null;
								if (instLoginUser == null)
								{
									WriteLoginRedirect(sw, ctx);
									return;
								}
							}
							else
							{
								Write404(sw, ctx);
								return;
							}
						}
					}
				}
			}
		}

		private void OnPost(PostEventArgs e)
		{
			System.IO.StreamWriter sw = new StreamWriter(e.Response.Stream);
			sw.WriteLine("HTTP/1.1 200 OK");
			sw.WriteLine();
			sw.WriteLine(String.Format("<html><head><title></title></head><body>Your stuff is {0}</body></html>", e.PostData.Count));
			sw.Flush();

			e.Response.End();
		}

		private static string FormDecode(string value)
		{
			string plusReplace = value.Replace('+', ' ');
			plusReplace = plusReplace.Replace("%2B", "+");
			return plusReplace;
		}

		private static void WriteGenericMessage(StreamWriter sw, SessionContext ctx, string v1, string v2)
		{
			sw.WriteLine("HTTP/1.1 404 Not Found");
			sw.WriteLine("Content-Type: text/html");
			sw.WriteLine("Server: Mocha User Interface Service");

			sw.WriteLine();

			sw.WriteLine(String.Format("<html><head><link rel=\"stylesheet\" type=\"text/css\" media=\"screen, projection\" href=\"/css/main.css\" /><title>{0}</title></head><body>", ctx.PageTitle));
			sw.WriteLine(String.Format("<h1>{0}</h1><p>{1}</p></body></html>", v1, v2));

			sw.Close();
		}

		private static void WriteLoginRedirect(StreamWriter sw, SessionContext ctx)
		{
			/*
				var redirectUrl = 'https://{originalUrl}/wday/authgwy/{tenant}/login.htmld?returnTo=%2f{tenant}%2fhome.htmld';
				var anchor = encodeURIComponent(window.location.hash);
				if (anchor.length > 0) {
				redirectUrl = redirectUrl.replace(/([?|&]returnTo=[^\&;]+)/, '$1' + anchor);
				}
				window.location = redirectUrl;							
			 */
			sw.WriteLine("HTTP/1.1 302 Found");
			sw.WriteLine(String.Format("Location: /madi/authgwy/{0}/login.htmld", ctx.TenantName));
			sw.WriteLine();
			sw.Close();
		}

		private static void Write404(StreamWriter sw, SessionContext ctx)
		{
			WriteGenericMessage(sw, ctx, "Not Found", "The requested URL was not found on this server.");
		}
		private static void Write401(StreamWriter sw, SessionContext ctx)
		{
			WriteGenericMessage(sw, ctx, "Not Authorized", "The requested URL was not found on this server.");
		}
	}
}
