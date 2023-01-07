using System.ComponentModel;
using Mocha.Core;

namespace Mocha.OMS
{
	public class AttributeOrRelationshipRequestedEventArgs : CancelEventArgs
	{
		public Instance TargetInstance { get; private set; } = null;
		public Instance AttributeOrRelationshipInstance { get; private set; } = null;
		public object Value { get; set; } = null;

		public AttributeOrRelationshipRequestedEventArgs(Instance instTarget, Instance instAttribute)
		{
			TargetInstance = instTarget;
			AttributeOrRelationshipInstance = instAttribute;
		}
	}
	public delegate void AttributeOrRelationshipRequestedEventHandler(object sender, AttributeOrRelationshipRequestedEventArgs e);
}