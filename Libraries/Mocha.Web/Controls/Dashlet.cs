using System;
using System.Web.UI;

namespace Mocha.Web.Controls
{
	public class Dashlet : MBS.Web.Controls.Panel
	{
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			// add to the ContentControls the actual implementation of Dashlet


			base.RenderBeginTag(writer);
		}
	}
}
