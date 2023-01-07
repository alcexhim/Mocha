using System;
using Mocha.Core;

namespace Mocha.OMS
{
	public abstract class RemoteOms : Oms
	{
		// Instance GetInstance(InstanceKey instanceID);
		public bool IsConnected { get; }
		public bool Initializing { get; }

		protected abstract void ConnectInternal(System.Net.IPAddress addr, int port);
		public void Connect(System.Net.IPAddress addr, int port)
		{
			ConnectInternal(addr, port);
		}
	}
}

