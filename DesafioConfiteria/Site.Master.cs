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
	public partial class SiteMaster : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["IdLocal"] != null)
			{
				Local local = LocalBLL.BuscarLocalPorId(Session["IdLocal"] as int? ?? throw new Exception());
				Page.Title = local.Nombre + " - " + Page.Title;
				navbarTitle.InnerHtml = $"<i>{local.Nombre}</i>";
			}
		}
	}
}