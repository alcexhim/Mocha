using System;
using System.Web.UI;
using MBS.Web;
using Mocha.Core;

namespace Mocha.Web.Controls
{
	public class Dashlet : MBS.Web.Controls.Panel
	{
		public InstanceKey Instance { get; set; } = InstanceKey.Empty;

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			this.Attributes.Add("data-instance-id", Instance.ToString());
			this.AddCssClass("uwt-loading");

			base.RenderBeginTag(writer);
		}
	}
}
