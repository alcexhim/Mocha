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

namespace Mocha.Web
{
	public class PageBuilder
	{
		private SessionContext sess = null;
		public Oms OMS { get { return sess.GetOms(); } }

		public PageBuilder(SessionContext context)
		{
			sess = context;
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

		private void WritePageStyles(System.Text.StringBuilder sw, Instance instPage)
		{
			Instance[] instStyles = OMS.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Style);
			sw.Append("body { ");
			foreach (Instance instStyle in instStyles)
			{
				RenderStyleSheet(sw, instStyle);
			}
			sw.Append(" }");

			Instance[] instComponents = OMS.GetRelatedInstances(instPage, KnownRelationshipGuids.Page__has__Page_Component);
			foreach (Instance instComponent in instComponents)
			{
				ApplyComponentStyle(sw, instComponent);
			}
		}
		public string GetPageStyle(Instance instPage)
		{
			Instance instMasterPage = OMS.GetRelatedInstance(instPage, KnownRelationshipGuids.Page__has_master__Page);

			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			if (instMasterPage != null)
			{
				WritePageStyles(sb, instMasterPage);
			}

			WritePageStyles(sb, instPage);
			return sb.ToString();
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
					ApplyInstIdAndCssClass(ctl, instPageComponents[i]);
					control.Controls.Add(ctl);
				}
			}
		}
		private void ApplyComponentStyle(System.Text.StringBuilder sw, Instance instComponent)
		{
			sw.Append(String.Format("[data-instance-id=\"{0}\"] {{", OMS.GetInstanceKey(instComponent)));
			Instance[] instComponentStyles = OMS.GetRelatedInstances(instComponent, KnownRelationshipGuids.Page_Component__has__Style);
			foreach (Instance instComponentStyle in instComponentStyles)
			{
				RenderStyleSheet(sw, instComponentStyle);
			}
			sw.Append(" } ");

			Instance instComponentClass = OMS.GetParentClass(instComponent);
			if (OMS.IsClassSubclassOf(instComponentClass, KnownInstanceGuids.Classes.ContainerPageComponent))
			{
				Instance[] instSubComponents = OMS.GetRelatedInstances(instComponent, KnownRelationshipGuids.Container_Page_Component__has__Page_Component);
				foreach (Instance instSubComponent in instSubComponents)
				{
					ApplyComponentStyle(sw, instSubComponent);
				}
			}
			if (instComponentClass.GlobalIdentifier == KnownInstanceGuids.Classes.PanelPageComponent)
			{
				Instance[] instHeaderComponents = OMS.GetRelatedInstances(instComponent, KnownRelationshipGuids.Panel_Page_Component__has_header__Page_Component);
				foreach (Instance inst2 in instHeaderComponents)
				{
					ApplyComponentStyle(sw, inst2);
				}
				Instance[] instContentComponents = OMS.GetRelatedInstances(instComponent, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
				foreach (Instance inst2 in instContentComponents)
				{
					ApplyComponentStyle(sw, inst2);
				}
				Instance[] instFooterComponents = OMS.GetRelatedInstances(instComponent, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
				foreach (Instance inst2 in instFooterComponents)
				{
					ApplyComponentStyle(sw, inst2);
				}
			}
		}

		private void RenderStyleSheet(System.Text.StringBuilder sw, Instance instStyle)
		{
			Instance[] instStylesImplemented = OMS.GetRelatedInstances(instStyle, KnownRelationshipGuids.Style__implements__Style);
			foreach (Instance inst in instStylesImplemented)
			{
				RenderStyleSheet(sw, inst);
			}

			Instance instBackgroundImage = OMS.GetRelatedInstance(instStyle, KnownRelationshipGuids.Style__gets_background_image_File_from__Return_Instance_Set_Method_Binding);
			if (instBackgroundImage != null)
			{
				OmsContext ctx = new OmsContext();
				object instFileRet = OMS.ExecuteMethod(instBackgroundImage, ctx);
				if (instFileRet is Instance[])
				{
					Instance instFile = ((Instance[])instFileRet)[0];
					sw.Append(String.Format("background-image: url('{0}');", sess.GetAttachmentUrl(instFile, sess.GetOmsAttachmentEntropy())));
				}
			}

			Instance[] instStyleRules = OMS.GetRelatedInstances(instStyle, KnownRelationshipGuids.Style__has__Style_Rule);
			foreach (Instance instStyleRule in instStyleRules)
			{
				Instance instStyleProperty = OMS.GetRelatedInstance(instStyleRule, KnownRelationshipGuids.Style_Rule__has__Style_Property);
				string cssName = OMS.GetAttributeValue<string>(instStyleProperty, KnownAttributeGuids.Text.CSSValue);
				string value = OMS.GetAttributeValue<string>(instStyleRule, KnownAttributeGuids.Text.Value);

				sw.Append(String.Format("{0}: {1};", cssName, value));
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
			InstanceKey instId = OMS.GetInstanceKey(inst);
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

					ApplyInstIdAndCssClass(ctl, instHeaderComponents[i]);
					panel.HeaderControls.Controls.Add(ctl);
				}
				Instance[] instContentComponents = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_content__Page_Component);
				for (int i = 0; i < instContentComponents.Length; i++)
				{
					Control ctl = CreatePageComponent(instContentComponents[i], context);
					if (ctl == null) continue;

					ApplyInstIdAndCssClass(ctl, instContentComponents[i]);
					panel.ContentControls.Controls.Add(ctl);
				}
				Instance[] instFooterComponents = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Panel_Page_Component__has_footer__Page_Component);
				for (int i = 0; i < instFooterComponents.Length; i++)
				{
					Control ctl = CreatePageComponent(instFooterComponents[i], context);
					if (ctl == null) continue;

					ApplyInstIdAndCssClass(ctl, instFooterComponents[i]);
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

				object bEditable = OMS.GetAttributeValue(inst, KnownAttributeGuids.Boolean.Editable, false);
				// item.Editable = (bool)bEditable;
				return item;
			}
			else if (parentClassInstance.GlobalIdentifier == KnownInstanceGuids.Classes.ElementPageComponent)
			{
				Instance element = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Element_Page_Component__has__Element);
				if (element == null)
					return null;

				Instance[] elementDisplayOptions = OMS.GetRelatedInstances(inst, KnownRelationshipGuids.Element_Page_Component__has__Element_Content_Display_Option);
				Instance ecDoSingular = OMS.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.Singular);

				bool singular = (OMS.InstanceSetContains(elementDisplayOptions, ecDoSingular));

				if (singular)
				{
					FormView fv = new FormView();
					fv.Attributes.Add("data-element-id", OMS.GetInstanceKey(element).ToString());

					Instance[] elementContents = OMS.GetRelatedInstances(element, KnownRelationshipGuids.Element__has__Element_Content);
					foreach (Instance elementContent in elementContents)
					{
						Instance elementContentInstance = OMS.GetRelatedInstance(elementContent, KnownRelationshipGuids.Element_Content__has__Instance);
						InstanceKey ecid = OMS.GetInstanceKey(elementContent);

						FormViewItem fvi = null;
						Instance elementContentInstanceParentClass = OMS.GetParentClass(elementContentInstance);
						if (elementContentInstanceParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
						{
							Instance[] instDisplayOptions = OMS.GetRelatedInstances(elementContent, KnownRelationshipGuids.Element_Content__has__Element_Content_Display_Option);
							Instance instDO_ObscuredText = OMS.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.ObscuredText);
							if (OMS.InstanceSetContains(instDisplayOptions, instDO_ObscuredText))
							{
								fvi = new FormViewItemPassword();
							}
							else
							{
								fvi = new FormViewItemText();
							}
						}
						else
						{
							fvi = new FormViewItemText();
							fvi.LabelVisible = false;
							fvi.Value = String.Format("(EC not implemented for class '{0}')", elementContentInstanceParentClass.GlobalIdentifier);
						}

						if (fvi != null)
						{
							fvi.Attributes.Add("data-ecid", OMS.GetInstanceKey(elementContent).ToString());
							fvi.Name = String.Format("ec__{0}", ecid.ToString());
							fvi.ClientIDMode = ClientIDMode.Static;
							fvi.Title = OMS.GetInstanceText(elementContentInstance);
							fv.Items.Add(fvi);
						}
					}
					return fv;
				}
				else
				{
					MBS.Web.Controls.ListView table = new ListView();
					table.Attributes.Add("data-element-id", OMS.GetInstanceKey(element).ToString());

					table.CssClass = GetClassAttribute(inst, "uwt-listview");
					table.Title = "Test PageBuilder";

					Instance[] elementContents = OMS.GetRelatedInstances(element, KnownRelationshipGuids.Element__has__Element_Content);

					foreach (Instance elementContent in elementContents)
					{
						Instance elementContentInstance = OMS.GetRelatedInstance(elementContent, KnownRelationshipGuids.Element_Content__has__Instance);
						InstanceKey ecid = OMS.GetInstanceKey(elementContentInstance);

						ListViewColumn lvc = new ListViewColumn();
						lvc.ID = OMS.GetInstanceKey(elementContent).ToString();
						lvc.Attributes.Add("data-ecid", OMS.GetInstanceKey(elementContentInstance).ToString());
						lvc.Title = OMS.GetInstanceText(elementContentInstance); // TODO: or .has override label Translation
						table.Columns.Add(lvc);
					}


					ListViewItem lvi = new ListViewItem();

					foreach (Instance elementContent in elementContents)
					{
						ListViewItemColumn lvic = new ListViewItemColumn();
						lvic.ColumnID = OMS.GetInstanceKey(elementContent).ToString();

						Instance elementContentInstance = OMS.GetRelatedInstance(elementContent, KnownRelationshipGuids.Element_Content__has__Instance);
						InstanceKey ecid = OMS.GetInstanceKey(elementContentInstance);

						Instance elementContentInstanceParentClass = OMS.GetParentClass(elementContentInstance);

						if (elementContentInstanceParentClass.GlobalIdentifier == KnownInstanceGuids.Classes.TextAttribute)
						{
							TextBox txt = new TextBox();

							Instance[] instDisplayOptions = OMS.GetRelatedInstances(elementContent, KnownRelationshipGuids.Element_Content__has__Element_Content_Display_Option);
							Instance instDO_ObscuredText = OMS.GetInstance(KnownInstanceGuids.ElementContentDisplayOptions.ObscuredText);
							if (OMS.InstanceSetContains(instDisplayOptions, instDO_ObscuredText))
							{
								txt.TextMode = System.Web.UI.WebControls.TextBoxMode.Password;
							}

							txt.ClientIDMode = ClientIDMode.Static;
							txt.ID = String.Format("ec__{0}", ecid.ToString());
							lvic.Control = txt;
						}
						else
						{
							System.Web.UI.WebControls.Label lbl = new System.Web.UI.WebControls.Label();
							lbl.Text = String.Format("(EC not implemented for class '{0}')", elementContentInstanceParentClass.GlobalIdentifier);
							lvic.Control = lbl;
						}

						lvi.Columns.Add(lvic);
					}

					table.Items.Add(lvi);
					return table;
				}
			}
			throw new NotSupportedException();
		}

		public void ApplyInstIdAndCssClass(Control ctl, Instance inst)
		{
			string instIdStr = OMS.GetInstanceKey(inst).ToString();
			if (ctl is System.Web.UI.WebControls.WebControl)
			{
				((System.Web.UI.WebControls.WebControl)ctl).Attributes.Add("data-instance-id", instIdStr);
				((System.Web.UI.WebControls.WebControl)ctl).AddCssClass(GetClassAttribute(inst));
			}
			else if (ctl is System.Web.UI.HtmlControls.HtmlControl)
			{
				((System.Web.UI.HtmlControls.HtmlControl)ctl).Attributes.Add("data-instance-id", instIdStr);
				((System.Web.UI.HtmlControls.HtmlControl)ctl).AddCssClass(GetClassAttribute(inst));
			}
		}

		private string GetClassAttribute(Instance inst, string requiredClasses = null)
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

			Instance instStyle = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Page_Component__has__Style);
			if (instStyle != null)
			{
				Instance[] instStyleClasses = OMS.GetRelatedInstances(instStyle, KnownRelationshipGuids.Style__has__Style_Class);
				for (int i = 0; i < instStyleClasses.Length; i++)
				{
					string value = OMS.GetAttributeValue<string>(instStyleClasses[i], KnownAttributeGuids.Text.Value);
					sbClassName.Append(value);
					if (i < instStyleClasses.Length - 1)
					{
						sbClassName.Append(' ');
					}
				}
			}
			if (sbClassName.Length > 0)
			{
				return sbClassName.ToString();
			}
			return null;
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

				ApplyInstIdAndCssClass(ctl, instComponents[i]);
				panel.Controls.Add(ctl);
			}

			return panel;
		}
		private System.Web.UI.HtmlControls.HtmlGenericControl CreateHeadingPageComponent(Instance inst, OmsContext context)
		{
			System.Web.UI.HtmlControls.HtmlGenericControl ctl = null;
			object jlevel = OMS.GetAttributeValue(inst, KnownAttributeGuids.Numeric.Level);
			ctl = new System.Web.UI.HtmlControls.HtmlGenericControl(String.Format("h{0}", jlevel));

			Instance instMethod = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Content_Page_Component__gets_content_from__Method);
			OmsContext ctx = new OmsContext();

			string value = OMS.ExecuteMethodReturningTextOrTranslation(instMethod, ctx);
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

			Instance instTargetFileMethod = OMS.GetRelatedInstance(inst, KnownRelationshipGuids.Image_Page_Component__has_source__Method);
			if (instTargetFileMethod == null)
				return null;

			OmsContext ctx = new OmsContext();
			Instance instTargetFile = ((Instance[]) OMS.ExecuteMethod(instTargetFileMethod, ctx)) [0];

			string attfmt = (OMS.GetAttributeValue(instTargetFile, KnownAttributeGuids.Text.ContentType) as string);
			string attvalue = (OMS.GetAttributeValue(instTargetFile, KnownAttributeGuids.Text.Value) as string);
			// image.ImageUrl = String.Format("data:{0};base64,{1}", attfmt, attvalue);
			image.ImageUrl = sess.GetAttachmentUrl(instTargetFile, sess.GetOmsAttachmentEntropy());

			return image;
		}
	}
}
