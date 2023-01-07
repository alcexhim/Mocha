//
//  PageBuilder2.cs
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
using System.Collections.Generic;
using System.IO;
using Mocha.Core;
using Mocha.OMS;
using UniversalEditor.IO;

namespace Mocha.Web.Server
{
	public class PageBuilder2
	{
		private Mocha.OMS.Oms oms = null;
		private SessionContext sess = null;
		public PageBuilder2(Mocha.OMS.Oms oms, SessionContext sess)
		{
			this.oms = oms;
			this.sess = sess;
		}

		private void RenderBeginTag(Instance inst, StreamWriter sw)
		{
			InstanceKey instId = oms.GetInstanceKey(inst);
			Instance instParentClass = oms.GetParentClass(inst);
			if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ImagePageComponent)
			{
				Instance instImage = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Image_Page_Component__has_source__Method);

				OmsContext ctx = new OmsContext();
				object val = oms.ExecuteMethod(instImage, ctx);

				string imgurl = sess.GetAttachmentUrl(((Instance[])val)[0], sess.GetOmsAttachmentEntropy());
				sw.WriteLine(String.Format("<img data-instance-id=\"{0}\" src=\"{1}\" />", instId, imgurl));
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
			{
				sw.WriteLine(String.Format("<div data-instance-id=\"{0}\" class=\"uwt-panel\">", instId));

				sw.WriteLine("<div class=\"uwt-header\">");
				Instance[] instHeaderComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_header__Page_Component);
				foreach (Instance inst2 in instHeaderComponents)
				{
					RenderPageComponent(inst2, sw);
				}
				sw.WriteLine("</div>");
				sw.WriteLine("<div class=\"uwt-content\">");
				Instance[] instContentComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
				foreach (Instance inst2 in instContentComponents)
				{
					RenderPageComponent(inst2, sw);
				}
				sw.WriteLine("</div>");
				sw.WriteLine("<div class=\"uwt-footer\">");
				Instance[] instFooterComponents = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
				foreach (Instance inst2 in instFooterComponents)
				{
					RenderPageComponent(inst2, sw);
				}
				sw.WriteLine("</div>");
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ButtonPageComponent)
			{
				sw.WriteFixedLengthString(String.Format("<button data-instance-id=\"{0}\"", instId));
				WriteClassAttribute(sw, inst);
				sw.Write(">");
				sw.Write(oms.GetTranslationValue(inst, KnownRelationshipGuids.Button_Page_Component__has_text__Translatable_Text_Constant));
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.DetailPageComponent)
			{
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.HeadingPageComponent)
			{
				decimal level = oms.GetAttributeValue<decimal>(inst, KnownAttributeGuids.Numeric.Level);
				sw.WriteFixedLengthString(String.Format("<h{0} data-instance-id=\"{1}\"", level, instId));
				WriteClassAttribute(sw, inst);

				OmsContext ctx = new OmsContext();

				Instance contentMethod = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Content_Page_Component__gets_content_from__Method);
				string text = oms.ExecuteMethodReturningTextOrTranslation(contentMethod, ctx);
				sw.WriteFixedLengthString(String.Format(">{0}", text));
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.SummaryPageComponent)
			{
				sw.WriteFixedLengthString(String.Format("<div data-instance-id=\"{0}\"", instId));
				WriteClassAttribute(sw, inst, "uwt-formview");

				sw.WriteFixedLengthString(">");

				sw.WriteLine("<div class=\"uwt-formview-item\">");

				sw.WriteLine("<div class=\"uwt-formview-item-label\">");
				sw.WriteLine("<label>Deprecated - Do Not Use</label>");
				sw.WriteLine("</div>");
				sw.WriteLine("<div class=\"uwt-formview-item-content\">");
				sw.WriteLine("<label>Please Replace with Element Page Component</label>");
				sw.WriteLine("</div>");

				sw.WriteLine("</div>");
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ElementPageComponent)
			{
				Instance element = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Element_Page_Component__has__Element);

				Instance[] elementDisplayOptions = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Element_Page_Component__has__Element_Content_Display_Option);
				Instance ecDoSingular = oms.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.Singular);

				bool singular = (oms.InstanceSetContains(elementDisplayOptions, ecDoSingular));

				if (singular)
				{
					sw.WriteFixedLengthString(String.Format("<div data-instance-id=\"{0}\" data-element-id=\"{1}\"", instId, oms.GetInstanceKey(element)));
					WriteClassAttribute(sw, inst, "uwt-formview");

					sw.WriteFixedLengthString(">");

					if (element != null)
					{
						Instance[] elementContents = oms.GetRelatedInstances(element, KnownRelationshipGuids.Element__has__Element_Content);
						foreach (Instance elementContent in elementContents)
						{
							Instance elementContentInstance = oms.GetRelatedInstance(elementContent, KnownRelationshipGuids.Element_Content__has__Instance);
							InstanceKey ecid = oms.GetInstanceKey(elementContentInstance);

							sw.WriteLine(String.Format("<div class=\"uwt-formview-item\" data-ecid=\"{0}\">", oms.GetInstanceKey(elementContentInstance)));

							sw.WriteLine("<div class=\"uwt-formview-item-label\">");
							sw.WriteLine(String.Format("<label>{0}</label>", oms.GetInstanceText(elementContentInstance)));
							sw.WriteLine("</div>");
							sw.WriteLine("<div class=\"uwt-formview-item-content\">");

							Instance elementContentInstanceParentClass = oms.GetParentClass(elementContentInstance);
							if (elementContentInstanceParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
							{
								Instance[] instDisplayOptions = oms.GetRelatedInstances(elementContent, KnownRelationshipGuids.Element_Content__has__Element_Content_Display_Option);
								Instance instDO_ObscuredText = oms.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.ObscuredText);
								if (oms.InstanceSetContains(instDisplayOptions, instDO_ObscuredText))
								{
									sw.WriteLine(String.Format("<input type=\"password\" name=\"{0}\" />", String.Format("ec__{0}", ecid.ToString().Replace("$", "_"))));
								}
								else
								{
									sw.WriteLine(String.Format("<input type=\"text\" name=\"{0}\" />", String.Format("ec__{0}", ecid.ToString().Replace("$", "_"))));
								}
							}
							else
							{
								sw.WriteLine(String.Format("(EC not implemented for class '{0}')", elementContentInstanceParentClass.GlobalIdentifier));
							}

							sw.WriteLine("</div>");

							sw.WriteLine("</div>");
						}
					}
				}
				else
				{
					sw.WriteFixedLengthString(String.Format("<table data-instance-id=\"{0}\" data-element-id=\"{1}\"", instId, oms.GetInstanceKey(element)));
					WriteClassAttribute(sw, inst, "uwt-listview");

					sw.WriteFixedLengthString(">");


					sw.WriteLine("<caption><span class=\"uwt-title\">Test PageBuilder</span><span class=\"uwt-listview-item-count-label\"><span class=\"uwt-listview-item-count\">1</span> item</span>");
					sw.WriteLine("<div class=\"uwt-controlbox\">");

					sw.WriteLine("<a class=\"uwt-listview-controlbox-export\" href=\"#\" title=\"Export\"><i class=\"fa fa-download\"></i></a>");
					sw.WriteLine("<a class=\"uwt-listview-controlbox-filter\" href=\"#\" title=\"Filter\"><i class=\"fa fa-filter\"></i></a>");
					sw.WriteLine("<a class=\"uwt-listview-controlbox-chart\" href=\"#\" title=\"Chart\"><i class=\"fa fa-bar-chart\"></i></a>");
					sw.WriteLine("<a class=\"uwt-listview-controlbox-columns\" href=\"#\" title=\"Columns\"><i class=\"fa fa-columns\"></i></a>");
					sw.WriteLine("<a class=\"uwt-listview-controlbox-expand\" href=\"#\" title=\"Expand\"><i class=\"fa fa-expand\"></i></a>");

					sw.WriteLine("</div></caption>");

					if (element != null)
					{
						Instance[] elementContents = oms.GetRelatedInstances(element, KnownRelationshipGuids.Element__has__Element_Content);
						sw.WriteLine("<thead>");

						sw.WriteLine("<tr>");
						foreach (Instance elementContent in elementContents)
						{
							Instance elementContentInstance = oms.GetRelatedInstance(elementContent, KnownRelationshipGuids.Element_Content__has__Instance);
							InstanceKey ecid = oms.GetInstanceKey(elementContentInstance);

							sw.WriteLine(String.Format("<th data-ecid=\"{0}\">", oms.GetInstanceKey(elementContentInstance)));
							sw.WriteLine(String.Format("<a href=\"#\">{0}</a>", oms.GetInstanceText(elementContentInstance)));
							sw.WriteLine("</th>");
						}

						sw.WriteLine("</thead>");

						sw.WriteLine("<tbody>");

						sw.WriteLine("<tr>");
						foreach (Instance elementContent in elementContents)
						{
							sw.WriteLine("<td>");
							Instance elementContentInstance = oms.GetRelatedInstance(elementContent, KnownRelationshipGuids.Element_Content__has__Instance);
							InstanceKey ecid = oms.GetInstanceKey(elementContentInstance);

							Instance elementContentInstanceParentClass = oms.GetParentClass(elementContentInstance);

							if (elementContentInstanceParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
							{
								Instance[] instDisplayOptions = oms.GetRelatedInstances(elementContent, KnownRelationshipGuids.Element_Content__has__Element_Content_Display_Option);
								Instance instDO_ObscuredText = oms.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.ObscuredText);
								if (oms.InstanceSetContains(instDisplayOptions, instDO_ObscuredText))
								{
									sw.WriteLine(String.Format("<input type=\"password\" name=\"{0}\" />", String.Format("ec__{0}", ecid.ToString().Replace("$", "_"))));
								}
								else
								{
									sw.WriteLine(String.Format("<input type=\"text\" name=\"{0}\" />", String.Format("ec__{0}", ecid.ToString().Replace("$", "_"))));
								}
							}
							else
							{
								sw.WriteLine(String.Format("(EC not implemented for class '{0}')", elementContentInstanceParentClass.GlobalIdentifier));
							}

							sw.WriteLine("</td>");
						}
						sw.WriteLine("</tr>");

						sw.WriteLine("</tbody>");
					}
				}
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ParagraphPageComponent)
			{
				sw.WriteFixedLengthString(String.Format("<p data-instance-id=\"{0}\">", instId));

				Instance instMethod = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Content_Page_Component__gets_content_from__Method);

				OmsContext ctx = new OmsContext();
				object methodResult = oms.ExecuteMethod(instMethod, ctx);
				if (methodResult is string)
				{
					sw.Write((string)methodResult);
				}
				else if (methodResult is Instance[])
				{
					sw.Write(oms.GetInstanceText(((Instance[])methodResult)[0]));
				}
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TabContainerPageComponent)
			{

			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.SequentialContainerPageComponent)
			{
				sw.WriteFixedLengthString(String.Format("<div data-instance-id=\"{0}\" class=\"uwt-layout-box", instId));

				Instance instOrientation = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Sequential_Container_Page_Component__has__Sequential_Container_Orientation);
				if (instOrientation != null)
				{
					if (instOrientation.GlobalIdentifier == KnownInstanceGuids.Orientation.Horizontal)
					{
						sw.Write(" uwt-orientation-horizontal");
					}
					else if (instOrientation.GlobalIdentifier == KnownInstanceGuids.Orientation.Vertical)
					{
						sw.Write(" uwt-orientation-vertical");
					}
				}
				sw.Write("\"");

				sw.Write(">");
			}
		}

		private void WriteClassAttribute(StreamWriter sw, Instance inst, string requiredClasses = null)
		{
			System.Text.StringBuilder sbClassName = new System.Text.StringBuilder();
			if (requiredClasses != null)
			{
				sbClassName.Append(requiredClasses);
				if (!requiredClasses.EndsWith(" ", StringComparison.OrdinalIgnoreCase))
				{
					sbClassName.Append(' ');
				}
			}

			Instance instStyle = oms.GetRelatedInstance(inst, KnownRelationshipGuids.Page_Component__has__Style);
			if (instStyle != null)
			{
				Instance[] instStyleClasses = oms.GetRelatedInstances(instStyle, KnownRelationshipGuids.Style__has__Style_Class);
				for (int i = 0; i < instStyleClasses.Length; i++)
				{
					string value = oms.GetAttributeValue<string>(instStyleClasses[i], KnownAttributeGuids.Text.Value);
					sbClassName.Append(value);
					if (i < instStyleClasses.Length - 1)
					{
						sbClassName.Append(' ');
					}
				}
			}
			if (sbClassName.Length > 0)
			{
				sw.WriteFixedLengthString(String.Format(" class=\"{0}\"", sbClassName.ToString()));
			}
		}

		private void RenderEndTag(Instance inst, StreamWriter sw)
		{
			Instance instParentClass = oms.GetParentClass(inst);
			if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ImagePageComponent)
			{

			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
			{
				sw.WriteLine("</div>");
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ButtonPageComponent)
			{
				sw.WriteLine("</button>");
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.DetailPageComponent)
			{

			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.HeadingPageComponent)
			{
				decimal level = oms.GetAttributeValue<decimal>(inst, KnownAttributeGuids.Numeric.Level);
				sw.WriteFixedLengthString(String.Format("</h{0}>", level));
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.SummaryPageComponent)
			{
				sw.WriteLine("</div>");
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ElementPageComponent)
			{
				Instance[] elementDisplayOptions = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Element_Page_Component__has__Element_Content_Display_Option);
				Instance ecDoSingular = oms.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.Singular);

				bool singular = (oms.InstanceSetContains(elementDisplayOptions, ecDoSingular));

				if (singular)
				{
					sw.WriteLine("</div>");
				}
				else
				{
					sw.WriteLine("</table>");
				}
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.ParagraphPageComponent)
			{
				sw.WriteLine("</p>");
			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TabContainerPageComponent)
			{

			}
			else if (instParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.SequentialContainerPageComponent)
			{
				sw.WriteLine("</div>");
				sw.WriteLine();
			}
		}


		public void RenderPageComponent(Instance inst, StreamWriter sw)
		{
			Instance instParentClass = oms.GetParentClass(inst);

			RenderBeginTag(inst, sw);
			sw.WriteLine();

			if (oms.IsClassSubclassOf(instParentClass, KnownInstanceGuids.Classes.ContainerPageComponent))
			{
				Instance[] instChildren = oms.GetRelatedInstances(inst, KnownRelationshipGuids.Container_Page_Component__has__Page_Component);
				foreach (Instance inst2 in instChildren)
				{
					RenderPageComponent(inst2, sw);
				}
				sw.WriteLine();
			}

			RenderEndTag(inst, sw);
		}

		public void RenderPage(Instance instPage, StreamWriter sw)
		{
			Instance instMasterPage = oms.GetRelatedInstance(instPage, KnownRelationshipGuids.Page__has_master__Page);

			sw.WriteLine("<html>");
			sw.WriteLine("<head>");
			sw.Write("<title>");
			sw.WriteLine(GetPageTitle(instPage));
			sw.WriteLine("</title>");

			Instance instTenant = oms.GetTenantInstance();
			Instance instShortcutIcon = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has_icon_image__File);

			sw.WriteLine(String.Format("<link rel=\"shortcut icon\" href=\"{0}\" />", sess.GetAttachmentUrl(instShortcutIcon, sess.GetOmsAttachmentEntropy())));
			sw.WriteLine("<link rel=\"stylesheet\" type=\"text/css\" href=\"/css/uwt.css\" />");
			sw.WriteLine("<style type=\"text/css\">");

			if (instMasterPage != null)
			{
				WritePageStyles(sw, instMasterPage);
			}

			WritePageStyles(sw, instPage);

			sw.WriteLine("</style>");
			sw.WriteLine("</head>");

			sw.WriteLine("<body>");
			sw.WriteLine("<form method=\"POST\">");
			sw.WriteLine(String.Format("<!-- page id: {0} -->", instPage.GlobalIdentifier));

			Instance[] pageComponents = oms.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Page_Component);

			foreach (Instance inst in pageComponents)
			{
				RenderPageComponent(inst, sw);
			}
			// sw.WriteLine(String.Format("<!DOCTYPE html>\n<html>\n<head><title>{0}</title>", ctx.PageTitle));
			// sw.WriteLine("<meta http-equiv=\"X-UA-Compatible\" content=\"chrome=1;IE=EDGE\"/>\n    <meta http-equiv=\"Content-type\" content=\"text/html; charset=UTF-8\"/>\n    <meta name=\"description\" content=\"\">\n    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no\">\n    <meta name=\"author\" content=\"\">\n    <script type=\"text/javascript\">\n        function scriptLoadFallback() {\n            if (window.location.search.indexOf(\"reloaded\") == -1) {\n                window.location.search += '&' + encodeURIComponent(\"reloaded\");\n            } else if (workday.maintenancePageUrl) {\n                window.location = workday.maintenancePageUrl;\n            } else {\n                window.location = \"https://community.workday.com/maintenance-page\";\n            }\n        }\n    </script>\n    <script type=\"text/javascript\">\n        // Add properties to workday\n        window.workday = window.workday || {};\n        workday.tenant = 'cityoforlando1';\n        workday.clientOrigin = 'https://wd5-impl.workday.com';\n        workday.language = 'en_US';\n        workday.clientVersion = '0';\n        workday.systemConfidenceLevel = 'PROD';\n        workday.oauthAuthorizationPending = false;\n        workday.proxyLoginEnabled = false;\n        workday.maintenancePageUrl = 'https://wd5-impl.workday.com/wday/drs/outage?t=cityoforlando1';\n        workday.deviceTrustDetailsUrl = '';\n        workday.pendingAuthDetailsUrl = '';\n        workday.webAuthnDetailsUrl = '';\n        workday.enableBluePrimaryButtons = 'false';\n\n        // Construct init params for GWT app\n        workday.initParams = {\n            authGatewayPath: '/wday/authgwy',\n            baseDir: '/wday/asset/ui-html/',\n            systemConfidenceLevel: 'PROD',\n            cdn: {\n                endpoint: 'wd5-impl.workdaycdn.com',\n                enabled: true,\n                allowed: true\n            },\n            proxyEnabled: false,\n            currentVersion: '20.0.04.045',\n            serviceType: 'authgwy',\n            loginAuthURL: '/cityoforlando1/login-auth.xml',\n            environment: 'Implementation  -   cityoforlando1',\n            environmentType: 'IMPL'\n        };\n    </script>\n    <script type=\"text/javascript\" onerror=\"scriptLoadFallback()\"\n            src=\"https://wd5-impl.workday.com/wday/asset/ui-html/base/shared-min.js?1671959227383\">\n    </script>\n    <script\n    type = \"text/javascript\"\n    src = \"https://wd5-impl.workday.com/wday/asset/uic-shared-vendors/shared-vendors.min.js\"/>\n    </script>\n    <script type=\"text/javascript\">\n        function isOAuthAuthorizationPending() {\n            return workday.oauthAuthorizationPending;\n        }\n\n        workday.appRoot = \"{\\\"widget\\\":\\\"appRoot\\\"}\";\n        workday.clientRenderer.renderLoginApp(workday.initParams);\n    </script>\n</head>\n<body>\n<div class=\"wrapper\">\n    <!-- DIV used by UIS acceptance tests to identify which login page is loaded -->\n    <div id=\"htmld_login\"></div>\n    <div id=\"container\">\n        <style>\n            #__gwt_historyFrame {\n                position: absolute;\n                top: -1px;\n                left: -1px;\n                width: 0px;\n                height: 0px;\n                border: 0px solid rgba(0, 0, 0, 0.00);\n            }\n        </style>\n        <div id=\"login-wrap\" class=\"clearfix\">\n            <div id=\"login-area\" data-automation-id=\"login\">\n                <div id=\"htmlContent\"></div>\n            </div>\n            <!-- END WORKDAY LOGIN -->\n        </div>\n        <div id=\"notice-detail\">\n        </div>\n    </div>\n    <div class=\"clearfix\"></div>\n</div>\n<div class=\"footer\">\n    <div id=\"schedule\">\n    </div>\n    <span id=\"copyright\">\n\t\t</span>\n\n    <span id=\"powered-by\"></span>\n</div>\n<iframe src=\"javascript:''\" id=\"__gwt_historyFrame\" tabIndex='-1'\n        style=\"position:absolute;width:0;height:0;border:0\"></iframe>\n<div id=\"spinnerContainer\"></div>\n<script type=\"text/javascript\">\n    /*<![CDATA[*/\n    function populateContent() {\n        if (uri['args']['preload'] == 'true') {\n            writeStaticFileAsScript('https://wd5-impl.workday.com/wday/asset/ui-html/update/WorkdayApp/WorkdayApp.nocache.js?' + workday.checksums[\"WorkdayApp.nocache.js\"]);\n        }\n    }\n\n    if (document.readyState != \"loading\") {\n        populateContent();\n    } else {\n        document.onreadystatechange = function () {\n            populateContent();\n            document.onreadystatechange = null;\n        }\n    }\n    /*]]>*/\n</script>\n</body>\n</html>");

			sw.WriteLine("</form>");
			sw.WriteLine("</body>");
			sw.WriteLine("</html>");
		}

		private string GetPageTitle(Instance instPage)
		{
			string title = oms.GetInstanceText(instPage);
			if (title == null)
			{
				Instance instMasterPage = oms.GetRelatedInstance(instPage, KnownRelationshipGuids.Page__has_master__Page);
				title = oms.GetInstanceText(instMasterPage);
			}
			return title;
		}

		private void WritePageStyles(StreamWriter sw, Instance instPage)
		{
			Instance[] instStyles = oms.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Style);
			sw.Write("body { ");
			foreach (Instance instStyle in instStyles)
			{
				RenderStyleSheet(sw, instStyle);
			}
			sw.Write(" }");

			Instance[] instComponents = oms.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Page_Component);
			foreach (Instance instComponent in instComponents)
			{
				ApplyComponentStyle(sw, instComponent);
			}
		}

		private void ApplyComponentStyle(StreamWriter sw, Instance instComponent)
		{
			sw.WriteFixedLengthString(String.Format("[data-instance-id=\"{0}\"] {{", oms.GetInstanceKey(instComponent)));
			Instance[] instComponentStyles = oms.GetRelatedInstances(instComponent, KnownRelationshipGuids.Page_Component__has__Style);
			foreach (Instance instComponentStyle in instComponentStyles)
			{
				RenderStyleSheet(sw, instComponentStyle);
			}
			sw.Write(" } ");

			Instance instComponentClass = oms.GetParentClass(instComponent);
			if (oms.IsClassSubclassOf(instComponentClass, KnownInstanceGuids.Classes.ContainerPageComponent))
			{
				Instance[] instSubComponents = oms.GetRelatedInstances(instComponent, KnownRelationshipGuids.Container_Page_Component__has__Page_Component);
				foreach (Instance instSubComponent in instSubComponents)
				{
					ApplyComponentStyle(sw, instSubComponent);
				}
			}
			if (instComponentClass.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
			{
				Instance[] instHeaderComponents = oms.GetRelatedInstances(instComponent, KnownRelationshipGuids.Panel_Page_Component__has_header__Page_Component);
				foreach (Instance inst2 in instHeaderComponents)
				{
					ApplyComponentStyle(sw, inst2);
				}
				Instance[] instContentComponents = oms.GetRelatedInstances(instComponent, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
				foreach (Instance inst2 in instContentComponents)
				{
					ApplyComponentStyle(sw, inst2);
				}
				Instance[] instFooterComponents = oms.GetRelatedInstances(instComponent, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
				foreach (Instance inst2 in instFooterComponents)
				{
					ApplyComponentStyle(sw, inst2);
				}
			}
		}

		private void RenderStyleSheet(StreamWriter sw, Instance instStyle)
		{
			Instance[] instStylesImplemented = oms.GetRelatedInstances(instStyle, KnownRelationshipGuids.Style__implements__Style);
			foreach (Instance inst in instStylesImplemented)
			{
				RenderStyleSheet(sw, inst);
			}

			Instance instBackgroundImage = oms.GetRelatedInstance(instStyle, KnownRelationshipGuids.Style__gets_background_image_File_from__Return_Instance_Set_Method_Binding);
			if (instBackgroundImage != null)
			{
				OmsContext ctx = new OmsContext();
				object instFileRet = oms.ExecuteMethod(instBackgroundImage, ctx);
				if (instFileRet is Instance[])
				{
					Instance instFile = ((Instance[])instFileRet)[0];
					sw.WriteFixedLengthString(String.Format("background-image: url('{0}');", sess.GetAttachmentUrl(instFile, sess.GetOmsAttachmentEntropy())));
				}
			}

			Instance[] instStyleRules = oms.GetRelatedInstances(instStyle, KnownRelationshipGuids.Style__has__Style_Rule);
			foreach (Instance instStyleRule in instStyleRules)
			{
				Instance instStyleProperty = oms.GetRelatedInstance(instStyleRule, KnownRelationshipGuids.Style_Rule__has__Style_Property);
				string cssName = oms.GetAttributeValue<string>(instStyleProperty, KnownAttributeGuids.Text.CSSValue);
				string value = oms.GetAttributeValue<string>(instStyleRule, KnownAttributeGuids.Text.Value);

				sw.WriteFixedLengthString(String.Format("{0}: {1};", cssName, value));
			}
		}
	}
}
