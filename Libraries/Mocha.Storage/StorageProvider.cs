using System;
namespace Mocha.Storage
{
	public abstract class StorageProvider
	{
		public StorageProvider()
		{
		}

		protected abstract void InitializeInternal();
		public void Initialize()
		{
			InitializeInternal();
		}

		protected abstract void WriteInstanceInternal(Instance instance);

		public void WriteInstance(Instance instance)
		{
			WriteInstanceInternal(instance);
		}

		/// <summary>
		/// Gets a collection of all <see cref="Instance" /> objects stored within this storage provider.
		/// </summary>
		/// <value>The instances.</value>
		public abstract Instance.InstanceCollection Instances { get; }
		/// <summary>
		/// Gets a collection of all <see cref="Attribute" /> objects stored within this storage provider.
		/// </summary>
		/// <value>The instances.</value>
		public abstract Attribute.AttributeCollection Attributes { get; }
		/// <summary>
		/// Gets a collection of all <see cref="Relationship" /> objects stored within this storage provider.
		/// </summary>
		/// <value>The instances.</value>
		public abstract Relationship.RelationshipCollection Relationships { get; }

	}
}
