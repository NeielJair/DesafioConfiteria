using DataAccessLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class ArticuloBLL
	{
		/// <summary>
		/// Devuelve una lista con todos los artículos
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Articulo> BuscarArticulosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return ArticuloDAL.BuscarArticulosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve una lista con todos los artículos activos
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Articulo> BuscarArticulosActivosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return ArticuloDAL.BuscarArticulosActivosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve la información de un artículo
		/// </summary>
		/// <param name="id">ID del artículo</param>
		public static Articulo BuscarArticuloPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del artículo no puede ser menor que 1");
			}

			return ArticuloDAL.BuscarArticuloPorId(id);
		}

		/// <summary>
		/// Actualiza los datos de un artículo
		/// </summary>
		/// <param name="articulo"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool ActualizarArticulo(Articulo articulo)
		{
			if (articulo == null)
			{
				throw new ArgumentNullException("El artículo no puede ser nulo");
			}
			if (articulo.IdArticulo < 1)
			{
				throw new ArgumentException("El ID del artículo no puede ser menor que 1");
			}
			if (articulo.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}
			if (articulo.IdRubro < 1)
			{
				throw new ArgumentException("El ID del rubro no puede ser menor que 1");
			}

			return ArticuloDAL.ActualizarArticulo(articulo);
		}

		/// <summary>
		/// Crea un artículo
		/// </summary>
		/// <param name="articulo"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool CrearArticulo(Articulo articulo)
		{
			if (articulo == null)
			{
				throw new ArgumentNullException("El artículo no puede ser nulo");
			}
			if (articulo.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}
			if (articulo.IdRubro < 1)
			{
				throw new ArgumentException("El ID del rubro no puede ser menor que 1");
			}

			return ArticuloDAL.CrearArticulo(articulo);
		}

		/// <summary>
		/// Elimina un artículo de la BDs
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool EliminarArticuloPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del artículo no puede ser menor que 1");
			}

			return ArticuloDAL.EliminarArticuloPorId(id);
		}
	}
}
