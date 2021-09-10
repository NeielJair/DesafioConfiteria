using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class LocalDAL : GlobalDAL
	{
		public static List<Local> BuscarLocales()
		{
			List<Local> locales = new List<Local>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarLocales", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Transaction = transaction;

				try
				{
					cmd.ExecuteNonQuery();
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Local local = new Local();
							local.IdLocal = rdr.GetInt32(0);
							local.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							local.Nombre = rdr.GetString(2);
							local.Direccion = rdr.GetString(3);
							local.Telefono = rdr.GetString(4);
							local.Email = rdr.GetString(5);

							locales.Add(local);
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

			return locales;
		}

		public static List<Local> BuscarLocalesActivos()
		{
			List<Local> locales = new List<Local>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarLocalesActivos", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Transaction = transaction;

				try
				{
					cmd.ExecuteNonQuery();
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Local local = new Local();
							local.IdLocal = rdr.GetInt32(0);
							local.FechaBaja = null;
							local.Nombre = rdr.GetString(1);
							local.Direccion = rdr.GetString(2);
							local.Telefono = rdr.GetString(3);
							local.Email = rdr.GetString(4);

							locales.Add(local);
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

			return locales;
		}

		public static Local BuscarLocalPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarLocalPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", id);
				cmd.Transaction = transaction;

				try
				{
					cmd.ExecuteNonQuery();
					Local local;
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						if (rdr.Read())
						{
							// Se encontró un local
							local = new Local();
							local.IdLocal = id;
							local.FechaBaja = rdr.IsDBNull(0) ? (DateTime?)null : rdr.GetDateTime(0);
							local.Nombre = rdr.GetString(1);
							local.Direccion = rdr.GetString(2);
							local.Telefono = rdr.GetString(3);
							local.Email = rdr.GetString(4);
						}
						else
						{
							// No se encontró un local
							local = null;
						}
					}
					transaction.Commit();
					return local;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}
	}
}
