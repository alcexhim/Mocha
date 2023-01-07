//
//  MyClass.cs
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
using System.Text;
using MBS.Networking;
using MBS.Networking.Protocols.HyperTextTransfer;
using MBS.Networking.Transports.TCP;
using Mocha.Core;
using UniversalEditor.Accessors;
using UniversalEditor.IO;

namespace Mocha.OMS
{
	public class OMSServer : Server
	{
		public string TenantName { get; set; }

		public OMSServer()
		{
			this.Service = new OMSService();
			this.Protocol = new OMSProtocol();
			this.Transport = new TCPTransport();
		}

		public event InstanceRequestedEventHandler InstanceRequested;
		protected virtual void OnInstanceRequested(InstanceRequestedEventArgs e)
		{
			InstanceRequested?.Invoke(this, e);
		}

		public event AttributeOrRelationshipRequestedEventHandler AttributeValueRequested;
		protected virtual void OnAttributeValueRequested(AttributeOrRelationshipRequestedEventArgs e)
		{
			AttributeValueRequested?.Invoke(this, e);
		}

		public event AttributeOrRelationshipRequestedEventHandler RelatedInstancesRequested;
		protected virtual void OnRelatedInstancesRequested(AttributeOrRelationshipRequestedEventArgs e)
		{
			RelatedInstancesRequested?.Invoke(this, e);
		}

