using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Mocha.Core;
using Mocha.OMS;

namespace Mocha.Web
{

	public partial class API : System.Web.UI.Page
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			Response.Clear();

			string action = RouteData.Values["action"]?.ToString();
			switch (action)
			{
				case "inst":
				{
					Oms oms = this.GetOMS();
					if (oms == null)
					{
						Response.Write("{ \"result\": \"failure\", \"message\": \"OMS not connected\" }");
						Response.End();
						return;
					}

					List<Instance> list = new List<Instance>();
					InstanceKey ik = InstanceKey.Parse(RouteData.Values["inst"]?.ToString());
					Instance inst = oms.GetInstance(ik);
					Instance instParentClass = oms.GetParentClass(inst);
					string classText = oms.GetInstanceText(instParentClass);
					string text = oms.GetInstanceText(inst);

					Response.Clear();

					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					sb.Append("{ \"result\": \"success\", \"items\": [");
					sb.Append("{ \"instanceId\": \"" + ik.ToString() + "\", \"title\": \"" + text + "\" }");
					sb.Append("] }");
					Response.Write(sb.ToString());
					Response.End();
					break;
				}
				case "search":
				{
					Oms oms = this.GetOMS();
					if (oms == null)
					{
						Response.Write("{ \"result\": \"failure\", \"message\": \"OMS not connected\" }");
						Response.End();
						return;
					}

					List<Instance> list = new List<Instance>();

					string query = Request.QueryString["q"].Trim();

					string strValidClassIDs = Request.QueryString["valid_class_ids"];
					bool searchSubclasses = Request.QueryString["search_subclasses"]?.ToLower() == "true";
					if (strValidClassIDs != null)
					{
						string[] aryValidClassIDs = strValidClassIDs.Split(new char[] { ',' });
						for (int i = 0; i < aryValidClassIDs.Length; i++)
						{
							Instance validInst = oms.GetInstance(InstanceKey.Parse(aryValidClassIDs[i]));
							if (validInst == null) continue;

							list.AddRange(oms.GetInstances(validInst, searchSubclasses));
						}
					}
					else
					{
						list.AddRange(oms.GetInstances(oms.GetInstance(KnownInstanceGuids.Classes.Task), true));
						list.AddRange(oms.GetInstances(oms.GetInstance(KnownInstanceGuids.Classes.Report), true));
					}

					List<Instance> filtered = new List<Instance>();
					for (int i = 0; i < list.Count; i++)
					{
						string text = oms.GetInstanceText(list[i]);
						if (text == null)
							continue;
						if (!text.ToLower().Contains(query.ToLower()))
						{
							continue;
						}

						filtered.Add(list[i]);
					}

					Response.Write("{ \"result\": \"success\", \"items\": [ ");
					for (int i = 0; i < filtered.Count; i++)
					{ 
						string text = oms.GetInstanceText(filtered[i]);
						InstanceKey ik = oms.GetInstanceKey(filtered[i]);
						
						Response.Write("{ ");
						Response.Write(String.Format("\"title\": \"{0}\", \"instanceId\": \"{1}\", \"targetUrl\": \"{2}\"", text, ik, String.Format("~/d/inst/{0}.htmld", ik)));
						Response.Write(" }");
						if (i < filtered.Count - 1)
						{
							Response.Write(",");
						}
					}

					Response.Write(" ] }");
					Response.End();
					break;
				}
				case "preview":
				{
					Oms oms = this.GetOMS();
					if (oms == null)
					{
						Response.Write("{ \"result\": \"failure\", \"message\": \"OMS not connected\" }");
						Response.End();
						return;
					}

					InstanceKey ik = InstanceKey.Parse(RouteData.Values["inst"]?.ToString());
					Instance inst = oms.GetInstance(ik);
						Instance instParentClass = oms.GetParentClass(inst);
					string classText = oms.GetInstanceText(instParentClass);
					string text = oms.GetInstanceText(inst);

					Response.Clear();

					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					sb.Append("{ \"result\": \"success\", ");
					sb.Append("\"preview\": {");

						sb.Append(String.Format("\"classTitle\": \"{0}\", \"instanceTitle\": \"{1}\", ", classText, text));

						sb.Append("\"actions\": [");

						Dictionary<string, List<InstanceKey>> tasks = new Dictionary<string, List<InstanceKey>>();
						List<Instance> actions = new List<Instance>();

						Instance[] instActions;
						instActions = oms.GetRelatedInstances(oms.GetInstance(KnownInstanceGuids.Classes.Instance), KnownRelationshipGuids.Class__has_related__Task, OmsSearchOption.None);
						actions.AddRange(instActions);

						instActions = oms.GetRelatedInstances(instParentClass, KnownRelationshipGuids.Class__has_related__Task, OmsSearchOption.SuperclassesAlways);
						actions.AddRange(instActions);

						for (int i = 0; i < actions.Count; i++)
						{
							Instance instAction = actions[i];
							Instance instTaskCategory = oms.GetRelatedInstance(instAction, KnownRelationshipGuids.Task__has__Task_Category);

							string taskCategoryTitle = oms.GetTranslationValue(instTaskCategory, KnownRelationshipGuids.Task__has_preview_action_title__Translation);
							if (String.IsNullOrEmpty(taskCategoryTitle))
								taskCategoryTitle = oms.GetInstanceText(instTaskCategory);

							if (taskCategoryTitle == null)
								taskCategoryTitle = "Uncategorized";
							
							if (!tasks.ContainsKey(taskCategoryTitle))
							{
								tasks[taskCategoryTitle] = new List<InstanceKey>();
							}
							tasks[taskCategoryTitle].Add(oms.GetInstanceKey(instAction));
						}

						int j = 0;
						foreach (string key in tasks.Keys)
						{
							sb.Append("{ \"title\": \"");
							sb.Append(key);
							sb.Append("\", \"actions\": [ ");

							List<InstanceKey> list = tasks[key];
							for (int i = 0; i < list.Count; i++)
							{
								Instance instAction = oms.GetInstance(list[i]);
								if (instAction == null)
								{
									continue;
								}

								sb.Append("{ ");

								sb.Append("\"title\": \"");

								Instance instPreviewActionTitleTranslation = oms.GetRelatedInstance(instAction, KnownRelationshipGuids.Task__has_preview_action_title__Translation);
								if (instPreviewActionTitleTranslation != null)
								{
									sb.Append(oms.GetInstanceText(instPreviewActionTitleTranslation));
								}
								else
								{
									sb.Append(oms.GetInstanceText(instAction));
								}
								sb.Append("\", \"instanceId\": \"");
								sb.Append(list[i].ToString());
								sb.Append("\", \"newWindow\": ");

								bool newWindow = oms.GetAttributeValue<bool>(instAction, new Guid("{4a211f11-c5c3-4b58-a7f4-ed62538c5a3d}"));
								if (newWindow)
								{
									sb.Append("true");
								}
								else
								{
									sb.Append("false");
								}

								sb.Append(" }");
								if (i < list.Count - 1)
								{
									sb.Append(", ");
								}
							}

							sb.Append("] }");

							if (j < tasks.Keys.Count - 1)
							{
								sb.Append(", ");
							}
							j++;
						}
						/*
						sb.Append("{ \"title\": \"Audit\", \"actions\": [");

						sb.Append("{ \"title\": \"View Audit Trail\" }");

						sb.Append("] }");

						sb.Append(",");

						sb.Append("{ \"title\": \"Driver\", \"actions\": [");

						sb.Append("{ \"title\": \"View Permit History\" }");

						sb.Append("] }");

						sb.Append(",");

						sb.Append("{ \"title\": \"Favorite\", \"actions\": [");

						sb.Append("{ \"title\": \"Add to Favorites\" }");
						sb.Append(",");
						sb.Append("{ \"title\": \"Manage Favorites\" }");

						sb.Append("] }");

						sb.Append(",");

						sb.Append("{ \"title\": \"Instance\", \"actions\": [");

						sb.Append("{ \"title\": \"Edit Instance\" }");
						sb.Append(",");
						sb.Append("{ \"title\": \"Find Instance References\" }"); // this is a report
						sb.Append(",");
						sb.Append("{ \"title\": \"Delete Instance\" }");

						sb.Append("] }");

						sb.Append(",");

						sb.Append("{ \"title\": \"Integration IDs\", \"actions\": [");

						sb.Append("{ \"title\": \"View IDs\" }");
						sb.Append(",");
						sb.Append("{ \"title\": \"Edit Reference ID\" }");
						sb.Append(",");
						sb.Append("{ \"title\": \"Maintain Reference IDs\" }");

						sb.Append("] }");

						*/

						sb.Append(" ]");

						sb.Append(", ");

						sb.Append("\"components\": [ ");

						// this should be Class.has summary Page...
						// --- with Page having a Page Component of Summary Page Component...
						// --- --- with the source relationship Class.has summary Report Field
						Instance[] instPreviewFields = oms.GetRelatedInstances(instParentClass, KnownRelationshipGuids.Class__has_summary__Report_Field, OmsSearchOption.SuperclassesAlways);

						sb.Append(" { \"type\": \"box\", \"orientation\": \"vertical\", \"components\": [ ");
						/*
						sb.Append(" { \"type\": \"box\", \"orientation\": \"horizontal\", \"components\": [ ");
						sb.Append(" { \"type\": \"button\", \"text\": \"Clone\" }, ");
						sb.Append(" { \"type\": \"button\", \"text\": \"Test\" }, ");
						sb.Append(" { \"type\": \"button\", \"text\": \"Toggle\" }, ");
						sb.Append(" { \"type\": \"button\", \"text\": \"Trace\" } ");
						sb.Append(" ] }");
						sb.Append(", ");					
						*/
						sb.Append("{ \"type\": \"summary\", \"items\": [ ");

						bool showGlobalIdentifier = true;
						if (showGlobalIdentifier)
						{
							sb.Append("{ \"type\": \"text\", \"text\": \"System ID\", \"value\": \"");
							sb.Append(inst.GlobalIdentifier.ToString("B"));
						}
						
						sb.Append("\" }");
						if (instPreviewFields.Length > 0)
						{
							sb.Append(", ");
						}
						for (int i = 0; i < instPreviewFields.Length; i++)
						{
							object value = oms.GetReportFieldEC(inst, instPreviewFields[i]);
							if (value == null)
							{
								sb.Append(" {\"type\":\"text\", \"text\": \"");
								sb.Append(oms.GetInstanceText(instPreviewFields[i]));
								sb.Append("\", \"value\": ");

								sb.Append("null");

								sb.Append(" } ");
							}
							else if (value is Mocha.Core.Instance)
							{
								sb.Append(" {\"type\":\"instance\", \"text\": \"");
								sb.Append(oms.GetInstanceText(instPreviewFields[i]));
								sb.Append("\", \"value\": \"" + oms.GetInstanceKey(value as Mocha.Core.Instance) + "\"");
								sb.Append(" } ");
							}
							else if (value is Mocha.Core.Instance[])
							{
								sb.Append(" {\"type\":\"instance\", \"text\": \"");
								sb.Append(oms.GetInstanceText(instPreviewFields[i]));

								Mocha.Core.Instance[] instanceKeys = (Mocha.Core.Instance[])value;

								string strInstanceKeys = "null";
								if (instanceKeys.Length > 0)
								{
									strInstanceKeys = String.Format("\"{0}\"", String.Join(",", oms.GetInstanceKeys(instanceKeys)));
								}

								sb.Append("\", \"value\": " + strInstanceKeys);
								sb.Append(" } ");
							}
							else
							{
								string valstr = value.ToString();
								Instance instFieldClass = oms.GetParentClass(instPreviewFields[i]);
								if (instFieldClass.GlobalIdentifier == KnownInstanceGuids.Classes.AttributeReportField)
								{
									Instance instAttribute = oms.GetRelatedInstance(instPreviewFields[i], KnownRelationshipGuids.Attribute_Report_Field__has_target__Attribute);
									Instance instAttributeClass = oms.GetParentClass(instAttribute);
									if (instAttributeClass.GlobalIdentifier == KnownInstanceGuids.Classes.BooleanAttribute)
									{
										bool valbool = Boolean.Parse(valstr);
										if (valbool)
										{
											valstr = "Yes";
										}
										else
										{
											valstr = "No";
										}
									}
								}

								sb.Append(" {\"type\":\"text\", \"text\": \"");
								sb.Append(oms.GetInstanceText(instPreviewFields[i]));
								sb.Append("\", \"value\": \"");

								sb.Append(valstr);

								sb.Append("\" } ");
							}
							if (i < instPreviewFields.Length - 1)
							{
								sb.Append(", ");
							}
						}
						sb.Append(" ] }");

						sb.Append(" ] }");

						sb.Append(" ]");

					sb.Append(" } }");
					Response.Write(sb.ToString());
					Response.End();
					break;
				}
			}
		}
	}
}
