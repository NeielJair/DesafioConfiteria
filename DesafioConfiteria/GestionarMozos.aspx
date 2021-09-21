<%@ Page Title="Gestionar mozos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarMozos.aspx.cs" Inherits="DesafioConfiteria.GestionarMozos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
			<asp:LinkButton ID="btnNuevoMozo" runat="server" CssClass="btn btn-success my-3" OnClientClick="showModal('modalModificar', 'Crear nuevo mozo', 'Crear')" OnClick="BtnNuevoMozo_click">
				<i class="bi-plus-lg" aria-hidden="true"></i>
				Crear nuevo mozo
			</asp:LinkButton>
			<div class="row">
				<div class="col-9 text-center align-self-center">
					<asp:GridView ID="gvMozos" runat="server" AutoGenerateColumns="false" OnRowCommand="GvMozos_RowCommand" CssClass="DBDisplay" OnRowDatabound="GvMozos_RowDatabound">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Identificador" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Documento" HeaderText="Número de documento" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="FechaContrato" HeaderText="Fecha de contratación" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="FechaBaja" HeaderText="Fecha de baja" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Comision" HeaderText="Comisión" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100%" ControlStyle-Width="100%">
                                <ItemTemplate>
									<div class="row">
										<div class="col">
											<asp:LinkButton runat="server" CommandName="Modificar" CssClass="btn btn-primary" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="showModal('modalModificar', 'Modificar datos del mozo', 'Guardar')">
												<i class="bi-pencil" aria-hidden="true"></i>
												Modificar
											</asp:LinkButton>
										</div>

										<div class="col">
											<asp:LinkButton ID="btnGvBaja" runat="server" CommandName="Dar de baja" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea darle de baja al mozo?');">
												<i class="bi-dash-circle-dotted" aria-hidden="true"></i>
												Dar de baja
											</asp:LinkButton>
											<asp:LinkButton ID="btnGvReactivar" runat="server" CommandName="Reactivar" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea reactivar el mozo?');">
												<i class="bi-plus-circle-dotted" aria-hidden="true"></i>
												Reactivar
											</asp:LinkButton>
										</div>

										<div class="col">
											<asp:LinkButton ID="btnGvEliminar" runat="server" CommandName="Eliminar" CssClass="btn btn-danger" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea ELIMINAR los datos del mozo?');">
												<i class="bi-trash" aria-hidden="true"></i>
												Eliminar
											</asp:LinkButton>
										</div>
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
							<asp:Label ID="lblDocumento" runat="server" Text="Número de documento" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbDocumento" runat="server" placeholder="Número de documento" type="number" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblNombre" runat="server" Text="Nombre" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbNombre" runat="server" placeholder="Nombre" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblApellido" runat="server" Text="Apellido" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbApellido" runat="server" placeholder="Apellido" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblComision" runat="server" Text="Comisión" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbComision" runat="server" placeholder="Comisión (entre 0 y 100)" CssClass="form-control"></asp:TextBox>
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

			var lblDocumento = $('#<%= lblDocumento.ClientID %>')[0];
			var tbDocumento = $('#<%= tbDocumento.ClientID %>')[0];

			var lblNombre = $('#<%= lblNombre.ClientID %>')[0];
			var tbNombre = $('#<%= tbNombre.ClientID %>')[0];

			var lblApellido = $('#<%= lblApellido.ClientID %>')[0];
			var tbApellido = $('#<%= tbApellido.ClientID %>')[0];

			var lblComision = $('#<%= lblComision.ClientID %>')[0];
			var tbComision = $('#<%= tbComision.ClientID %>')[0];

			lblDocumento.style.color = 'black';
			lblNombre.style.color = 'black';
			lblApellido.style.color = 'black';
			lblComision.style.color = 'black';


			if (tbDocumento.value === '' || tbDocumento.value < 0) {
				lblDocumento.style.color = 'red';
				valid = false;
			}
			if (tbNombre.value === '') {
				lblNombre.style.color = 'red';
				valid = false;
			}
			if (tbApellido.value === '') {
				lblApellido.style.color = 'red';
				valid = false;
			}
			regex = /^(\d*(\.|\,){1}\d+|\d*){1}/g;
			matched = tbComision.value.match(regex);
			if (tbComision.value === '' || isNaN(parseFloat(tbComision.value)) || parseFloat(tbComision.value) < 0 || parseFloat(tbComision.value) > 100 || matched == null || matched.length !== 1) {
				lblComision.style.color = 'red';
				valid = false;
			}

			if (valid) {
				closeModal('modalModificar');
			}

			return valid;
		}
	</script>
</asp:Content>
