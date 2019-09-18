<%@ Page Language="C#" Inherits="Mocha.Web.ShowRelationshipsPage" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ShowRelationships.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/Default.master" %>
<%@ Register TagPrefix="wcx" Namespace="MBS.Web.Controls" Assembly="MBS.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="aspcContent">
	<wcx:ListView runat="server" ID="lv">
		<Columns>
			<wcx:ListViewColumn ID="lvcInstanceID" Title="Instance ID" GroupTitle="Instance" />
		</Columns>
	</wcx:ListView>
</asp:Content>