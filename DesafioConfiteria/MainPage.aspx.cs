using BusinessLogicLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DesafioConfiteria
{
	public partial class MainPage : Page
	{
        private int idLocal;
		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				idLocal = Session["IdLocal"] as int? ?? throw new ArgumentNullException();
			}
			catch
			{
				Response.Redirect("ElegirLocal");
			}
			Local local = LocalBLL.BuscarLocalPorId(idLocal);

			headerLocal.InnerText = $"{local.Nombre}";
		}
	}
}