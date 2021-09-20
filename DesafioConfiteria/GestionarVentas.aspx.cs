using BusinessLogicLayer;
using Entidades;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace DesafioConfiteria
{
	public partial class GestionarVentas : System.Web.UI.Page
	{
        private static int idLocal;
        private static Local local;
        private static Ticket current;
        private static InformeVentas currentInforme;
        private static List<Ticket> tickets;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                idLocal = Int32.Parse(Request.QueryString["IdLocal"]);
                local = LocalBLL.BuscarLocalPorId(idLocal);

                calendar.SelectedDate = DateTime.Today;
                upCalendar.Update();

                SetupGridview();
            }
        }

        private void SetupGridview()
		{
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[6] { 
                    new DataColumn("Index"), 
                    new DataColumn("FechaVenta"), 
                    new DataColumn("FechaBaja"), 
                    new DataColumn("Mozo"),
                    new DataColumn("PrecioTotal"),
                    new DataColumn("Comision") });

            tickets = TicketBLL.BuscarTicketsPorIdLocal(idLocal);

            foreach (Ticket ticket in tickets)
            {
                dt.Rows.Add(
                    ticket.Id, 
                    ticket.FechaVenta, 
                    ticket.FechaBaja?.ToString() ?? "—", 
                    ticket.Mozo.NombreCompleto(),
                    $"AR${ticket.SacarTotal():0.00}",
                    $"AR${ticket.SacarComision():0.00}");
            }

            gvTickets.DataSource = dt;
            gvTickets.DataBind();
        }

        private void SetupGvTicket()
		{
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[3] {
                    new DataColumn("Articulo"),
                    new DataColumn("Cantidad"),
                    new DataColumn("Precio") });

            foreach (Ticket.Detalle detalle in current.Detalles)
            {
                dt.Rows.Add(
                    detalle.Articulo.Nombre,
                    $"x{detalle.Cantidad:0.00}",
                    $"AR${detalle.Articulo.Precio:0.00}");
            }

            gvModalTicket.DataSource = dt;
            gvModalTicket.DataBind();
        }

        private void SetupGvResumenDiario(DateTime date)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[4] {
                    new DataColumn("ArticuloId"),
                    new DataColumn("Nombre"),
                    new DataColumn("Cantidad"),
                    new DataColumn("Precio") });

            List<Ticket> tickets = TicketBLL.BuscarTicketsActivosPorIdLocal(idLocal); 
            tickets =
                (from ticket in tickets
                 where ticket.FechaVenta.Date == date
                 select ticket).ToList();
            // TODO make SP

            // Resumir toda la información en un único ticket
            Ticket totalTicket = new Ticket();
            totalTicket.Detalles = new List<Ticket.Detalle>();
            totalTicket.FechaVenta = date;
            foreach (Ticket ticket in tickets)
			{
                foreach (Ticket.Detalle detalle in ticket.Detalles)
				{
                    Ticket.Detalle totalEntry = totalTicket.Detalles.Find(
                        d => d.Articulo.Id == detalle.Articulo.Id);

                    if (totalEntry == null)
					{
                        totalTicket.Detalles.Add(detalle);
					}
					else
					{
                        totalEntry.Cantidad += detalle.Cantidad;
					}
				}
			}

            // Llenar el grid view con los datos de dicho ticket
            foreach (Ticket.Detalle detalle in totalTicket.Detalles)
            {
                dt.Rows.Add(
                    detalle.Articulo.Id,
                    detalle.Articulo.Nombre,
                    $"{detalle.Cantidad:0.00}",
                    $"AR${detalle.Articulo.Precio:0.00}");
            }

            current = totalTicket;
            gvResumenDiario.DataSource = dt;
            gvResumenDiario.DataBind();
        }

        private void SetupGvMozoInforme(DateTime date)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[5] {
                    new DataColumn("Mozo"),
                    new DataColumn("Cantidad"),
                    new DataColumn("Importe"),
                    new DataColumn("Comision"),
                    new DataColumn("Total") });

            List<Ticket> tickets = TicketBLL.BuscarTicketsActivosPorIdLocal(idLocal);
            tickets =
                (from ticket in tickets
                 where ticket.FechaVenta.Date == date
                 select ticket).ToList();
            // TODO make SP

            // Resumir toda la información
            currentInforme = new InformeVentas(date);

            foreach (Ticket ticket in tickets)
            {
                foreach (Ticket.Detalle detalle in ticket.Detalles)
                {
                    currentInforme.RegistrarVenta(ticket.Mozo, detalle.Cantidad, detalle.Articulo.Precio);
                }
            }

            // Llenar el grid view con la información
            foreach (ResumenMozo resumen in currentInforme)
            {
                dt.Rows.Add(
                    $"{resumen.Mozo.Id} - {resumen.Mozo.NombreCompleto()}",
                    resumen.Cantidad.ToString(),
                    $"AR${resumen.Importe:0.00}",
                    $"{resumen.Mozo.Comision}%",
                    $"AR${resumen.SacarComision():0.00}");
            }

            gvMozoInforme.DataSource = dt;
            gvMozoInforme.DataBind();
        }

        private Ticket RowToTicket(GridViewRow row)
		{
            int id = Int32.Parse(row.Cells[0].Text);
            return tickets.Find(ticket => ticket.Id == id);
        }

        protected void Gv_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            // Cambiar el texto del botón si es que el rubro no está vigente
            bool estaVigente = e.Row.Cells[2].Text == "—";
            foreach (Control ctrl in e.Row.Cells[6].Controls)
			{
                if (estaVigente && ctrl.ID == "btnGvReactivar")
                {
                    ctrl.Visible = false;
                } 
                else if (!estaVigente && ctrl.ID == "btnGvBaja")
				{
                    ctrl.Visible = false;
				}
            }
        }

        protected void Gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            current = null;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTickets.Rows[rowIndex];
            Ticket ticket = RowToTicket(row);

            switch (e.CommandName)
            {
                case "ToPDF":
                    current = ticket;
                    SetupGvTicket();
                    break;

                case "Dar de baja":
                    ticket.FechaBaja = DateTime.Now;
                    if (!TicketBLL.ActualizarTicketFechaBaja(ticket))
					{
                        MessageBox.Show(
                            message: "No se pudo actualizar el ticket",
                            type: "error");
                    }
                    break;

                case "Reactivar":
                    ticket.FechaBaja = null;
                    if (!TicketBLL.ActualizarTicketFechaBaja(ticket))
					{
                        MessageBox.Show(
                            message: "No se pudo actualizar el ticket",
                            type: "error");
                    }
                    break;

                case "Eliminar":
                    if (!TicketBLL.EliminarTicketPorId(ticket.Id))
					{
                        MessageBox.Show(
                            message: "No se pudo eliminar el ticket",
                            type: "error");
                    }
                    break;

                default:
                    break;
			}

            SetupGridview();
            upMain.Update();
        }

        protected void GvModalTicket_OnDatabound(object sender, EventArgs e)
        {
            // Header
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            string text = $@"
                <center>{local.Nombre}</center><br/>
                <i>Ubicación</i>: {local.Direccion}<br/>
                <i>Email</i>: {local.Email}<br/>
                <i>Tel.</i>: {local.Telefono}<br/>
                <u>Venta del {current.FechaVenta}</u><br/>
            ";

            cell.Text = text;
            cell.ColumnSpan = 4;
            row.Controls.Add(cell);

            gvModalTicket.HeaderRow.Parent.Controls.AddAt(0, row);

            // Footer
            if (gvModalTicket.FooterRow != null)
			{
                gvModalTicket.ShowFooter = true;
                int index = gvModalTicket.Rows.Count;
                row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
                cell = new TableHeaderCell();
                text = $@"
                    <i>Cantidad total</i>: {current.CantidadTotal()}<br/>
                    <i>Mozo</i>: {current.Mozo.Id} - {current.Mozo.NombreCompleto()}<br/>
                    <i>Comisión</i>: AR${current.Mozo.Comision}<br/>
                    <u>Total: AR${current.SacarTotal()}</u>
                ";

                cell.Text = text;
                cell.ColumnSpan = 4;
                row.Controls.Add(cell);

                gvModalTicket.FooterRow.Parent.Controls.AddAt(index + 2, row);
            }

            upModalTicket.Update();
        }

        protected void GvResumenDiario_OnDatabound(object sender, EventArgs e)
        {
            // Header
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            string text = $@"
                <center>
                    {local.Nombre}<br/>
                    <u>Informe de ventas del {current.FechaVenta.ToShortDateString()}</u><br/>
                </center>
            ";

            cell.Text = text;
            cell.ColumnSpan = 4;
            row.Controls.Add(cell);

            gvResumenDiario.HeaderRow.Parent.Controls.AddAt(0, row);

            // Footer
            if (gvResumenDiario.FooterRow != null)
			{
                gvResumenDiario.ShowFooter = true;
                int index = gvResumenDiario.Rows.Count;
                row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
                cell = new TableHeaderCell();
                text = $@"</br>
                    <u>Total ventas del día: AR${current.SacarTotal()}</u>
                ";

                cell.Text = text;
                cell.ColumnSpan = 4;
                row.Controls.Add(cell);

                gvResumenDiario.FooterRow.Parent.Controls.AddAt(index + 2, row);
            }

            upModalResumenDiario.Update();
        }

        protected void GvMozoInforme_OnDatabound(object sender, EventArgs e)
        {
            // Header
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableHeaderCell cell = new TableHeaderCell();
            string text = $@"
                <center>
                    {local.Nombre}<br/>
                    Informe de ventas por mozo del día {currentInforme.Fecha.ToShortDateString()}
                </center><br/>
            ";

            cell.Text = text;
            cell.ColumnSpan = 5;
            row.Controls.Add(cell);

            gvMozoInforme.HeaderRow.Parent.Controls.AddAt(0, row);

            // Footer
            if (gvMozoInforme.FooterRow != null)
			{
                gvMozoInforme.ShowFooter = true;
                int index = gvMozoInforme.Rows.Count;
                row = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Normal);
                cell = new TableHeaderCell();
                text = $@"
                    <u>Total a pagar: AR${currentInforme?.SacarTotal() ?? 0:0.00}</u>
                ";

                cell.Text = text;
                cell.ColumnSpan = 5;
                row.Controls.Add(cell);

                gvMozoInforme.FooterRow.Parent.Controls.AddAt(index + 2, row);
            }
            
            upModalMozoInforme.Update();
        }

        protected void BtnImprimirTicket_click(object sender, EventArgs e)
		{
            MessageBox.Show( 
                message: "WIP",
                type: "error");
        }

        protected void BtnImprimirResumenDiario_click(object sender, EventArgs e)
        {
            MessageBox.Show(
                message: "WIP",
                type: "error");
        }

        protected void BtnImprimirMozoInforme_click(object sender, EventArgs e)
        {
            MessageBox.Show(
                message: "WIP",
                type: "error");
        }

        protected void BtnResumenDiario_click(object sender, EventArgs e)
		{
            DateTime date = calendar.SelectedDate.Date;
            SetupGvResumenDiario(date);
        }

        protected void BtnMozoInforme_click(object sender, EventArgs e)
        {
            DateTime date = calendar.SelectedDate.Date;
            SetupGvMozoInforme(date);
        }
	}
}