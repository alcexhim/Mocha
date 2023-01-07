<%@ Page Language="C#" Inherits="Mocha.Web.UML" CodeBehind="UML.aspx.cs" MasterPageFile="~/MasterPages/Default.master" %>
<%@ MasterType VirtualPath="~/MasterPages/Default.master" %>
<%@ Register TagPrefix="wcx" Namespace="MBS.Web.Controls" Assembly="MBS.Web.Controls" %>
<%@ Register TagPrefix="mcx" Namespace="Mocha.Web.Controls" Assembly="Mocha.Web" %>
<asp:Content runat="server" ContentPlaceHolderID="aspcContent">
	
	<div class="uwt-uml-diagram">
		
		<div class="uwt-uml-class">
			
			<div class="uwt-title">Business</div>
			<div class="uwt-content">
				
				<div class="uwt-uml-attributes">
					<div class="uwt-title">Attributes</div>
					<div class="uwt-content">
						<ul>
							<li>
								<span class="uwt-uml-attribute-title"><a href="#">Name</a></span>
								<span class="uwt-uml-attribute-datatype"><a href="#">Text Attribute</a></span>
							</li>
							<li>
								<span class="uwt-uml-attribute-title"><a href="#">End Date</a></span>
								<span class="uwt-uml-attribute-datatype"><a href="#">Text Attribute</a></span>
							</li>
						</ul>
					</div>
				</div>
				
				<div class="uwt-uml-methods">
					<div class="uwt-title">Methods</div>
					<div class="uwt-content">
						<ul>
							<li>
								<span class="uwt-uml-attribute-title"><a href="#">get Inactive Businesses (SS)*P*S</a></span>
								<span class="uwt-uml-attribute-datatype"><a href="#">Select Instance Set Method</a></span>
							</li>
						</ul>
					</div>
				</div>
				
			</div>
			
		</div>
		
	</div>
	
</asp:Content>