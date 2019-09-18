using System;
using UniversalEditor.ObjectModels.PropertyList;
namespace Mocha.Storage.Local
{
	public class LocalStorageProvider : StorageProvider
	{
		public string BasePath { get; set; }
		public LocalStorageProvider()
		{
			BasePath = FindBasePath();
		}
		public LocalStorageProvider(string basePath)
		{
			BasePath = basePath;
		}

		private string FindBasePath()
		{
			// first check our .config file for a basepath
			string basePathConfig = System.Configuration.ConfigurationManager.AppSettings["BasePath"];
			if (basePathConfig != null)
			{
				return basePathConfig;
			}

			System.Reflection.Assembly entryAsm = System.Reflection.Assembly.GetEntryAssembly();
			if (entryAsm == null)
			{
				entryAsm = System.Reflection.Assembly.GetCallingAssembly();
			}
			if (entryAsm == null)
			{
				entryAsm = System.Reflection.Assembly.GetExecutingAssembly();
			}
			if (entryAsm == null)
			{
				Console.WriteLine("Error: no base path specified");
			}
			return System.IO.Path.GetDirectoryName(entryAsm.Location);
		}

		private string dbg_CurrentFileName = null;

		private static string MakeRelativePath(string absolutePath, string basePath)
		{
			string retval = String.Empty;
			if (absolutePath.StartsWith(basePath))
			{
				retval = absolutePath.Substring(basePath.Length);
			}
			if (retval.StartsWith("/")) retval = retval.Substring(1);
			return retval;
		}

		private void LoadConfigurationFile(string fileName, System.Collections.Generic.List<object> listTenants, System.Collections.Generic.List<Group> listLateBoundRelationships)
		{
			Console.WriteLine("F: " + MakeRelativePath(fileName, BasePath));

			UniversalEditor.DataFormats.PropertyList.JavaScriptObjectNotation.JSONDataFormat json = new UniversalEditor.DataFormats.PropertyList.JavaScriptObjectNotation.JSONDataFormat();
			UniversalEditor.ObjectModels.PropertyList.PropertyListObjectModel plom = new UniversalEditor.ObjectModels.PropertyList.PropertyListObjectModel();
			UniversalEditor.Accessors.FileAccessor fa = new UniversalEditor.Accessors.FileAccessor(fileName);
			UniversalEditor.Document.Load(plom, json, fa, true);

			object[] objTenants = (plom.Groups[0].Properties["Tenants"].Value as object[]);
			if (objTenants != null)
			{
				dbg_CurrentFileName = fileName;
				foreach (object objTenant in objTenants)
				{
					listTenants.Add(objTenant);
				}
			}
		}

		private void LoadTenantsJSON(object[] objTenants, System.Collections.Generic.List<Group> listLateBoundRelationships)
		{
			foreach (object objTenant in objTenants)
			{
				if (objTenant is Group)
				{
					Group gTenant = (objTenant as Group);

					string globalIdentifier = gTenant.Properties["ID"].Value.ToString();

					Tenant tenant = null;
					if (gTenant.Properties.Contains("Name"))
					{
						string name = gTenant.Properties["Name"].Value.ToString();
						tenant = Tenant.Create(name, new Guid(globalIdentifier));
					}
					else
					{
						tenant = Tenant.GetByGlobalIdentifier(new Guid(globalIdentifier));
					}

					if (gTenant.Properties.Contains("Objects"))
					{
						object[] objObjects = (gTenant.Properties["Objects"].Value as object[]);
						foreach (object objObject in objObjects)
						{
							if (objObject is Group)
							{
								Group grpObject = (objObject as Group);

								Instance instObj = LoadInstanceJSON(grpObject, listLateBoundRelationships);
							}
						}
					}
				}
			}
		}

