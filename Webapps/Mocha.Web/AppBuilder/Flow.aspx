<%@Page Inherits="Mocha.Web.AppBuilder.Flow" MasterPageFile="~/MasterPages/Default.master" %>
<%@MasterType VirtualPath="~/MasterPages/Default.master" %>
<%@Register TagPrefix="wcx" Namespace="MBS.Web.Controls" Assembly="MBS.Web.Controls" %>

<asp:Content runat="server" ContentPlaceHolderID="aspcContent">
	
	<div>
		Workflow Definition: Dashboard
		
		<wcx:Flowchart runat="server">
			
			<Items>
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Start" IconName="question-circle-o" Subtitle="Page Loaded" />
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-indigo" Title="Respond with Page" IconName="file-text-o" Subtitle="DashboardPage" />
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="End" IconName="question-circle-o" Subtitle="Flow Finished" />
				
			</Items>
			
		</wcx:Flowchart>
	</div>
	
	<div>
	
		Workflow Definition : Report Page [PG]
		
		<wcx:Flowchart runat="server">
			
			<Items>
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Start" IconName="question-circle-o" Subtitle="Page Loaded" />
				
				<wcx:FlowchartConditionalItem runat="server" CssClass="uwt-color-warning" Title="Branch on Conditions" IconName="code-fork" Subtitle="`Report.get Prompts` is not Empty">
					<TrueItems>
						<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Return" IconName="question-circle-o">
							<Content>
								<div>True</div>
							</Content>
						</wcx:FlowchartItem>
					</TrueItems>
					<FalseItems>
						<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Return" IconName="question-circle-o">
							<Content>
								<div>False</div>
							</Content>
						</wcx:FlowchartItem>
					</FalseItems>
				</wcx:FlowchartConditionalItem>
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Build Element" IconName="file-code-o" Subtitle="PageTestElement1 : Report@get Prompts ; Build with Instance sets from RSMBs : `Report@get Report.has Prompt (GR)`" />
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-indigo" Title="Respond with Element" IconName="list-alt" Subtitle="PageTestElement1" />
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-success" Title="Send HTTP Request" IconName="send" Visible="false" Subtitle="SendHTTPRequest" />
				
				<wcx:FlowchartConditionalItem runat="server" CssClass="uwt-color-warning" Title="Branch on Conditions" IconName="code-fork" Subtitle="BranchOnConditions">
					<TrueItems>
						<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Return" IconName="question-circle-o">
							<Content>
								<div>True</div>
							</Content>
						</wcx:FlowchartItem>
					</TrueItems>
					<FalseItems>
						<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="Return" IconName="question-circle-o">
							<Content>
								<div>False</div>
							</Content>
						</wcx:FlowchartItem>
					</FalseItems>
				</wcx:FlowchartConditionalItem>
				
				
				<wcx:FlowchartItem runat="server" CssClass="uwt-color-primary" Title="End" IconName="question-circle-o" Subtitle="Flow Finished" />
				
			</Items>
			
		</wcx:Flowchart>
	
	</div>
</asp:Content>