using System;
using System.Collections.Generic;
using System.Web.UI;
using Mocha.Core;

namespace Mocha.Web.Controls
{
	public class FormViewItemInstance : MBS.Web.Controls.FormViewItem
	{
		public List<InstanceKey> ValidClassIDs { get; } = new List<InstanceKey>();
		public bool Multiselect { get; set; } = false;

		public List<InstanceKey> SelectedInstances { get; } = new List<InstanceKey>();

		protected override Control RenderInternal()
		{
			InstanceBrowser ib = new InstanceBrowser();
			ib.Editable = !ReadOnly;
			for (int i = 0; i < ValidClassIDs.Count; i++)
			{
				ib.ValidClassIDs.Add(ValidClassIDs[i]);
			}
			for (int i = 0; i < SelectedInstances.Count; i++)
			{
				ib.InstanceReferences.Add(SelectedInstances[i]);
			}
			return ib;
		}
	}
}
