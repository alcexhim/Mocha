//
//  PageBuilder.cs
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

using MBS.Web;
using MBS.Web.Controls;
using Mocha.Core;
using Mocha.OMS;
using Mocha.OMS.OMSComponents;
using Mocha.Web.Controls;

namespace Mocha.Web.Server
{
	public class PageBuilder
	{
		public Oms OMS { get; } = null;
		public PageBuilder(Oms oms)
		{
			OMS = oms;
		}

		public Control RenderOMSComponent(OMSComponent comp)
		{
			if (comp is OMSDetailComponent)
			{
				ListView lvReport = new ListView();

				// PageBuilder component : Detail Page Component
				// gets columns from:	Detail Page Component.has column source Method
				// gets rows from:		Detail Page Component.has row source Method

				OMSDetailComponent cmp = (comp as OMSDetailComponent);
				foreach (OMSDetailComponent.OMSDetailColumn col in cmp.Columns)
				{
					ListViewColumn lvc = new ListViewColumn();
					lvc.ID = "ReportColumn_" + col.InstanceID.ToString();
					lvc.Title = col.Title;
					lvc.Attributes.Add("data-instance-id", col.InstanceID.ToString()); // report column inst id
					lvReport.Columns.Add(lvc);
				}
				foreach (OMSDetailComponent.OMSDetailRow row in cmp.Rows)
				{
					ListViewItem lvi = new ListViewItem();
					lvi.Attributes.Add("data-instance-id", row.InstanceID.ToString());
					foreach (OMSDetailComponent.OMSDetailRowColumn col in row.Columns)
					{
						ListViewItemColumn lvic = null;
						if (col.InstanceIDs != null)
						{
							lvic = new Mocha.Web.Controls.ListViewItemColumnInstance();
							lvic.Attributes.Add("data-rcid", col.ColumnInstanceID.ToString()); // report column inst id
							for (int i = 0; i < col.InstanceIDs.Length; i++)
							{
								(lvic as Mocha.Web.Controls.ListViewItemColumnInstance).InstanceIDs.Add(col.InstanceIDs[i]);
							}
							(lvic as Mocha.Web.Controls.ListViewItemColumnInstance).DisplayAsCount = col.DisplayAsCount;
							// (lvic as Mocha.Web.Controls.ListViewItemColumnInstance).Text = col.Value;
						}
						else
						{
							lvic = new ListViewItemColumn();
							lvic.Value = col.Value;
						}
						lvic.ColumnID = "ReportColumn_" + col.ColumnInstanceID.ToString();
						lvi.Columns.Add(lvic);
					}
					lvReport.Items.Add(lvi);
				}
				return lvReport;
			}
			else if (comp is OMSSummaryComponent)
			{
				OMSSummaryComponent cmp = (comp as OMSSummaryComponent);
				FormView fv = new FormView();
				fv.CssClass = "mcx-summary";
				fv.Attributes["data-instance-id"] = cmp.InstanceID.ToString();
				foreach (OMSSummaryComponent.OMSSummaryField field in cmp.Fields)
				{
					if (field is OMSSummaryComponent.OMSSummaryFieldText)
					{
						OMSSummaryComponent.OMSSummaryFieldText fld = (field as OMSSummaryComponent.OMSSummaryFieldText);
						FormViewItemText fvi = new FormViewItemText();
						fvi.Attributes["data-instance-id"] = field.InstanceID.ToString();
						fvi.Title = fld.Title;
						fvi.ReadOnly = fld.ReadOnly;
						fvi.Value = fld.Value?.ToString();
						fv.Items.Add(fvi);
					}
					else if (field is OMSSummaryComponent.OMSSummaryFieldInstance)
					{
						OMSSummaryComponent.OMSSummaryFieldInstance fld = (field as OMSSummaryComponent.OMSSummaryFieldInstance);
						FormViewItemInstance fvi = new FormViewItemInstance();
						fvi.Attributes["data-instance-id"] = field.InstanceID.ToString();
						fvi.Title = fld.Title;
						fvi.ReadOnly = fld.ReadOnly;
						fvi.Value = fld.Value?.ToString();
						for (int i = 0; i < fld.ValidClassIDs.Count; i++)
						{
							fvi.ValidClassIDs.Add(fld.ValidClassIDs[i]);
						}
						fv.Items.Add(fvi);
					}
					else if (field is OMSSummaryComponent.OMSSummaryFieldDateTime)
					{
						OMSSummaryComponent.OMSSummaryFieldDateTime fld = (field as OMSSummaryComponent.OMSSummaryFieldDateTime);
						FormViewItemDateTime fvi = new FormViewItemDateTime();
						fvi.Attributes["data-instance-id"] = field.InstanceID.ToString();
						fvi.Title = fld.Title;
						fvi.ReadOnly = fld.ReadOnly;
						fvi.Value = fld.Value?.ToString();
						fv.Items.Add(fvi);
					}
					else if (field is OMSSummaryComponent.OMSSummaryFieldBoolean)
					{
						OMSSummaryComponent.OMSSummaryFieldBoolean fld = (field as OMSSummaryComponent.OMSSummaryFieldBoolean);
						FormViewItemBoolean fvi = new FormViewItemBoolean();
						fvi.Attributes["data-instance-id"] = field.InstanceID.ToString();
						fvi.Title = fld.Title;
						fvi.ReadOnly = fld.ReadOnly;
						fvi.Value = fld.Value?.ToString();
						fv.Items.Add(fvi);
					}
					fv.Items[fv.Items.Count - 1].Name = String.Format("SummaryComponent_{0}_{1}", cmp.InstanceID.ToString(), field.InstanceID.ToString());
				}
				return fv;
			}
			else if (comp is OMSTabContainerComponent)
			{
				TabContainer tbs = new TabContainer();
				OMSTabContainerComponent cmp = (comp as OMSTabContainerComponent);
				foreach (OMSTabContainerComponent.TabPage page in cmp.TabPages)
				{
					TabPage pg = new TabPage();
					pg.Title = page.Title;
					foreach (OMSComponent comp1 in page.Components)
					{
						Control ctl1 = RenderOMSComponent(comp1);
						pg.Controls.Add(ctl1);
					}
					tbs.TabPages.Add(pg);
				}
				return tbs;
			}
			else if (comp is OMSSequentialContainerComponent)
			{
				System.Web.UI.HtmlControls.HtmlGenericControl div = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
				div.AddCssClass("uwt-layout uwt-layout-box");
				div.AddCssClass("uwt-orientation-vertical");

				foreach (OMSComponent comp2 in ((OMSContainerComponent)comp).Components)
				{
					System.Web.UI.HtmlControls.HtmlGenericControl div2 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
					div2.AddCssClass("uwt-layout-item");

					Control ctl2 = RenderOMSComponent(comp2);
					if (ctl2 == null)
					{
						// MADI error: PageBuilder failed to create component ()
						continue;
					}
					div2.Controls.Add(ctl2);

					div.Controls.Add(div2);
				}

				return div;
			}
			else if (comp is OMSHeaderComponent)
			{
				int level = ((OMSHeaderComponent)comp).Level;
				if (level < 1 || level > 6)
				{
					// add MADI error: level must be between 1 and 6 inclusive
					return null;
				}

				System.Web.UI.HtmlControls.HtmlGenericControl h = new System.Web.UI.HtmlControls.HtmlGenericControl(String.Format("h{0}", level));
				h.InnerText = ((OMSHeaderComponent)comp).Text;
				return h;
			}
			else if (comp is OMSParagraphComponent)
			{
				System.Web.UI.HtmlControls.HtmlGenericControl h = new System.Web.UI.HtmlControls.HtmlGenericControl("p");
				h.InnerText = ((OMSParagraphComponent)comp).Text;
				return h;
			}
			else if (comp is OMSImageComponent)
			{
				InstanceKey targetFile = ((OMSImageComponent)comp).TargetFileInstanceID;
				if (targetFile == InstanceKey.Empty)
					return null;

				Instance instTargetFile = OMS.GetInstance(targetFile);

				System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
				if (instTargetFile != null)
				{
					image.ImageUrl = String.Format("~/Images/Uploads/{0}.png", instTargetFile.GlobalIdentifier.ToString("B").ToLower());
				}
				return image;
			}
			else if (comp is OMSPanelComponent)
			{
				Panel panel = new Panel();
				OMSPanelComponent pcomp = (OMSPanelComponent)comp;
				for (int i = 0; i < pcomp.HeaderComponents.Count; i++)
				{
					Control ctl = RenderOMSComponent(pcomp.HeaderComponents[i]);
					if (ctl == null)
						continue;

					panel.HeaderControls.Controls.Add(ctl);
				}
				for (int i = 0; i < pcomp.ContentComponents.Count; i++)
				{
					Control ctl = RenderOMSComponent(pcomp.ContentComponents[i]);
					if (ctl == null)
						continue;

					panel.ContentControls.Controls.Add(ctl);
				}
				for (int i = 0; i < pcomp.FooterComponents.Count; i++)
				{
					Control ctl = RenderOMSComponent(pcomp.FooterComponents[i]);
					if (ctl == null)
						continue;

					panel.FooterControls.Controls.Add(ctl);
				}
				return panel;
			}
			return null;
		}

