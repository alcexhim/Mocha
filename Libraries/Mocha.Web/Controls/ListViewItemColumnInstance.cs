using System.Text;
using MBS.Web.Controls;

namespace Mocha.Web.Controls
{
	public class ListViewItemColumnInstance : ListViewItemColumn
	{
		public string Text { get; set; } = null;

		public override System.Web.UI.Control RenderControl()
		{
			// value in this case is instance id e.g. 142$108
			InstanceBrowser adw = new InstanceBrowser();
			if (Text != null)
				adw.Text = Text;
			adw.Editable = false;
			if (!string.IsNullOrEmpty(this.Value))
			{
				adw.InstanceReference = new InstanceClassIDPair(this.Value);
			}

			// <div data-instance-id="46$7" style="display: inline-block;" class="InstanceDisplayWidget">
			// 		<div class="AdditionalDetailWidget Text Ellipsis">
			//			<a class="AdditionalDetailText" href="//localhost:10080/mocha/prod_sys/instances/modify/46$7">Edit Standard Report</a>
			//			<a class="AdditionalDetailButton">&nbsp;</a>
			//			<div class="Content">
			//				<div class="MenuItems Empty">
			//					<div class="Header">Available Actions</div>
			//					<div class="Content">
			//						<ul class="Menu">
			//							<li class="Arrow"></li>
			//						</ul>
			//					</div>
			//				</div>
			//				<div class="PreviewContent">
			//					<div class="Header">
			//						<span class="ClassTitle">UI Task</span>
			//						<span class="ObjectTitle">
			//							<a href="#">Edit Standard Report</a>
			//						</span>
			//					</div>
			//					<div class="Content">
			//						<div class="PropertyGrid">
			//							<div class="Property">
			//								<div class="PropertyName">Global Identifier</div>
			//								<div class="PropertyValue">5E0E39F2FEA8473AB11CB9CE42C42B02</div>
			//							</div>
			//						</div>
			//					</div>
			//				</div>
			//			</div>
			//		</div>
			//		<ul class="Menu Popup" style="left: 626px; top: 580px;">
			//			<li class="MenuItem Visible Command">
			//				<a href="#">See in New Tab</a>
			//			</li>
			//			<li class="MenuItem Command">
			//				<a href="#">Copy URL</a></li><li class=\"MenuItem Command\"><a href=\"#\">Copy Text</a></li><li class=\"MenuItem Separator\"></li><li class=\"MenuItem Command\"><a href=\"#\">Copy Instance ID (46$7)</a></li><li class=\"MenuItem Command\"><a href=\"#\">Copy Text and Instance ID</a></li><li class=\"MenuItem Visible Separator\"></li><li class=\"MenuItem Visible Command\"><a href=\"#\">Modify Instance (46$7)</a></li><li class=\"MenuItem Visible Command\"><a href=\"#\">Modify Instance in New Window</a></li><li class=\"MenuItem Visible Separator\"></li><li class=\"MenuItem Visible Command\"><a href=\"#\">Search Instance ID (46$7)</a></li><li class=\"MenuItem Visible Command\"><a href=\"#\">Search Instance ID in New Window</a></li><li class=\"MenuItem Visible Separator\"></li><li class=\"MenuItem Visible Command\"><a href=\"#\">View Printable Version</a></li><li class=\"MenuItem Visible Command\"><a href=\"#\">Export to Spreadsheet</a></li>
			//		</ul>
			//	</div>
			return adw;
		}
	}
}