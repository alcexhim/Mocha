<%@Page Inherits="Mocha.Web.LoginPage" MasterPageFile="~/MasterPages/Blank.master" %>
<%@MasterType VirtualPath="~/MasterPages/Blank.master" %>
<%@Register TagPrefix="wcx" Assembly="MBS.Web.Controls" Namespace="MBS.Web.Controls" %>

<asp:Content runat="server" ContentPlaceHolderID="aspcContent">
	<div style="margin-left: auto; margin-right: auto; margin-top: 50px; width: 600px;"> 
		<div style="text-align: center; margin-bottom: 50px;">
			<asp:Image runat="server" ImageUrl="~/Images/MainIcon.svg" />
		</div>
		<div style="text-align: center; margin-top: 32px; margin-bottom: 32px; font-size: 18pt; font-weight: 100;">
			Welcome to Your New Tenant
		</div>
		<wcx:Panel runat="server" CssClass="uwt-color-primary">
			<ContentControls>
				<h4>Please sign in to access this feature</h4>
				<wcx:FormView runat="server" CssClass="uwt-expand">
					<wcx:FormViewItemText runat="server" Name="user_name" Title="User name" />
					<wcx:FormViewItemPassword runat="server" Name="user_pass" Title="Password" />
				</wcx:FormView>
				<p>
					Once you're logged in, you can customize the 'Login Header Text' and 'Login Footer Text' properties of your new tenant.
				</p>
			</ContentControls>
			<FooterControls>
				<div style="float: left;">
					Don't have an account? <a href="#">Create one now</a>
				</div>
				<asp:Button runat="server" CssClass="uwt-color-primary" Text="Log In" />
			</FooterControls>
		</wcx:Panel>
		<p style="margin-top: 100px; text-align: center; font-size: 10pt; color: #a0a0a0">
			<asp:Label runat="server" ID="lblLegalNoticeText" />
		</p>
	</div>
</asp:Content>