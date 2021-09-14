using Entidades;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class TicketDAL : GlobalDAL
	{
		public static List<Ticket> BuscarTicketsPorIdLocal(int idLocal)
		{
			List<Ticket> tickets = new List<Ticket>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarTicketsPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Ticket ticket = new Ticket();
							ticket.Id = rdr.GetInt32(0);
							ticket.IdLocal = idLocal;
							ticket.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							ticket.FechaVenta = rdr.GetDateTime(2);

							ticket.Mozo = MozoDAL.BuscarMozoPorId_Unsafe(conn, transaction, id: rdr.GetInt32(3));
							ticket.Detalles = BuscarDetallesTicketPorIdTicket_Unsafe(conn, transaction, ticket.Id);

							tickets.Add(ticket);
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

			return tickets;
		}

		public static List<Ticket> BuscarTicketsActivosPorIdLocal(int idLocal)
		{
			List<Ticket> tickets = new List<Ticket>();
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.BuscarTicketsActivosPorIdLocal", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", idLocal);
				cmd.Transaction = transaction;

				try
				{
					using (SqlDataReader rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							Ticket ticket = new Ticket();
							ticket.Id = rdr.GetInt32(0);
							ticket.IdLocal = idLocal;
							ticket.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
							ticket.FechaVenta = rdr.GetDateTime(2);

							ticket.Mozo = MozoDAL.BuscarMozoPorId_Unsafe(conn, transaction, id: rdr.GetInt32(3));
							ticket.Detalles = BuscarDetallesTicketPorIdTicket_Unsafe(conn, transaction, ticket.Id);

							tickets.Add(ticket);
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

			return tickets;
		}

		public static Ticket BuscarTicketPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				try
				{
					Ticket ticket = BuscarTicketPorId_Unsafe(conn, transaction, id);
					transaction.Commit();
					return ticket;
				}
				catch
				{
					transaction.Rollback();
					throw;
				}
			}
		}

		public static bool ActualizarTicket(Ticket ticket)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.ActualizarTicketPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdTicket", ticket.Id);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)ticket.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FechaVenta", ticket.FechaVenta);
				cmd.Parameters.AddWithValue("@IdMozo", ticket.Mozo);
				cmd.Transaction = transaction;

				bool result = true;
				try
				{
					result &= cmd.ExecuteNonQuery() > 0;

					foreach (Ticket.Detalle detalle in ticket.Detalles)
					{
						result &= ActualizarDetallesTicket_Unsafe(conn, transaction, detalle);
					}

					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					throw;
				}

				return result;
			}
		}

		public static bool CrearTicket(Ticket ticket)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.CrearTicket", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdLocal", ticket.IdLocal);
				cmd.Parameters.AddWithValue("@FechaBaja", (object)ticket.FechaBaja ?? DBNull.Value);
				cmd.Parameters.AddWithValue("@FechaVenta", ticket.FechaVenta);
				cmd.Parameters.AddWithValue("@IdMozo", ticket.Mozo.Id);
				cmd.Transaction = transaction;

				int result;
				try
				{
					result = cmd.ExecuteNonQuery();

					foreach (Ticket.Detalle detalle in ticket.Detalles)
					{
						CrearDetalleTicket_Unsafe(conn, transaction, detalle);
					}

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

		public static bool EliminarTicketPorId(int id)
		{
			using (SqlConnection conn = SetupConnection())
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();

				SqlCommand cmd = new SqlCommand("dbo.EliminarTicketPorId", conn);
				cmd.CommandType = System.Data.CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@IdTicket", id);
				cmd.Transaction = transaction;

				int result;
				try
				{
					result = cmd.ExecuteNonQuery();
					EliminarDetallesTicketPorIdTicket_Unsafe(conn, transaction, id);
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
		/// Busca ticket por id asumiendo una conexión abierta y
		/// una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static Ticket BuscarTicketPorId_Unsafe(SqlConnection conn, SqlTransaction transaction, int id)
		{
			SqlCommand cmd = new SqlCommand("dbo.BuscarTicketPorId", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdTicket", id);
			cmd.Transaction = transaction;

			try
			{
				Ticket ticket;
				using (SqlDataReader rdr = cmd.ExecuteReader())
				{
					if (rdr.Read())
					{
						// Se encontró un ticket
						ticket = new Ticket();
						ticket.Id = id;
						ticket.IdLocal = rdr.GetInt32(0);
						ticket.FechaBaja = rdr.IsDBNull(1) ? (DateTime?)null : rdr.GetDateTime(1);
						ticket.FechaVenta = rdr.GetDateTime(2);

						ticket.Mozo = MozoDAL.BuscarMozoPorId_Unsafe(conn, transaction, id: rdr.GetInt32(3));
						ticket.Detalles = BuscarDetallesTicketPorIdTicket_Unsafe(conn, transaction, id);
					}
					else
					{
						// No se encontró un ticket
						ticket = null;
					}
				}
				return ticket;
			}
			catch
			{
				throw;
			}
		}

		/* (Ticket.Detalle)DAL */

		/// <summary>
		/// Busca los detalles de un ticket por id asumiendo una conexión abierta y
		/// una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="idTicket"></param>
		/// <returns></returns>
		protected static List<Ticket.Detalle> BuscarDetallesTicketPorIdTicket_Unsafe(SqlConnection conn, SqlTransaction transaction, int idTicket)
		{
			List<Ticket.Detalle> detalles = new List<Ticket.Detalle>();

			SqlCommand cmd = new SqlCommand("dbo.BuscarDetallesTicketPorIdTicket", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdTicket", idTicket);
			cmd.Transaction = transaction;

			try
			{
				Ticket.Detalle detalle;
				using (SqlDataReader rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						detalle = new Ticket.Detalle();
						detalle.Id = rdr.GetInt32(0);
						detalle.Articulo = ArticuloDAL.BuscarArticuloPorId_Unsafe(conn, transaction, id: rdr.GetInt32(1));
						detalle.Cantidad = rdr.GetInt32(2);

						detalles.Add(detalle);
					}
				}
			}
			catch
			{
				throw;
			}
			return detalles;
		}

		/// <summary>
		/// Busca los detalles de un ticket por id asumiendo una conexión abierta y
		/// una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="idTicket"></param>
		/// <returns></returns>
		protected static bool ActualizarDetallesTicket_Unsafe(SqlConnection conn, SqlTransaction transaction, Ticket.Detalle detalle)
		{
			SqlCommand cmd = new SqlCommand("dbo.ActualizarDetallesTicketPorId", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdDetalle", detalle.Id);
			cmd.Parameters.AddWithValue("@IdArticulo", detalle.Articulo.Id);
			cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
			cmd.Transaction = transaction;

			int result;
			try
			{
				result = cmd.ExecuteNonQuery();
			}
			catch
			{
				throw;
			}

			return result > 0;
		}

		/// <summary>
		/// Crea el detalle de una entrada de ticket por id asumiendo una 
		/// conexión abierta y una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="idTicket"></param>
		/// <returns></returns>
		protected static bool CrearDetalleTicket_Unsafe(SqlConnection conn, SqlTransaction transaction, Ticket.Detalle detalle)
		{
			SqlCommand cmd = new SqlCommand("dbo.CrearDetalleTicket", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdTicket", detalle.Id);
			cmd.Parameters.AddWithValue("@IdArticulo", detalle.Articulo.Id);
			cmd.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
			cmd.Transaction = transaction;

			int result;
			try
			{
				result = cmd.ExecuteNonQuery();
			}
			catch
			{
				throw;
			}

			return result > 0;
		}

		/// <summary>
		/// Elimina los detalles de un ticket por id asumiendo una 
		/// conexión abierta y una transacción iniciada y manipulada externamente
		/// </summary>
		/// <param name="conn">Conexión ya abierta</param>
		/// <param name="transaction">Transacción ya iniciada</param>
		/// <param name="idTicket"></param>
		/// <returns></returns>
		protected static bool EliminarDetallesTicketPorIdTicket_Unsafe(SqlConnection conn, SqlTransaction transaction, int idTicket)
		{
			SqlCommand cmd = new SqlCommand("dbo.EliminarDetallesTicketPorIdTicket", conn);
			cmd.CommandType = System.Data.CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@IdTicket", idTicket);
			cmd.Transaction = transaction;

			int result;
			try
			{
				result = cmd.ExecuteNonQuery();
			}
			catch
			{
				throw;
			}

			return result > 0;
		}
	}
}
