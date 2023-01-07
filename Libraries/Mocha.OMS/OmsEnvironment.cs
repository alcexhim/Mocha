using System;
using Mocha.Storage;

namespace Mocha.OMS
{
	public class OmsEnvironment
	{
		public OmsEnvironment(StorageProvider storageProvider)
		{
			StorageProvider = storageProvider;
		}

		public bool Initializing { get; private set; }

		public StorageProvider StorageProvider { get; private set; }

		public void Initialize()
		{
			if (Initializing) throw new InvalidOperationException("Still loading don't bother me");

			Initializing = true;

			StorageProvider.Initialize();

			Initializing = false;
		}
	}
}