		private Instance LoadInstanceJSON(Group grpObject, System.Collections.Generic.List<Group> listLateBoundRelationships, Instance instParentClass = null)
		{
			Instance instObj = null;
			bool instCreated = false;

			if (grpObject.Properties.Contains("ID"))
			{
				Guid objId = Guid.Empty;
				if (grpObject.Properties.Contains("ID"))
				{
					objId = new Guid(grpObject.Properties["ID"].Value.ToString());
				}
				string objName = null;
				if (grpObject.Properties.Contains("Name"))
				{
					objName = grpObject.Properties["Name"].Value.ToString();
				}

				if (objId != Guid.Empty)
				{
					instObj = Instances.GetByGlobalIdentifier(objId);
				}
				else if (objName != null)
				{
					instObj = Instances.GetByName(objName);
				}

				if (instParentClass == null && Instances.Count > 0)
				{
					instParentClass = Instances[0];
				}

				if (grpObject.Properties.Contains("Relationships"))
				{
					object[] objRels = (object[])grpObject.Properties["Relationships"].Value;
					foreach (object objRel in objRels)
					{
						Group grpRel = objRel as Group;
						if (grpRel == null) continue;

						grpRel.Name = "Relationship";
						grpRel.Properties.Add("lateBoundRelationshipSourceInstance", objId);
						listLateBoundRelationships.Add(grpRel);
					}
				}
				if (grpObject.Properties.Contains("TranslatableValues"))
				{
					// TranslatableValues on Instance
					object[] objRels = (object[])grpObject.Properties["TranslatableValues"].Value;
					foreach (object objRel in objRels)
					{
						Group grpRel = objRel as Group;
						if (grpRel == null) continue;

						grpRel.Name = "TranslatableValue";
						grpRel.Properties.Add("lateBoundRelationshipSourceInstance", objId);
						listLateBoundRelationships.Add(grpRel);
					}
				}

				if (instObj == null)
				{
					instObj = Instances.Add(instParentClass, objId, objName);
					instCreated = true;
				}
				if (objName != null)
				{
					instObj.Name = objName;
				}
			}
			else if (grpObject.Properties.Contains("Name"))
			{
				instObj = Instances.GetByName(grpObject.Properties["Name"].Value.ToString());
			}

			if (grpObject.Properties.Contains("Instances"))
			{
				object[] values = (object[])grpObject.Properties["Instances"].Value;
				foreach (object o in values)
				{
					if (o is Group)
					{
						Instance inst2 = LoadInstanceJSON(o as Group, listLateBoundRelationships, instObj);
					}
				}
			}

			if (instObj != null && instCreated)
			{
				Console.WriteLine("    I: " + instObj.ToString());
			}
			return instObj;
		}

		private void LoadAttributesJSON(object[] objTenants)
		{
			foreach (object objTenant in objTenants)
			{
				if (objTenant is Group)
				{
					Group gTenant = (objTenant as Group);
					if (gTenant.Properties["ID"] == null)
					{
						Console.Error.WriteLine("Tenant ID is null");
						continue;
					}

					string globalIdentifier = gTenant.Properties["ID"].Value.ToString();

					Tenant tenant = null;
					if (gTenant.Properties.Contains("Name"))
					{
						string name = gTenant.Properties["Name"].Value.ToString();
						tenant = Tenant.Create(name, new Guid(globalIdentifier));
					}
					else
					{
						tenant = Tenant.GetByGlobalIdentifier(new Guid(globalIdentifier));
					}

					if (gTenant.Properties.Contains("Objects"))
					{
						object[] objObjects = (gTenant.Properties["Objects"].Value as object[]);
						foreach (object objObject in objObjects)
						{
							if (objObject is Group)
							{
								Group grpObject = (objObject as Group);
								if (grpObject.Properties.Contains("Instances"))
								{
									object[] oInstances = (grpObject.Properties["Instances"].Value as object[]);
									foreach (object oInstance in oInstances)
									{
										Group grpInst = (oInstance as Group);

										Guid guid = new Guid(grpInst.Properties["ID"].Value.ToString());
										Instance instObj = Instances[guid];
										LoadAttributesForInstanceJSON(grpInst, instObj);
									}
								}
							}
						}
					}
				}
			}
		}

		private void LoadAttributesForInstanceJSON(Group grpObject, Instance instObj)
		{
			if (grpObject.Properties.Contains("AttributeValues"))
			{
				object[] values = (object[])grpObject.Properties["AttributeValues"].Value;
				foreach (object o in values)
				{
					if (o is Group)
					{
						SetAttributeValueJSON(o as Group, instObj);
					}
				}
			}
		}

		private void SetAttributeValueJSON(Group g, Instance inst)
		{
			Property propID = g.Properties["ID"];
			Property propValue = g.Properties["Value"];

			string idstr = (propID != null) ? propID.Value.ToString() : String.Empty;
			Guid id = Guid.Empty;
			try
			{
				id = new Guid(idstr);
			}
			catch (FormatException ex)
			{
				Console.Error.WriteLine("Unable to parse GUID " + idstr);
				return;
			}

			Instance instAttribute = Instances[id];
			Instance instUser = null;
			
			object value = (propValue != null) ? propValue.Value : null;
			
			if (propID != null)
			{
				Console.WriteLine("    A: {0}     {1}        {2}", inst, id.ToString("B"), value == null ? "null" : value.ToString());
				Attributes[inst, instAttribute] = new Attribute(inst, instAttribute, value, instUser);
			}
		}

