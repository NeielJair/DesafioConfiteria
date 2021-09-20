using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
	public class InformeVentas : List<ResumenMozo>
	{
		public DateTime Fecha { get; set; }

		public InformeVentas(DateTime fecha)
		{
			this.Fecha = fecha;
		}

		public void RegistrarVenta(Mozo mozo, int cantidad, decimal precio)
		{
			ResumenMozo resumen = this.Find(r => r.Mozo.Id == mozo.Id);
			if (resumen == null)
			{
				resumen = new ResumenMozo(mozo);
				this.Add(resumen);
			}

			resumen.RegistrarVenta(cantidad, precio);
		}
		public decimal SacarTotal()
		{
			decimal total = 0;
			foreach (ResumenMozo resumen in this)
			{
				total += resumen.SacarComision();
			}
			return total;
		}
	}
	public class ResumenMozo
	{
		public Mozo Mozo { get; set; }
		public int Cantidad { get; set; }
		public decimal Importe { get; set; }

		public ResumenMozo() { }
		public ResumenMozo(Mozo mozo)
		{
			this.Mozo = mozo;
		}
		public void RegistrarVenta(int cantidad, decimal precio)
		{
			Cantidad += cantidad;
			Importe += cantidad * precio;
		}
		public decimal SacarComision()
		{
			return Importe * (decimal)Mozo.Comision / 100m;
		}
	}
}
