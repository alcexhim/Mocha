//
//  MochaExtensions.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2021 Mike Becker's Software
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
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web
{
	public static class MochaExtensions
	{
		public static string ExpandRelativePath(this System.Web.UI.Page page, string path)
		{
			string currentTenantname = page.GetCurrentTenantName();
			path = path.Replace("~/", String.Format("~/{0}/", currentTenantname));
			return MBS.Web.ExtensionMethods.ExpandRelativePath(page, path);
		}

		public static string GetCurrentTenantName(this System.Web.UI.Page page)
		{
			string CurrentTenantName = System.Configuration.ConfigurationManager.AppSettings["Tenant.Default.Name"];
			if (page.Request.QueryString["tenantName"] != null)
			{
				string tenantName = page.Request.QueryString["tenantName"];

				// prevent someone from appending ?tenantName=... to trick us
				int indexOfComma = tenantName.IndexOf(',');
				if (indexOfComma > -1)
				{
					tenantName = tenantName.Substring(0, indexOfComma);
				}
				return tenantName;
			}

			string[] pathsplit = page.Request.Path.Split(new char[] { '/' });
			if (pathsplit.Length >= 2)
			{
				CurrentTenantName = pathsplit[1];
				if (CurrentTenantName == "madi")
				{
					CurrentTenantName = pathsplit[3];
				}
			}
			return CurrentTenantName;
		}
		public static string GetCurrentTenantName(this System.Web.UI.MasterPage page)
		{
			return page.Page.GetCurrentTenantName();
		}

		public static void ClearTenantedVariable(this System.Web.UI.Page page, string variableName)
		{
			page.ClearTenantedVariable(page.GetCurrentTenantName(), variableName);
		}
		public static void ClearTenantedVariable(this System.Web.UI.Page page, string tenantName, string variableName)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			dict.Remove(tenantName);
		}
		public static bool HasTenantedVariable(this System.Web.UI.Page page, string variableName)
		{
			return page.HasTenantedVariable(page.GetCurrentTenantName(), variableName);
		}
		public static bool HasTenantedVariable(this System.Web.UI.Page page, string tenantName, string variableName)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			return dict.ContainsKey(tenantName);
		}
		public static object GetTenantedVariable(this System.Web.UI.Page page, string variableName, object defaultValue = null)
		{
			return page.GetTenantedVariable(page.GetCurrentTenantName(), variableName, defaultValue);
		}
		public static object GetTenantedVariable(this System.Web.UI.Page page, string tenantName, string variableName, object defaultValue = null)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			if (dict.ContainsKey(tenantName))
				return dict[tenantName];
			return defaultValue;
		}

		public static void ClearTenantedVariable(this System.Web.UI.MasterPage page, string variableName)
		{
			page.ClearTenantedVariable(page.GetCurrentTenantName(), variableName);
		}
		public static void ClearTenantedVariable(this System.Web.UI.MasterPage page, string tenantName, string variableName)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			dict.Remove(tenantName);
		}
		public static bool HasTenantedVariable(this System.Web.UI.MasterPage page, string variableName)
		{
			return page.HasTenantedVariable(page.GetCurrentTenantName(), variableName);
		}
		public static bool HasTenantedVariable(this System.Web.UI.MasterPage page, string tenantName, string variableName)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			return dict.ContainsKey(tenantName);
		}
		public static object GetTenantedVariable(this System.Web.UI.MasterPage page, string variableName, object defaultValue = null)
		{
			return page.GetTenantedVariable(page.GetCurrentTenantName(), variableName, defaultValue);
		}
		public static object GetTenantedVariable(this System.Web.UI.MasterPage page, string tenantName, string variableName, object defaultValue = null)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			if (dict.ContainsKey(tenantName))
				return dict[tenantName];
			return defaultValue;
		}

		public static void SetTenantedVariable(this System.Web.UI.Page page, string variableName, object value)
		{
			page.SetTenantedVariable(page.GetCurrentTenantName(), variableName, value);
		}
		public static void SetTenantedVariable(this System.Web.UI.Page page, string tenantName, string variableName, object value)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			dict[tenantName] = value;
		}

		public static void SetTenantedVariable(this System.Web.UI.MasterPage page, string variableName, object value)
		{
			page.SetTenantedVariable(page.GetCurrentTenantName(), variableName, value);
		}
		public static void SetTenantedVariable(this System.Web.UI.MasterPage page, string tenantName, string variableName, object value)
		{
			if (!(page.Session[variableName] is Dictionary<string, object> dict))
			{
				dict = new Dictionary<string, object>();
				page.Session[variableName] = dict;
			}
			dict[tenantName] = value;
		}

		public static System.Collections.Generic.Dictionary<InstanceKey, byte[]> GetOmsAttachmentEntropy(this System.Web.UI.Page page)
		{
			object sess = page.GetTenantedVariable("SessionContext");
			if (sess == null)
			{
				SessionContext ctx = new SessionContext();
				ctx.TenantName = page.GetCurrentTenantName();
				page.SetTenantedVariable("SessionContext", ctx);
			}
			return ((SessionContext)page.GetTenantedVariable("SessionContext")).GetOmsAttachmentEntropy();
		}

		public static string FormatPageTitle(this System.Web.UI.Page page, string pageTitle)
		{
			Oms oms = page.GetOMS();
			if (oms == null) return pageTitle;

			oms.TenantName = page.GetCurrentTenantName();

			Instance instTenant = oms.GetTenantInstance();
			Instance instApplication = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has__Application);
			return String.Format("{0} - {1}", pageTitle, oms.GetInstanceText(instApplication));
		}

		/// <summary>
		/// Retrieves the <see cref="Oms" /> for the given <see cref="System.Web.UI.Page" />.
		/// </summary>
		/// <returns>The oms.</returns>
		/// <param name="page">Page.</param>
		public static Oms GetOMS(this System.Web.UI.Page page)
		{
			SessionContext ctx = (page.GetTenantedVariable("SessionContext") as SessionContext);
			if (ctx == null)
			{
				return null;
			}

			Oms oms = ctx.GetOms();
			if (oms == null)
			{
				return null;
			}

			lock (oms)
			{
				oms.TenantName = page.GetCurrentTenantName();
			}
			return oms;
		}
	}
}
