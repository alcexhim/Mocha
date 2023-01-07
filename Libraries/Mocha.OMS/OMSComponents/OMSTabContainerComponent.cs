using System;
namespace Mocha.OMS.OMSComponents
{
	public class OMSTabContainerComponent
		: OMSComponent
	{
		public class TabPage
		{
			public class TabPageCollection
				: System.Collections.ObjectModel.Collection<TabPage>
			{
			}

			public string Title { get; set; } = String.Empty;
			public OMSComponentCollection Components { get; } = new OMSComponentCollection();
		}

		public TabPage.TabPageCollection TabPages { get; } = new TabPage.TabPageCollection();
	}
}
