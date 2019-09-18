using System;
using System.Collections.Generic;
using System.Text;
using UniversalEditor;
using UniversalEditor.Accessors;
using UniversalEditor.DataFormats.PropertyList.JavaScriptObjectNotation;
using UniversalEditor.ObjectModels.PropertyList;

namespace Mocha
{
	public class Instance
	{
		private int lastInstanceID = 0;

		public class InstanceCollection
			: System.Collections.ObjectModel.Collection<Instance>
		{
			private Dictionary<Guid, Instance> _instanceCache = new Dictionary<Guid, Instance>();
			private Dictionary<InstanceClassIDPair, Instance> _instanceCacheByID = new Dictionary<InstanceClassIDPair, Instance>();
			private Dictionary<string, Instance> _instancesByName = new Dictionary<string, Instance>();

			public Instance this[Guid id]
			{
				get
				{
					if (_instanceCache.ContainsKey(id))
						return _instanceCache[id];
					return null;
				}
			}
			public Instance this[InstanceClassIDPair id]
			{
				get
				{
					if (_instanceCacheByID.ContainsKey(id))
						return _instanceCacheByID[id];
					return null;
				}
			}

			private int lastInstanceID = 0;

			public Instance Add(Guid guidParentClass, Guid id, string name = null)
			{
				Instance instParentClass = this[guidParentClass];
				return Add(instParentClass, id, name);
			}
			public Instance Add(Instance instParentClass, Guid id, string name = null)
			{
				Instance inst = new Instance();
				if (instParentClass != null)
				{
					instParentClass.lastInstanceID++;
					inst.ID = instParentClass.lastInstanceID;
				}
				else
				{
					lastInstanceID++;
					inst.ID = lastInstanceID;
					inst.lastInstanceID++;
					instParentClass = inst;
				}
				inst.ParentClass = instParentClass;
				inst.GlobalIdentifier = id;
				inst.Name = name;
				Add(inst);
				return inst;
			}

			public Instance GetByGlobalIdentifier(Guid id)
			{
				return this[id];
			}
			public Instance GetByName(string name)
			{
				if (_instancesByName.ContainsKey(name))
					return _instancesByName[name];
				return null;
			}

			public Instance GetByID(InstanceClassIDPair id)
			{
				return this[id];
			}

			protected override void InsertItem(int index, Instance item)
			{
				base.InsertItem(index, item);

				if (item.GlobalIdentifier != Guid.Empty)
				{
					_instanceCache[item.GlobalIdentifier] = item;
				}
				_instanceCacheByID[new InstanceClassIDPair((item.ParentClass == null ? 1 : item.ParentClass.ID), item.ID)] = item;
				if (item.Name != null)
					_instancesByName[item.Name] = item;
			}

			public Instance[] Get(Instance instParentClass)
			{
				List<Instance> list = new List<Instance>();
				foreach (Instance inst in this)
				{
					if (inst.ParentClass == instParentClass) list.Add(inst);
				}
				return list.ToArray();
			}

			public Instance[] ToArray()
			{
				List<Instance> list = new List<Instance>();
				foreach (Instance item in this)
				{
					list.Add(item);
				}
				return list.ToArray();
			}
		}

		private int lastChildInstanceID = 0;

		private Instance()
		{
		}

		public int ID { get; private set; } = 0;
		public Instance ParentClass { get; private set; } = null;
		public Guid GlobalIdentifier { get; private set; } = Guid.Empty;
		public string Name { get; set; } = null;








		public override bool Equals(object obj)
		{
			if (!(obj is Instance)) return false;
			return String.Equals(GetInstanceIDString(), (obj as Instance).GetInstanceIDString());
		}







		// keep these around for now

		public InstanceClassIDPair GetInstanceIDPair()
		{
			int classID = 1;
			if (ParentClass != null) classID = ParentClass.ID;
			return new InstanceClassIDPair(classID, ID);
		}
		public string GetInstanceIDString()
		{
			return GetInstanceIDPair().ToString();
		}

		public string ToJSONString()
		{
			PropertyListObjectModel plom = new PropertyListObjectModel();
			JSONDataFormat json = new JSONDataFormat();

			plom.Properties.Add("InstanceID", this.GetInstanceIDString());
			plom.Properties.Add("GlobalIdentifier", this.GlobalIdentifier);
			plom.Properties.Add("Name", this.Name);

			StringAccessor sa = new StringAccessor();
			Document.Save(plom, json, sa);

			string value = sa.ToString();
			return value;
		}

		public static Instance[] FromJSONPropertyList(IPropertyListContainer plom)
		{
			if (plom.Groups.Count > 0)
			{
				// multiple instances ,in groups
				List<Instance> list = new List<Instance>();
				foreach (Group g in plom.Groups)
				{
					Instance inst = FromJSONPropertyList(g)[0];
					list.Add(inst);
				}
				return list.ToArray();
			}
			else
			{
				// single instance
				Property propInstID = plom.Properties["InstanceID"];
				Property propGlobalIdentifier = plom.Properties["GlobalIdentifier"];
				Property propName = plom.Properties["Name"];

				if (propInstID != null)
				{
					InstanceClassIDPair pair = new InstanceClassIDPair(propInstID.Value.ToString());

					Instance inst = new Instance();
					inst.ID = pair.InstanceID;

					inst.ParentClass = new Instance();
					inst.ParentClass.ID = pair.ClassID;

					if (propGlobalIdentifier != null)
					{
						inst.GlobalIdentifier = new Guid(propGlobalIdentifier.Value.ToString());
					}
					if (propName != null)
					{
						if (propName.Value != null)
						{
							inst.Name = propName.Value.ToString();
						}
					}

					return new Instance[] { inst };
				}
			}
			return null;
		}

		public override string ToString()
		{
			return GetInstanceIDString() + ' ' + Name;
		}
	}
}
