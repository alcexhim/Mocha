using System;
namespace Mocha.Core
{
    public abstract class Attribute
    {
        protected Attribute()
        {
        }

        public Instance TargetInstance { get; }
        public Instance AttributeInstance { get; }
        public object Value { get; }

        public DateTime ModificationDate { get; }
        public Instance ModificationUser { get; }
    }
}
