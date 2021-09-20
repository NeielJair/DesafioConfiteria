using BusinessLogicLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace DesafioConfiteria
{
	public partial class GenerarVenta : System.Web.UI.Page
	{
        private static int idLocal;
        private static Ticket ticket = new Ticket();
        private static List<Mozo> mozos = new List<Mozo>();
        private static List<Articulo> articulos = new List<Articulo>();

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                idLocal = Int32.Parse(Request.QueryString["IdLocal"]);

                ticket.IdLocal = idLocal;
                ticket.FechaBaja = null;
                ticket.Detalles = new List<Ticket.Detalle>();

                SetupGridview();
                SetupMozoDDL();
                SetupArticuloDDL();
                UpdateUI();
            }
        }

        private void SetupMozoDDL()
		{
            ddlMozo.Items.Clear();

            mozos = MozoBLL.BuscarMozosActivosPorIdLocal(idLocal);
            ddlMozo.Items.Add(new ListItem("Seleccione un mozo", "-1"));

            int i = 0;
            foreach (Mozo mozo in mozos)
            {
                ListItem item = new ListItem(mozo.NombreCompleto(), i.ToString());
                item.Attributes.Add("title", $"DNI: {mozo.Documento}");

                ddlMozo.Items.Add(item);
                i++;
            }
        }

        private void SetupArticuloDDL()
        {
            articulos = ArticuloBLL.BuscarArticulosActivosPorIdLocal(idLocal);

            ddlArticulo.Items.Clear();
            ddlArticulo.Items.Add(new ListItem("Seleccione un artículo", "-1"));

            int i = 0;
            foreach (Articulo articulo in articulos)
            {
                ListItem item = new ListItem(articulo.Nombre, i.ToString());
                item.Attributes.Add("title", articulo.Rubro.Nombre);

                ddlArticulo.Items.Add(item);
                i++;
            }
        }

        private void SetupGridview()
		{
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[5] { 
                    new DataColumn("Index"), 
                    new DataColumn("Cantidad"),
                    new DataColumn("Articulo"), 
                    new DataColumn("PrecioUnidad"),
                    new DataColumn("PrecioTotal")});

            int i = 0;
            foreach (Ticket.Detalle detalle in ticket.Detalles)
            {
                dt.Rows.Add(
                    i,
                    detalle.Cantidad, 
                    detalle.Articulo.Nombre, 
                    "$" + detalle.Articulo.Precio.ToString("0.00"),
                    "$" + (detalle.Articulo.Precio * detalle.Cantidad).ToString("0.00"));
                i++;
            }

            gvRubros.DataSource = dt;
            gvRubros.DataBind();
        }

        private void UpdateUI()
		{
            decimal total = 0;
            foreach (Ticket.Detalle detalle in ticket.Detalles)
			{
                total += detalle.Articulo.Precio * detalle.Cantidad;
			}
            lblTotal.Text = "Total a pagar: AR$" + total.ToString("0.00");

            lblComision.Text = "Comisión al mozo: $0.00";
            if (ticket.Mozo != null)
            {
                lblComision.Text = $"Comisión al mozo ({ticket.Mozo.Comision}%): AR$" + ((decimal)ticket.Mozo.Comision / 100m * total).ToString("0.00");
            }
        }

        private void SetupModal()
		{
            tbCantidad.Text = "1";
            ddlArticulo.SelectedIndex = 0;

            upModal.Update();
        }

        private Ticket.Detalle RowToDetalle(GridViewRow row)
		{
            return ticket.Detalles[Int32.Parse(row.Cells[0].Text)];
        }

        protected void GvRubros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvRubros.Rows[rowIndex];
            Ticket.Detalle detalle = RowToDetalle(row); //TODO ver si funciona así

            switch (e.CommandName)
            {
                case "Sumar":
                    detalle.Cantidad++;
                    break;

                case "Restar":
                    if (detalle.Cantidad <= 1)
                        goto cmdEliminar; // fallthrough

                    detalle.Cantidad--;
                    break;

                case "Eliminar":
                    cmdEliminar:
                    ticket.Detalles.Remove(detalle);
                    break;

                default:
                    break;
			}

            SetupGridview();
            UpdateUI();
            upMain.Update();
        }

        protected void DdlMozos_SelectedItemChanged(object sender, EventArgs e)
		{
            if (ddlMozo.SelectedIndex == 0)
			{
                ticket.Mozo = null;
			}
			else
			{
                ticket.Mozo = mozos[Int32.Parse(ddlMozo.SelectedValue)];
			}

            UpdateUI();
            upMain.Update();
        }

        protected void BtnCrear_click(object sender, EventArgs e)
        {
            Ticket.Detalle detalle = new Ticket.Detalle();

			try
			{
                detalle.Cantidad = Int32.Parse(tbCantidad.Text);
			}
			catch
			{
                MessageBox.Show(
                    title: "No se pudo añadir el artículo",
                    message: "La cantidad es inválida",
                    type: "error");
                return;
            }

            detalle.Articulo = articulos[Int32.Parse(ddlArticulo.SelectedValue)];

            ticket.Detalles.Add(detalle);

            SetupGridview();
            UpdateUI();
            upMain.Update();
        }

        protected void BtnAnadirArticulo_click(object sender, EventArgs e)
		{
            SetupModal();
        }

        protected void BtnGenerarTicket_click(object sender, EventArgs e)
        {
            ticket.FechaVenta = DateTime.Now;

            if (ticket.Detalles.Count == 0)
			{
                MessageBox.Show(
                    message: "Agregue artículos a la venta",
                    type: "error");
                return;
            }

            if (TicketBLL.CrearTicket(ticket))
			{
                MessageBox.ShowConfirmation(
                    title: "La venta fue generada exitosamente",
                    message: "¿Desea descargar el ticket? (WIP, no va a hacer nada)");
			}
			else
			{
                MessageBox.Show(
                    message: "No se pudo generar el ticket",
                    type: "error");
            }
        }
    }
}