		protected override void OnClientConnected(ClientConnectedEventArgs e)
		{
			base.OnClientConnected(e);

			Console.WriteLine("MochaOMS: client connected");

			while (true)
			{
				bool retval = e.Client.WaitForData();
				if (!retval) // timed out
					continue;

				Console.WriteLine("MochaOMS: reading " + e.Client.Available.ToString() + " bytes of data");

				Request pkt = null;
				try
				{
					pkt = (Protocol as HyperTextTransferProtocol).GetRequest(e.Client);
				}
				catch (ArgumentException ex)
				{
					Response resp = KnownResponses.BadRequest;
					(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
					continue;
				}

				if (pkt != null)
				{
					if (pkt.Method == "GET")
					{
						// sample request:
						// 
						// GET /instances/by-iid/1$1 OMS/1.0
						// 
						// sample response:
						// 
						// 200 OK
						// { JSON object that describes instance }

						string[] path = pkt.Path.Split(new char[] { '/' });
						if (path.Length > 1)
						{
							if (path[1] == "instances")
							{
								if (path.Length > 2)
								{
									Response resp = null;
									Instance inst = null;
									if (path[2] == "by-iid")
									{
										if (path.Length > 3)
										{
											inst = GetInstance(path[3]);
										}
										else
										{
											resp = KnownResponses.BadRequest;
											resp.Content = "Must specify GUID";
										}
									}
									else if (path[2] == "by-uuid")
									{
										if (path.Length > 3)
										{
											inst = GetInstance(new Guid(path[3]));
										}
										else
										{
											resp = KnownResponses.BadRequest;
											resp.Content = "Must specify GUID";
										}
									}
									else
									{
										resp = KnownResponses.BadRequest;
										resp.Content = "Must specify request type (by-iid, by-uuid)";
									}

									if (inst != null)
									{
										if (path.Length > 4)
										{
											if (path[4] == "instances")
											{
												Instance[] insts = GetInstances(inst);

												resp = KnownResponses.OK;
												resp.Headers.Add("User-Agent", "MochaOMS/1.0");
												StringBuilder sb = new StringBuilder();
												sb.Append('[');
												foreach (Instance inst1 in insts)
												{
													sb.Append(inst1.ToJSONString());
													if (Array.IndexOf(insts, inst1) < insts.Length - 1) sb.Append(',');
												}
												sb.Append(']');
												resp.Content = sb.ToString();
												(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
												continue;
											}
											else if (path[4] == "execute")
											{
												// hack let's do this
												Instance instParentClass = this.GetParentClass(inst);

												/*
												 * Default Action comes from the 'Class `x`.has default Task' relationship
												 * add this to your class definition in XQJS:											 
													{

														"Comment": "Class `Class`.has default Task `View Class`",
														"RelationshipID": "{CF396DAA-75EA-4148-8468-C66A71F2262D}",
														"InverseRelationshipID": "{FFDFCEB1-1064-41B9-A577-C4975C930694}",
														"targetInstances":
														[
															"{uuid of the instance of UITask}"
														]
													}											 
												 */

												Instance instDefaultAction = this.GetRelatedInstance(instParentClass, KnownRelationshipGuids.Class__has_default__Task);

												if (instDefaultAction == null)
												{
													resp = KnownResponses.BadRequest;

													resp.Headers.Add("User-Agent", "MochaOMS/1.0");
													(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
													continue;
												}
												/*
												else if (instDefaultAction.GetInstanceKey() == (new InstanceKey(46, 10)))
												{
													// FIXME: don't hardcode this
													// the Report task should be as follows:
																									
													 { "title": "Report Title", "items": [
													 	{
													 		"type": "detail",
													 		"columns": [
														 		// from report
														 	],
														 	"rows": [
														 		// from report
														 	]
													 	}
													 ] }

													StringBuilder sb = new StringBuilder();
													string strInstanceTitle = GetInstanceTitle(inst);
													string strInstanceDescription = GetInstanceTitle(inst, new Guid("{D5AA18A7-7ACD-4792-B039-6C620A151BAD}"));
													sb.Append("{\"title\": \"" + strInstanceTitle + "\", \"description\": \"" + strInstanceDescription + "\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");

													Instance[] instReportColumns = GetRelatedInstances(inst, KnownRelationshipGuids.Report__has__Report_Column);
													for (int i = 0; i < instReportColumns.Length; i++)
													{
														Instance instReportColumn = instReportColumns[i];

														sb.Append('{');
														sb.Append("\"iid\": \"");
														sb.Append(instReportColumn.GetInstanceIDString());
														sb.Append('"');
														sb.Append(',');
														sb.Append("\"title\": \"");

														Instance instReportField = GetRelatedInstance(instReportColumn, KnownRelationshipGuids.Report_Column__has__Report_Field);


														sb.Append(GetInstanceTitle(instReportField));
														sb.Append('"');
														sb.Append('}');

														if (i < instReportColumns.Length - 1)
															sb.Append(',');
													}

													sb.Append("], \"rows\":[");

													Instance instReportDataSource = GetRelatedInstance(inst, KnownRelationshipGuids.Report__has__Report_Data_Source);
													Instance instReportDataSourceMethod = GetRelatedInstance(instReportDataSource, KnownRelationshipGuids.Report_Data_Source__has_source__Method);
													Instance instMethodBinding = GetRelatedInstance(instReportDataSourceMethod, KnownRelationshipGuids.Method__has__Method_Binding);
													Instance instSourceClass = GetRelatedInstance(instMethodBinding, KnownRelationshipGuids.Return_Instance_Set_Method_Binding__has_source__Class);

													Instance[] insts = GetInstances(instSourceClass);
													foreach (Instance retvalInst in insts)
													{
														sb.Append('{');
														sb.Append(String.Format("\"iid\":\"{0}\",", retvalInst.GetInstanceIDString()));
														sb.Append("\"columns\":[");
														for (int i = 0; i < instReportColumns.Length; i++)
														{
															sb.Append('{');
															sb.Append(String.Format("\"colid\":\"{0}\"", instReportColumns[i].GetInstanceIDString()));
															if (i == 0)
															{
																sb.Append(',');
																sb.Append(String.Format("\"text\":\"{0}\"", GetInstanceTitle(retvalInst)));
																sb.Append(',');
																sb.Append(String.Format("\"iid\":\"{0}\"", retvalInst.GetInstanceIDString()));
															}
															sb.Append('}');
															if (i < instReportColumns.Length - 1)
																sb.Append(',');
														}
														sb.Append("]");
														sb.Append('}');
														if (Array.IndexOf(insts, retvalInst) < insts.Length - 1)
															sb.Append(',');
													}

													sb.Append("] }"); // rows

													sb.Append("] }"); // items

													resp = KnownResponses.OK;
													resp.Headers.Add("User-Agent", "MochaOMS/1.0");
													resp.Content = sb.ToString();
												}
												else if (instDefaultAction.GetInstanceKey() == (new InstanceKey(46, 11)))
												{
													// FIXME: I SAID DON'T HARDCODE THIS
													// view class

													// FIXME: definition of relationships should be able to be defined anywhere
													// currently the definition of relationships such as "class.has default Task"
													// must be defined in the FIRST OCCURRENCE OF THE JSON OBJECT
													//		(e.g. 000-Classes/000-Class.xqjs)
													// rather than in a later file
													//		(e.g. Tasks/View Class.xqjs)

													StringBuilder sb = new StringBuilder();
													string strInstanceTitle = GetInstanceTitle(inst);
													string strInstanceDescription = GetInstanceTitle(inst, new Guid("{D5AA18A7-7ACD-4792-B039-6C620A151BAD}"));
													sb.Append("{\"title\": \"" + strInstanceTitle + "\", \"description\": \"" + strInstanceDescription + "\", \"items\": [");

													sb.Append("{ \"type\": \"summary\", \"fields\": [");
													sb.Append("{ \"type\": \"text\", \"readonly\": \"true\", \"title\": \"Instructions\", \"value\": \"These are sample instructions for this Form View\" },");
													sb.Append("{ \"type\": \"text\", \"title\": \"Test Field #1\" },");
													sb.Append("{ \"type\": \"text\", \"title\": \"Another Test Field\" }");
													sb.Append("]},");

													sb.Append("{ \"type\": \"tab-container\", \"tabpages\": [");

													sb.Append("{ \"title\": \"Attributes\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");
													sb.Append("{ \"title\": \"Attribute\" },");
													sb.Append("{ \"title\": \"Value\" }");
													sb.Append("]}");

													sb.Append("]},");
													sb.Append("{ \"title\": \"Relationships\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");
													sb.Append("{ \"title\": \"Relationship\" },");
													sb.Append("{ \"title\": \"Target instances\" }");
													sb.Append("]}");

													sb.Append("]},");
													sb.Append("{ \"title\": \"Instances\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");
													sb.Append("{ \"title\": \"ID\" },");
													sb.Append("{ \"title\": \"Instance\" }");
													sb.Append("]}");

													sb.Append("]}");

													sb.Append("] }"); // items

													resp = KnownResponses.OK;
													resp.Headers.Add("User-Agent", "MochaOMS/1.0");
													resp.Content = sb.ToString();
												}
												else if (instDefaultAction.GetInstanceKey() == (new InstanceKey(46, 12)))
												{
													// FIXME: you must have cloth ears. THIS SHOULD NOT BE HARDCODED
													// view class

													// FIXME: definition of relationships should be able to be defined anywhere
													// currently the definition of relationships such as "class.has default Task"
													// must be defined in the FIRST OCCURRENCE OF THE JSON OBJECT
													//		(e.g. 000-Classes/000-Class.xqjs)
													// rather than in a later file
													//		(e.g. Tasks/View Class.xqjs)

													StringBuilder sb = new StringBuilder();
													string strInstanceTitle = GetInstanceTitle(inst);
													string strInstanceDescription = GetInstanceTitle(inst, new Guid("{D5AA18A7-7ACD-4792-B039-6C620A151BAD}"));
													sb.Append("{\"title\": \"" + strInstanceTitle + "\", \"description\": \"" + strInstanceDescription + "\", \"items\": [");

													sb.Append("{ \"type\": \"summary\", \"fields\": [");
													sb.Append("{ \"type\": \"text\", \"readonly\": \"true\", \"title\": \"Instructions\", \"value\": \"These are sample instructions for this Form View\" },");
													sb.Append("{ \"type\": \"text\", \"title\": \"Test Field #1\" },");
													sb.Append("{ \"type\": \"text\", \"title\": \"Another Test Field\" }");
													sb.Append("]},");

													sb.Append("{ \"type\": \"tab-container\", \"tabpages\": [");

													sb.Append("{ \"title\": \"Attributes\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");
													sb.Append("{ \"title\": \"Attribute\" },");
													sb.Append("{ \"title\": \"Value\" }");
													sb.Append("]}");

													sb.Append("]},");
													sb.Append("{ \"title\": \"Relationships\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");
													sb.Append("{ \"title\": \"Relationship\" },");
													sb.Append("{ \"title\": \"Target instances\" }");
													sb.Append("]}");

													sb.Append("]},");
													sb.Append("{ \"title\": \"Instances\", \"items\": [");

													sb.Append("{ \"type\": \"detail\", \"columns\": [");
													sb.Append("{ \"title\": \"ID\" },");
													sb.Append("{ \"title\": \"Instance\" }");
													sb.Append("]}");

													sb.Append("]}");

													sb.Append("] }"); // items

													resp = KnownResponses.OK;
													resp.Headers.Add("User-Agent", "MochaOMS/1.0");
													resp.Content = sb.ToString();
												}
												*/
											}
											else if (path[4] == "attributes" || path[4] == "related-instances")
											{
												if (path.Length > 5)
												{
													Instance instAttributeOrRelationship = null;
													switch (path[5])
													{
														case "by-iid":
														{
															if (path.Length > 6)
															{
																instAttributeOrRelationship = GetInstance(path[6]);
															}
															break;
														}
														case "by-uuid":
														{
															if (path.Length > 6)
															{
																instAttributeOrRelationship = GetInstance(new Guid(path[6]));
															}
															break;
														}
													}

													if (instAttributeOrRelationship != null)
													{
														if (path[4] == "attributes")
														{
															string value = this.GetAttributeValue(inst, instAttributeOrRelationship)?.ToString();
															if (value == null)
															{
																resp = KnownResponses.NotFound;
															}
															else
															{
																resp = KnownResponses.OK;
																resp.Content = value;
															}

															resp.Headers.Add("User-Agent", "MochaOMS/1.0");
															(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
															continue;
														}
														else if (path[4] == "related-instances")
														{
															Instance[] ary = GetRelatedInstances(inst, instAttributeOrRelationship);
															if (ary == null)
															{
																resp = KnownResponses.NotFound;
															}
															else
															{
																resp = KnownResponses.OK;

																StringBuilder sb = new StringBuilder();
																sb.Append('[');
																foreach (Instance inst1 in ary)
																{
																	sb.Append(inst1.ToJSONString());
																	if (Array.IndexOf(ary, inst1) < ary.Length - 1) sb.Append(',');
																}
																sb.Append(']');

																resp.Content = sb.ToString();
															}

															resp.Headers.Add("User-Agent", "MochaOMS/1.0");
															(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
															continue;
														}
													}
													else
													{
														// 1$145 is not found
														resp = KnownResponses.NotFound;
														resp.Headers.Add("User-Agent", "MochaOMS/1.0");
														(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
														continue;
													}
												}
												else
												{
													AttributeOrRelationshipRequestedEventArgs ee = new AttributeOrRelationshipRequestedEventArgs(inst, null);
													OnRelatedInstancesRequested(ee);
													if (ee.Cancel) continue;

													Relationship[] rels = (ee.Value as Relationship[]);

													// return all relationships on this instance
													resp = KnownResponses.OK;

													StringBuilder sb = new StringBuilder();
													sb.Append('[');
													foreach (Relationship rel in rels)
													{
														sb.Append(rel.ToJSONString());
														if (Array.IndexOf(rels, rel) < rels.Length - 1) sb.Append(',');
													}
													sb.Append(']');

													resp.Content = sb.ToString();
												}
											}
										}
										else
										{
											resp = KnownResponses.OK;
											resp.Headers.Add("User-Agent", "MochaOMS/1.0");
											resp.Content = inst.ToJSONString();
										}
									}
									else
									{
										resp = KnownResponses.NotFound;
									}

									(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
								}
								else
								{
									InstanceRequestedEventArgs ee = new InstanceRequestedEventArgs(InstanceRequestedQueryType.All, InstanceRequestedIDType.None, String.Empty);
									OnInstanceRequested(ee);

									Instance[] instances = ee.Instances;

									StringBuilder sb = new StringBuilder();
									sb.Append('[');
									foreach (Instance inst in instances)
									{
										sb.Append(inst.ToJSONString());
										if (Array.IndexOf(instances, inst) < instances.Length - 1) sb.Append(',');
									}
									sb.Append(']');

									Response resp = KnownResponses.OK;
									resp.Content = sb.ToString();

									(this.Protocol as HyperTextTransferProtocol).SendResponse(e.Client, resp);
								}
							}
						}
					}
				}
			}
		}

		// BEGIN: MOCHA COMMON

		public Instance[] GetInstances(Instance instParentClass, bool searchSubclasses = false)
		{
			if (instParentClass == null) return new Instance[0];

			InstanceRequestedEventArgs ee1 = new InstanceRequestedEventArgs(InstanceRequestedQueryType.All, InstanceRequestedIDType.InstanceId, instParentClass.GlobalIdentifier.ToString("B"));
			// ee1.SearchSubclasses = searchSubclasses;
			OnInstanceRequested(ee1);

			if (ee1.Cancel) return new Instance[0];
			return ee1.Instances;
		}

		public string GetInstanceTitle(Instance inst, string defaultValue = "")
		{
			return GetInstanceTitle(inst, Guid.Empty, defaultValue);
		}
		public string GetInstanceTitle(Instance inst, Guid labeledByInstID, string defaultValue = "")
		{
			if (labeledByInstID == Guid.Empty)
			{
				Instance instClassLabeledBy = this.GetRelatedInstance(this.GetParentClass(inst), KnownRelationshipGuids.Class__instance_labeled_by__String);
				if (instClassLabeledBy == null) return String.Empty;

				Instance[] instString__has__String_Component = this.GetRelatedInstances(instClassLabeledBy, KnownRelationshipGuids.String__has__String_Component);

				StringBuilder sb = new StringBuilder();
				foreach (Instance instStringComponent in instString__has__String_Component)
				{
					// ESI
					Instance instRel = this.GetRelatedInstance(instStringComponent, KnownRelationshipGuids.Extract_Single_Instance_String_Component__has__Relationship);

					Instance instAAA = GetRelatedInstance(inst, instRel);
					string val = GetTTC(inst, instAAA);
					sb.Append(val);
				}
				return sb.ToString();
			}
			else
			{
				Instance instRel = GetInstance(labeledByInstID);
				Instance instTTC = GetRelatedInstance(inst, instRel);
				return GetTTC(inst, instTTC, defaultValue);
			}
		}
		public string GetTTC(Instance inst, Instance instTTC, string defaultValue = "")
		{
			string value = defaultValue;
			if (instTTC != null)
			{
				Instance instTTCValue = this.GetRelatedInstance(instTTC, KnownRelationshipGuids.Translation__has__Translation_Value);

				Instance Attribute___Value = GetInstance(KnownAttributeGuids.Text.Value);
				value = this.GetAttributeValue(instTTCValue, Attribute___Value)?.ToString();
			}
			return value;
		}

		public Instance GetRelatedInstance(Instance target, Guid relationshipID)
		{
			throw new NotImplementedException();
		}

		public Instance[] GetRelatedInstances(Instance target, Guid relationshipID)
		{
			throw new NotImplementedException();
		}

		public object GetAttributeValue(Instance instTarget, Guid instAttributeID, DateTime effectiveDate, object defaultValue = null)
		{
			AttributeOrRelationshipRequestedEventArgs ee = new AttributeOrRelationshipRequestedEventArgs(instTarget, new Instance(instAttributeID));
			OnAttributeValueRequested(ee);
			if (ee.Cancel)
				return defaultValue;

			if (ee.Value == null)
				return defaultValue;
			return ee.Value;
		}

		public Instance GetRelatedInstance(Instance inst, Instance instAttributeOrRelationship)
		{
			Instance[] ary = GetRelatedInstances(inst, instAttributeOrRelationship);
			if (ary == null)
				return null;

			if (ary.Length > 0) return ary[0];
			return null;
		}
		public Instance[] GetRelatedInstances(Instance inst, Instance instAttributeOrRelationship)
		{
			AttributeOrRelationshipRequestedEventArgs ee = new AttributeOrRelationshipRequestedEventArgs(inst, instAttributeOrRelationship);
			OnRelatedInstancesRequested(ee);
			if (!ee.Cancel)
				return (Instance[])ee.Value;
			return null;
		}

		public Instance GetInstance(Guid v)
		{
			InstanceRequestedEventArgs ee = new InstanceRequestedEventArgs(InstanceRequestedQueryType.SpecificInstance, InstanceRequestedIDType.GlobalIdentifier, v.ToString());
			OnInstanceRequested(ee);
			if (!ee.Cancel) return ee.Instances[0];
			return null;
		}

		public Instance GetInstance(string iid)
		{
			InstanceRequestedEventArgs ee = new InstanceRequestedEventArgs(InstanceRequestedQueryType.SpecificInstance, InstanceRequestedIDType.InstanceId, iid);
			OnInstanceRequested(ee);
			if (!ee.Cancel) return ee.Instances[0];
			return null;
		}

		public Instance GetInstance(InstanceKey iid)
		{
			InstanceRequestedEventArgs ee = new InstanceRequestedEventArgs(InstanceRequestedQueryType.SpecificInstance, InstanceRequestedIDType.InstanceId, iid.ToString());
			OnInstanceRequested(ee);
			if (!ee.Cancel) return ee.Instances[0];
			return null;
		}

		public IOmsResponse ExecuteInstance(Instance instance)
		{
			return null;
		}

		public Instance GetRelatedInstance(Instance target, Guid relationshipID, bool searchSubclasses)
		{
			throw new NotImplementedException();
		}

		public Instance[] GetRelatedInstances(Instance target, Guid relationshipID, bool searchSubclasses)
		{
			throw new NotImplementedException();
		}
		// END: MOCHA COMMON
	}
}
