using BusinessLogicLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace DesafioConfiteria
{
	public partial class GestionarLocales : System.Web.UI.Page
	{
        private static List<Local> locales;
        private static Local current;

        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session["IdLocal"] = null;
                SetupGridview();
            }
        }

        private void SetupGridview()
		{
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] { 
                new DataColumn("Id"), 
                new DataColumn("Nombre"), 
                new DataColumn("Direccion"), 
                new DataColumn("Telefono"), 
                new DataColumn("Email"), });

            locales = LocalBLL.BuscarLocales();

            foreach (Local local in locales)
            {
                dt.Rows.Add(
                    local.Id, 
                    local.Nombre, 
                    local.Direccion, 
                    local.Telefono, 
                    local.Email);
            }

            gvLocales.DataSource = dt;
            gvLocales.DataBind();
        }

        private void SetupModal(Local local)
		{
            bool isCreate = local == null;

            btnGuardar.Visible = !isCreate;
            btnCrear.Visible = isCreate;
            upModalFooter.Update();

            tbNombre.Text = (!isCreate) ? local.Nombre : "";
            tbDireccion.Text = (!isCreate) ? local.Direccion : "";
            tbTelefono.Text = (!isCreate) ? local.Telefono : "";
            tbEmail.Text = (!isCreate) ? local.Email : "";
            tbPassword.Text = "";

            current = local;
            upModal.Update();
        }

        private Local RowToLocal(GridViewRow row)
		{
            return LocalBLL.BuscarLocalPorId(Int32.Parse(row.Cells[0].Text));
        }

        protected void Gv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            current = null;

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvLocales.Rows[rowIndex];
            Local local = RowToLocal(row);
            string password = tbPassword.Text;

            switch (e.CommandName)
            {
                case "Modificar":
                    SetupModal(local);
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
            current.Direccion = tbDireccion.Text;
            current.Telefono = tbTelefono.Text;
            current.Email = tbEmail.Text;

			try
			{
                if (LocalBLL.ActualizarLocal(current, tbPassword.Text))
                {
                    SetupGridview();
                    upMain.Update();
                }
                else
                {
                    MessageBox.Show(
                        message: "No se pudo modificar el local",
                        type: "error");
                }
			}
			catch (Exceptions.IncorrectPasswordException ex)
			{
                MessageBox.Show(
                    message: ex.Message,
                    type: "error");

            }
            
            current = null;
        }

        protected void BtnCrear_click(object sender, EventArgs e)
        {
            Local local = new Local();
            local.FechaBaja = null;
            local.Nombre = tbNombre.Text;
            local.Direccion = tbDireccion.Text;
            local.Telefono = tbTelefono.Text;
            local.Email = tbEmail.Text;

            if (LocalBLL.CrearLocal(local, tbPassword.Text))
			{
                SetupGridview();
                upMain.Update();
			}
			else
			{
                MessageBox.Show(
                    message: "No se pudo crear el local",
                    type: "error");
            }
        }

        protected void BtnNuevoLocal_click(object sender, EventArgs e)
		{
            SetupModal(null);
        }
    }
}