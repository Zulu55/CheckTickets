using System;

namespace CheckTickets.Models
{
    public class Ticket
    {
		public int TicketId { get; set; }

		public string TicketCode { get; set; }

		public DateTime DateTime { get; set; }

		public int UserId { get; set; }
    }
}
