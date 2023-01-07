using System;
using System.Collections.Generic;

namespace Mocha.Core
{
    public class Relationship
    {
        public Relationship(Instance sourceInstance, Instance relationshipInstance)
        {
			SourceInstance = sourceInstance;
			RelationshipInstance = relationshipInstance;
        }

		public Instance SourceInstance { get; }
		public Instance RelationshipInstance { get; }
	}
}
