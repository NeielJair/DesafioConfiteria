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
	public partial class GestionarArticulos : System.Web.UI.Page
	{
        private static int idLocal;
        private static Articulo current;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                idLocal = Int32.Parse(Request.QueryString["IdLocal"]);

                SetupGridview();
            }
        }

        private void SetupGridview()
		{
            DataTable dt = new DataTable();
            dt.Columns.AddRange(
                new DataColumn[5] { 
                    new DataColumn("Id"), 
                    new DataColumn("Nombre"), 
                    new DataColumn("FechaBaja"),
                    new DataColumn("Rubro"),
                    new DataColumn("Precio")});

            List<Articulo> articulos = ArticuloBLL.BuscarArticulosPorIdLocal(idLocal);

            foreach (Articulo articulo in articulos)
            {
                dt.Rows.Add(
                    articulo.Id, 
                    articulo.Nombre, 
                    articulo.FechaBaja?.ToString() ?? "—", 
                    $"{articulo.Rubro.Id} - {articulo.Rubro.Nombre}",
                    "$" + articulo.Precio.ToString("0.00"));
            }

            gvRubros.DataSource = dt;
            gvRubros.DataBind();
        }

        private void SetupModal(Articulo articulo)
		{
            bool isCreate = articulo == null;

            btnGuardar.Visible = !isCreate;
            btnCrear.Visible = isCreate;
            upModalFooter.Update();

            tbNombre.Text = (!isCreate) ? articulo.Nombre : "";

            // Setup ddlRubro
            ddlRubro.Items.Clear();
            List<Rubro> rubros = RubroBLL.BuscarRubrosActivosPorIdLocal(idLocal);
            ddlRubro.Items.Add(new ListItem("Seleccione un rubro", "-1"));
            foreach (Rubro rubro in rubros)
            {
                ListItem item = new ListItem(rubro.Nombre, rubro.Id.ToString());
                item.Attributes.Add("title", $"ID: {rubro.Id}");

                ddlRubro.Items.Add(item);
            }

            if (articulo?.Rubro != null)
			{
                ddlRubro.Items.FindByValue(articulo.Rubro.Id.ToString()).Selected = true;
            }

            // Setup precio
            string strPesos = "0";
            string strCentavos = "00";
            if (articulo != null)
			{
                int precioPesos = (int)Math.Floor(articulo.Precio);
                int precioCentavos = (int)Math.Floor((articulo.Precio - precioPesos) * 100);

                strPesos = precioPesos.ToString();
                strCentavos = precioCentavos.ToString();
                while (strCentavos.Length < 2)
				{
                    strCentavos += "0";
				}
            }

            tbPrecioPesos.Text = strPesos;
            tbPrecioCentavos.Text = strCentavos;

            current = articulo;
            upModal.Update();
        }

        private Articulo RowToArticulo(GridViewRow row)
		{
            return ArticuloBLL.BuscarArticuloPorId(Int32.Parse(row.Cells[0].Text));
        }

        protected void GvRubros_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            // Cambiar el texto del botón si es que el rubro no está vigente
            bool estaVigente = e.Row.Cells[2].Text == "—";
            foreach (Control ctrl in e.Row.Cells[5].Controls)
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

        protected void GvRubros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            current = null;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvRubros.Rows[rowIndex];
            Articulo articulo = RowToArticulo(row);

            switch (e.CommandName)
            {
                case "Modificar":
                    SetupModal(articulo);
                    break;

                case "Dar de baja":
                    articulo.FechaBaja = DateTime.Now;
                    if (!ArticuloBLL.ActualizarArticulo(articulo))
					{
                        MessageBox.Show(
                            message: "No se pudo actualizar el artículo",
                            type: "error");
                    }
                    break;

                case "Reactivar":
                    articulo.FechaBaja = null;
                    if (!ArticuloBLL.ActualizarArticulo(articulo))
					{
                        MessageBox.Show(
                            message: "No se pudo actualizar el artículo",
                            type: "error");
                    }
                    break;

                case "Eliminar":
                    if (!ArticuloBLL.EliminarArticuloPorId(articulo.Id))
					{
                        MessageBox.Show(
                            message: "No se pudo eliminar el artículo",
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
            current.Rubro.Id = Int32.Parse(ddlRubro.SelectedValue);
            current.Precio = Decimal.Parse($"{tbPrecioPesos.Text}.{tbPrecioCentavos.Text}", CultureInfo.InvariantCulture);

            if (ArticuloBLL.ActualizarArticulo(current))
			{
                SetupGridview();
                upMain.Update();
			}
			else
			{
                MessageBox.Show(
                    title: "No se pudo modificar el artículo", 
                    message: "Probablemente ese nombre ya está en uso",
                    type: "error");
            }

            current = null;
        }

        protected void BtnCrear_click(object sender, EventArgs e)
        {
            Articulo articulo = new Articulo();
            articulo.IdLocal = idLocal;
            articulo.FechaBaja = null;
            articulo.Nombre = tbNombre.Text;

            articulo.Rubro = new Rubro();
            articulo.Rubro.Id = Int32.Parse(ddlRubro.SelectedValue);

			try
			{
                articulo.Precio = Decimal.Parse($"{tbPrecioPesos.Text}.{tbPrecioCentavos.Text}", CultureInfo.InvariantCulture);
            }
            catch
			{
                MessageBox.Show(
                    title: "No se pudo crear el artículo",
                    message: "El precio es inválido",
                    type: "error");
                return;
            }

            if (ArticuloBLL.CrearArticulo(articulo))
			{
                SetupGridview();
                upMain.Update();
			}
			else
			{
                MessageBox.Show(
                    title: "No se pudo crear el artículo",
                    message: "Probablemente ese nombre ya está en uso",
                    type: "error");
            }
        }

        protected void BtnNuevoArticulo_click(object sender, EventArgs e)
		{
            SetupModal(null);
        }
    }
}