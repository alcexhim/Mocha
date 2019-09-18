using System;
using System.Collections.Generic;

namespace Mocha
{
	public class Tenant
	{
		public string Name { get; private set; } = String.Empty;
		public Guid GlobalIdentifier { get; private set; } = Guid.Empty;

		private Tenant(string name, Guid globalIdentifier)
		{
			Name = name;
			GlobalIdentifier = globalIdentifier;
		}

		private static Dictionary<Guid, Tenant> _tenantsByGlobalIdentifier = new Dictionary<Guid, Tenant>();

		public static Tenant Create(string name, Guid globalIdentifier)
		{
			Tenant tenant = new Tenant(name, globalIdentifier);
			_tenantsByGlobalIdentifier[globalIdentifier] = tenant;
			return tenant;
		}

		public static Tenant GetByGlobalIdentifier(Guid guid)
		{
			if (_tenantsByGlobalIdentifier.ContainsKey(guid))
				return _tenantsByGlobalIdentifier[guid];
			return null;
		}

		

		public override string ToString()
		{
			return Name;
		}
	}
}
