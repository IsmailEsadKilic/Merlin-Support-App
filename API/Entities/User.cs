namespace API.Entities
{
    public class User 
    {
        public int Id { get; set; }
        public string RowGuid { get; set; } // this is a typo in the database
        public string NameSurname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Gsml { get; set; }
        public string Permission { get; set; } //'101|102|103' // not permissions
    }
}