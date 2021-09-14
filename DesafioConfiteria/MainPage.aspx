<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="DesafioConfiteria.MainPage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<div class="row h-100">
				<div class="col text-center align-self-center">
					<asp:Button ID="btnArticulos" runat="server" Text="Gestionar artículos" CssClass="btn btn-primary" OnClientClick="Redirect('GestionarArticulos')" />
					<asp:Button ID="btnMozos" runat="server" Text="Gestionar mozos" CssClass="btn btn-primary" OnClientClick="Redirect('GestionarMozos')" />
				</div>
				<div class="col text-center align-self-center">
					<asp:Button ID="btnRubros" runat="server" Text="Gestionar rubros" CssClass="btn btn-primary" OnClientClick="Redirect('GestionarRubros')" />
					<asp:Button ID="btnTicket" runat="server" Text="Crear nuevo ticket" CssClass="btn btn-primary" OnClientClick="Redirect('CrearTicket')" />
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
