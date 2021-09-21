<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ElegirLocal.aspx.cs" Inherits="DesafioConfiteria.ElegirLocal" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<script>
		document.getElementById('masterNavbar').style.visibility = "hidden";

		function Ingresar() {
			var ddl = document.getElementById("<%= ddlLocal.ClientID %>");

			if (ddl.selectedIndex == 0) {
				swal({
					text: "Seleccione un local",
					icon: "error",
					button: "Ok",
				});

				return false;
			}
			return true;
		}
	</script>

	<asp:UpdatePanel runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<div class="row">
				<div class="col-5">
					<asp:Label runat="server" Text="Local" CssClass="font-weight-bold"></asp:Label>
					<asp:DropDownList ID="ddlLocal" runat="server" CssClass="form-control mb-2"></asp:DropDownList>

					<asp:Label runat="server" Text="Contraseña" CssClass="font-weight-bold"></asp:Label>
					<asp:TextBox ID="tbPassword" runat="server" CssClass="form-control mb-2" type="password"></asp:TextBox>

					<asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary" OnClientClick="return Ingresar()" OnClick="BtnIngresar_click" />

				</div>
			</div>
			<asp:Button ID="btnGestionar" runat="server" Text="Gestionar locales" CssClass="btn btn-primary mt-5" PostBackUrl="GestionarLocales" />
		</ContentTemplate>
		<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnIngresar" EventName="Click" />
        </Triggers>
	</asp:UpdatePanel>

</asp:Content>
