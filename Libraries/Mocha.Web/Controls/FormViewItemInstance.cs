using System;
using System.Web.UI;

namespace Mocha.Web.Controls
{
	public class FormViewItemInstance : MBS.Web.Controls.FormViewItem
	{
		public bool Multiselect { get; set; } = false;

		protected override Control RenderInternal()
		{
			System.Web.UI.WebControls.TextBox text = new System.Web.UI.WebControls.TextBox();
			return text;
		}
	}
}
