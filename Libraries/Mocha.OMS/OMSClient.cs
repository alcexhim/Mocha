//
//  Client.cs
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

using MBS.Framework.Collections.Generic;
using MBS.Networking;
using MBS.Networking.Protocols.HyperTextTransfer;

using UniversalEditor;
using UniversalEditor.Accessors;
using UniversalEditor.IO;

using UniversalEditor.ObjectModels.PropertyList;
using UniversalEditor.DataFormats.PropertyList.JavaScriptObjectNotation;
using System.Collections.Generic;
using MBS.Networking.Transports.TCP;
using System.Text;
using Mocha.OMS.OMSComponents;
using Mocha.Core;

namespace Mocha.OMS
{
	public class OMSClient : MBS.Networking.Client
	{
		public OMSClient()
		{
			this.Protocol = new OMS.OMSProtocol();
			this.Service = new OMS.OMSService();
			this.Transport = new TCPTransport();
		}

		public Instance GetInstance(InstanceKey instanceID)
		{
			Request req = new Request();
			req.Path = "/instances/by-iid/" + instanceID.ToString();
			req.Method = "GET";
			req.Protocol = "OMS/1.0";
			(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

			bool retval = this.WaitForData();
			if (!retval) return null;	// timed out, so we can't be sure this is valid

			Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);

			if (resp == null) return null;

			if (resp.ResponseCode == 200)
			{
				if (resp.Content == null) return null;

				// OK, we have a JSON object representing an Instance
				PropertyListObjectModel plom = new PropertyListObjectModel();
				JSONDataFormat json = new JSONDataFormat();
				StringAccessor sa = new StringAccessor(resp.Content);

				Document.Load(plom, json, sa);

				Instance[] insts = InstanceFromJSONPropertyList(plom);
				if (insts.Length > 0)
				{
					return insts[0];
				}
				return null;
			}

			return null;

		}

		private Instance[] InstanceFromJSONPropertyList(PropertyListObjectModel plom)
		{
			return new Instance[0];
		}
		private Instance[] InstanceFromJSONPropertyList(Group group)
		{
			return new Instance[0];
		}

		private Dictionary<Guid, Instance> InstancesByGlobalIdentifier = new Dictionary<Guid, Instance>();

