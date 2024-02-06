using API.Entities;

namespace API.DTOs
{
    public class CustomerProductListDto
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Version { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Description { get; set; }
    }
}