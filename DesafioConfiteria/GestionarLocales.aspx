<%@ Page Title="Gestionar locales" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarLocales.aspx.cs" Inherits="DesafioConfiteria.GestionarLocales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script>
		document.getElementById('masterNavbar').style.visibility = "hidden";
	</script>

	<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
			<asp:LinkButton ID="btnNuevoRubro" runat="server" CssClass="btn btn-success my-3" OnClientClick="showModal('modalModificar', 'Crear nuevo local', 'Crear')" OnClick="BtnNuevoLocal_click">
				<i class="bi-plus-lg" aria-hidden="true"></i>
				Crear nuevo local
			</asp:LinkButton>

			<div class="row">
				<div class="col-9 text-center align-self-center">
					<asp:GridView ID="gvLocales" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" OnRowCommand="Gv_RowCommand" CssClass="DBDisplay">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Identificador" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Direccion" HeaderText="Dirección" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
									<div class="btn-group" role="group" aria-label="Basic example">
										<asp:LinkButton runat="server" CommandName="Modificar" CssClass="btn btn-primary" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="showModal('modalModificar', 'Modificar rubro', 'Guardar')">
											<i class="bi-pencil" aria-hidden="true"></i>
											Modificar
										</asp:LinkButton>
									</div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

	<div class="modal fade" id="modalModificar" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Modal title</h5>
                </div>

                <div class="modal-body">
					<asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Label ID="lblNombre" runat="server" Text="Nombre del local" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbNombre" runat="server" placeholder="Nombre del local" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblDireccion" runat="server" Text="Dirección del local" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbDireccion" runat="server" placeholder="Dirección del local" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblTelefono" runat="server" Text="Teléfono del local" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbTelefono" runat="server" placeholder="Teléfono del local" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblEmail" runat="server" Text="Email del local" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbEmail" runat="server" placeholder="Email del local" CssClass="form-control"></asp:TextBox>

							<asp:Label ID="lblContraseña" runat="server" Text="Contraseña" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbPassword" runat="server" CssClass="form-control" type="password"></asp:TextBox>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
							<asp:AsyncPostBackTrigger ControlID="btnCrear" EventName="Click" />
						</Triggers>
					</asp:UpdatePanel>
                </div>

                <div class="modal-footer">
					<asp:UpdatePanel ID="upModalFooter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary modal-confirm-btn" OnClientClick="return modalValidate();" OnClick="BtnGuardar_click" />
							<asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn btn-primary modal-confirm-btn" OnClientClick="return modalValidate();" OnClick="BtnCrear_click" />
							<button type="button" class="btn btn-secondary" onclick="closeModal('modalModificar');">Cerrar</button>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script>
		function modalValidate() {
			var valid = true;

			var lblNombre = $('#<%= lblNombre.ClientID %>')[0];
			var tbNombre = $('#<%= tbNombre.ClientID %>')[0];

			var lblDireccion = $('#<%= lblDireccion.ClientID %>')[0];
			var tbDireccion = $('#<%= tbDireccion.ClientID %>')[0];

			var lblTelefono = $('#<%= lblTelefono.ClientID %>')[0];
			var tbTelefono = $('#<%= tbTelefono.ClientID %>')[0];

			var lblEmail = $('#<%= lblEmail.ClientID %>')[0];
			var tbEmail = $('#<%= tbEmail.ClientID %>')[0];

			lblNombre.style.color = 'black';
			lblDireccion.style.color = 'black';
			lblTelefono.style.color = 'black';
			lblEmail.style.color = 'black';

			if (tbNombre.value === '') {
				lblNombre.style.color = 'red';
				valid = false;
			}
			if (tbDireccion.value === '') {
				lblDireccion.style.color = 'red';
				valid = false;
			}
			if (tbTelefono.value === '') {
				lblTelefono.style.color = 'red';
				valid = false;
			}
			if (tbEmail.value === '' || tbEmail.value.indexOf('@') <= 0) {
				lblEmail.style.color = 'red';
				valid = false;
			}

			if (valid) {
				closeModal('modalModificar');
			}

			return valid;
		}
	</script>
</asp:Content>
