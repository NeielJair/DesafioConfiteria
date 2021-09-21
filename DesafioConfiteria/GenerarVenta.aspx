<%@ Page Title="Generar venta" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerarVenta.aspx.cs" Inherits="DesafioConfiteria.GenerarVenta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
			<div class="row">
				<asp:Label ID="lblMozo" runat="server" Text="Mozo" CssClass="font-weight-bold my-2"></asp:Label>
				<div class="col-9 text-center align-self-center">
					<asp:DropDownList ID="ddlMozo" runat="server" CssClass="form-control mb-2" AutoPostBack="true" OnSelectedIndexChanged="DdlMozos_SelectedItemChanged"></asp:DropDownList>

					<asp:GridView ID="gvDetalles" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" OnRowCommand="GvDetalles_RowCommand" CssClass="DBDisplay" >
                        <Columns>
                            <asp:BoundField DataField="Index" HeaderText="Entrada nro." ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Articulo" HeaderText="Artículo" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PrecioUnidad" HeaderText="Precio (unidad)" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="PrecioTotal" HeaderText="Total" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
									<div class="row">
										<div class="btn-group" role="group">
											<asp:LinkButton runat="server" CommandName="Sumar" ToolTip="Sumar uno" CssClass="btn btn-primary" CommandArgument="<%# Container.DataItemIndex %>">
												<i class="bi-plus-lg" aria-hidden="true"></i>
											</asp:LinkButton>

											<asp:LinkButton runat="server" CommandName="Restar" ToolTip="Sacar uno" CssClass="btn btn-secondary" CommandArgument="<%# Container.DataItemIndex %>">
												<i class="bi-dash-lg" aria-hidden="true"></i>
											</asp:LinkButton>
											
											<asp:LinkButton ID="btnGvEliminar" runat="server" CommandName="Eliminar" CssClass="btn btn-danger" CommandArgument="<%# Container.DataItemIndex %>">
												<i class="bi-trash" aria-hidden="true"></i>
												Remover
											</asp:LinkButton>
										</div>
									</div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
				</div>
			</div>
			<div class="row">
				<asp:Label ID="lblTotal" runat="server" Text="Total a pagar: —"></asp:Label>
				<asp:Label ID="lblComision" runat="server" Text="Comisión al mozo: —"></asp:Label>

				<asp:LinkButton ID="btnAnadirArticulo" runat="server" CssClass="btn btn-success my-3" OnClientClick="showModal('modalAñadir', 'Crear nuevo artículo', 'Crear')" OnClick="BtnAnadirArticulo_click">
					<i class="bi-plus-lg" aria-hidden="true"></i>
					Añadir artículo
				</asp:LinkButton>

				<asp:LinkButton ID="btnGenerarTicket" runat="server" CssClass="btn btn-outline-success" OnClientClick="return formValidate(this);" OnClick="BtnGenerarTicket_click">
					<i class="bi-plus-lg" aria-hidden="true"></i>
					Generar ticket
				</asp:LinkButton>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

	<div class="modal fade" id="modalAñadir" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Modal title</h5>
                </div>

                <div class="modal-body">
					<asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Label ID="lblArticulo" runat="server" Text="Artículo" CssClass="font-weight-bold"></asp:Label>
							<asp:DropDownList ID="ddlArticulo" runat="server" CssClass="form-control mb-2"></asp:DropDownList>

							<asp:Label ID="lblCantidad" runat="server" Text="Cantidad" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbCantidad" runat="server" placeholder="Cantidad" Text="1" CssClass="form-control" type="number" min="1"></asp:TextBox>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnCrear" EventName="Click" />
						</Triggers>
					</asp:UpdatePanel>
                </div>

                <div class="modal-footer">
					<asp:UpdatePanel ID="upModalFooter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn btn-primary modal-confirm-btn" OnClientClick="return modalValidate();" OnClick="BtnCrear_click" />
							<button type="button" class="btn btn-secondary" onclick="closeModal('modalAñadir');">Cerrar</button>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <script>
		function modalValidate() {
			var valid = true;

			var lblArticulo = $('#<%= lblArticulo.ClientID %>')[0];
			var ddlArticulo = $('#<%= ddlArticulo.ClientID %>')[0];

			var lblCantidad = $('#<%= lblCantidad.ClientID %>')[0];
			var tbCantidad = $('#<%= tbCantidad.ClientID%>')[0];

			lblArticulo.style.color = 'black';
			lblCantidad.style.color = 'black';

			if (ddlArticulo.options[ddlArticulo.selectedIndex].value == -1) {
				lblArticulo.style.color = 'red';
				valid = false;
			}
			if (tbCantidad.value === '' || tbCantidad.value <= 0 || isNaN(parseInt(tbCantidad.value))) {
				lblCantidad.style.color = 'red';
				valid = false;
			}

			if (valid) {
				closeModal('modalAñadir');
			}

			return valid;
		}

		function formValidate(btn) {
			var lblMozo = $('#<%= lblMozo.ClientID %>')[0];
			var ddlMozo = $('#<%= ddlMozo.ClientID %>')[0];

			lblMozo.style.color = 'black';

			if (ddlMozo.options[ddlMozo.selectedIndex].value == -1) {
				lblMozo.style.color = 'red';
				return false;
			}

			return swalWarningConfirm(btn, '¿Está seguro que desea generar la venta?');
		}
	</script>
</asp:Content>
