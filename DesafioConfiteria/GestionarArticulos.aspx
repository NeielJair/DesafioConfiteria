<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarArticulos.aspx.cs" Inherits="DesafioConfiteria.GestionarArticulos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
			<asp:LinkButton ID="btnNuevoArticulo" runat="server" CssClass="btn btn-success my-3" OnClientClick="showModal('modalModificar', 'Crear nuevo artículo', 'Crear')" OnClick="BtnNuevoArticulo_click">
				<i class="bi-plus-lg" aria-hidden="true"></i>
				Crear nuevo artículo
			</asp:LinkButton>
			<div class="row">
				<div class="col-9 text-center align-self-center">
					<asp:GridView ID="gvRubros" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" OnRowCommand="GvRubros_RowCommand" CssClass="DBDisplay" OnRowDatabound="GvRubros_RowDatabound">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Identificador" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="FechaBaja" HeaderText="Fecha de baja" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Rubro" HeaderText="Rubro" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
									<div class="row">
										<div class="col">
											<asp:LinkButton runat="server" CommandName="Modificar" CssClass="btn btn-primary" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="showModal('modalModificar', 'Modificar artículo', 'Guardar')">
												<i class="bi-pencil" aria-hidden="true"></i>
												Modificar
											</asp:LinkButton>
										</div>

										<div class="col">
											<asp:LinkButton ID="btnGvBaja" runat="server" CommandName="Dar de baja" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea darle de baja al artículo?');">
												<i class="bi-dash-circle-dotted" aria-hidden="true"></i>
												Dar de baja
											</asp:LinkButton>
											<asp:LinkButton ID="btnGvReactivar" runat="server" CommandName="Reactivar" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea reactivar el artículo?');">
												<i class="bi-plus-circle-dotted" aria-hidden="true"></i>
												Reactivar
											</asp:LinkButton>
										</div>

										<div class="col">
											<asp:LinkButton ID="btnGvEliminar" runat="server" CommandName="Eliminar" CssClass="btn btn-danger" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea ELIMINAR el artículo?');">
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
							<asp:Label ID="lblNombre" runat="server" Text="Nombre del artículo" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbNombre" runat="server" placeholder="Nombre del artículo" CssClass="form-control mb-2"></asp:TextBox>

							<asp:Label ID="lblRubro" runat="server" Text="Rubro" CssClass="font-weight-bold"></asp:Label>
							<asp:DropDownList ID="ddlRubro" runat="server" CssClass="form-control"></asp:DropDownList>
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

			var lblRubro = $('#<%= lblRubro.ClientID %>')[0];
			var ddlRubro = $('#<%= ddlRubro.ClientID%>')[0];

			lblNombre.style.color = 'black';
			lblRubro.style.color = 'black';

			if (tbNombre.value === '') {
				lblNombre.style.color = 'red';
				valid = false;
			}

			if (ddlRubro.options[ddlRubro.selectedIndex].value == -1) {
				lblRubro.style.color = 'red';
				valid = false;
			}

			if (valid) {
				closeModal('modalModificar');
			}

			return valid;
		}
	</script>
</asp:Content>
