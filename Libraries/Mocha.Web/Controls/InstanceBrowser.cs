using System.Web.UI;
using MBS.Web;

namespace Mocha.Web.Controls
{
	public class InstanceBrowser : System.Web.UI.WebControls.Panel
	{
		public bool Editable { get; set; } = true;
		public InstanceClassIDPair InstanceReference { get; set; } = InstanceClassIDPair.Empty;

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			this.AddCssClass("mcx-instancebrowser");
			if (Editable) this.AddCssClass("mcx-editable");

			this.Attributes.Add("data-instance-id", InstanceReference.ToString());

			System.Web.UI.WebControls.TextBox txt = new System.Web.UI.WebControls.TextBox();
			this.Controls.Add(txt);

			MBS.Web.Controls.ActionPreviewButton apb = new MBS.Web.Controls.ActionPreviewButton();
			apb.Text = InstanceReference.ToString();
			this.Controls.Add(apb);

			base.RenderBeginTag(writer);
		}
	}
}