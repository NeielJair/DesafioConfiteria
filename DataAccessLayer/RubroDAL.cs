using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class RubroDAL : GlobalDAL
	{
		public static List<Rubro> BuscarRubrosPorIdLocal(int idLocal)
		{
			List<Rubro> rubros = new List<Rubro>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarRubrosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Rubro rubro = new Rubro();
							rubro.Id = rdr.GetInt32(0);
							rubro.IdLocal = idLocal;
							rubro.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							rubro.Nombre = rdr.GetString(2);

							rubros.Add(rubro);
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

			return rubros;
		}

		public static List<Rubro> BuscarRubrosActivosPorIdLocal(int idLocal)
		{
			List<Rubro> rubros = new List<Rubro>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarRubrosActivosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Rubro rubro = new Rubro();
							rubro.Id = rdr.GetInt32(0);
							rubro.IdLocal = idLocal;
							rubro.FechaBaja = null;
							rubro.Nombre = rdr.GetString(1);

							rubros.Add(rubro);
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

			return rubros;
		}

		public static Rubro BuscarRubroPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					Rubro rubro = BuscarRubroPorId_Unsafe(conn, transaction, id);
					transaction.Commit();
					return rubro;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public static bool ActualizarRubro(Rubro rubro)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.ActualizarRubroPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdRubro", rubro.Id);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)rubro.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Nombre", rubro.Nombre);
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

		public static bool CrearRubro(Rubro rubro)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.CrearRubro", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", rubro.IdLocal);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)rubro.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Nombre", rubro.Nombre);
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

		public static bool EliminarRubroPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.EliminarRubroPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdRubro", id);
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
		/// Busca rubro por id asumiendo una conexión abierta y
		/// una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Rubro BuscarRubroPorId_Unsafe(SqlConnection conn, SqlTransaction transaction, int id)
		{
			SqlCommand cmd = new SqlCommand("dbo.BuscarRubroPorId", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdRubro", id);
			cmd.Transaction = transaction;

			try
			{
				Rubro rubro;
				using (SqlDataReader rdr = cmd.ExecuteReader())
				{
					if (rdr.Read())
					{
						// Se encontró un rubro
						rubro = new Rubro();
						rubro.Id = id;
						rubro.IdLocal = rdr.GetInt32(0);
						rubro.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
						rubro.Nombre = rdr.GetString(2);
					}
					else
					{
						// No se encontró un rubro
						rubro = null;
					}
				}
				return rubro;
			}
			catch
			{
				throw;
			}
		}
	}
}
