using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
	public class Articulo
	{
		public int Id { get; set; }
		public int IdLocal { get; set; }
		public DateTime? FechaBaja { get; set; }
		public string Nombre { get; set; }
		public Rubro Rubro;
	}
}
