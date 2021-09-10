<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarRubros.aspx.cs" Inherits="DesafioConfiteria.GestionarRubros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
		function toggleModal(title, btnConfirm) {
			var mdl = $('#modalModificarRubro');
			mdl.find('.modal-title').text(title);
			mdl.find('.modal-confirm-btn').text(btnConfirm);
			mdl.modal();
		}

		function closeModal(title, btnConfirm) {
			try {
				var mdl = $('#modalModificarRubro');
				mdl.find('.modal-title').text(title);
				mdl.find('.modal-confirm-btn').text(btnConfirm);
				mdl.modal();
				return true;
			} catch {
				return false;
			}
		}

        function swalWarningConfirm(btn, content) {
			if (btn.dataset.confirmed) {
				// Si la acción ya fue confirmada, proceder
				btn.dataset.confirmed = false;
				return true;
			} else {
				// Pedirle confirmación al usuario
				event.preventDefault();
				swal({
					text: content,
					icon: "warning",
					buttons: ["No", "Sí"],
					dangerMode: true
				})
					.then(function (response) {
						if (response) {
							// Confirmar que el próximo evento de click debe ser ignorado
							//en la parte de JS
							btn.dataset.confirmed = true;
							btn.click();
						}
					}).catch(function (reason) {
						// La acción fue cancelada
					});
			}
		}
	</script>

	<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
			<div class="row">
				<div class="col text-center align-self-center">
					<asp:GridView ID="gvRubros" runat="server" AutoGenerateColumns="false" OnRowCommand="GvRubros_RowCommand" CssClass="DBDisplay" OnRowDatabound="GvRubros_RowDatabound">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Identificador" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="FechaBaja" HeaderText="Fecha de baja" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                            <asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button Text="Modificar" runat="server" CommandName="Modificar" CssClass="btn btn-primary" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="toggleModal('Modificar rubro', 'Guardar')" />
                                    <asp:Button Text="Dar de baja" ID="btnGvBaja" runat="server" CommandName="Dar de baja" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea darle de baja al rubro?');" />
                                    <asp:Button Text="Reactivar" ID="btnGvReactivar" runat="server" CommandName="Reactivar" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea reactivar el rubro?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
				</div>
				<asp:Button ID="btnNuevoRubro" runat="server" Text="Crear nuevo rubro" OnClientClick="toggleModal('Crear nuevo rubro', 'Crear')" OnClick="BtnNuevoRubro_click" />
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

    
	<div class="modal fade" id="modalModificarRubro" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Modal title</h5>
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <div class="modal-body">
					<asp:UpdatePanel ID="upModal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Label ID="lblNombre" runat="server" Text="Nombre del rubro" CssClass="font-weight-bold"></asp:Label>
							<asp:TextBox ID="tbNombre" runat="server" placeholder="Nombre del rubro" CssClass="form-control"></asp:TextBox>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>

                <div class="modal-footer">
					<asp:UpdatePanel ID="upModalFooter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary modal-confirm-btn" OnClick="BtnGuardar_click" />
							<asp:Button ID="btnCrear" runat="server" Text="Crear" CssClass="btn btn-primary modal-confirm-btn" OnClick="BtnCrear_click" />
							<button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
		
</asp:Content>
