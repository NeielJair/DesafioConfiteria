using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
	public class Ticket
	{
        public int IdTicket { get; set; }
        public int IdLocal { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime FechaVenta { get; set; }
        public int IdMozo { get; set; }
        public List<Tuple<Articulo, int>> Detalle { get; set; }
    }
}
