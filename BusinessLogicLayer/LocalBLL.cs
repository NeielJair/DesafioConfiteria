using DataAccessLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class LocalBLL
	{
		/// <summary>
		/// Devuelve una lista con todos los locales
		/// </summary>
		public static List<Local> BuscarLocales()
		{
			return LocalDAL.BuscarLocales();
		}

		/// <summary>
		/// Devuelve una lista con todos los locales activos
		/// </summary>
		public static List<Local> BuscarLocalesActivos()
		{
			return LocalDAL.BuscarLocalesActivos();
		}

		/// <summary>
		/// Devuelve la información de un local
		/// </summary>
		/// <param name="id">ID del local</param>
		public static Local BuscarLocalPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return LocalDAL.BuscarLocalPorId(id);
		}
	}
}
