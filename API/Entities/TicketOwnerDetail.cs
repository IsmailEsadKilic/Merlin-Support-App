namespace API.Entities
{
    public partial class TicketOwnerDetail
    {
        public long Id { get; set; }
        public long TicketId { get; set; }
        public long UserId { get; set; }
    }
}