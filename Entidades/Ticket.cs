using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
	public class Ticket
	{
        public class Detalle
        {
            public int Id { get; set; }
            public Articulo Articulo { get; set; }
            public int Cantidad { get; set; }
        }

        public int Id { get; set; }
        public int IdLocal { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime FechaVenta { get; set; }
        public Mozo Mozo { get; set; }
        public List<Detalle> Detalles { get; set; }

        public decimal SacarTotal()
		{
            decimal total = 0;
            foreach (Detalle detalle in Detalles)
			{
                total += detalle.Articulo.Precio * detalle.Cantidad;
			}
            return total;
		}
        public decimal SacarComision()
		{
            return SacarTotal() * (Decimal)Mozo.Comision;
		}
    }
}
