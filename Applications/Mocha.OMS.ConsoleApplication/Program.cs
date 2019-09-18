//
//  Program.cs
//
//  Author:
//       Michael Becker <alcexhim@gmail.com>
//
//  Copyright (c) 2019 
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Mocha.Storage.Local;

namespace Mocha.OMS.ConsoleApplication
{
	class Program
	{
		private static Environment env = null;
		private static OMSServer svr = null;

		public static void Main(string[] args)
		{
			env = new Environment(new LocalStorageProvider());
			env.Initialize();
			// The OMS server responds to InstanceRequested events.


			svr = new OMSServer();
			svr.AttributeValueRequested += svr_AttributeValueRequested;
			svr.InstanceRequested += svr_InstanceRequested;
			svr.RelatedInstancesRequested += svr_RelatedInstancesRequested;
			Console.WriteLine("MochaOMS: starting server on port " + svr.Protocol.Port.ToString());
			svr.Start();
		}

		private static void svr_RelatedInstancesRequested(object sender, AttributeOrRelationshipRequestedEventArgs e)
		{
			if (e.AttributeOrRelationshipInstance == null)
			{
				Relationship[] rels = env.StorageProvider.Relationships[e.TargetInstance];
				e.Value = rels;
			}
			else
			{
				Relationship rel = env.StorageProvider.Relationships[e.TargetInstance, e.AttributeOrRelationshipInstance];
				if (rel != null)
				{
					System.Collections.Generic.List<Instance> list = new System.Collections.Generic.List<Instance>();
					foreach (Instance inst in rel.DestinationInstances)
					{
						list.Add(inst);
					}
					e.Value = list.ToArray();
				}
			}
		}


		private static void svr_AttributeValueRequested(object sender, AttributeOrRelationshipRequestedEventArgs e)
		{
			Attribute att = env.StorageProvider.Attributes[e.TargetInstance, e.AttributeOrRelationshipInstance];
			if (att != null)
			{
				e.Value = att.Value;
			}
		}


		private static void svr_InstanceRequested(object sender, InstanceRequestedEventArgs e)
		{
			switch (e.QueryType)
			{
				case InstanceRequestedQueryType.All:
				{
					if (String.IsNullOrEmpty(e.Query))
					{
						// return all instances (this might be slow)
						e.Instances = env.StorageProvider.Instances.ToArray();
					}
					else
					{
						Instance instParentClass = null;
						switch (e.IDType)
						{
							case InstanceRequestedIDType.GlobalIdentifier:
							{
								Guid guid =  new Guid(e.Query);
								env.StorageProvider.Instances.GetByGlobalIdentifier(guid);
								break;
							}
							case InstanceRequestedIDType.InstanceId:
							{
								string[] parts = e.Query.Split('$');
								int classID = Int32.Parse(parts[0]);
								int instanceID = Int32.Parse(parts[1]);
								instParentClass = env.StorageProvider.Instances.GetByID(new InstanceClassIDPair(classID, instanceID));
								break;
							}
						}
						e.Instances = env.StorageProvider.Instances.Get(instParentClass);
					}
					break;
				}
				case InstanceRequestedQueryType.SpecificInstance:
				{
					switch (e.IDType)
					{
						case InstanceRequestedIDType.GlobalIdentifier:
						{
							Guid guid =  new Guid(e.Query);
							e.Instances = new Instance[] { env.StorageProvider.Instances.GetByGlobalIdentifier(guid) };
							break;
						}
						case InstanceRequestedIDType.InstanceId:
						{
							string[] parts = e.Query.Split('$');
							int classID = Int32.Parse(parts[0]);
							int instanceID = Int32.Parse(parts[1]);
							e.Instances = new Instance[] { env.StorageProvider.Instances.GetByID(new InstanceClassIDPair(classID, instanceID)) };
							break;
						}
					}
					break;
				}
			}
		}
	}
}
