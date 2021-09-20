using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class MozoDAL : GlobalDAL
	{
		public static List<Mozo> BuscarMozosPorIdLocal(int idLocal)
		{
			List<Mozo> mozos = new List<Mozo>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarMozosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Mozo mozo = new Mozo();
							mozo.Id = rdr.GetInt32(0);
							mozo.IdLocal = idLocal;
							mozo.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							mozo.FechaContrato = rdr.GetDateTime(2);
							mozo.Nombre = rdr.GetString(3);
							mozo.Apellido = rdr.GetString(4);
							mozo.Documento = rdr.GetInt32(5);
							mozo.Comision = rdr.GetDecimal(6);

							mozos.Add(mozo);
						}
					}
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}

			return mozos;
		}

		public static List<Mozo> BuscarMozosActivosPorIdLocal(int idLocal)
		{
			List<Mozo> mozos = new List<Mozo>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarMozosActivosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Mozo mozo = new Mozo();
							mozo.Id = rdr.GetInt32(0);
							mozo.IdLocal = idLocal;
							mozo.FechaBaja = null;
							mozo.FechaContrato = rdr.GetDateTime(1);
							mozo.Nombre = rdr.GetString(2);
							mozo.Apellido = rdr.GetString(3);
							mozo.Documento = rdr.GetInt32(4);
							mozo.Comision = rdr.GetDecimal(5);

							mozos.Add(mozo);
						}
					}
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}

			return mozos;
		}

		public static Mozo BuscarMozoPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					Mozo mozo = BuscarMozoPorId_Unsafe(conn, transaction, id);
					transaction.Commit();
					return mozo;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public static bool ActualizarMozo(Mozo mozo)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.ActualizarMozoPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdMozo", mozo.Id);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)mozo.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FechaContrato", mozo.FechaContrato);
				cmd.Parameters.AddWithValue("@Nombre", mozo.Nombre);
				cmd.Parameters.AddWithValue("@Apellido", mozo.Apellido);
				cmd.Parameters.AddWithValue("@Documento", mozo.Documento);
				cmd.Parameters.AddWithValue("@Comision", mozo.Comision);
				cmd.Transaction = transaction;

				int result;
				try
				{
					result = cmd.ExecuteNonQuery();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					result = 0;
					throw;
				}

				return result > 0;
			}
		}

		public static bool CrearMozo(Mozo mozo)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.CrearMozo", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", mozo.IdLocal);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)mozo.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FechaContrato", mozo.FechaContrato);
				cmd.Parameters.AddWithValue("@Nombre", mozo.Nombre);
				cmd.Parameters.AddWithValue("@Apellido", mozo.Apellido);
				cmd.Parameters.AddWithValue("@Documento", mozo.Documento);
				cmd.Parameters.AddWithValue("@Comision", mozo.Comision);
				cmd.Transaction = transaction;

				int result;
				try
				{
					result = cmd.ExecuteNonQuery();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					result = 0;
					throw;
				}

				return result > 0;
			}
		}

		public static bool EliminarMozoPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.EliminarMozoPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdMozo", id);
				cmd.Transaction = transaction;

				int result;
				try
				{
					result = cmd.ExecuteNonQuery();
					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					result = 0;
					throw;
				}

				return result > 0;
			}
		}

		/// <summary>
		/// Busca mozo por id asumiendo una conexión abierta y
		/// una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Mozo BuscarMozoPorId_Unsafe(SqlConnection conn, SqlTransaction transaction, int id)
		{
			SqlCommand cmd = new SqlCommand("dbo.BuscarMozoPorId", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdMozo", id);
			cmd.Transaction = transaction;

			try
			{
				Mozo mozo;
				using (SqlDataReader rdr = cmd.ExecuteReader())
				{
					if (rdr.Read())
					{
						// Se encontró un mozo
						mozo = new Mozo();
						mozo.Id = id;
						mozo.IdLocal = rdr.GetInt32(0);
						mozo.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
						mozo.FechaContrato = rdr.GetDateTime(2);
						mozo.Nombre = rdr.GetString(3);
						mozo.Apellido = rdr.GetString(4);
						mozo.Documento = rdr.GetInt32(5);
						mozo.Comision = rdr.GetDecimal(6);
					}
					else
					{
						// No se encontró un mozo
						mozo = null;
					}
				}
				return mozo;
			}
			catch
			{
				throw;
			}
		}
	}
}
