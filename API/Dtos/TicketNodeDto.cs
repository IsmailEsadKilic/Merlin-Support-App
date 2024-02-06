
namespace API.Dtos
{
    public class TicketNodeDto
    {
        public long Id { get; set; }
        public string RowGuid { get; set; }
        public string Node { get; set; }
        public int RecordUserId { get; set; }
        public string RecordUserName { get; set; }
        public long TicketId { get; set; }
        public DateTime RecordDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}