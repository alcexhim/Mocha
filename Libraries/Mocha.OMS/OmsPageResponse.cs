using System;
using Mocha.Core;

namespace Mocha.OMS
{
	public class OmsPageResponse : IOmsResponse
	{
		/// <summary>
		/// Returns the collection of <see cref="OMSComponent" />s that are defined on this page.
		/// </summary>
		/// <value>The <see cref="OMSComponent" />s that are defined on this page.</value>
		public OMSComponent.OMSComponentCollection Components { get; } = new OMSComponent.OMSComponentCollection();

		public string Title { get; set; } = String.Empty;
		public string Description { get; set; } = String.Empty;
		public InstanceKey DescriptionInstance { get; set; } = InstanceKey.Empty;
	}
}