		public void RenderPage(Instance instPage, Control control)
		{
			OmsContext context = new OmsContext();
			Instance[] instPageComponents = OMS.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Page_Component);
			for (int i = 0; i < instPageComponents.Length; i++)
			{
				Control ctl = CreatePageComponent(instPageComponents[i], context);
				if (ctl != null)
				{
					control.Controls.Add(ctl);
				}
			}
		}

		internal void RenderResponse(IOmsResponse resp, Control page)
		{
			for (int i = 0; i < resp.Components.Count; i++)
			{
				Control comp = RenderOMSComponent(resp.Components[i]);
				if (comp == null) continue;

				page.Controls.Add(comp);
			}
		}

		public Control CreatePageComponent(Instance inst, OmsContext context)
		{
			Instance parentClassInstance = OMS.GetParentClass(inst);
			if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.SequentialContainerPageComponent)
			{
				return CreateSequentialContainerPageComponent(inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ImagePageComponent)
			{
				return CreateImagePageComponent(inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.HeadingPageComponent)
			{
				return CreateHeadingPageComponent(inst, context);
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ParagraphPageComponent)
			{
				System.Web.UI.HtmlControls.HtmlGenericControl p = new System.Web.UI.HtmlControls.HtmlGenericControl("p");
				Instance instText = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Content_Page_Component__gets_content_from__Method);
				p.InnerHtml = OMS.ExecuteMethodReturningTextOrTranslation(instText, context);
				return p;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
			{
				MBS.Web.Controls.Panel panel = new MBS.Web.Controls.Panel();

				Instance[] instHeaderComponents = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_header__Page_Component);
				for (int i = 0; i < instHeaderComponents.Length; i++)
				{
					Control ctl = CreatePageComponent(instHeaderComponents[i], context);
					if (ctl == null) continue;

					panel.HeaderControls.Controls.Add(ctl);
				}
				Instance[] instContentComponents = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
				for (int i = 0; i < instContentComponents.Length; i++)
				{
					Control ctl = CreatePageComponent(instContentComponents[i], context);
					if (ctl == null) continue;

					panel.ContentControls.Controls.Add(ctl);
				}
				Instance[] instFooterComponents = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
				for (int i = 0; i < instFooterComponents.Length; i++)
				{
					Control ctl = CreatePageComponent(instFooterComponents[i], context);
					if (ctl == null) continue;

					panel.FooterControls.Controls.Add(ctl);
				}

				return panel;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ButtonPageComponent)
			{
				System.Web.UI.WebControls.Button item = new System.Web.UI.WebControls.Button();
				item.Text = OMS.GetTranslationValue(inst, KnownRelationshipGuids.Button_Page_Component__has_text__Translatable_Text_Constant);
				return item;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.DetailPageComponent)
			{
				MBS.Web.Controls.ListView lv = new ListView();

				Instance instColumnSourceRSMB = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Detail_Page_Component__has_column_source__Method_Binding);
				// column source can return an Instance Set of columns, or a method returning an instance set of columns
				object columnSourceValue = OMS.ExecuteMethod(instColumnSourceRSMB, context);

				if (columnSourceValue is Instance[])
				{
					Instance[] reportColumns = (Instance[])columnSourceValue;
					foreach (Instance instColumn in reportColumns)
					{
						ListViewColumn lvc = CreateListViewColumnForReportColumn(instColumn);
						if (lvc != null)
							lv.Columns.Add(lvc);
					}
				}

				Instance instRowSourceRSMB = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Detail_Page_Component__has_row_source__Method_Binding);

