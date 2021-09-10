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
					cmd.ExecuteNonQuery();
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Articulo articulo = new Articulo();
							articulo.IdArticulo = rdr.GetInt32(0);
							articulo.IdLocal = idLocal;
							articulo.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							articulo.Nombre = rdr.GetString(2);
							articulo.IdRubro = rdr.GetInt32(3);

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
					cmd.ExecuteNonQuery();
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Articulo articulo = new Articulo();
							articulo.IdArticulo = rdr.GetInt32(0);
							articulo.IdLocal = idLocal;
							articulo.FechaBaja = null;
							articulo.Nombre = rdr.GetString(1);
							articulo.IdRubro = rdr.GetInt32(2);

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

				SqlCommand cmd = new SqlCommand("dbo.BuscarArticuloPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdArticulo", id);
				cmd.Transaction = transaction;

				try
				{
					cmd.ExecuteNonQuery();
					Articulo articulo;
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						if (rdr.Read())
						{
							// Se encontró un artículo
							articulo = new Articulo();
							articulo.IdArticulo = id;
							articulo.IdLocal = rdr.GetInt32(0);
							articulo.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							articulo.Nombre = rdr.GetString(2);
							articulo.IdRubro = rdr.GetInt32(3);
						}
						else
						{
							// No se encontró un artículo
							articulo = null;
						}
					}
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
				cmd.Parameters.AddWithValue("@IdArticulo", articulo.IdArticulo);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)articulo.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@Nombre", articulo.Nombre);
				cmd.Parameters.AddWithValue("@IdRubro", articulo.IdRubro);
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
				cmd.Parameters.AddWithValue("@IdRubro", articulo.IdRubro);
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
	}
}
