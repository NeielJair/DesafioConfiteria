<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="DesafioConfiteria.MainPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<h1 id="headerLocal" class="my-5" runat="server"></h1>

			<div class="d-grid gap-2">
					<asp:Button ID="btnArticulos" runat="server" Text="Gestionar artículos" CssClass="btn btn-primary" PostBackUrl="GestionarArticulos" />
					<asp:Button ID="btnMozos" runat="server" Text="Gestionar mozos" CssClass="btn btn-primary" PostBackUrl="GestionarMozos" />
					<asp:Button ID="btnRubros" runat="server" Text="Gestionar rubros" CssClass="btn btn-primary" PostBackUrl="GestionarRubros" />
					<asp:Button ID="btnTicket" runat="server" Text="Generar venta (crear ticket)" CssClass="btn btn-primary" PostBackUrl="GenerarVenta" />
					<asp:Button ID="btnVentas" runat="server" Text="Gestionar ventas y ver resumenes" CssClass="btn btn-primary" PostBackUrl="GestionarVentas" />
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
