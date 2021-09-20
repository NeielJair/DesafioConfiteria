using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class ArticuloDAL : GlobalDAL
	{
		public static List<Articulo> BuscarArticulosPorIdLocal(int idLocal)
		{
			List<Articulo> articulos = new List<Articulo>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarArticulosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Articulo articulo = new Articulo();
							articulo.Id = rdr.GetInt32(0);
							articulo.IdLocal = idLocal;
							articulo.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							articulo.Nombre = rdr.GetString(2);

							articulo.Rubro = RubroDAL.BuscarRubroPorId_Unsafe(conn, transaction, id: rdr.GetInt32(3));

							articulo.Precio = rdr.GetDecimal(4);

							articulos.Add(articulo);
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

			return articulos;
		}

		public static List<Articulo> BuscarArticulosActivosPorIdLocal(int idLocal)
		{
			List<Articulo> articulos = new List<Articulo>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarArticulosActivosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Articulo articulo = new Articulo();
							articulo.Id = rdr.GetInt32(0);
							articulo.IdLocal = idLocal;
							articulo.FechaBaja = null;
							articulo.Nombre = rdr.GetString(1);

							articulo.Rubro = RubroDAL.BuscarRubroPorId_Unsafe(conn, transaction, id: rdr.GetInt32(2));

							articulo.Precio = rdr.GetDecimal(3);

							articulos.Add(articulo);
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

			return articulos;
		}

		public static Articulo BuscarArticuloPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					Articulo articulo = BuscarArticuloPorId_Unsafe(conn, transaction, id);
					transaction.Commit();
					return articulo;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public static bool ActualizarArticulo(Articulo articulo)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.ActualizarArticuloPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdArticulo", articulo.Id);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)articulo.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Nombre", articulo.Nombre);
				cmd.Parameters.AddWithValue("@IdRubro", articulo.Rubro.Id);
				cmd.Parameters.AddWithValue("@Precio", articulo.Precio);
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

		public static bool CrearArticulo(Articulo articulo)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.CrearArticulo", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", articulo.IdLocal);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)articulo.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Nombre", articulo.Nombre);
				cmd.Parameters.AddWithValue("@IdRubro", articulo.Rubro.Id);
				cmd.Parameters.AddWithValue("@Precio", articulo.Precio);
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

		public static bool EliminarArticuloPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.EliminarArticuloPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdArticulo", id);
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
		/// Busca artículo por id asumiendo una conexión abierta y
		/// una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Articulo BuscarArticuloPorId_Unsafe(SqlConnection conn, SqlTransaction transaction, int id)
		{
			SqlCommand cmd = new SqlCommand("dbo.BuscarArticuloPorId", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdArticulo", id);
			cmd.Transaction = transaction;

			try
			{
				Articulo articulo;
				using (SqlDataReader rdr = cmd.ExecuteReader())
				{
					if (rdr.Read())
					{
						// Se encontró un artículo
						articulo = new Articulo();
						articulo.Id = id;
						articulo.IdLocal = rdr.GetInt32(0);
						articulo.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
						articulo.Nombre = rdr.GetString(2);

						articulo.Rubro = RubroDAL.BuscarRubroPorId_Unsafe(conn, transaction, id: rdr.GetInt32(3));

						articulo.Precio = rdr.GetDecimal(4);
					}
					else
					{
						// No se encontró un artículo
						articulo = null;
					}
				}
				return articulo;
			}
			catch
			{
				throw;
			}
		}
	}
}
