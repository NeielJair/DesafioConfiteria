using BusinessLogicLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DesafioConfiteria
{
	public partial class GestionarRubros : System.Web.UI.Page
	{
        private static int idLocal;
        private static Rubro current;

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
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Id"), new DataColumn("Nombre"), new DataColumn("FechaBaja") });

            List<Rubro> rubros = RubroBLL.BuscarRubrosPorIdLocal(idLocal);

            foreach (Rubro rubro in rubros)
            {
                dt.Rows.Add(rubro.IdRubro, rubro.Nombre, rubro.FechaBaja?.ToString() ?? "—");
            }

            gvRubros.DataSource = dt;
            gvRubros.DataBind();
        }

        private void SetupModal(Rubro rubro)
		{
            bool isCreate = rubro == null;

            btnGuardar.Visible = !isCreate;
            btnCrear.Visible = isCreate;
            upModalFooter.Update();

            tbNombre.Text = (!isCreate) ? rubro.Nombre : "";

            current = rubro;
            upModal.Update();
        }

        private Rubro RowToRubro(GridViewRow row)
		{
            Rubro rubro = new Rubro();
            rubro.IdRubro = Int32.Parse(row.Cells[0].Text);
            rubro.IdLocal = idLocal;
            rubro.Nombre = row.Cells[1].Text;
            rubro.FechaBaja = (row.Cells[2].Text != "—") ? DateTime.Parse(row.Cells[2].Text) : (DateTime?)null;
            return rubro;
        }

        protected void GvRubros_RowDatabound(object sender, GridViewRowEventArgs e)
        {
            // Cambiar el texto del botón si es que el rubro no está vigente
            bool estaVigente = e.Row.Cells[2].Text == "—";
            foreach (Control ctrl in e.Row.Cells[3].Controls)
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
            Rubro rubro = RowToRubro(row);

            switch (e.CommandName)
            {
                case "Modificar":
                    SetupModal(rubro);
                    break;

                case "Dar de baja":
                    rubro.FechaBaja = DateTime.Now;
                    RubroBLL.ActualizarRubro(rubro);
                    break;

                case "Reactivar":
                    rubro.FechaBaja = null;
                    RubroBLL.ActualizarRubro(rubro);
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

            RubroBLL.ActualizarRubro(current);

            SetupGridview();
            upMain.Update();

            current = null;
        }

        protected void BtnCrear_click(object sender, EventArgs e)
        {
            Rubro rubro = new Rubro();
            rubro.IdLocal = idLocal;
            rubro.FechaBaja = null;
            rubro.Nombre = tbNombre.Text;

            RubroBLL.CrearRubro(rubro);

            SetupGridview();
            upMain.Update();
        }

        protected void BtnNuevoRubro_click(object sender, EventArgs e)
		{
            SetupModal(null);
        }
    }
}