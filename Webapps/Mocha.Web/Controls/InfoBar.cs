//
//  InfoBar.cs
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
using System.Web.UI;
using System.Web.UI.WebControls;

using MBS.Web;
using Mocha.Core;
using Mocha.OMS;
using Mocha.Web.MasterPages;

namespace Mocha.Web.Controls
{
	public class InfoBar : WebControl
	{
		protected override HtmlTextWriterTag TagKey => HtmlTextWriterTag.Div;

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			this.AddCssClass("mocha-tenant-infobar");
			this.AddCssClass("uwt-badge");

			string CurrentTenantName = Page.GetCurrentTenantName();

			Label lblInfoBarTenantType = new Label();
			lblInfoBarTenantType.Text = "Tenant Not Found";
			Controls.Add(lblInfoBarTenantType);

			Label lblSystemVersionSeparator = new Label();
			lblSystemVersionSeparator.Text = " - ";
			Controls.Add(lblSystemVersionSeparator);

			Label lblSystemVersion = new Label();
			lblSystemVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Controls.Add(lblSystemVersion);

			Label lblTenantNameSeparator = new Label();
			lblTenantNameSeparator.Text = " - ";
			Controls.Add(lblTenantNameSeparator);

			Label lblTenantName = new Label();
			lblTenantName.Text = CurrentTenantName;
			Controls.Add(lblTenantName);

			Oms oms = Page.GetOMS();
			if (oms == null)
			{
				Visible = false;

				base.RenderBeginTag(writer);
				return;
			}

			Instance instTenant = oms.GetTenantInstance();
			if (instTenant == null)
			{
				Visible = false;

				base.RenderBeginTag(writer);
				return;
			}

			Instance instTenantType = oms.GetRelatedInstance(instTenant, KnownRelationshipGuids.Tenant__has__Tenant_Type);
			string badgeBackgroundColor = oms.GetAttributeValue<string>(instTenantType, KnownAttributeGuids.Text.BackgroundColor);
			BackColor = (MBS.Framework.Drawing.Color.Parse(badgeBackgroundColor)).ToGDIColor();
			string badgeForegroundColor = oms.GetAttributeValue<string>(instTenantType, KnownAttributeGuids.Text.ForegroundColor);
			ForeColor = (MBS.Framework.Drawing.Color.Parse(badgeForegroundColor)).ToGDIColor();

			bool badgeDisplayVersion = oms.GetAttributeValue<bool>(instTenantType, KnownAttributeGuids.Boolean.DisplayVersionInBadge);
			lblSystemVersionSeparator.Visible = badgeDisplayVersion;
			lblSystemVersion.Visible = badgeDisplayVersion;

			string badgeTitle = oms.GetTranslationValue(instTenantType, KnownRelationshipGuids.Tenant_Type__has_title__Translatable_Text_Constant);
			lblInfoBarTenantType.Text = badgeTitle;

			base.RenderBeginTag(writer);
		}

	}
}
