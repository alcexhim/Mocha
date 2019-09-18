<%@ Page Language="C#" Inherits="Mocha.Web.InstancePage" CodeBehind="Instance.aspx.cs" MasterPageFile="~/MasterPages/Default.master" %>
<%@ MasterType VirtualPath="~/MasterPages/Default.master" %>
<%@ Register TagPrefix="wcx" Namespace="MBS.Web.Controls" Assembly="MBS.Web" %>
<%@ Register TagPrefix="mcx" Namespace="Mocha.Web.Controls" Assembly="Mocha.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="aspcContent">
	<asp:Panel runat="server" ID="pnlDashboard">
		<div style="padding: 128px; text-align: center; display: none;">
			<i style="font-size: 96px;" class="fa fa-shield"></i>
			<h1>Your dashboard has nothing on it!</h1>
			<h2>You can fix that by <a href="#" onclick="document.getElementById('ctl00_ctl00_aspcContent_aspcContent_pnlDashboard').style.display ='none'; document.getElementById('ctl00_ctl00_aspcContent_aspcContent_pnlDashboardContent').style.display='block';">adding some content</a> or <a href="#">changing your default page</a></h2>
		</div>
	</asp:Panel>
	<asp:Panel runat="server" ID="pnlDashboardContent">
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
										<wcx:ListViewColumn Title="Date created" />
										<wcx:ListViewColumn Title="Assigned to" />
										<wcx:ListViewColumn Title="Reported by" GroupTitle="Issues" />
										<wcx:ListViewColumn Title="Description" GroupTitle="Issues" />
									</Columns>
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
</asp:Content>