<%@ Page Language="C#" Inherits="Mocha.Web.InstancePage" CodeBehind="Instance.aspx.cs" MasterPageFile="~/MasterPages/Default.master" %>
<%@ MasterType VirtualPath="~/MasterPages/Default.master" %>
<%@ Register TagPrefix="wcx" Namespace="MBS.Web.Controls" Assembly="MBS.Web.Controls" %>
<%@ Register TagPrefix="mcx" Namespace="Mocha.Web.Controls" Assembly="Mocha.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="aspcContent">
	<wcx:Window runat="server" id="wndPageBuilderAddComponent" Title="Add PageBuilder Component">
		<ContentControls>
			<div class="uwt-columns">
				<div class="uwt-column-row">
					<div class="uwt-column uwt-column-3">
						<h2>Containers</h2>
						<ul class="uwt-menu">
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Box</span>
									<span class="uwt-description">Generic horizontal or vertical container</span>
								</a>
							</li>
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">ButtonGroup</span>
									<span class="uwt-description">Vertical collection of clickable buttons</span>
								</a>
							</li>
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Menu</span>
									<span class="uwt-description">Vertical collection of clickable menu items</span>
								</a>
							</li>
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">TabContainer</span>
									<span class="uwt-description">Multiple tab pages that are themselves containers</span>
								</a>
							</li>
						</ul>
					</div>
					<div class="uwt-column uwt-column-3">
						<h2>Text</h2>
						<ul class="uwt-menu">
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Header</span>
									<span class="uwt-description">Large header text blocks</span>
								</a>
							</li>
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Paragraph</span>
									<span class="uwt-description">Plain block of text</span>
								</a>
							</li>
						</ul>
					
					</div>
					<div class="uwt-column uwt-column-3">
						<h2>Data</h2>
						<ul class="uwt-menu">
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Summary</span>
									<span class="uwt-description">Multiple details in a form, one detail per line</span>
								</a>
							</li>
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Detail</span>
									<span class="uwt-description">Table of instances with columns for details</span>
								</a>
							</li>
							<li class="uwt-visible">
								<a href="#">
									<span class="uwt-title">Chart</span>
									<span class="uwt-description">Graphically visualize data with fancy animations and colors</span>
								</a>
							</li>
						</ul>
					</div>
				</div>
			</div>
		</ContentControls>
	</wcx:Window>
	<asp:Panel runat="server" ID="pnlPageBuilder" Style="display: none">
		<div class="mcx-pagebuilder-wrapper">
			<div class="mcx-pagebuilder-toolbar">
				<div class="mcx-pagebuilder-item-title">
					<div class="mcx-pagebuilder-item-instance-title">Main Page Heading</div>
					<div class="mcx-pagebuilder-item-instance-type">heading, level 2 ( text: &quot; My heading 1 &quot; , translated )</div>
					<div class="mcx-pagebuilder-item-controlbox">
						<a class="uwt-button" href="#"><i class="fa fa-gear"></i></a>
					</div>
				</div>
			</div>
			<div class="mcx-pagebuilder-item-content">
				<h1>My heading 1</h1>
			</div>
		</div>
		<div class="mcx-pagebuilder-placeholder">+</div>
		<div class="mcx-pagebuilder-wrapper">
			<div class="mcx-pagebuilder-toolbar">
				<div class="mcx-pagebuilder-item-title">
					<div class="mcx-pagebuilder-item-instance-title">&nbsp;</div>
					<div class="mcx-pagebuilder-item-instance-type">heading, level 2  ( text: &quot; Choose an app &quot; , translated )</div>
					<div class="mcx-pagebuilder-item-controlbox">
						<a class="uwt-button" href="#"><i class="fa fa-gear"></i></a>
					</div>
				</div>
			</div>
			<div class="mcx-pagebuilder-item-content">
				<h2>Choose an app</h2>
			</div>
		</div>
		<div class="mcx-pagebuilder-wrapper">
			<div class="mcx-pagebuilder-toolbar">
				<div class="mcx-pagebuilder-item-title">box, 2 items, horizontal</div>
			</div>
			<div class="mcx-pagebuilder-item-content">
				<div class="uwt-columns uwt-columns-2">
					<div class="uwt-column-row">
						<div class="uwt-column uwt-column-2">
							<div class="mcx-pagebuilder-placeholder">+</div>
						</div>
						<div class="uwt-column uwt-column-2">
							<div class="mcx-pagebuilder-placeholder">+</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlDashboard">
		<asp:Panel runat="server" ID="pnlDashboardEmpty">
			<div style="padding: 128px; text-align: center;">
				<i style="font-size: 96px;" class="fa fa-shield"></i>
				<h1>Your dashboard has nothing on it!</h1>
				<h2>You can fix that by <a href="#" onclick="PageBuilder.Begin();">adding some content</a> or <a href="#">changing your default page</a></h2>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="pnlDashboardContent">
			
		</asp:Panel>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlDashboardContentSample" Visible="false">
		<asp:Label runat="server" id="label" />
		<div class="uwt-columns">
			<div class="uwt-column-row">
				<div class="uwt-column uwt-column-4">
					<div class="uwt-tile uwt-color-toyo">
						<div class="uwt-header">Market Item Management</div>
						<div class="uwt-content">4 items <small>awaiting approval</small></div>
						<div class="uwt-footer">
							<a href="#">manage market items</a>
						</div>
					</div>
				</div>
				<div class="uwt-column uwt-column-4">
					<div class="uwt-tile uwt-color-success">
						<div class="uwt-header">Tenant Classes</div>
						<div class="uwt-content">4 classes <small>available on this tenant</small></div>
						<div class="uwt-footer">
							<a href="#">view classes</a>
							<a href="#">create a new class</a>
						</div>
					</div>
				</div>
				<div class="uwt-column uwt-column-4">
					<div class="uwt-tile uwt-color-orange">
						<div class="uwt-header">Market Item Management</div>
						<div class="uwt-content">4 items <small>awaiting approval</small></div>
						<div class="uwt-footer">
							<a href="#">manage market items</a>
						</div>
					</div>
				</div>
				<div class="uwt-column uwt-column-4">
					<div class="uwt-tile uwt-color-alizarin">
						<div class="uwt-header">Issue Tracking</div>
						<div class="uwt-content">7 issues <small>waiting a response since June</small></div>
						<div class="uwt-footer">
							<a href="#">manage market items</a>
						</div>
					</div>
				</div>
			</div>
			
			
			
			<div class="uwt-column-row">
				<div class="uwt-column uwt-column-1">
					<wcx:Panel runat="server" Title="My Applications">
						<ContentControls>
							<asp:Panel runat="server" id="pnlApplications">
								<wcx:ListView runat="server" CssClass="uwt-expand" Title="In Progress">
									<Columns>
										<wcx:ListViewColumn Title="Project" />
										<wcx:ListViewColumn Title="Due date" />
										<wcx:ListViewColumn Title="Assigned to" />
									</Columns>
								</wcx:ListView>
							</asp:Panel>
						</ContentControls>
					</wcx:Panel>
				</div>
			</div>
			
			
			<div class="uwt-column-row">
				<div class="uwt-column uwt-column-1">
					<wcx:Panel runat="server" Title="Open Trouble Tickets">
						<ContentControls>
							<asp:Panel runat="server" id="pnlTroubleTickets">
								<wcx:ListView runat="server" CssClass="uwt-expand" Title="Open">
									<Columns>
										<wcx:ListViewColumn ID="colDateCreated" Title="Date created" />
										<wcx:ListViewColumn ID="colAssignedTo" Title="Assigned to" />
										<wcx:ListViewColumn ID="colReportedBy" Title="Reported by" GroupTitle="Issues" />
										<wcx:ListViewColumn ID="colDescription" Title="Description" GroupTitle="Issues" />
									</Columns>
									<Items>
										<wcx:ListViewItem>
											<Columns>
												<wcx:ListViewItemColumn ColumnID="colDateCreated" Value="Today" />
												<mcx:ListViewItemColumnInstance ColumnID="colAssignedTo" Value="1$23" />
												<wcx:ListViewItemColumn ColumnID="colReportedBy" Value="beckermj / Michael J Becker (14773)" />
												<wcx:ListViewItemColumn ColumnID="colDescription" Value="the system doesnt save correctly" />
											</Columns>
										</wcx:ListViewItem>
									</Items>
								</wcx:ListView>
							</asp:Panel>
						</ContentControls>
					</wcx:Panel>
				</div>
			</div>
			
			<div class="uwt-column-row">
				<div class="uwt-column uwt-column-2">
					<wcx:Panel runat="server" Title="My Projects">
						<ContentControls>
							<asp:Panel runat="server" id="pnlObjects">
								<wcx:ListView runat="server" CssClass="uwt-expand" Title="In Progress">
									<Columns>
										<wcx:ListViewColumn Title="Project" />
										<wcx:ListViewColumn Title="Due date" />
										<wcx:ListViewColumn Title="Assigned to" />
									</Columns>
								</wcx:ListView>
							</asp:Panel>
						</ContentControls>
					</wcx:Panel>
				</div>
				<div class="uwt-column uwt-column-2">
					<mcx:Dashlet runat="server" Title="My Reports" InstanceID="75$3" />
				</div>
			</div>
		</div>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlTask" Visible="false">
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlElementContent" Visible="false">

		<div class="uwt-layout uwt-layout-box uwt-orientation-vertical">
			
			<div class="uwt-layout-item">
				<div class="uwt-toolbar" style="padding-bottom: 32px;">
					
					<wcx:Button runat="server" Text="Edit" />
					<wcx:Button runat="server" Text="Edit Element" />
					<wcx:Button runat="server" Text="View BEM" />
					<wcx:Button runat="server" Text="Edit BEM" />
					<wcx:Button runat="server" Text="Edit Work Data" />
				
				</div>
			</div>
			
			<div class="uwt-layout-item">
				
				<wcx:FormView runat="server" ID="fvElementContent" ReadOnly="true">
					<Items>
						<mcx:FormViewItemInstance Name="fvElementContent_ForElement" Title="For Element" ReadOnly="true" />
						<wcx:FormViewItemText Name="fvElementContent_Order" Title="Order" />
						<mcx:FormViewItemInstance Name="fvElementContent_DefaultDataType" Title="Default Data Type" ReadOnly="true" />
						<mcx:FormViewItemInstance Name="fvElementContent_DataTypeOverride" Title="Data Type Override" />
						<mcx:FormViewItemInstance Title="BEM Process" />
						<mcx:FormViewItemInstance Title="Default Label" />
						<mcx:FormViewItemInstance Title="Override Label" />
						<mcx:FormViewItemInstance Title="Uses Display Options" />
						<mcx:FormViewItemInstance Title="Edit Context" />
					</Items>
				</wcx:FormView>
				
				<h2>Derived EC</h2>
				<wcx:ListView runat="server" Title="Parameters from Context">
					<Columns>
						<wcx:ListViewColumn Title="Assigns to Parm" />
						<%-- Derived EC Parameter.on context Derived EC Context Type --%>
						<wcx:ListViewColumn Title="Context Type" />
						<wcx:ListViewColumn Title="Specific Work Data" />
						<wcx:ListViewColumn Title="On Multiple Selection" />
					</Columns>
				</wcx:ListView>
				<wcx:ListView runat="server" Title="Derived Value">
					<Columns>
						<wcx:ListViewColumn Title="True Conditions" GroupTitle="Update Condition" />
						<wcx:ListViewColumn Title="False Conditions" GroupTitle="Update Condition" />
						<wcx:ListViewColumn Title="Use Any Condition" GroupTitle="Update Condition" />
						
						<wcx:ListViewColumn Title="Executable returning Work Data" />
						
						<wcx:ListViewColumn Title="on Construct" GroupTitle="Do not Update" />
						<wcx:ListViewColumn Title="on UI Change" GroupTitle="Do not Update" />
						<wcx:ListViewColumn Title="on Submit" GroupTitle="Do not Update" />
						
						<wcx:ListViewColumn Title="Default Value" />
					</Columns>
				</wcx:ListView>
				<wcx:ListView runat="server" ID="lvElementContentDerivedValidations" Title="Derived Validations (valid if true)">
					<Columns>
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsValidation" Title="Validation" />
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsValidationClassification" Title="Validation Classification" />
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsTrueConditions" Title="True Conditions" GroupTitle="Condition to Evaluate" />
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsFalseConditions" Title="False Conditions" GroupTitle="Condition to Evaluate" />
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsUseAnyCondition" Title="Use Any Condition" GroupTitle="Condition to Evaluate" />
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsOnlyOnSubmit" Title="Only on Submit" />
						<wcx:ListViewColumn ID="lvcElementContentDerivedValidationsErrorWordBucket" Title="Error Word Bucket" />
					</Columns>
				</wcx:ListView>
			</div>
		</div>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlClass" Visible="false">
		<h1 style="color: #ff0000; font-weight: bold; font-size: 18pt;">~Non-PageBuilder Page~</h1>
		
		<wcx:TabContainer runat="server">
			<TabPages>
				<wcx:TabPage runat="server" Title="Definition View">
					
					<wcx:TabContainer runat="server">
						<TabPages>
							
							<wcx:TabPage runat="server" Title="Structure">
								
								<div class="uwt-layout uwt-layout-box uwt-orientation-vertical">
									
									<div class="uwt-layout-item">
										<div class="uwt-toolbar" style="padding-bottom: 32px;">
											
											<wcx:Button runat="server" Text="Edit" />
											<wcx:Button runat="server" Text="Add Attribute" />
											<wcx:Button runat="server" Text="Add Relationship" />
											<wcx:Button runat="server" Text="View CCP" />
											<wcx:Button runat="server" Text="Edit CCP" />
											<wcx:Button runat="server" Text="Edit Class as Instance Set" />
											<wcx:Button runat="server" Text="Visualize UML" />
										
										</div>
									</div>
									<div class="uwt-layout-item">
										
										<div class="uwt-layout uwt-layout-box uwt-orientation-horizontal">
											<div class="uwt-layout-item">
												
												<wcx:FormView runat="server" id="fvStructure" ReadOnly="true">
													<Items>
														<wcx:FormViewItemText Name="fvStructure_Name" Title="Name" />
														<mcx:FormViewItemInstance Name="fvStructure_Module" Title="Module" />
														<wcx:FormViewItemText Name="fvStructure_AllowModuleAssigned" Title="Allow Module Assigned" Value="No" />
														<mcx:FormViewItemInstance Name="fvStructure_Access" Title="Access" />
														<mcx:FormViewItemInstance Name="fvStructure_ClassSpecification" Title="Class Specification" />
														<wcx:FormViewItemText Name="fvStructure_GlobalIdentifier" Title="Global Identifier" />
													</Items>
												</wcx:FormView>
												
											</div>
											
											<div class="uwt-layout-item">
										
												<wcx:FormView runat="server" id="fvInheritance" ReadOnly="true">
													<Items>
														<mcx:FormViewItemInstance Name="fvInheritance_Superclasses" Title="Superclasses" />
														<mcx:FormViewItemInstance Name="fvInheritance_Subclasses" Title="Subclasses" />
													</Items>
												</wcx:FormView>
												
											</div>
											
											<div class="uwt-layout-item">
												
										
												<wcx:FormView runat="server" id="fvD3" ReadOnly="true">
													<Items>
														<wcx:FormViewItemText Name="fvD3_EffectiveDataManager" Title="Effective Data Manager" />
														<wcx:FormViewItemText Name="fvD3_ActiveCacheStrategy" Title="Active Cache Strategy" />
														<wcx:FormViewItemText Name="fvD3_Indexes" Title="Indexes" />
														<wcx:FormViewItemText Name="fvD3_QueryDefinition" Title="Query Definition" />
														<wcx:FormViewItemText Name="fvD3_Comment" Title="Comment" />
														<wcx:FormViewItemText Name="fvD3_Documentation" Title="Documentation" />
														<wcx:FormViewItemText Name="fvD3_SPressInterop" Title="S-Press Interop Enabled" />
														<wcx:FormViewItemText Name="fvD3_SPressComponent" Title="S-Press Component" />
													</Items>
												</wcx:FormView>
												
												
											</div>
										</div>
									</div>
									
									<div class="uwt-layout-item">
										
												
										<h2>Attributes</h2>
										<wcx:ListView runat="server" id="lvAttributes" Width="100%">
											
											<Columns>
												<wcx:ListViewColumn ID="lvcAttribute" Title="Attribute" />
												<wcx:ListViewColumn ID="lvcAttributeType" Title="Attribute Type" />
											</Columns>
											
										</wcx:ListView>
									</div>
									
									<div class="uwt-layout-item">
										
										<h2>Relationships</h2>
										<wcx:ListView runat="server" id="lvRelationships" Width="100%">
											
											<Columns>
												<wcx:ListViewColumn ID="lvcRelationship" Title="Relationship" />
												<wcx:ListViewColumn ID="lvcInstances" Title="Instances" />
											</Columns>
											
										</wcx:ListView>
									</div>
									
								</div>
								
							</wcx:TabPage>
							
							<wcx:TabPage Title="Instances">
										
								<h2>Instances</h2>
								
								<!--
									TODO: convert to a McxListView with the following properties:
								
								For Report:
								Build fields from RSMB: Report@get Report Column
								
								
								Build rows from RSMB: Class@get Instances for Class parm (GRS)[rsmb]
									(Get Referenced Instance Set)
									Use Relationship: Class@get Class.has Instance(GR)*P*S[rsmb]
								
									** This method should return all Instances of the specified Class.
								
								Build fields from RSMB: Report@generate Report Field from Relationship parm (SS)*P*S [rsmb]
									(Select from Instance Set)
									Loop on Instance Set: Class@get Attributes for Class parm (GRS)[rsmb]
										(Get Referenced Instance Set)
										Use Relationship: Class@get Class.has Attribute(GR)*P*S[rsmb]
								
									Create Instance of Class: Report Field
									Assign Relationship:
										Report Field.has source Method
											Attribute@get Value
										Report Field.source instance
											(This Instance)
								-->
								<wcx:ListView runat="server" id="lvInstances" Width="100%">
									
									<Columns>
										<wcx:ListViewColumn ID="lvcInstance" Title="Instance" />
									</Columns>
									
								</wcx:ListView>
							</wcx:TabPage>
							
							<wcx:TabPage Title="Instance Set Methods">
								
							</wcx:TabPage>
							
							<wcx:TabPage Title="Attribute Methods">
								
							</wcx:TabPage>
							
							<wcx:TabPage Title="Element Methods">
								
							</wcx:TabPage>
							
							<wcx:TabPage Title="Unit Tests">
								
							</wcx:TabPage>
							
							<wcx:TabPage Title="Processing">
								
							</wcx:TabPage>
						</TabPages>
					</wcx:TabContainer>
				
				</wcx:TabPage>
							
				<wcx:TabPage Title="Public View">
					
				</wcx:TabPage>
							
				<wcx:TabPage Title="Tasks">
					
				</wcx:TabPage>
							
				<wcx:TabPage Title="Frameworks">
					
				</wcx:TabPage>
							
				<wcx:TabPage Title="Fields">
					
				</wcx:TabPage>
							
				<wcx:TabPage Title="Utilities">
					
				</wcx:TabPage>
							
				<wcx:TabPage Title="Exceptions">
					
				</wcx:TabPage>
							
				<wcx:TabPage Title="Metrics">
					
				</wcx:TabPage>
			</TabPages>
		</wcx:TabContainer>
		
			
	</asp:Panel>
	
	
	
	<asp:Panel runat="server" ID="pnlMethod" Visible="false">
		<h1 style="color: #ff0000; font-weight: bold; font-size: 18pt;">~Non-PageBuilder Page~</h1>						
								
		<div class="uwt-layout uwt-layout-box uwt-orientation-vertical">
			
			<div class="uwt-layout-item">
				<div class="uwt-toolbar" style="padding-bottom: 32px;">
					
					<wcx:Button runat="server" Text="Edit" />
					<wcx:Button runat="server" Text="Create New MB" />
					<wcx:Button runat="server" Text="Visualize UML" />
				
				</div>
			</div>
	
			<div class="uwt-layout-item">
				
				<h2>Documentation</h2>
				
				<wcx:FormView ID="fvMethodDocumentation" runat="server" ReadOnly="true">
					<Items>
						<mcx:FormViewItemInstance Name="fviDescription" Title="Description" />
					</Items>
				</wcx:FormView>
				<wcx:ListView runat="server" ID="lvMethodParameters" Title="Parameters">
					<Columns>
						<wcx:ListViewColumn runat="server" ID="colMethodParmParm" Title="Parm" />
						<wcx:ListViewColumn runat="server" ID="colMethodParmRequired" Title="Required" />
						<wcx:ListViewColumn runat="server" ID="colMethodParmParmType" Title="Parm Type" />
						<wcx:ListViewColumn runat="server" ID="colMethodParmDescription" Title="Description" />
					</Columns>
				</wcx:ListView>
				
			</div>
				
			<div class="uwt-layout-item">
				
				<div class="uwt-layout uwt-layout-box uwt-orientation-horizontal">
					
					<div class="uwt-layout-item">
						
						<h2>Definition</h2>
						
						<wcx:FormView ID="fvMethodDefinition" runat="server" ReadOnly="true">
							<Items>
								<mcx:FormViewItemInstance Name="fviForClass" Title="For Class" />
								<wcx:FormViewItemText Name="fviVerb" Title="Verb" />
								<wcx:FormViewItemText Name="fviName" Title="Name" />
								<mcx:FormViewItemInstance Name="fviModule" Title="Module" />
								<mcx:FormViewItemInstance Name="fviAccess" Title="Access" />
								<wcx:FormViewItemBoolean Name="fviStatic" Title="Static" />
							</Items>
						</wcx:FormView>
						
					</div>
					<div class="uwt-layout-item">
						
						<h2>Parameters</h2>
						
						<wcx:FormView runat="server" ID="fvMethodParms" ReadOnly="true">
							<Items>
								<mcx:FormViewItemInstance Name="fviOptionalParms" Title="Optional parms" />
								<mcx:FormViewItemInstance Name="fviRequiredParmsNullAllowed" Title="Required parms (null allowed)" />
								<mcx:FormViewItemInstance Name="fviRequiredParmsNotNull" Title="Required parms (not null)" />
							</Items>
						</wcx:FormView>
						
						<h2>Return</h2>
						
						<wcx:FormView runat="server" ID="fvMethodReturn" ReadOnly="true">
							<Items>
								<mcx:FormViewItemInstance Name="fviReturnAttribute" Title="Attribute" />
							</Items>
						</wcx:FormView>
						
					</div>
					<div class="uwt-layout-item">
						
						<h2>Details</h2>
						
						<wcx:FormView runat="server" ID="fvMethodDetails" ReadOnly="true">
							<Items>
								<mcx:FormViewItemInstance Name="fviComment" Title="Comment" />
								<mcx:FormViewItemInstance Name="fviTags" Title="Tags" />
								<mcx:FormViewItemInstance Name="fviDocumentationLink" Title="Documentation" />
							</Items>
						</wcx:FormView>
						
					</div>
					
				</div>
				
			</div>
			
			<div class="uwt-layout-item">
				
				<h2>Method Implementation</h2>
				
				<asp:Panel runat="server" ID="pnlMethodImplementationGRA" Visible="false">
					
					<wcx:FormView runat="server">
						
						<Items>
							
							<mcx:FormViewItemInstance Name="fviGRAImplementationLoopOnInstanceSet" Title="Loop on Instance Set" />
							<mcx:FormViewItemInstance Name="fviGRAImplementationGetAttribute" Title="Get Attribute" />
							<mcx:FormViewItemInstance Name="fviGRAImplementationUseAccumulationFunction" Title="Use Accumulation Function" />
							
						</Items>
						
					</wcx:FormView>
				</asp:Panel>
				
				<asp:Panel runat="server" id="pnlMethodImplementationSAC" Visible="false">
					
					<wcx:ListView runat="server" ID="lvCases" Title="Cases">
						
						<Columns>
							<wcx:ListViewColumn Title="Case" />
							<wcx:ListViewColumn Title="True Conditions" />
							<wcx:ListViewColumn Title="False Conditions" />
							<wcx:ListViewColumn Title="Use Any Condition" />
							<wcx:ListViewColumn Title="Executable returning Attribute" />
						</Columns>
						
					</wcx:ListView>
					
				</asp:Panel>
				
			</div>
			
		</div>
	</asp:Panel>
	
	<asp:Panel runat="server" id="pnlMethodBinding" Visible="false">
		<!-- THE FUCK I TELL YOU STOP HARDCODING SHIT -->
		<h1 style="color: #ff0000; font-weight: bold; font-size: 18pt;">~Non-PageBuilder Page~</h1>						
		
		<div class="uwt-layout uwt-layout-box uwt-orientation-vertical">
			
			<div class="uwt-layout-item">
				<div class="uwt-toolbar" style="padding-bottom: 32px;">
					
					<wcx:Button runat="server" Text="Edit" />
					<wcx:Button runat="server" Text="Edit Method" />
					<wcx:Button runat="server" Text="Visualize" />
				
				</div>
			</div>
			
			<div class="uwt-layout-item">
				<div class="uwt-toolbar" style="padding-bottom: 32px;">
					
					<wcx:Button runat="server" Text="Clone" />
					<wcx:Button runat="server" Text="Test" />
					<wcx:Button runat="server" Text="Toggle" />
					<wcx:Button runat="server" Text="Trace" />
				
				</div>
			</div>
			
			<div class="uwt-layout-item">
				<wcx:FormView runat="server">
					<wcx:FormViewItemText runat="server" Title="Documentation" />
				</wcx:FormView>
			</div>
			
			<div class="uwt-layout-item">
				<h2>Method Binding Definition</h2>
				<wcx:FormView runat="server" ReadOnly="true" ID="fvMethodBindingDefinition">
					<mcx:FormViewItemInstance runat="server" Name="fviMethodBindingExecutesMethod" Title="Executes Method" />
					<mcx:FormViewItemInstance runat="server" Title="Parms used by Method Binding" />
				</wcx:FormView>
				
				<wcx:ListView runat="server" ID="lvMethodBindingParameters">
					<Columns>
						<wcx:ListViewColumn runat="server" Title="" />
						<wcx:ListViewColumn runat="server" ID="colMethodBindingParmTrueConditions" Title="True Conditions" GroupTitle="Condition to Evaluate" />
						<wcx:ListViewColumn runat="server" ID="colMethodBindingParmFalseConditions" Title="False Conditions" GroupTitle="Condition to Evaluate" />
						<wcx:ListViewColumn runat="server" ID="colMethodBindingParmUseAnyCondition" Title="Use Any Condition" GroupTitle="Condition to Evaluate" />
						<wcx:ListViewColumn runat="server" ID="colMethodBindingParmAssignsTo" Title="Assigns to Parm" GroupTitle="Parameter Assignments" />
						<wcx:ListViewColumn runat="server" ID="colMethodBindingParmAssignsFrom" Title="Assigns from RWMB" GroupTitle="Parameter Assignments" />
						<wcx:ListViewColumn runat="server" Title="" />
					</Columns>
				</wcx:ListView>
			</div>
		</div>
	</asp:Panel>
	
	<asp:Panel runat="server" ID="pnlTestMethodBinding" Visible="false">
		<!-- eww, I hate this hardcoded shit, make it stop -->
		<h1 style="color: #ff0000; font-weight: bold; font-size: 18pt;">~Non-PageBuilder Page~</h1>						
		
		<h1>Test Method Binding</h1>
		<h2><mcx:InstanceBrowser runat="server" ID="ibTestMethodBinding" Editable="false" /></h2>
		
		<asp:Panel runat="server" id="pnlTestMethodBindingReturned" Visible="false">
			<wcx:FormView runat="server" ID="fvTestMethodBinding" ReadOnly="true">
				<Items>
					<wcx:FormViewItemText Name="fviTestMethodBindingID" runat="server" Title="Global Identifier" />
					<mcx:FormViewItemInstance Name="fviTestMethodBindingReturnType" runat="server" Title="Return Type" />
				</Items>
			</wcx:FormView>
		</asp:Panel>
		
		<h2>Test Parms</h2>
		<wcx:FormView runat="server" ID="fvTestMethodBindingParameters">
			<Items>
			</Items>
		</wcx:FormView>
		
		
		<h2>Test Context</h2>
	</asp:Panel>
</asp:Content>
<asp:Content runat="server" id="aspcPageFooter" ContentPlaceHolderID="aspcPageFooter">
	<asp:Panel runat="server" ID="pnlPageFooterButtons">
		<asp:Button runat="server" ID="cmdPageFooterOK" CssClass="uwt-button uwt-color-primary" Text="OK" />
		<a id="cmdPageFooterCancel" class="uwt-button" href="#">Cancel</a>
	</asp:Panel>
</asp:Content>