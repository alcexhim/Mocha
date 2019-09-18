using System;
namespace Mocha.OMS.OMSComponents
{
	public class OMSSummaryComponent
		: OMSComponent
	{
		public class OMSSummaryFieldText
			: OMSSummaryField
		{
			public OMSSummaryFieldText(InstanceClassIDPair instanceID, string title, string value)
				: base(instanceID, title, value)
			{
			}
		}

		public abstract class OMSSummaryField
		{
			public InstanceClassIDPair InstanceID { get; set; }
			public string Title { get; set; } = String.Empty;
			public string Value { get; set; } = String.Empty;
			public bool ReadOnly { get; set; } = false;


			public OMSSummaryField(InstanceClassIDPair instanceID, string title, string value)
			{
				InstanceID = instanceID;
				Title = title;
				Value = value;
			}

			public class OMSSummaryFieldCollection
				: System.Collections.ObjectModel.Collection<OMSSummaryField>
			{
			}
		}

		public OMSSummaryField.OMSSummaryFieldCollection Fields { get; } = new OMSSummaryField.OMSSummaryFieldCollection();
	}
}
