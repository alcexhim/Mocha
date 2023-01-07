using System;
namespace Mocha.Core
{
    public class Instance
    {
        public Instance(Guid globalIdentifier, int? index = null)
        {
            GlobalIdentifier = globalIdentifier;
			Index = index;
        }

		public Guid GlobalIdentifier { get; } = Guid.Empty;
		public int? Index { get; } = null;
	}
}
