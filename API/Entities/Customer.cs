namespace API.Entities
{
    public class Customer 
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
        public IList<CustomerProductList> CustomerProductLists { get; set; }
        public IList<Ticket> Tickets { get; set; }
    }

    public class CustomPropertyDescriptor
    {
        public long Id { get; set; }

        public string Label { get; set; }

        public string DefaultValue { get; set; }

        public bool IsRequired { get; set; }
    }
}