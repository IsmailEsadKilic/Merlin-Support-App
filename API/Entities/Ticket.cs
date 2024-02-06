namespace API.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public int CustomerTicketId { get; set; } // ticket counter
        public DateTime RecordDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime FirstResponseTime { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Team Team { get; set; }
        public int TeamId { get; set; }
        public Priority Priority { get; set; }
        public int PriorityId { get; set; }
        public User User { get; set; } // user who created the ticket //previous name: RecordUser 
        public int UserId { get; set; } // user who created the ticket //previous name: RecordUserId
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public int TicketStateId { get; set; } = 1; // 1: Open, 2: pending, 3: solved, 9: deleted
        public TicketType TicketType { get; set; }
        public int TicketTypeId { get; set; }
    }
}