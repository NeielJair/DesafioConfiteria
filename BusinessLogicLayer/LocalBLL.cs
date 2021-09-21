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

		/// <summary>
		/// Actualiza los datos de un local
		/// </summary>
		/// <param name="local"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		/// <exception cref="IncorrectPasswordException">Cuando la contraseña no matchea</exception>
		public static bool ActualizarLocal(Local local, string password)
		{
			if (local == null)
			{
				throw new ArgumentNullException("El local no puede ser nulo");
			}
			if (local.Id < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return LocalDAL.ActualizarLocal(local, password);
		}

		/// <summary>
		/// Crea un local
		/// </summary>
		/// <param name="Local"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool CrearLocal(Local Local, string password)
		{
			if (Local == null)
			{
				throw new ArgumentNullException("El local no puede ser nulo");
			}

			return LocalDAL.CrearLocal(Local, password);
		}

		/// <summary>
		/// Elimina un local de la BDs
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		/// <exception cref="IncorrectPasswordException">Cuando la contraseña no matchea</exception>
		public static bool EliminarLocalPorId(int id, string password)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return LocalDAL.EliminarLocalPorId(id, password);
		}

		/// <summary>
		/// Informa si la contraseña es correcta
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Devuelve true si la contraseña matchea</returns>
		public static bool LoginPorId(int id, string password)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return LocalDAL.LoginPorId(id, password);
		}
	}
}
