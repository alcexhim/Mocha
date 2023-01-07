using System;
using System.Collections.Generic;
using System.Web.UI;

using MBS.Web;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web.Controls
{
	public class InstanceBrowser : System.Web.UI.WebControls.Panel
	{
		public bool DisplayAsCount { get; set; } = false;
		public bool Editable { get; set; } = true;
		public bool MultiSelect { get; set; } = false;

		public System.Collections.Generic.List<InstanceKey> InstanceReferences { get; } = new System.Collections.Generic.List<InstanceKey>();
		public List<InstanceKey> ValidClassIDs { get; } = new List<InstanceKey>();

		public string Text { get; set; } = null;

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			this.AddCssClass("mcx-instancebrowser");
			if (Editable) this.AddCssClass("mcx-editable");
			if (MultiSelect) this.AddCssClass("mcx-multiselect");
			if (InstanceReferences.Count == 0)
			{
				this.AddCssClass("mcx-empty");
			}
			if (DisplayAsCount)
			{
				this.AddCssClass("mcx-display-as-count");
			}

			this.Attributes.Add("data-instance-ids", String.Join(",", InstanceReferences.ToArray()));
			this.Attributes.Add("data-valid-class-ids", String.Join(",", ValidClassIDs.ToArray()));

			System.Web.UI.WebControls.TextBox txt = new System.Web.UI.WebControls.TextBox();
			txt.AutoCompleteType = System.Web.UI.WebControls.AutoCompleteType.Disabled;
			txt.ID = String.Format("{0}_TextBox", this.ID);
			if (Text != null)
				txt.Text = Text;
			this.Controls.Add(txt);

			System.Web.UI.WebControls.HiddenField hidden = new System.Web.UI.WebControls.HiddenField();
			hidden.ID = String.Format("{0}_Hidden", this.ID);
			this.Controls.Add(hidden);

			Oms oms = Page.GetOMS();

			System.Web.UI.HtmlControls.HtmlGenericControl ul = new System.Web.UI.HtmlControls.HtmlGenericControl("ul");

			if (DisplayAsCount)
			{
				System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");
				System.Web.UI.HtmlControls.HtmlAnchor a = new System.Web.UI.HtmlControls.HtmlAnchor();
				a.AddCssClass("mcx-count-link");
				a.InnerText = InstanceReferences.Count.ToString();
				li.Controls.Add(a);
				ul.Controls.Add(li);
			}
			else
			{
				for (int i = 0; i < InstanceReferences.Count; i++)
				{
					Instance inst = oms.GetInstance(InstanceReferences[i]);

					System.Web.UI.HtmlControls.HtmlGenericControl li = new System.Web.UI.HtmlControls.HtmlGenericControl("li");

					MBS.Web.Controls.ActionPreviewButton apb = new MBS.Web.Controls.ActionPreviewButton();

					apb.PreviewContent.Controls.Add(new System.Web.UI.HtmlControls.HtmlGenericControl("h1") { InnerHtml = oms.GetInstanceText(inst) });

					apb.Attributes["data-preview-url"] = String.Format("~/api/preview/{0}", InstanceReferences[i]);

					/*
					MBS.Web.Controls.FormView fvPreview = new MBS.Web.Controls.FormView();

					Instance classInstance = oms.GetParentClass(inst);
					Instance[] instSummaryFields = oms.GetRelatedInstances(classInstance, KnownRelationshipGuids.Class__has_summary__Report_Field);

					for (int j = 0;  j < instSummaryFields.Length; j++)
					{
						object ec = oms.GetReportFieldEC(inst, instSummaryFields[j]);
						if (ec == null)
						{
							MBS.Web.Controls.FormViewItemText fvi1 = new MBS.Web.Controls.FormViewItemText();
							fvi1.ReadOnly = true;
							fvi1.Title = oms.GetInstanceText(instSummaryFields[j]);
							fvi1.Value = "(empty)";
							fvi1.CssClass = "mcx-empty";
							fvPreview.Items.Add(fvi1);
						}
						else if (ec is Instance)
						{
							FormViewItemInstance fvi1 = new FormViewItemInstance();
							fvi1.ReadOnly = true;
							fvi1.Title = oms.GetInstanceText(instSummaryFields[j]);
							fvi1.SelectedInstances.Add(oms.GetInstanceKey((Instance)ec));
							fvPreview.Items.Add(fvi1);
						}
						else if (ec is string)
						{
							MBS.Web.Controls.FormViewItemText fvi1 = new MBS.Web.Controls.FormViewItemText();
							fvi1.ReadOnly = true;
							fvi1.Title = oms.GetInstanceText(instSummaryFields[j]);
							fvi1.Value = (string)ec;
							fvPreview.Items.Add(fvi1);
						}
					}
					apb.PreviewContent.Controls.Add(fvPreview);
					*/

					apb.Attributes["data-instance-id"] = InstanceReferences[i].ToString();

					string tenantName = Page.GetCurrentTenantName();
					apb.TargetUrl = String.Format("~/{0}/d/inst/{1}.htmld", tenantName, InstanceReferences[i]);

					if (Text != null)
					{
						apb.Text = Text;
					}
					else if (oms != null)
					{
						if (inst != null)
						{
							apb.Text = oms.GetInstanceText(inst);
							if (String.IsNullOrEmpty(apb.Text))
							{
								apb.Text = oms.GetTTC(inst);
							}
						}
						else
						{
							apb.Text = String.Format("(NULL INSTANCE: {0})", InstanceReferences[i]);
						}
					}
					else
					{
						apb.Text = InstanceReferences[i].ToString();
					}

					li.Controls.Add(apb);
					ul.Controls.Add(li);
				}
			}
			this.Controls.Add(ul);

			base.RenderBeginTag(writer);
		}
	}
}
 