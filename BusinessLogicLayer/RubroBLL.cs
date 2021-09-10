using DataAccessLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class RubroBLL
	{
		/// <summary>
		/// Devuelve una lista con todos los rubros
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Rubro> BuscarRubrosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return RubroDAL.BuscarRubrosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve una lista con todos los rubros activos
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Rubro> BuscarRubrosActivosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return RubroDAL.BuscarRubrosActivosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve la información de un rubro
		/// </summary>
		/// <param name="id">ID del rubro</param>
		public static Rubro BuscarRubroPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del rubro no puede ser menor que 1");
			}

			return RubroDAL.BuscarRubroPorId(id);
		}

		/// <summary>
		/// Actualiza los datos de un rubro
		/// </summary>
		/// <param name="rubro"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool ActualizarRubro(Rubro rubro)
		{
			if (rubro == null)
			{
				throw new ArgumentNullException("El rubro no puede ser nulo");
			}
			if (rubro.IdRubro < 1)
			{
				throw new ArgumentException("El ID del rubro no puede ser menor que 1");
			}
			if (rubro.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return RubroDAL.ActualizarRubro(rubro);
		}

		/// <summary>
		/// Crea un rubro
		/// </summary>
		/// <param name="rubro"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool CrearRubro(Rubro rubro)
		{
			if (rubro == null)
			{
				throw new ArgumentNullException("El rubro no puede ser nulo");
			}
			if (rubro.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return RubroDAL.CrearRubro(rubro);
		}
	}
}
