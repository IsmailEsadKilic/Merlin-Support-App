namespace API.DTOs
{ 
    public class TicketDto
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public int CustomerTicketId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime FirstResponseTime { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public IList<TeamMemberDto> TeamMemberDtos { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int RecordUserId { get; set; }
        public string RecordUserName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int TicketStateId { get; set; }
        public string TicketStateName { get; set; }
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; }

    }
}