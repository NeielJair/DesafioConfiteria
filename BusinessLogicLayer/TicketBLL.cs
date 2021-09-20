using DataAccessLayer;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class TicketBLL
	{
		/// <summary>
		/// Devuelve una lista con todos los tickets
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Ticket> BuscarTicketsPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return TicketDAL.BuscarTicketsPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve una lista con todos los tickets activos
		/// </summary>
		/// <param name="idLocal">ID del local</param>
		public static List<Ticket> BuscarTicketsActivosPorIdLocal(int idLocal)
		{
			if (idLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}

			return TicketDAL.BuscarTicketsActivosPorIdLocal(idLocal);
		}

		/// <summary>
		/// Devuelve la información de un ticket
		/// </summary>
		/// <param name="id">ID del artículo</param>
		public static Ticket BuscarTicketPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del ticket no puede ser menor que 1");
			}

			return TicketDAL.BuscarTicketPorId(id);
		}

		/// <summary>
		/// Actualiza los datos de un ticket
		/// </summary>
		/// <param name="ticket"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool ActualizarTicket(Ticket ticket)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("El ticket no puede ser nulo");
			}
			if (ticket.Id < 1)
			{
				throw new ArgumentException("El ID del ticket no puede ser menor que 1");
			}
			if (ticket.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}
			if (ticket.Mozo.Id < 1)
			{
				throw new ArgumentException("El ID del mozo no puede ser menor que 1");
			}
			foreach (Ticket.Detalle detalle in ticket.Detalles)
			{
				if (detalle.Articulo.Id < 1)
				{
					throw new ArgumentException("El ID de ningún artículo puede ser menor que 1");
				}
			}

			return TicketDAL.ActualizarTicket(ticket);
		}

		/// <summary>
		/// Actualiza la fecha de baja de un ticket
		/// </summary>
		/// <param name="ticket"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool ActualizarTicketFechaBaja(Ticket ticket)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("El ticket no puede ser nulo");
			}
			if (ticket.Id < 1)
			{
				throw new ArgumentException("El ID del ticket no puede ser menor que 1");
			}

			return TicketDAL.ActualizarTicketFechaBaja(ticket);
		}

		/// <summary>
		/// Crea un ticket
		/// </summary>
		/// <param name="ticket"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool CrearTicket(Ticket ticket)
		{
			if (ticket == null)
			{
				throw new ArgumentNullException("El artículo no puede ser nulo");
			}
			if (ticket.IdLocal < 1)
			{
				throw new ArgumentException("El ID del local no puede ser menor que 1");
			}
			if (ticket.Mozo.Id < 1)
			{
				throw new ArgumentException("El ID del mozo no puede ser menor que 1");
			}
			foreach (Ticket.Detalle detalle in ticket.Detalles)
			{
				if (detalle.Articulo.Id < 1)
				{
					throw new ArgumentException("El ID de ningún artículo puede ser menor que 1");
				}
			}

			return TicketDAL.CrearTicket(ticket);
		}

		/// <summary>
		/// Elimina un ticket de la BDs
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Devuelve true si la operación fue exitosa</returns>
		public static bool EliminarTicketPorId(int id)
		{
			if (id < 1)
			{
				throw new ArgumentException("El ID del ticket no puede ser menor que 1");
			}

			return TicketDAL.EliminarTicketPorId(id);
		}
	}
}
