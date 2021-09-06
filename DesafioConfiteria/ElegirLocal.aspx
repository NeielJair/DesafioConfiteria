<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ElegirLocal.aspx.cs" Inherits="DesafioConfiteria.ElegirLocal" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
		document.getElementById('masterNavbar').style.visibility = "hidden";
	</script>
	
	<asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-control dropdown">
	</asp:DropDownList>

</asp:Content>
