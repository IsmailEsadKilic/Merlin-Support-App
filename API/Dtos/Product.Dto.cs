namespace API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string RowGuid { get; set; }
        public bool CustomerHasValidLicense { get; set; }
    }
}