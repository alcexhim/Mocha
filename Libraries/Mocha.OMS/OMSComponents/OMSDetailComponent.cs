using System;
using Mocha.Core;

namespace Mocha.OMS.OMSComponents
{
	public class OMSDetailComponent
		: OMSComponent
	{
		public class OMSDetailColumn
		{
			public class OMSDetailColumnCollection
				: System.Collections.ObjectModel.Collection<OMSDetailColumn>
			{
			}

			/// <summary>
			/// Gets or sets the instance identifier for this <see cref="OMSDetailColumn" />.
			/// </summary>
			/// <value>The instance identifier.</value>
			public InstanceKey InstanceID { get; set; } = InstanceKey.Empty;
			public string Title { get; set; } = String.Empty;

			public OMSDetailColumn(InstanceKey instanceID, string title)
			{
				InstanceID = instanceID;
				Title = title;
			}
		}
		public class OMSDetailRow
		{
			public class OMSDetailRowCollection
				: System.Collections.ObjectModel.Collection<OMSDetailRow>
			{
			}
			public OMSDetailRowColumn.OMSDetailRowColumnCollection Columns { get; } = new OMSDetailRowColumn.OMSDetailRowColumnCollection();
			public InstanceKey InstanceID { get; set; } = InstanceKey.Empty;
		}
		public class OMSDetailRowColumn
		{
			public class OMSDetailRowColumnCollection
				: System.Collections.ObjectModel.Collection<OMSDetailRowColumn>
			{
			}

			public InstanceKey ColumnInstanceID { get; set; } = InstanceKey.Empty;
			public InstanceKey[] InstanceIDs { get; set; } = null;
			public string Value { get; set; } = String.Empty;
			public bool DisplayAsCount { get; set; } = false;

			public OMSDetailRowColumn(InstanceKey columnInstanceID, string value, InstanceKey instanceId)
			{
				ColumnInstanceID = columnInstanceID;
				InstanceIDs = new InstanceKey[] { instanceId };
				Value = value;
			}
			public OMSDetailRowColumn(InstanceKey columnInstanceID, string value, InstanceKey[] instanceIds = null)
			{
				ColumnInstanceID = columnInstanceID;
				InstanceIDs = instanceIds;
				Value = value;
			}
		}

		public OMSDetailColumn.OMSDetailColumnCollection Columns { get; } = new OMSDetailColumn.OMSDetailColumnCollection();
		public OMSDetailRow.OMSDetailRowCollection Rows { get; } = new OMSDetailRow.OMSDetailRowCollection();
	}
}
