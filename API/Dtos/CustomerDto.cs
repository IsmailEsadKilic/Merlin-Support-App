namespace API.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string RowGuid { get; set; }
        public string CompanyName { get; set; }
        public string AddressPhone { get; set; }
        public string Fax { get; set; }
        public string TaxOffice { get; set; }
        public string TaxNumber { get; set; }
        public string CustomerEmail { get; set; }
        public string JsonData { get; set; }
        public IList<CustomerProductListDto> CustomerProductListDtos { get; set; } //CustomerProductListDtos
    }
}