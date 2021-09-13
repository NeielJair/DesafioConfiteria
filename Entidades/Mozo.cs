using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
	public class Mozo
	{
		public int Id { get; set; }
		public int IdLocal { get; set; }
		public DateTime? FechaBaja { get; set; }
		public DateTime FechaContrato { get; set; }
		public string Nombre { get; set; }
		public string Apellido { get; set; }
		public int Documento { get; set; }
		public double Comision { get; set; }
	}
}
