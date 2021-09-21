using BusinessLogicLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utils;

namespace DesafioConfiteria
{
	public partial class ElegirLocal : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["IdLocal"] = null;
				List<Local> locales = LocalBLL.BuscarLocalesActivos();

				ddlLocal.Items.Clear();
				ddlLocal.Items.Add(new ListItem("Seleccione un local", "-1"));
				foreach (Local local in locales)
				{
					ListItem item = new ListItem(local.Nombre, local.Id.ToString());
					item.Attributes.Add("title", $"Ubicado en {local.Direccion}");
					ddlLocal.Items.Add(item);
				}
			}
		}

		private void RunJS(string function)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), function, function, true);
		}

		protected void BtnIngresar_click(object sender, EventArgs e)
		{
			int id = Int32.Parse(ddlLocal.SelectedValue);
			if (LocalBLL.LoginPorId(id, tbPassword.Text))
			{
				Session["IdLocal"] = id;
				Response.Redirect("MainPage");
			}
			else
			{
				MessageBox.Show(
					message: "Contraseña incorrecta",
					type: "error");
			}
			
		}
	}
}