		protected override void InitializeInternal()
		{
			string[] fileNames = System.IO.Directory.GetFiles(BasePath, "*.xqjs", System.IO.SearchOption.AllDirectories);

			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>(fileNames);
			// SORTING IS REQUIRED !!! DO NOT REMOVE
			list.Sort();

			System.Collections.Generic.List<object> listTenants = new System.Collections.Generic.List<object>();
			System.Collections.Generic.List<Group> listLateBoundRelationships = new System.Collections.Generic.List<Group>();
			foreach (string fileName in list)
			{
				LoadConfigurationFile(fileName, listTenants, listLateBoundRelationships);
			}

			object[] objTenants = listTenants.ToArray();
			LoadTenantsJSON(objTenants, listLateBoundRelationships);
			LoadAttributesJSON(objTenants);

			// load known instances into memory
			Instance instTranslatableTextConstant = Instances[KnownInstanceGUIDs.TranslatableTextConstant];
			Instance instRelationship___Translatable_Text_Constant__has__Translatable_Text_Constant_Value = Instances[KnownInstanceGUIDs.Relationship.Translatable_Text_Constant__has__Translatable_Text_Constant_Value];

			foreach (Group g in listLateBoundRelationships)
			{
				Property pInstance = g.Properties["lateBoundRelationshipSourceInstance"];
				if (pInstance == null) continue;

				Property pComment = g.Properties["Comment"];

				Instance instSource = Instances[(Guid)pInstance.Value];

				if (g.Name == "Relationship")
				{

					Instance instRelationship = null, instInverseRelationship = null;

					Property pRelationshipId = g.Properties["RelationshipID"];
					if (pRelationshipId != null)
					{
						Console.WriteLine("Applying relationship {0} '{1}'", pRelationshipId.Value.ToString(), pComment != null ? pComment.Value : String.Empty);

						Guid gidRelationshipId = new Guid(pRelationshipId.Value.ToString());
						instRelationship = Instances[gidRelationshipId];
					}

					Relationship rel = new Relationship();
					rel.SourceInstance = instSource;
					rel.RelationshipInstance = instRelationship;

					Property pInverseRelationshipId = g.Properties["InverseRelationshipID"];
					if (pInverseRelationshipId != null)
					{
						Guid gidRelationshipId = new Guid(pRelationshipId.Value.ToString());
						instInverseRelationship = Instances[gidRelationshipId];
					}

					Property pDestinationInstances = g.Properties["DestinationInstances"];
					if (pDestinationInstances != null)
					{
						object[] refs = (object[])pDestinationInstances.Value;
						foreach (object _ref in refs)
						{
							if (_ref is string)
							{
								string sref = (_ref as string);

								// shim for lazy configurators
								if (sref.StartsWith("{") && !sref.EndsWith("}"))
									sref = sref + "}";

								Guid idref = new Guid(sref);

								Instance instRef = Instances[idref];
								if (instRef != null)
								{
									rel.DestinationInstances.Add(instRef);
								}
								else
								{
									Console.Error.WriteLine("referenced instance '{0}' does not exist!", idref);
								}
							}
						}
					}

					Relationships.Add(rel);
				}
				else if (g.Name == "TranslatableValue")
				{
					Instance instRelationship = null;

					Property pRelationshipId = g.Properties["RelationshipID"];
					if (pRelationshipId != null)
					{
						Console.WriteLine("Applying TranslatableValue {0} '{1}'", pRelationshipId.Value.ToString(), pComment != null ? pComment.Value : String.Empty);

						Guid gidRelationshipId = new Guid(pRelationshipId.Value.ToString());
						instRelationship = Instances[gidRelationshipId];

						if (g.Properties["Values"] != null)
						{
							object[] objValues = (g.Properties["Values"].Value as object[]);
							if (objValues != null)
							{
								Instance instTTC = Instances.Add(KnownInstanceGUIDs.TranslatableTextConstant, Guid.Empty);

								Relationship rel = new Relationship();
								rel.SourceInstance = instTTC;
								rel.RelationshipInstance = instRelationship___Translatable_Text_Constant__has__Translatable_Text_Constant_Value;

								foreach (object objValue in objValues)
								{
									Group gValue = (objValue as Group);

									Property propLanguageInstanceID = gValue.Properties["LanguageInstanceID"];
									Property propValue = gValue.Properties["Value"];

									if (propLanguageInstanceID != null && propValue != null)
									{
										Console.WriteLine("    Adding language {0} = {1}", propLanguageInstanceID, propValue);

										Guid gidLanguageInstanceID = new Guid(propLanguageInstanceID.Value.ToString());
										string value = propValue.Value.ToString();

										// create an instance of TranslatableTextConstant with the specified values
										Instance instTTCValue = Instances.Add(KnownInstanceGUIDs.TranslatableTextConstantValue, Guid.Empty);

										Instance instAttributeValue = Instances[KnownAttributeGUIDs.Value];
										Attributes.Add(new Attribute(instTTCValue, instAttributeValue, value));

										Console.WriteLine("Set attribute {0} on {1} to {2}", instAttributeValue.GetInstanceIDString(), instTTCValue.GetInstanceIDString(), value);

										// set the attributes
										rel.DestinationInstances.Add(instTTCValue);
									}
								}
								Relationships.Add(rel);

								// add the ... has Translatable Text Constant relationship
								Relationship rela = new Relationship();
								rela.SourceInstance = instSource;
								rela.RelationshipInstance = instRelationship;
								rela.DestinationInstances.Add(instTTC);
								Relationships.Add(rela);
							}
						}
					}

				}
			}
		}
		protected override void WriteInstanceInternal(Instance instance)
		{
		}

		public override Instance.InstanceCollection Instances { get; } = new Instance.InstanceCollection();
		public override Attribute.AttributeCollection Attributes { get; } = new Attribute.AttributeCollection();
		public override Relationship.RelationshipCollection Relationships { get; } = new Relationship.RelationshipCollection();
	}
}
