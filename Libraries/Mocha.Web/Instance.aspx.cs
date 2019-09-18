using System;
using System.Web;
using System.Web.UI;
using MBS.Web.Controls;
using Mocha.OMS;
using Mocha.OMS.OMSComponents;

namespace Mocha.Web
{

    public partial class InstancePage : System.Web.UI.Page
    {
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			object oCid = RouteData.Values["ClassID"];
			object oIid = RouteData.Values["InstanceID"];
			if (oCid == null && oIid == null) return;

			int cid = -1, iid = -1;
			if (!(Int32.TryParse(oCid.ToString(), out cid) && Int32.TryParse(oIid.ToString(), out iid)))
			{
				label.Text = "parse failed";
				return;
			}

			OMSClient oms = (Session["OMS"] as OMSClient);
			if (oms == null) return;

			Instance inst = oms.GetInstance(new InstanceClassIDPair(cid, iid));
			if (inst == null)
			{
				label.Text = "not found";
			}
			else
			{
				label.Text = inst.ToString();
			}
			pnlTask.Visible = true;
			pnlDashboard.Visible = false;
			pnlDashboardContent.Visible = false;

			OMSResponse resp = oms.ExecuteInstance(inst);
			Page.Title = resp.Title;

			System.Web.UI.WebControls.Label lblTaskTitle = (Master.FindControl("lblTaskTitle") as System.Web.UI.WebControls.Label);
			Master.PageTitle = resp.Title;
			Master.PageSubtitle = resp.Description;

			foreach (OMSComponent comp in resp.Components)
			{
				Control ctl = RenderComponent(comp);

				if (ctl != null)
					pnlTask.Controls.Add(ctl);
			}
		}


		private Control RenderComponent(OMSComponent comp)
		{
			if (comp is OMSDetailComponent)
			{
				ListView lvReport = new ListView();

				OMSDetailComponent cmp = (comp as OMSDetailComponent);
				foreach (OMSDetailComponent.OMSDetailColumn col in cmp.Columns)
				{
					ListViewColumn lvc = new ListViewColumn();
					lvc.ID = "ReportColumn_" + col.InstanceID.ToString();
					lvc.Title = col.Title;
					lvReport.Columns.Add(lvc);
				}
				foreach (OMSDetailComponent.OMSDetailRow row in cmp.Rows)
				{
					ListViewItem lvi = new ListViewItem();
					foreach (OMSDetailComponent.OMSDetailRowColumn col in row.Columns)
					{
						ListViewItemColumn lvic = new ListViewItemColumn();
						// Controls.ListViewItemColumnInstance lvic = new Controls.ListViewItemColumnInstance()
						lvic.ColumnID = "ReportColumn_" + col.ColumnInstanceID.ToString();
						lvic.Value = col.Value;
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
				foreach (OMSSummaryComponent.OMSSummaryField field in cmp.Fields)
				{
					if (field is OMSSummaryComponent.OMSSummaryFieldText)
					{
						OMSSummaryComponent.OMSSummaryFieldText fld = (field as OMSSummaryComponent.OMSSummaryFieldText);
						FormViewItemText fvi = new FormViewItemText();
						fvi.Name = "ReportColumn_" + fld.Title.Replace(' ', '_');
						fvi.Title = fld.Title;
						fvi.ReadOnly = fld.ReadOnly;
						fvi.Value = fld.Value;
						fv.Items.Add(fvi);
					}
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
						Control ctl1 = RenderComponent(comp1);
						pg.Controls.Add(ctl1);
					}
					tbs.TabPages.Add(pg);
				}
				return tbs;
			}
			return null;
		}

		/*
		protected override void Render(HtmlTextWriter writer)
		{
			// if (otask == null) return;
			writer.Write("<!DOCTYPE html>");
			writer.Write("<html>");
			writer.Write("<head>");
			writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + Request.ApplicationPath + "StyleSheets/uwt.css\" />");
			writer.Write("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + Request.ApplicationPath + "StyleSheets/Slate/uwt.css\" />");
			writer.Write("</head>");
			writer.Write("<body>");
			writer.Write("<div class=\"uwt-header\">&nbsp;</div>");
			writer.Write("<div class=\"uwt-sidebar\">&nbsp;</div>");
			writer.Write("<div class=\"uwt-page\">");


			// page content goes here
			writer.Write("<h1>View Instance <a href=\"#\">" + oname + "</a></h1>");

			writer.Write("</div>");
			writer.Write("<div class=\"uwt-footer\">&nbsp;</div>");
			writer.Write("</body>");
			writer.Write("</html>");
			writer.Flush();
		}
		*/
	}
}