				lv.Title = OMS.GetTranslationValue(inst, KnownRelationshipGuids.Detail_Page_Component__has_caption__Translation);
				return lv;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.SummaryPageComponent)
			{
				MBS.Web.Controls.FormView item = new MBS.Web.Controls.FormView();
				item.CssClass = "mcx-summary";
				item.Attributes["data-instance-id"] = OMS.GetInstanceKey(inst).ToString();

				object bEditable = OMS.GetAttributeValue(inst, KnownAttributeGuids.Boolean.Editable, false);
				// item.Editable = (bool)bEditable;
				return item;
			}
			throw new NotSupportedException();
		}

		private ListViewColumn CreateListViewColumnForReportColumn(Instance instColumn)
		{
			ListViewColumn lvc = new ListViewColumn();
			InstanceKey ik = OMS.GetInstanceKey(instColumn);
			lvc.ID = "ReportColumn_" + ik.ToString();
			lvc.Title = OMS.GetInstanceText(instColumn);
			lvc.Attributes.Add("data-instance-id", ik.ToString()); // report column inst id
			return lvc;
		}


		private System.Web.UI.WebControls.Panel CreateSequentialContainerPageComponent(Instance inst, OmsContext context)
		{
			System.Web.UI.WebControls.Panel panel = new System.Web.UI.WebControls.Panel();

			Instance instOrientation = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Sequential_Container_Page_Component__has__Sequential_Container_Orientation);
			// TODO: set the orientation of the Panel for the Sequential Container Page Component

			Instance[] instComponents = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Container_Page_Component__has__Page_Component);
			for (int i = 0; i < instComponents.Length; i++)
			{
				Control ctl = CreatePageComponent(instComponents[i], context);
				if (ctl == null) continue;

				panel.Controls.Add(ctl);
			}

			return panel;
		}
		private System.Web.UI.HtmlControls.HtmlGenericControl CreateHeadingPageComponent(Instance inst, OmsContext context)
		{
			System.Web.UI.HtmlControls.HtmlGenericControl ctl = null;
			object jlevel = OMS.GetAttributeValue(inst, KnownAttributeGuids.Numeric.Level);
			ctl = new System.Web.UI.HtmlControls.HtmlGenericControl("h" + jlevel.ToString());

			string value = OMS.GetTranslationValue(inst, new Guid("{C5027DC2-53EE-4FC0-9BA6-F2B883F7DAD8}"));
			ctl.InnerHtml = value;
			return ctl;
		}

		// FIXME: BIG PROBLEM!!!
		// right now the CLIENT is bouncing back and forth requesting EVERY LITTLE INSTANCE From the server
		// especially when loading Pages, the CLIENT should make ONE SINGLE REQUEST for the page
		// the server should then push out EVERYTHING The client needs to make sense of the page
		// (e.g. push out an entire JSON response representing all the page components on the page)
		// rather than the client having to do this... handling 1000+ requests to and from the OMS each time.

		// tl;dr : THERE SHOULD ONLY BE ONE REQUEST TO THE OMS PER PAGE!!!

		private System.Web.UI.WebControls.Image CreateImagePageComponent(Instance inst, OmsContext context)
		{
			System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();

			Instance instTargetFile = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Image_Page_Component__has_source__Method);
			if (instTargetFile == null)
				return null;

			string attfmt = (OMS.GetAttributeValue(instTargetFile, KnownAttributeGuids.Text.ContentType) as string);
			string attvalue = (OMS.GetAttributeValue(instTargetFile, KnownAttributeGuids.Text.Value) as string);
			// image.ImageUrl = String.Format("data:{0};base64,{1}", attfmt, attvalue);
			image.ImageUrl = String.Format("~/Images/Uploads/{0}.png", instTargetFile.GlobalIdentifier.ToString("b"));

			return image;
		}
	}
}
