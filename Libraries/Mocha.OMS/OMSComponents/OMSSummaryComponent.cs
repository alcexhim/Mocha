using System;
using System.Collections.Generic;
using Mocha.Core;

namespace Mocha.OMS.OMSComponents
{
	public class OMSSummaryComponent
		: OMSComponent
	{
		public class OMSSummaryFieldText
			: OMSSummaryField
		{
			public OMSSummaryFieldText(InstanceKey instanceID, string title, string value)
				: base(instanceID, title, value)
			{
			}
		}
		public class OMSSummaryFieldInstance
			: OMSSummaryField
		{
			public List<InstanceKey> ValidClassIDs { get; } = new List<InstanceKey>();

			public OMSSummaryFieldInstance(InstanceKey instanceID, string title, InstanceKey[] value)
				: base(instanceID, title, value)
			{
			}
		}
		public class OMSSummaryFieldBoolean
			: OMSSummaryField
		{
			public OMSSummaryFieldBoolean(InstanceKey instanceID, string title)
				: base(instanceID, title, null)
			{

			}
			public OMSSummaryFieldBoolean(InstanceKey instanceID, string title, bool? value)
				: base(instanceID, title, value)
			{

			}
		}
		public class OMSSummaryFieldDateTime
			: OMSSummaryField
		{
			public OMSSummaryFieldDateTime(InstanceKey instanceID, string title)
				: base(instanceID, title, null)
			{

			}
			public OMSSummaryFieldDateTime(InstanceKey instanceID, string title, DateTime? value)
				: base(instanceID, title, value)
			{

			}
		}

		public abstract class OMSSummaryField
		{
			public InstanceKey InstanceID { get; set; }
			public string Name { get; set; } = null;
			public string Title { get; set; } = String.Empty;
			public object Value { get; set; } = null;
			public bool ReadOnly { get; set; } = false;
			public bool Required { get; set; } = false;

			public OMSSummaryField(InstanceKey instanceID, string title, object value)
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
