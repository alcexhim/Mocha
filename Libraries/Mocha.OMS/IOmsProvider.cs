using System;

namespace Mocha.OMS
{
	public interface IOmsProvider
	{
		Instance GetInstance(InstanceClassIDPair instanceID);
		Instance GetInstance(Guid globalIdentifier);
		Instance[] GetInstances(Instance parentClassInstance = null);
		bool IsConnected { get; }
		bool Initializing { get; }
		void Connect(System.Net.IPAddress addr, int port);

		object GetAttributeValue(Instance instTarget, Instance instAttribute, object defaultValue = null);
		T GetAttributeValue<T>(Instance instTarget, Instance instAttribute, T defaultValue = default(T));
	}
}

