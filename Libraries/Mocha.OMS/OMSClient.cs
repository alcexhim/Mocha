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

namespace Mocha.OMS
{
	public class OMSClient : MBS.Networking.Client, IOmsProvider
	{
		public OMSClient()
		{
			this.Protocol = new OMS.OMSProtocol();
			this.Service = new OMS.OMSService();
			this.Transport = new TCPTransport();
		}

		public Instance GetInstance(InstanceClassIDPair instanceID)
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

				Instance inst = Instance.FromJSONPropertyList(plom) [0];
				return inst;
			}

			return null;

		}

		/// <summary>
		/// Retrieves the <see cref="Instance" /> with the specified Global Identifier from the OMS.
		/// </summary>
		/// <returns>The instance, or null if no instance with the specified Global Identifier exists in the OMS.</returns>
		/// <param name="globalIdentifier">The GUID of the instance to look up.</param>
		public Instance GetInstance(Guid globalIdentifier)
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

				Instance inst = Instance.FromJSONPropertyList(plom) [0];
				return inst;
			}

			return null;

		}

		public string GetTTC(Instance inst, Instance instTTC, string defaultValue = "")
		{
			string value = defaultValue;
			if (instTTC != null)
			{
				Instance instTTCValue = GetRelatedInstance(instTTC, KnownInstanceGUIDs.Relationship.Translatable_Text_Constant__has__Translatable_Text_Constant_Value);

				Instance Attribute___Value = GetInstance(KnownAttributeGUIDs.Value);
				value = GetAttributeValue<string>(instTTCValue, Attribute___Value);
			}
			return value;
		}

		public string BamString(Instance instString, Instance instInstance)
		{
			StringBuilder sb = new StringBuilder();

			Instance[] stringComponents = GetRelatedInstances(instString, KnownInstanceGUIDs.Relationship.String__has__String_Component);

			foreach (Instance instStringComponent in stringComponents)
			{
				// assuming this is an Extract Single Instance String Component for now...
				Instance instRel = GetRelatedInstance(instStringComponent, KnownInstanceGUIDs.Relationship.Extract_Single_Instance_String_Component__has__Relationship);
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

		public string GetInstanceTitle(Instance inst, string defaultValue = "")
		{
			Instance instClassLabeledBy = GetRelatedInstance(inst.ParentClass, KnownInstanceGUIDs.Relationship.Class__instance_labeled_by__String);
			if (instClassLabeledBy == null) return String.Empty;

			Instance[] instString__has__String_Component = GetRelatedInstances(instClassLabeledBy, KnownInstanceGUIDs.Relationship.String__has__String_Component);

			StringBuilder sb = new StringBuilder();
			foreach (Instance instStringComponent in instString__has__String_Component)
			{
				// ESI
				Instance instRel = GetRelatedInstance(instStringComponent, KnownInstanceGUIDs.Relationship.Extract_Single_Instance_String_Component__has__Relationship);

				Instance instAAA = GetRelatedInstance(inst, instRel);
				string val = GetTTC(inst, instAAA);
				sb.Append(val);
			}

			Instance instTTC = GetRelatedInstance(inst, KnownInstanceGUIDs.Relationship.Class__has_title__Translatable_Text_Constant);
			string value = GetTTC(inst, instTTC);

			return value;
		}

		public Instance[] GetInstances(Instance parentClassInstance = null)
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
					foreach (Group g in plom.Groups)
					{
						Instance inst = Instance.FromJSONPropertyList(g) [0];
						list.Add(inst);
					}
				}
			}
			else
			{
				Request req = new Request
				{
					Path = "/instances/by-iid/" + parentClassInstance.GetInstanceIDString() + "/instances",
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
		public object GetAttributeValue(Instance instTarget, Instance instAttribute, object defaultValue = null)
		{
			return GetAttributeValue<object>(instTarget, instAttribute, defaultValue);
		}
		public T GetAttributeValue<T>(Instance instTarget, Instance instAttribute, T defaultValue = default(T))
		{
			Request req = new Request();
			req.Path = String.Format("/instances/by-iid/{0}/attributes/by-iid/{1}", instTarget.GetInstanceIDString(), instAttribute.GetInstanceIDString());
			req.Method = "GET";
			req.Protocol = "OMS/1.0";
			(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

			this.WaitForData();

			Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);

			bool success = (resp.ResponseCode == 200);
			if (success)
			{
				object value = resp.Content;
				return (T)value;
			}

			return defaultValue;
		}

		public Relationship[] GetRelationships(Instance instTarget)
		{
			List<Relationship> list = new List<Relationship>();

			Request req = new Request();
			req.Path = String.Format("/instances/by-iid/{0}/related-instances", instTarget.GetInstanceIDString());
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

			Request req = new Request();
			req.Path = String.Format("/instances/by-iid/{0}/related-instances/by-iid/{1}", instTarget.GetInstanceIDString(), instRelationship.GetInstanceIDString());
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

		private void HandleArrayResponse(Response resp, List<Instance> list)
		{
			PropertyListObjectModel plom = new PropertyListObjectModel();
			JSONDataFormat json = new JSONDataFormat();
			StringAccessor sa = new StringAccessor(resp.Content);

			Document.Load(plom, json, sa);

			Instance[] insts = Instance.FromJSONPropertyList(plom);
			foreach (Instance inst in insts)
			{
				list.Add(inst);
			}
		}
		private void HandleArrayResponse(Response resp, List<Relationship> list)
		{
			PropertyListObjectModel plom = new PropertyListObjectModel();
			JSONDataFormat json = new JSONDataFormat();
			StringAccessor sa = new StringAccessor(resp.Content);

			Document.Load(plom, json, sa);

			Relationship[] rels = Relationship.FromJSONPropertyList(plom);
			foreach (Relationship rel in rels)
			{
				list.Add(rel);
			}
		}

		public OMSComponent RenderSummaryComponent(Instance inst, Group g)
		{
			OMSSummaryComponent omsresp = new OMSSummaryComponent();

			Property propFields = g.Properties["fields"];
			if (propFields != null)
			{
				object[] oFields = (propFields.Value as object[]);
				if (oFields != null)
				{
					foreach (object oField in oFields)
					{
						Group gField = (oField as Group);
						if (gField == null) continue;

						Property propFieldType = gField.Properties["type"];
						if (propFieldType == null) continue;

						switch (propFieldType.Value.ToString())
						{
							case "text":
							{
								OMSSummaryComponent.OMSSummaryFieldText field = new OMSSummaryComponent.OMSSummaryFieldText((gField.Properties["iid"] != null ? (new InstanceClassIDPair(gField.Properties["iid"].Value.ToString())) : InstanceClassIDPair.Empty), gField.Properties["title"].Value.ToString(), (gField.Properties["value"] != null ? gField.Properties["value"].Value.ToString() : null));
								field.ReadOnly = (gField.Properties["readonly"] != null && gField.Properties["readonly"].Value.ToString() == "true");
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

			Property propColumns = g.Properties["columns"];
			if (propColumns != null)
			{
				object[] gColumns = (propColumns.Value as object[]);
				if (gColumns != null)
				{
					foreach (object oColumn in gColumns)
					{
						Group gColumn = (oColumn as Group);
						if (gColumn == null) continue;

						InstanceClassIDPair instanceID = gColumn.Properties["iid"] != null ?
							new InstanceClassIDPair(gColumn.Properties["iid"].Value.ToString()) :
							InstanceClassIDPair.Empty;

						string title = (gColumn.Properties["title"] != null ? gColumn.Properties["title"].Value.ToString() : String.Empty);
						omsresp.Columns.Add(new OMSDetailComponent.OMSDetailColumn(instanceID, title));
					}
				}
			}

			Property propRows = g.Properties["rows"];
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
						row.InstanceID = new InstanceClassIDPair(gRow.Properties["iid"].Value.ToString());

						object[] oCols = (gRow.Properties["columns"].Value as object[]);
						foreach (object oCol in oCols)
						{
							Group gCol = (oCol as Group);
							if (gCol == null) continue;

							string text = String.Empty;
							if (gCol.Properties["text"] != null)
							{
								text = gCol.Properties["text"].Value.ToString();
							}

							OMSDetailComponent.OMSDetailRowColumn rrc = new OMSDetailComponent.OMSDetailRowColumn(new InstanceClassIDPair(gCol.Properties["colid"].Value.ToString()), text);
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

			Property propTabPages = g.Properties["tabpages"];
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
						Property propTitle = gTabPage.Properties["title"];
						if (propTitle != null)
						{
							page.Title = propTitle.Value.ToString();
						}

						Property propComponents = gTabPage.Properties["items"];
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


		public OMSResponse ExecuteInstance(Instance inst)
		{
			Request req = new Request();
			req.Path = String.Format("/instances/by-iid/{0}/execute", inst.GetInstanceIDString());
			req.Method = "GET";
			req.Protocol = "OMS/1.0";
			(this.Protocol as HyperTextTransferProtocol).SendRequest(this, req);

			WaitForData();

			Response resp = (this.Protocol as HyperTextTransferProtocol).GetResponse(this);
			if (resp != null && resp.ResponseCode == 200)
			{
				List<Header> listHeaders = new List<Header>(resp.Headers);

				Instance instParentClass = inst.ParentClass;

				// Mocha.NET relationships do not inherit... yet 
				// See Configuration/000-Classes/010-000-Report.xqjs for relationship {CF396DAA-75EA-4148-8468-C66A71F2262D}
				//		(Class.has default Task)
				// redefined on StandardReport when it is already defined on base class Report

				Instance[] instSuperClasses = GetRelatedInstances(inst, KnownInstanceGUIDs.Relationship.Class__has_super__Class);
				Instance instDefaultAction = GetRelatedInstance(instParentClass, KnownInstanceGUIDs.Relationship.Class__has_default__Task);

				StringAccessor sa = new StringAccessor(resp.Content);
				PropertyListObjectModel plom = new PropertyListObjectModel();
				JSONDataFormat df = new JSONDataFormat();
				Document.Load(plom, df, sa);

				Group g = plom.Groups[0];
				Property propTitle = g.Properties["title"];
				Property propDescription = g.Properties["description"];

				Property propItems = g.Properties["items"];
				object[] oItems = (propItems.Value as object[]);


				OMSResponse omsresp = new OMSResponse();
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
			Property propType = gItem.Properties["type"];
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

	}
}