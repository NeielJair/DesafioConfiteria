<%@ Page Title="Gestionar ventas y ver resumenes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GestionarVentas.aspx.cs" Inherits="DesafioConfiteria.GestionarVentas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script>
		function toggleCollapseResumenes() {
			$('#collapseResumenes').collapse('toggle');
		}

		function toggleCollapseTickets() {
			$('#collapseTickets').collapse('toggle');
		}
	</script>

	<asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
		<ContentTemplate>
			<div class="btn-group" role="group" aria-label="Basic example">
				<asp:LinkButton runat="server" CssClass="btn btn-primary my-3" OnClientClick="toggleCollapseResumenes();">
					<i class="bi-calendar-event" aria-hidden="true"></i>
					Ver resumenes por día
				</asp:LinkButton>

				<asp:LinkButton runat="server" CssClass="btn btn-secondary my-3" OnClientClick="toggleCollapseTickets();">
					<i class="bi-card-list" aria-hidden="true"></i>
					Ver lista de tickets
				</asp:LinkButton>
			</div>
			
			<div class="collapse mb-2" id="collapseResumenes">
				<div class="card card-body">
					<asp:UpdatePanel ID="upCalendar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
						<ContentTemplate>
							<asp:Calendar ID="calendar" runat="server"></asp:Calendar>
						</ContentTemplate>
					</asp:UpdatePanel>

					<div class="btn-group" role="group" aria-label="Basic example">
						<asp:LinkButton ID="btnResumenDiario" runat="server" CssClass="btn btn-success my-3" OnClientClick="showModal('modalResumenDiario')" OnClick="BtnResumenDiario_click">
							<i class="bi-receipt-cutoff" aria-hidden="true"></i>
							Ver resumen diario
						</asp:LinkButton>

						<asp:LinkButton ID="btnMozoInforme" runat="server" CssClass="btn btn-success my-3" OnClientClick="showModal('modalMozoInforme')" OnClick="BtnMozoInforme_click">
							<i class="bi-file-earmark-person" aria-hidden="true"></i>
							Ver resumen de ventas por mozo
						</asp:LinkButton>
					</div>
				</div>
			</div>

			<div class="collapse row" id="collapseTickets">
				<div class="col-9 text-center align-self-center card card-body">
					<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:GridView ID="gvTickets" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" OnRowCommand="Gv_RowCommand" CssClass="DBDisplay" OnRowDatabound="Gv_RowDatabound">
								<Columns>
									<asp:BoundField DataField="Index" HeaderText="Venta nro." ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="FechaVenta" HeaderText="Fecha de venta" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="FechaBaja" HeaderText="Fecha de baja" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Mozo" HeaderText="Mozo" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="PrecioTotal" HeaderText="Total" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Comision" HeaderText="Comisión" ItemStyle-HorizontalAlign="Center" />
									<asp:TemplateField HeaderText="Acciones" ItemStyle-HorizontalAlign="Center">
										<ItemTemplate>
											<div class="row">
												<div class="btn-group" role="group">
													<asp:LinkButton runat="server" CommandName="ToPDF" ToolTip="Imprimir a PDF" CssClass="btn btn-outline-danger" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="showModal('modalTicket')">
														<i class="bi-file-pdf" aria-hidden="true"></i>
													</asp:LinkButton>

													<asp:LinkButton ID="btnGvBaja" runat="server" CommandName="Dar de baja" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea darle de baja al ticket?');">
														<i class="bi-dash-circle-dotted" aria-hidden="true"></i>
														Dar de baja
													</asp:LinkButton>
													<asp:LinkButton ID="btnGvReactivar" runat="server" CommandName="Reactivar" CssClass="btn btn-warning" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea reactivar el ticket?');">
														<i class="bi-plus-circle-dotted" aria-hidden="true"></i>
														Reactivar
													</asp:LinkButton>

													<asp:LinkButton ID="btnGvEliminar" runat="server" CommandName="Eliminar" CssClass="btn btn-danger" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return swalWarningConfirm(this, '¿Está seguro que desea ELIMINAR el ticket?');">
														<i class="bi-trash" aria-hidden="true"></i>
														Eliminar
													</asp:LinkButton>
												</div>
											</div>
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

	<div class="modal fade" id="modalTicket" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Ver ticket</h5>
                </div>

                <div class="modal-body">
					<asp:UpdatePanel ID="upModalTicket" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:GridView ID="gvModalTicket" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" CssClass="DBDisplay" OnDataBound="GvModalTicket_OnDatabound" ShowHeaderWhenEmpty="true">
								<Columns>
									<asp:BoundField DataField="Articulo" HeaderText="Artículo" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Precio" HeaderText="Precio U." ItemStyle-HorizontalAlign="Center" />
								</Columns>
								<EmptyDataTemplate>No hay ningún artículo</EmptyDataTemplate>
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>

                <div class="modal-footer">
					<asp:UpdatePanel ID="upModalFooter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Button ID="btnImprimirTicket" runat="server" Text="Imprimir" CssClass="btn btn-primary modal-confirm-btn" OnClientClick="printModalTicket()" />
							<button type="button" class="btn btn-secondary" onclick="closeModal('modalTicket');">Cerrar</button>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

	<div class="modal fade" id="modalResumenDiario" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Ver resumen diario</h5>
                </div>

                <div class="modal-body">
					<asp:UpdatePanel ID="upModalResumenDiario" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:GridView ID="gvResumenDiario" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" CssClass="DBDisplay" OnDataBound="GvResumenDiario_OnDatabound" ShowHeaderWhenEmpty="true">
								<Columns>
									<asp:BoundField DataField="ArticuloId" HeaderText="Artículo" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Nombre" HeaderText="Descripción" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Precio" HeaderText="Importe" ItemStyle-HorizontalAlign="Center" />
								</Columns>
								<EmptyDataTemplate>No hay ninguna venta</EmptyDataTemplate>
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>

                <div class="modal-footer">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Button ID="btnImprimirResumenDiario" runat="server" Text="Imprimir" CssClass="btn btn-primary modal-confirm-btn" OnClientClick="printResumenDiario()" />
							<button type="button" class="btn btn-secondary" onclick="closeModal('modalResumenDiario');">Cerrar</button>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

	<div class="modal fade" id="modalMozoInforme" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Ver informe de ventas por mozo</h5>
                </div>

                <div class="modal-body">
					<asp:UpdatePanel ID="upModalMozoInforme" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:GridView ID="gvMozoInforme" runat="server" style='width:100%; overflow:auto;' AutoGenerateColumns="false" CssClass="DBDisplay" OnDataBound="GvMozoInforme_OnDatabound" ShowHeaderWhenEmpty="true" >
								<Columns>
									<asp:BoundField DataField="Mozo" HeaderText="Mozo" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Cantidad" HeaderText="Cant. Artículos" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Importe" HeaderText="Importe Total" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Comision" HeaderText="Comisión" ItemStyle-HorizontalAlign="Center" />
									<asp:BoundField DataField="Total" HeaderText="Total A Pagar" ItemStyle-HorizontalAlign="Center" />
								</Columns>
								<EmptyDataTemplate>No hay ninguna venta</EmptyDataTemplate>
							</asp:GridView>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>

                <div class="modal-footer">
					<asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
						<ContentTemplate>
							<asp:Button ID="btnImprimirMozoInforme" runat="server" Text="Imprimir" CssClass="btn btn-primary modal-confirm-btn" OnClientClick="printMozoInforme()" />
							<button type="button" class="btn btn-secondary" onclick="closeModal('modalMozoInforme');">Cerrar</button>
						</ContentTemplate>
					</asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

	<script>
		function printElement(elem, append, delimiter) {
			var domClone = elem.cloneNode(true);

			var printSection = document.getElementById("printSection");

			if (!printSection) {
				printSection = document.createElement("div");
				printSection.id = "printSection";
				document.body.appendChild(printSection);
			}

			if (append !== true) {
				printSection.innerHTML = "";
			} else if (append === true) {
				if (typeof (delimiter) === "string") {
					printSection.innerHTML += delimiter;
				} else if (typeof (delimiter) === "object") {
					printSection.appendChild(delimiter);
				}
			}

			printSection.className += " printable";
			printSection.appendChild(domClone);
			window.print();
		}

		function printModalTicket() {
			printElement(document.getElementById("<%= gvModalTicket.ClientID %>"), false);
		}

		function printResumenDiario() {
			printElement(document.getElementById("<%= gvResumenDiario.ClientID %>"), false);
		}

		function printMozoInforme() {
			printElement(document.getElementById("<%= gvMozoInforme.ClientID %>"), false);
		}

		window.addEventListener('afterprint', (event) => {
			document.getElementById("printSection").innerHTML = "";
		});
	</script>
</asp:Content>
