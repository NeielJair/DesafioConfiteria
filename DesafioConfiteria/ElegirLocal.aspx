<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ElegirLocal.aspx.cs" Inherits="DesafioConfiteria.ElegirLocal" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<script>
		document.getElementById('masterNavbar').style.visibility = "hidden";

		function Ingresar() {
			var ddl = document.getElementById("<%=ddlLocal.ClientID%>");
			var value = ddl.options[ddl.selectedIndex].value;

			if (value == -1) {
				swal({
					text: "Seleccione un local",
					icon: "error",
					button: "Ok",
				});

				return false;
			} else {
				return true;
			}
		}
	</script>

	<asp:UpdatePanel runat="server" UpdateMode="Conditional">
		<ContentTemplate>
			<div class="row h-100">
				<div class="col text-center align-self-center">
					<asp:DropDownList ID="ddlLocal" runat="server"></asp:DropDownList>
					<asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn btn-primary" OnClientClick="return Ingresar()" OnClick="BtnIngresar_click" />
				</div>
			</div>
		</ContentTemplate>
		<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnIngresar" EventName="Click" />
        </Triggers>
	</asp:UpdatePanel>

</asp:Content>
