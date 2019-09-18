using System;
using System.Collections.Generic;

using Mocha.Storage.Local;

namespace Mocha.OMS
{
	public class LocalOMS : IOmsProvider
	{
		public Instance GetInstance(InstanceClassIDPair id)
		{
			return Environment.StorageProvider.Instances[id];
		}
		public Instance GetInstance(Guid globalIdentifier)
		{
			return Environment.StorageProvider.Instances[globalIdentifier];
		}
		public Instance[] GetInstances(Instance parentClassInstance = null)
		{
			List<Instance> list = new List<Instance>();
			foreach (Instance inst in Environment.StorageProvider.Instances)
			{
				Console.WriteLine("parent class for " + inst.ToString() + " is " + (inst.ParentClass == null ? "null" : inst.ParentClass.ToString()));
				Console.WriteLine("looking for " + (parentClassInstance != null ? parentClassInstance.ToString() : "null"));
				if (parentClassInstance == null || inst.ParentClass == parentClassInstance)
					list.Add(inst);
			}
			return list.ToArray();
		}
		public object GetAttributeValue(Instance instTarget, Instance instAttribute, object defaultValue = null)
		{
			return GetAttributeValue<object>(instTarget, instAttribute, defaultValue);
		}
		public T GetAttributeValue<T>(Instance instTarget, Instance instAttribute, T defaultValue = default(T))
		{
			if (Environment.StorageProvider.Attributes[instTarget, instAttribute] != null)
			{
				return (T)Environment.StorageProvider.Attributes[instTarget, instAttribute].Value;
			}
			return defaultValue;
		}
		
		public bool IsConnected { get; private set; } = false; // always true after initial load
		public bool Initializing { get; } = false;

		public LocalOMS()
		{
		}

		public Environment Environment { get; private set; } = null;
		
		public void Connect(System.Net.IPAddress addr, int port)
		{
			// do nothing; this is a local OMS
			Environment = new Environment(new LocalStorageProvider());
			Environment.Initialize();
			IsConnected = true;
		}
	}	
}

