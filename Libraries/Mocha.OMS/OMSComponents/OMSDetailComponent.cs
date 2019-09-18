using System;
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
			public InstanceClassIDPair InstanceID { get; set; } = InstanceClassIDPair.Empty;
			public string Title { get; set; } = String.Empty;

			public OMSDetailColumn(InstanceClassIDPair instanceID, string title)
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
			public InstanceClassIDPair InstanceID { get; set; } = InstanceClassIDPair.Empty;
		}
		public class OMSDetailRowColumn
		{
			public class OMSDetailRowColumnCollection
				: System.Collections.ObjectModel.Collection<OMSDetailRowColumn>
			{
			}

			public InstanceClassIDPair ColumnInstanceID { get; set; } = InstanceClassIDPair.Empty;
			public string Value { get; set; } = String.Empty;

			public OMSDetailRowColumn(InstanceClassIDPair columnInstanceID, string value)
			{
				ColumnInstanceID = columnInstanceID;
				Value = value;
			}
		}

		public OMSDetailColumn.OMSDetailColumnCollection Columns { get; } = new OMSDetailColumn.OMSDetailColumnCollection();
		public OMSDetailRow.OMSDetailRowCollection Rows { get; } = new OMSDetailRow.OMSDetailRowCollection();
	}
}
