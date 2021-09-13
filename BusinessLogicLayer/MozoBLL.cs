using DataAccessLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class MozoBLL
	{
		/// <summary>
		/// Devuelve una lista con todos los mozos
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Mozo> BuscarMozosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return MozoDAL.BuscarMozosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve una lista con todos los mozos activos
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Mozo> BuscarMozosActivosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return MozoDAL.BuscarMozosActivosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve la información de un mozo
		/// </summary>
		/// <param name="id">ID del mozo</param>
		public static Mozo BuscarMozoPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del mozo no puede ser menor que 1");
			}

			return MozoDAL.BuscarMozoPorId(id);
		}

		/// <summary>
		/// Actualiza los datos de un mozo
		/// </summary>
		/// <param name="mozo"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool ActualizarMozo(Mozo mozo)
		{
			if (mozo == null)
			{
				throw new ArgumentNullException("El mozo no puede ser nulo");
			}
			if (mozo.Id < 1)
			{
				throw new ArgumentException("El ID del mozo no puede ser menor que 1");
			}
			if (mozo.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}
			if (mozo.Comision < 0 || mozo.Comision > 100)
			{
				throw new ArgumentException("La comisión del mozo debe estar entre 0 y 100");
			}

			return MozoDAL.ActualizarMozo(mozo);
		}

		/// <summary>
		/// Crea un mozo
		/// </summary>
		/// <param name="mozo"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool CrearMozo(Mozo mozo)
		{
			if (mozo == null)
			{
				throw new ArgumentNullException("El mozo no puede ser nulo");
			}
			if (mozo.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}
			if (mozo.Comision < 0 || mozo.Comision > 100)
			{
				throw new ArgumentException("La comisión del mozo debe estar entre 0 y 100");
			}

			return MozoDAL.CrearMozo(mozo);
		}

		/// <summary>
		/// Elimina un mozo de la BDs
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool EliminarMozoPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del mozo no puede ser menor que 1");
			}

			return MozoDAL.EliminarMozoPorId(id);
		}
	}
}