		/// <summary>
		/// Retrieves the <see cref="Instance" /> with the specified Global Identifier from the OMS.
		/// </summary>
		/// <returns>The instance, or null if no instance with the specified Global Identifier exists in the OMS.</returns>
		/// <param name="globalIdentifier">The GUID of the instance to look up.</param>
		public Instance GetInstance(Guid globalIdentifier)
		{
			if (!InstancesByGlobalIdentifier.ContainsKey(globalIdentifier))
			{
				Request req = new Request();
				req.Path = "/instances/by-uuid/" + globalIdentifier.ToString("B");
				req.Method = "GET";
				req.Protocol = "OMS/1.0";
				(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

				this.WaitForData();

				Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
				if (resp == null) return null;

				if (resp.ResponseCode == 200)
				{
					// OK, we have a JSON object representing an Instance
					PropertyListObjectModel plom = new PropertyListObjectModel();
					JSONDataFormat json = new JSONDataFormat();
					StringAccessor sa = new StringAccessor(resp.Content);

					Document.Load(plom, json, sa);

					Instance inst = InstanceFromJSONPropertyList(plom)[0];
					UpdateParentInst(inst);

					InstancesByGlobalIdentifier[globalIdentifier] = inst;
				}
			}
			if (InstancesByGlobalIdentifier.ContainsKey(globalIdentifier))
				return InstancesByGlobalIdentifier[globalIdentifier];
			return null;

		}

		private void UpdateParentInst(Instance inst)
		{
			Instance parentInst = GetInstance(inst.GlobalIdentifier);
			// eww
			inst.GetType().GetProperty("ParentClass", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).SetValue(inst, parentInst);
		}

		public string BamString(Instance instString, Instance instInstance)
		{
			StringBuilder sb = new StringBuilder();

			Instance[] stringComponents = GetRelatedInstances(instString, KnownRelationshipGuids.String__has__String_Component);

			foreach (Instance instStringComponent in stringComponents)
			{
				// assuming this is an Extract Single Instance String Component for now...
				Instance instRel = GetRelatedInstance(instStringComponent, KnownRelationshipGuids.Extract_Single_Instance_String_Component__has__Relationship);
				if (instRel != null)
				{
					// Extract Single Instance String Component.has Relationship
					// this is the relationship used to extract the string
					Instance instTTC = GetRelatedInstance(instInstance, instRel);
					string value = GetTTC(instInstance, instTTC);
					sb.Append(value);
				}
			}


			return sb.ToString();
		}

		public Instance[] GetSuperClasses(Instance inst)
		{
			return GetRelatedInstances(inst, KnownRelationshipGuids.Class__has_super__Class);
		}

		public string GetInstanceTitle(Instance inst, string defaultValue = "")
		{
			Instance instClassLabeledBy = GetRelatedInstance(this.GetParentClass(inst), KnownRelationshipGuids.Class__instance_labeled_by__String);
			if (instClassLabeledBy == null) return String.Empty;

			// TODO: We need to move away from String Components, since they are just executing Methods now, and
			// return the entire Instance Title as a Build Attribute Method.

			// get Value for Translation Value parm(GA)[ramb]:
			//		get Translation Value for Translation in Default Display Language (GA)[ramb]:
			//			get Translation Value for Translation in Language(GA):
			//			where Language = Language@get Default Display Language for Current User
			//				Language@get Default Display Language for Current User(SAC)*P[rsmb]:
			//				SAC - Conditional Select Attribute Method
			//				condition User@get Language is not empty	User@get Language(SS)*P[rsmb]
			//															Tenant@get Default Language(SS)*P[rsmb]


			// The method call would look like this:
			// RAMB - get Value [TX] for Translation Value for specified translation and language on This Instance
			//		calls GRA - Get Referenced Attribute method, Attribute = Value[TX], loop on instance set = specified translation, relationship = Translation.has Translation Value
			// where Language = RSMB - get User Selected Language and Translation = specified

			Instance[] instString__has__String_Component = GetRelatedInstances(instClassLabeledBy, KnownRelationshipGuids.String__has__String_Component);

			StringBuilder sb = new StringBuilder();
			foreach (Instance instStringComponent in instString__has__String_Component)
			{
				// ESI
				Instance instRel = GetRelatedInstance(instStringComponent, KnownRelationshipGuids.Extract_Single_Instance_String_Component__has__Relationship);

				Instance instAAA = GetRelatedInstance(inst, instRel);
				string val = GetTTC(inst, instAAA);
				sb.Append(val);
			}

			Instance instTTC = GetRelatedInstance(inst, KnownRelationshipGuids.Class__has_title__Translatable_Text_Constant);
			string value = GetTTC(inst, instTTC);

			return value;
		}

		public Instance[] GetInstances(Instance parentClassInstance = null, bool searchSubclasses = false)
		{
			List<Instance> list = new List<Instance>();
			if (parentClassInstance == null)
			{
				Request req = new Request
				{
					Path = "/instances",
					Method = "GET",
					Protocol = "OMS/1.0"
				};
				(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

				this.WaitForData();

				Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
				if (resp.ResponseCode == 200)
				{
					PropertyListObjectModel plom = new PropertyListObjectModel();
					JSONDataFormat json = new JSONDataFormat();
					StringAccessor sa = new StringAccessor(resp.Content);

					Document.Load(plom, json, sa);
					foreach (Group g in plom.Items.OfType<Group>())
					{
						Instance inst = InstanceFromJSONPropertyList(g) [0];
						list.Add(inst);
					}
				}
			}
			else
			{
				Request req = new Request
				{
					Path = "/instances/by-gid/" + parentClassInstance.GlobalIdentifier + "/instances",
					Method = "GET",
					Protocol = "OMS/1.0"
				};
				(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

				this.WaitForData();

				Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
				if (resp.ResponseCode == 200)
				{
					HandleArrayResponse(resp, list);
				}
			}
			return list.ToArray();
		}

		protected override void OnDataReceived(DataReceivedEventArgs e)
		{
			base.OnDataReceived(e);
		}

		public bool Initializing
		{
			get
			{
				Request req = new Request();
				req.Path = "/instances/by-iid/1$1";
				req.Method = "GET";
				req.Protocol = "OMS/1.0";
				(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

				this.WaitForData();

				Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);

				bool success = (resp.ResponseCode == 200);
				return !success;
			}
		}

		public string TenantName { get; set; }

		private Dictionary<Guid, Dictionary<Guid, object>> AttributeValues = new Dictionary<Guid, Dictionary<Guid, object>>();
		private Dictionary<Guid, Dictionary<Guid, List<Instance>>> RelatedInstances = new Dictionary<Guid, Dictionary<Guid, List<Instance>>>();

		public object GetAttributeValue(Instance instTarget, Guid attributeGlobalIdentifier, DateTime effectiveDate, object defaultValue = null)
		{
			if (!AttributeValues.ContainsKey(instTarget.GlobalIdentifier))
			{
				AttributeValues.Add(instTarget.GlobalIdentifier, new Dictionary<Guid, object>());
			}

			Dictionary<Guid, object> dict = AttributeValues[instTarget.GlobalIdentifier];
			if (!dict.ContainsKey(attributeGlobalIdentifier))
			{
				Request req = new Request();
				req.Path = String.Format("/instances/by-iid/{0}/attributes/by-gid/{1}", instTarget.GlobalIdentifier, attributeGlobalIdentifier);
				req.Method = "GET";
				req.Protocol = "OMS/1.0";
				(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

				this.WaitForData();

				Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);

				bool success = (resp.ResponseCode == 200);
				if (success)
				{
					object value = resp.Content;
					dict[attributeGlobalIdentifier] = value;
					// fall through to end of method
				}
				else
				{
					return defaultValue;
				}
			}
			return dict[attributeGlobalIdentifier];
		}

		public Relationship[] GetRelationships(Instance instTarget)
		{
			List<Relationship> list = new List<Relationship>();

			Request req = new Request();
			req.Path = String.Format("/instances/by-gid/{0}/related-instances", instTarget.GlobalIdentifier);
			req.Method = "GET";
			req.Protocol = "OMS/1.0";
			(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

			this.WaitForData();

			Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
			if (resp.ResponseCode == 200)
			{
				HandleArrayResponse(resp, list);
			}

			return list.ToArray();
		}

		public Instance GetRelatedInstance(Instance instTarget, Guid uuidRelationship)
		{
			Instance instRelationship = GetInstance(uuidRelationship);
			return GetRelatedInstance(instTarget, instRelationship);
		}
		public Instance GetRelatedInstance(Instance instTarget, Instance instRelationship)
		{
			Instance[] insts = GetRelatedInstances(instTarget, instRelationship);
			if (insts.Length > 0)
			{
				return insts[0];
			}
			return null;
		}
		public Instance[] GetRelatedInstances(Instance instTarget, Guid uuidRelationship)
		{
			Instance instRelationship = GetInstance(uuidRelationship);
			return GetRelatedInstances(instTarget, instRelationship);
		}
		public Instance[] GetRelatedInstances(Instance instTarget, Instance instRelationship)
		{
			List<Instance> list = new List<Instance>();

			if (!RelatedInstances.ContainsKey(instTarget.GlobalIdentifier))
			{
				RelatedInstances.Add(instTarget.GlobalIdentifier, new Dictionary<Guid, List<Instance>>());
			}
			if (!RelatedInstances[instTarget.GlobalIdentifier].ContainsKey(instRelationship.GlobalIdentifier))
			{
				Request req = new Request();
				req.Path = String.Format("/instances/by-gid/{0}/related-instances/by-gid/{1}", instTarget.GlobalIdentifier, instRelationship.GlobalIdentifier);
				req.Method = "GET";
				req.Protocol = "OMS/1.0";
				(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

				this.WaitForData();

				Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
				if (resp.ResponseCode == 200)
				{
					HandleArrayResponse(resp, list);
				}

				RelatedInstances[instTarget.GlobalIdentifier].Add(instRelationship.GlobalIdentifier, list);
			}
			return RelatedInstances[instTarget.GlobalIdentifier][instRelationship.GlobalIdentifier].ToArray();
		}

		private void HandleArrayResponse(Response resp, List<Instance> list)
		{
			PropertyListObjectModel plom = new PropertyListObjectModel();
			JSONDataFormat json = new JSONDataFormat();
			StringAccessor sa = new StringAccessor(resp.Content);

			Document.Load(plom, json, sa);

			Instance[] insts = InstanceFromJSONPropertyList(plom);
			foreach (Instance inst in insts)
			{
				UpdateParentInst(inst);
				list.Add(inst);
			}
		}
		private void HandleArrayResponse(Response resp, List<Relationship> list)
		{
			PropertyListObjectModel plom = new PropertyListObjectModel();
			JSONDataFormat json = new JSONDataFormat();
			StringAccessor sa = new StringAccessor(resp.Content);

			Document.Load(plom, json, sa);

			Relationship[] rels = RelationshipFromJSONPropertyList(plom);
			foreach (Relationship rel in rels)
			{
				list.Add(rel);
			}
		}

		public Relationship[] RelationshipFromJSONPropertyList(PropertyListObjectModel plom)
		{
			throw new NotImplementedException();
		}

		public OMSComponent RenderSummaryComponent(Instance inst, Group g)
		{
			OMSSummaryComponent omsresp = new OMSSummaryComponent();

			Property propFields = g.Items.OfType<Property>("fields");
			if (propFields != null)
			{
				object[] oFields = (propFields.Value as object[]);
				if (oFields != null)
				{
					foreach (object oField in oFields)
					{
						Group gField = (oField as Group);
						if (gField == null) continue;

						Property propFieldType = gField.Items.OfType<Property>("type");
						if (propFieldType == null) continue;

						switch (propFieldType.Value.ToString())
						{
							case "text":
							{
								OMSSummaryComponent.OMSSummaryFieldText field = new OMSSummaryComponent.OMSSummaryFieldText((gField.Items.OfType<Property>("iid") != null ? (new InstanceKey(gField.Items.OfType<Property>("iid").Value.ToString())) : InstanceKey.Empty), gField.Items.OfType<Property>("title").Value.ToString(), (gField.Items.OfType<Property>("value") != null ? gField.Items.OfType<Property>("value").Value.ToString() : null));
								field.ReadOnly = (gField.Items.OfType<Property>("readonly") != null && gField.Items.OfType<Property>("readonly").Value.ToString() == "true");
								omsresp.Fields.Add(field);
								break;
							}
						}
					}
				}
			}
			return omsresp;
		}

		public OMSComponent RenderDetailComponent(Instance inst, Group g)
		{
			OMSDetailComponent omsresp = new OMSDetailComponent();

			Property propColumns = g.Items.OfType<Property>("columns");
			if (propColumns != null)
			{
				object[] gColumns = (propColumns.Value as object[]);
				if (gColumns != null)
				{
					foreach (object oColumn in gColumns)
					{
						Group gColumn = (oColumn as Group);
						if (gColumn == null) continue;

						InstanceKey instanceID = gColumn.Items.OfType<Property>("iid") != null ?
							new InstanceKey(gColumn.Items.OfType<Property>("iid").Value.ToString()) :
							InstanceKey.Empty;

						string title = (gColumn.Items.OfType<Property>("title") != null ? gColumn.Items.OfType<Property>("title").Value.ToString() : String.Empty);
						omsresp.Columns.Add(new OMSDetailComponent.OMSDetailColumn(instanceID, title));
					}
				}
			}

			Property propRows = g.Items.OfType<Property>("rows");
			if (propRows != null)
			{
				object[] gRows = (propRows.Value as object[]);
				if (gRows != null)
				{
					foreach (object oRow in gRows)
					{
						Group gRow = (oRow as Group);
						if (gRow == null) continue;

						OMSDetailComponent.OMSDetailRow row = new OMSDetailComponent.OMSDetailRow();
						row.InstanceID = new InstanceKey(gRow.Items.OfType<Property>("iid").Value.ToString());

						object[] oCols = (gRow.Items.OfType<Property>("columns").Value as object[]);
						foreach (object oCol in oCols)
						{
							Group gCol = (oCol as Group);
							if (gCol == null) continue;

							InstanceKey iid = InstanceKey.Empty;
							string text = String.Empty;
							if (gCol.Items.OfType<Property>("iid") != null)
							{
								iid = new InstanceKey(gCol.Items.OfType<Property>("iid").Value.ToString());
							}
							if (gCol.Items.OfType<Property>("text") != null)
							{
								text = gCol.Items.OfType<Property>("text").Value.ToString();
							}

							OMSDetailComponent.OMSDetailRowColumn rrc = new OMSDetailComponent.OMSDetailRowColumn(new InstanceKey(gCol.Items.OfType<Property>("colid").Value.ToString()), text, iid);
							row.Columns.Add(rrc);
						}
						omsresp.Rows.Add(row);
					}
				}
			}
			return omsresp;
		}
		public OMSComponent RenderTabContainerComponent(Instance inst, Group g)
		{
			OMSTabContainerComponent omsresp = new OMSTabContainerComponent();

			Property propTabPages = g.Items.OfType<Property>("tabpages");
			if (propTabPages != null)
			{
				object[] oTabPages = (propTabPages.Value as object[]);
				if (oTabPages != null)
				{
					foreach (object oTabPage in oTabPages)
					{
						Group gTabPage = (oTabPage as Group);
						if (gTabPage == null) continue;

						OMSTabContainerComponent.TabPage page = new OMSTabContainerComponent.TabPage();
						Property propTitle = gTabPage.Items.OfType<Property>("title");
						if (propTitle != null)
						{
							page.Title = propTitle.Value.ToString();
						}

						Property propComponents = gTabPage.Items.OfType<Property>("items");
						if (propComponents != null)
						{
							object[] oComponents = (object[])propComponents.Value;
							foreach (object oComponent in oComponents)
							{
								Group gComponent = (oComponent as Group);
								if (gComponent == null) continue;

								OMSComponent omscomp = RenderComponent(inst, gComponent);
								if (omscomp != null)
									page.Components.Add(omscomp);
							}
						}		

						omsresp.TabPages.Add(page);
					}
				}
			}
			return omsresp;
		}


		public IOmsResponse ExecuteInstance(Instance inst)
		{
			Request req = new Request();
			req.Path = String.Format("/instances/by-gid/{0}/execute", inst.GlobalIdentifier);
			req.Method = "GET";
			req.Protocol = "OMS/1.0";
			(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

			WaitForData();

			Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
			if (resp != null && resp.ResponseCode == 200)
			{
				List<Header> listHeaders = new List<Header>(resp.Headers);

				Instance instParentClass = this.GetParentClass(inst);

				// Mocha.NET relationships do not inherit... yet 
				// See Configuration/000-Classes/010-000-Report.xqjs for relationship {CF396DAA-75EA-4148-8468-C66A71F2262D}
				//		(Class.has default Task)
				// redefined on StandardReport when it is already defined on base class Report

				Instance[] instSuperClasses = GetRelatedInstances(inst, KnownRelationshipGuids.Class__has_super__Class);
				Instance instDefaultAction = GetRelatedInstance(instParentClass, KnownRelationshipGuids.Class__has_default__Task);

				StringAccessor sa = new StringAccessor(resp.Content);
				PropertyListObjectModel plom = new PropertyListObjectModel();
				JSONDataFormat df = new JSONDataFormat();
				Document.Load(plom, df, sa);

				Group g = plom.Items[0] as Group;
				Property propTitle = g.Items.OfType<Property>("title");
				Property propDescription = g.Items.OfType<Property>("description");

				Property propItems = g.Items.OfType<Property>("items");
				object[] oItems = (propItems.Value as object[]);


				OmsPageResponse omsresp = new OmsPageResponse();
				if (propTitle != null && propTitle.Value != null)
				{
					omsresp.Title = propTitle.Value.ToString();
				}
				if (propDescription != null && propDescription.Value != null)
				{
					omsresp.Description = propDescription.Value.ToString();
				}

				foreach (object oItem in oItems)
				{
					if (oItem is Group)
					{
						Group gItem = (oItem as Group);

						OMSComponent omscomp = RenderComponent(inst, gItem);
						if (omscomp != null)
							omsresp.Components.Add(omscomp);
					}
				}
				return omsresp;
			}
			return null;
		}

		private OMSComponent RenderComponent(Instance inst, Group gItem)
		{
			Property propType = gItem.Items.OfType<Property>("type");
			if (propType == null) return null;
			string type = propType.Value.ToString();

			OMSComponent omscomp = null;
			if (type == "summary")
			{
				omscomp = RenderSummaryComponent(inst, gItem);
			}
			else if (type == "detail")
			{
				omscomp = RenderDetailComponent(inst, gItem);
			}
			else if (type == "tab-container")
			{
				omscomp = RenderTabContainerComponent(inst, gItem);
			}
			return omscomp;
		}







		// COMMON METHODS

		public string GetTTC(Instance instTTC, string defaultValue = "")
		{
			return GetTTC(null, instTTC, defaultValue);
		}

		/// <summary>
		/// Gets the value of the Translatable Text Constant related to the specified <see cref="Instance" /> via the relationship with the given Relationship <see cref="Guid" />.
		/// </summary>
		/// <returns><see cref="String"/> value of the Translatable Text Constant related to the specified <see cref="Instance" /> via the given Relationship <see cref="Guid" />.</returns>
		/// <param name="inst">The <see cref="Instance" /> for which to query a value.</param>
		/// <param name="guidRelationship">The <see cref="Guid" /> of the relationship for which to query a value.</param>
		/// <param name="defaultValue">The default value returned if the Translatable Text Constant is not defined on the specified <see cref="Instance" /> for the given Relationship.</param>
		public string GetTTC(Instance inst, Guid guidRelationship, string defaultValue = "")
		{
			return GetTTC(inst, GetInstance(guidRelationship), defaultValue);
		}
		public string GetTTC(Instance inst, Instance instRelationshipOrTTC, string defaultValue = "")
		{
			string value = defaultValue;
			if (inst != null)
			{
				instRelationshipOrTTC = GetRelatedInstance(inst, instRelationshipOrTTC);
			}
			if (instRelationshipOrTTC != null)
			{
				Instance instTTCValue = GetRelatedInstance(instRelationshipOrTTC, KnownRelationshipGuids.Translation__has__Translation_Value);

				Instance Attribute___Value = GetInstance(KnownAttributeGuids.Text.Value);
				value = this.GetAttributeValue<string>(instTTCValue, Attribute___Value);
			}
			return value;
		}

		public Instance GetRelatedInstance(Instance target, Guid relationshipID, bool searchSubclasses)
		{
			throw new NotImplementedException();
		}

		public Instance[] GetRelatedInstances(Instance target, Guid relationshipID, bool searchSubclasses)
		{
			throw new NotImplementedException();
		}
	}
}