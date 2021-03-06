using BusinessLogicLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace DesafioConfiteria
{
	public partial class GestionarMozos : System.Web.UI.Page
	{
        private static int idLocal;
        private static Mozo current;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                try
                {
                    idLocal = Session["IdLocal"] as int? ?? throw new ArgumentNullException();
                }
                catch
                {
                    Response.Redirect("ElegirLocal");
                }

                SetupGridview();
            }
        }

        private void SetupGridview()
		{
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[6] { 
                    new DataColumn("Id"), 
                    new DataColumn("Documento"), 
                    new DataColumn("Nombre"),
                    new DataColumn("FechaContrato"),
                    new DataColumn("FechaBaja"),
                    new DataColumn("Comision")});

            List<Mozo> mozos = MozoBLL.BuscarMozosPorIdLocal(idLocal);

            foreach (Mozo mozo in mozos)
            {
                dt.Rows.Add(
                    mozo.Id, 
                    mozo.Documento, 
                    $"{mozo.Nombre} {mozo.Apellido}",
                    mozo.FechaContrato.ToString(),
                    mozo.FechaBaja?.ToString() ?? "—",
                    $"{mozo.Comision}%");
            }

            gvMozos.DataSource = dt;
            gvMozos.DataBind();
        }

        private void SetupModal(Mozo mozo)
		{
            bool isCreate = mozo == null;

            btnGuardar.Visible = !isCreate;
            btnCrear.Visible = isCreate;
            upModalFooter.Update();

            tbDocumento.Text = (!isCreate) ? mozo.Documento.ToString() : "";
            tbNombre.Text = (!isCreate) ? mozo.Nombre : "";
            tbApellido.Text = (!isCreate) ? mozo.Apellido : "";
            tbComision.Text = (!isCreate) ? mozo.Comision.ToString() : "";

            current = mozo;
            upModal.Update();
        }

        private Mozo RowToMozo(GridViewRow row)
		{
            return MozoBLL.BuscarMozoPorId(Int32.Parse(row.Cells[0].Text));
        }

        protected void GvMozos_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            // Cambiar el texto del botón si es que el rubro no está vigente
            bool estaVigente = e.Row.Cells[4].Text == "—";
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

        protected void GvMozos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            current = null;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvMozos.Rows[rowIndex];
            Mozo mozo = RowToMozo(row);

            switch (e.CommandName)
            {
                case "Modificar":
                    SetupModal(mozo);
                    break;

                case "Dar de baja":
                    mozo.FechaBaja = DateTime.Now;
                    if (!MozoBLL.ActualizarMozo(mozo))
					{
                        MessageBox.Show(
                            message: "No se pudieron actualizar los datos del mozo",
                            type: "error");
                    }
                    break;

                case "Reactivar":
                    mozo.FechaBaja = null;
                    if (!MozoBLL.ActualizarMozo(mozo))
					{
                        MessageBox.Show(
                            message: "No se pudieron actualizar los datos del mozo",
                            type: "error");
                    }
                    break;

                case "Eliminar":
                    if (!MozoBLL.EliminarMozoPorId(mozo.Id))
					{
                        MessageBox.Show(
                            message: "No se pudieron eliminar los datos del mozo",
                            type: "error");
                    }
                    break;

                default:
                    break;
			}

            SetupGridview();
            upMain.Update();
        }

        protected void BtnGuardar_click(object sender, EventArgs e)
		{
            current.Nombre = tbNombre.Text;
            current.Apellido = tbApellido.Text;
            current.Documento = Int32.Parse(tbDocumento.Text);


            try
            {
                current.Comision = decimal.Parse(tbComision.Text.Replace('.', ','));
            }
            catch
            {
                MessageBox.Show(
                    title: "No se pudieron actualizar los datos del mozo",
                    message: "El valor de la comisión no está escrito correctamente",
                    type: "error");
                return;
            }

            if (MozoBLL.ActualizarMozo(current))
			{
                SetupGridview();
                upMain.Update();
			}
			else
			{
                MessageBox.Show(
                    message: "No se pudieron actualizar los datos del mozo",
                    type: "error");
            }

            current = null;
        }

        protected void BtnCrear_click(object sender, EventArgs e)
        {
            Mozo mozo = new Mozo();
            mozo.IdLocal = idLocal;
            mozo.FechaBaja = null;
            mozo.FechaContrato = DateTime.Now;
            mozo.Nombre = tbNombre.Text;
            mozo.Apellido = tbApellido.Text;
            mozo.Documento = Int32.Parse(tbDocumento.Text);

			try
			{
                mozo.Comision = decimal.Parse(tbComision.Text.Replace('.', ','));
            }
			catch
			{
                MessageBox.Show(
                    title: "No se pudo crear el mozo",
                    message: "El valor de la comisión no está escrito correctamente",
                    type: "error");
                return;
            }

            if (MozoBLL.CrearMozo(mozo))
			{
                SetupGridview();
                upMain.Update();
			}
			else
			{
                MessageBox.Show(
                    message: "No se pudo crear el mozo",
                    type: "error");
            }
        }

        protected void BtnNuevoMozo_click(object sender, EventArgs e)
		{
            SetupModal(null);
        }
    }
}