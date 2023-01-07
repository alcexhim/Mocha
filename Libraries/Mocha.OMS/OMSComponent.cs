using System;
using Mocha.Core;

namespace Mocha.OMS
{
	public abstract class OMSComponent
	{
		public class OMSComponentCollection
			: System.Collections.ObjectModel.Collection<OMSComponent>
		{
		}

		public InstanceKey InstanceID { get; set; } = InstanceKey.Empty;

		public string Name { get; set; } = String.Empty;
		public string Type { get; set; } = String.Empty;

		// public OMSComponentProperty.OMSComponentPropertyCollection Properties { get; } = new OMSComponentProperty.OMSComponentPropertyCollection();
	}
}