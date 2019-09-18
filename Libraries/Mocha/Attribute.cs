//
//  MochaAttribute.cs
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
using System.Collections.Generic;

namespace Mocha
{
	/// <summary>
	/// Represents a single setting of an attribute value for a given target instance.
	/// </summary>
	public class Attribute : IComparable<Attribute>
	{
		public class AttributeCollection
			: System.Collections.ObjectModel.Collection<Attribute>
		{
			private struct _AttributeKey
			{
				public InstanceClassIDPair instTarget;
				public InstanceClassIDPair instAttribute;
				
				public _AttributeKey(InstanceClassIDPair target, InstanceClassIDPair attribute)
				{
					this.instTarget = target;
					this.instAttribute = attribute;
				}

				public override string ToString()
				{
					return String.Format("({0}, {1})", instTarget, instAttribute);
				}
			}

			private Dictionary<_AttributeKey, Attribute> _keyed = new Dictionary<_AttributeKey, Attribute>();

			protected override void InsertItem(int index, Attribute item)
			{
				base.InsertItem(index, item);

				_AttributeKey key = new _AttributeKey(item.TargetInstance.GetInstanceIDPair(), item.AttributeInstance.GetInstanceIDPair());
				_keyed[key] = item;
			}

			public Attribute this[Instance instTarget, Instance instAttribute]
			{
				get
				{
					if (instTarget == null || instAttribute == null)
						return null;

					_AttributeKey key = new _AttributeKey(instTarget.GetInstanceIDPair(), instAttribute.GetInstanceIDPair());
					if (_keyed.ContainsKey(key))
					{
						return _keyed[key];
					}
					return null;
				}
				set
				{
					_AttributeKey key = new _AttributeKey(instTarget.GetInstanceIDPair(), instAttribute.GetInstanceIDPair());
					_keyed[key] = new Attribute(instTarget, instAttribute, value.Value, value.EffectiveDate, value.UserInstance);

					Console.WriteLine("Actually setting attribute with key '{0}' to value '{1}'", key, value);
				}
			}

		}

		public Attribute(Instance target, Instance attribute, object value, Instance user = null)
		{
			TargetInstance = target;
			AttributeInstance = attribute;
			Value = value;
			EffectiveDate = DateTime.Now;
			UserInstance = user;
		}
		public Attribute(Instance target, Instance attribute, object value, DateTime effectiveDate, Instance user = null)
		{
			TargetInstance = target;
			AttributeInstance = attribute;
			Value = value;
			EffectiveDate = effectiveDate;
			UserInstance = user;
		}

		/// <summary>
		/// The <see cref="Instance" /> of the attribute being set.
		/// </summary>
		/// <value>The attribute instance.</value>
		public Instance AttributeInstance { get; set; }
		/// <summary>
		/// The <see cref="Instance" /> on which the attribute is being set.
		/// </summary>
		/// <value>The target instance.</value>
		public Instance TargetInstance { get; set; }
		/// <summary>
		/// The value of the attribute.
		/// </summary>
		/// <value>The value.</value>
		public object Value { get; set; }

		/// <summary>
		/// The effective date of this attribute setting.
		/// </summary>
		/// <value>The effective date.</value>
		public DateTime EffectiveDate { get; set; }
		/// <summary>
		/// The <see cref="Instance" /> of the user who set this attribute.
		/// </summary>
		/// <value>The user instance.</value>
		public Instance UserInstance { get; set; }

		public int CompareTo(Attribute other)
		{
			return EffectiveDate.CompareTo(other.EffectiveDate);
		}
	}
}
