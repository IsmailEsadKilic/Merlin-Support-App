using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using API.enums;
using API.CryptologyLibrary.Providers;
using API.CryptologyLibrary.Services;

namespace API.Data
{
    // public class Customer 
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public string CompanyName { get; set; }
    //     public string AddressPhone { get; set; }
    //     public string Fax { get; set; }
    //     public string TaxOffice { get; set; }
    //     public string TaxNumber { get; set; }
    //     public string CustomerEmail { get; set; }
    //     public string JsonData { get; set; }
    // }
    // public class CustomPropertyDescriptor
    // {
    //     public long Id { get; set; }

    //     public string Label { get; set; }

    //     public string DefaultValue { get; set; }

    //     public bool IsRequired { get; set; }

    //     public long UserDefinedDataTemplateId { get; set; }
    // }

    public class SeedData
    {
        public static async Task SeedAdmin(DataContext context)
        {
            if (await context.Users.AnyAsync())
            {
                Console.WriteLine("Admin already exists");
                return;
            }

            ICryptology cryptoMerlin = CryptologyServiceLocator.CryptologyProvider("UserCrypto");
            
            var password = cryptoMerlin.Encrypt("Admin");

            //seed the admin
            var admin = new User
            {
                RowGuid = Guid.NewGuid().ToString(),
                NameSurname = "Admin",
                UserName = "Admin",
                Password = password,
                Email = "",
                Gsml = "",
                //get all permissions
            };

            var permissions = new List<string>();

            var en = UserPermissionHelper.GetPermissionsDict();

            foreach (var permission in en)
            {
                permissions.Add(permission.Key);
            }

            admin.Permission = string.Join("|", permissions);

            context.Users.Add(admin);

            await context.SaveChangesAsync();
        }

        public static async Task SeedCustomers(DataContext context)
        {
            if (await context.Customers.AnyAsync()) return;

            var customers = new List<Customer>
            {
                new Customer
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    CompanyName = "Company 1 (delete me)",
                    AddressPhone = "Address 1 (delete me)",
                    Fax = "Fax 1 (delete me)",
                    TaxOffice = "Tax Office 1 (delete me)",
                    TaxNumber = "Tax Number 1 (delete me)",
                    CustomerEmail = "Customer Email 1 (delete me)",
                    // JsonData = {"Satral Model":"debug","Software Versiyon":"debug","CPU ID":"debug","Pbx IP Adres Main":"debug","Pbx IP Adres CPU 1":"","Pbx IP Adres CPU 2":"","GD IP Adres 1":"","GD IP Adres 2":"","INTIP IP Adress 1":"","INTIP IP Adress 2":"","Node Number":"","MTCL Password":"","ROOT Password":"","SWINST Password":"","RMA Password":"","Modem No 1":"","Modem No 2":"","Uzak Erişim Bilgileri":"","Voice Mail":"","Digital User":"","IP User":"","SIP User":"","Analog User":"","Digital Dış Hat PRI":"","Analog Dış Hat":"","SIP Trunk":"","BRI":"","GAP Dect Dahili Konv":"","Wi-fi Dect User":"","Dect Baz İstasonu":"","4760 veya 8770 Versiyon":"","4760 veya 8770 Password":"","4760 veya 8770 PC Password":"","Automatic Att.":"","Hotel":"","CCS Version":"","Agent Sayısı":"","Networking":"","SPS Süresi":""}
                    JsonData = "{\"Satral Model\":\"debug\",\"Software Versiyon\":\"debug\",\"CPU ID\":\"debug\",\"Pbx IP Adres Main\":\"debug\",\"Pbx IP Adres CPU 1\":\"\",\"Pbx IP Adres CPU 2\":\"\",\"GD IP Adres 1\":\"\",\"GD IP Adres 2\":\"\",\"INTIP IP Adress 1\":\"\",\"INTIP IP Adress 2\":\"\",\"Node Number\":\"\",\"MTCL Password\":\"\",\"ROOT Password\":\"\",\"SWINST Password\":\"\",\"RMA Password\":\"\",\"Modem No 1\":\"\",\"Modem No 2\":\"\",\"Uzak Erişim Bilgileri\":\"\",\"Voice Mail\":\"\",\"Digital User\":\"\",\"IP User\":\"\",\"SIP User\":\"\",\"Analog User\":\"\",\"Digital Dış Hat PRI\":\"\",\"Analog Dış Hat\":\"\",\"SIP Trunk\":\"\",\"BRI\":\"\",\"GAP Dect Dahili Konv\":\"\",\"Wi-fi Dect User\":\"\",\"Dect Baz İstasonu\":\"\",\"4760 veya 8770 Versiyon\":\"\",\"4760 veya 8770 Password\":\"\",\"4760 veya 8770 PC Password\":\"\",\"Automatic Att.\":\"\",\"Hotel\":\"\",\"CCS Version\":\"\",\"Agent Sayısı\":\"\",\"Networking\":\"\",\"SPS Süresi\":\"\"}"
                },
                new Customer
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    CompanyName = "Company 2 (delete me)",
                    AddressPhone = "Address 2 (delete me)",
                    Fax = "Fax 2 (delete me)",
                    TaxOffice = "Tax Office 2 (delete me)",
                    TaxNumber = "Tax Number 2 (delete me)",
                    CustomerEmail = "Customer Email 2 (delete me)",
                    // JsonData = "json data 2 (delete me)"
                    JsonData = "{\"Satral Model\":\"debug\",\"Software Versiyon\":\"debug\",\"CPU ID\":\"debug\",\"Pbx IP Adres Main\":\"debug\",\"Pbx IP Adres CPU 1\":\"\",\"Pbx IP Adres CPU 2\":\"\",\"GD IP Adres 1\":\"\",\"GD IP Adres 2\":\"\",\"INTIP IP Adress 1\":\"\",\"INTIP IP Adress 2\":\"\",\"Node Number\":\"\",\"MTCL Password\":\"\",\"ROOT Password\":\"\",\"SWINST Password\":\"\",\"RMA Password\":\"\",\"Modem No 1\":\"\",\"Modem No 2\":\"\",\"Uzak Erişim Bilgileri\":\"\",\"Voice Mail\":\"\",\"Digital User\":\"\",\"IP User\":\"\",\"SIP User\":\"\",\"Analog User\":\"\",\"Digital Dış Hat PRI\":\"\",\"Analog Dış Hat\":\"\",\"SIP Trunk\":\"\",\"BRI\":\"\",\"GAP Dect Dahili Konv\":\"\",\"Wi-fi Dect User\":\"\",\"Dect Baz İstasonu\":\"\",\"4760 veya 8770 Versiyon\":\"\",\"4760 veya 8770 Password\":\"\",\"4760 veya 8770 PC Password\":\"\",\"Automatic Att.\":\"\",\"Hotel\":\"\",\"CCS Version\":\"\",\"Agent Sayısı\":\"\",\"Networking\":\"\",\"SPS Süresi\":\"\"}"
                },
            };

            foreach (var customer in customers)
            {
                context.Customers.Add(customer);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedTeams(DataContext context)
        {

            //     public class Team
            //     {
            //         public int Id { get; set; }
            //         public string RowGuid { get; set; }  //this is a typo in the database as well
            //         public string TeamName { get; set; }
            //         ICollection<TeamMember> TeamMembers { get; set; }
            //     }

            //     public class TeamMember
            //     {
            //         public int Id { get; set; }
            //         public int TeamId { get; set; }
            //         public int UserId { get; set; }
            //     }

            if (await context.Teams.AnyAsync()) return;

            var teams = new List<Team>
            {
                new Team
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    TeamName = "Team 1 (delete me)",
                },
                new Team
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    TeamName = "Team 2 (delete me)",
                },
            };

            foreach (var team in teams)
            {
                context.Teams.Add(team);
            }

            await context.SaveChangesAsync();

            var teamMembers = new List<TeamMember>
            {
                new TeamMember
                {
                    TeamId = 1,
                    UserId = 1
                },
                new TeamMember
                {
                    TeamId = 2,
                    UserId = 1
                },
            };

            foreach (var teamMember in teamMembers)
            {
                context.TeamMembers.Add(teamMember);
            }

            await context.SaveChangesAsync();

        }

        public static async Task SeedTicketTypes(DataContext context)
        {
            if (await context.TicketTypes.AnyAsync()) return;

            // public int Id { get; set; }
            // public string RowGuid { get; set; }
            // public string Name { get; set; }
            // public int ListIndex { get; set; }

            var ticketTypes = new List<TicketType>
            {
                new TicketType
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    Name = "Ticket Type 1 (delete me)",
                    ListIndex = 1
                },
                new TicketType
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    Name = "Ticket Type 2 (delete me)",
                    ListIndex = 2
                },
            };

            foreach (var ticketType in ticketTypes)
            {
                context.TicketTypes.Add(ticketType);
            }

            await context.SaveChangesAsync();

        }

        public static async Task SeedPriorities(DataContext context)
        {
            if (await context.Priorities.AnyAsync()) return;

            // public int Id { get; set; }
            // public string RowGuid { get; set; }
            // public string PriorityName { get; set; }
            // public TimeType TimeType { get; set; }
            // public int Time { get; set; }
            // public int ListIndex { get; set; }

            var priorities = new List<Priority>
            {
                new Priority
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    PriorityName = "Priority 1 (delete me)",
                    TimeType = TimeType.Minutes,
                    Time = 1,
                    ListIndex = 1
                },
                new Priority
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    PriorityName = "Priority 2 (delete me)",
                    TimeType = TimeType.Days,
                    Time = 2,
                    ListIndex = 2
                },
            };

            foreach (var priority in priorities)
            {
                context.Priorities.Add(priority);
            }

            await context.SaveChangesAsync();

        }

                public static async Task SeedProducts(DataContext context)
        {
                //             public class Product
                // {
                //     public int Id { get; set; }
                //     public string ProductName { get; set; }
                //     public string RowGuid { get; set; }
                    
                // }

            if (await context.Products.AnyAsync()) return;

            var products = new List<Product>
            {
                new Product
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    ProductName = "Product 1 (delete me)",
                },
                new Product
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    ProductName = "Product 2 (delete me)",
                },
            };

            foreach (var product in products)
            {
                context.Products.Add(product);
            }

            await context.SaveChangesAsync();
        }

        
        public static async Task SeedCustomerProductLists(DataContext context)
        {
                //             public class CustomerProductList
                // {
                //     public int Id { get; set; }
                //     public string RowGuid { get; set; }
                //     public int ProductId { get; set; }
                //     public string Version { get; set; }
                //     public DateTime FirstDate { get; set; }
                //     public DateTime EndDate { get; set; }
                //     public int CustomerId { get; set; }
                //     public string Description { get; set; }
                // }

            if (await context.CustomerProductLists.AnyAsync()) return;

            var customerProductLists = new List<CustomerProductList>
            {
                new CustomerProductList
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    ProductId = 1,
                    Version = "Version 1 (delete me)",
                    FirstDate = DateTime.Now - TimeSpan.FromDays(365),
                    EndDate = DateTime.Now + TimeSpan.FromDays(365),
                    CustomerId = 1,
                    Description = "Description 1 (delete me)"
                },
                new CustomerProductList
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    ProductId = 2,
                    Version = "Version 2 (delete me)",
                    FirstDate = DateTime.Now - TimeSpan.FromDays(365),
                    EndDate = DateTime.Now + TimeSpan.FromDays(365),
                    CustomerId = 2,
                    Description = "Description 2 (delete me)"
                },
            };

            foreach (var customerProductList in customerProductLists)
            {
                context.CustomerProductLists.Add(customerProductList);
            }

            await context.SaveChangesAsync();
            
        }

    //         public class Ticket
    // {
    //     public int Id { get; set; }
    //     public string RowGuid { get; set; }
    //     public int CustomerTicketId { get; set; }
    //     public DateTime RecordDate { get; set; }
    //     public DateTime ClosedDate { get; set; }
    //     public DateTime FirstResponseTime { get; set; }
    //     public string Subject { get; set; }
    //     public string Description { get; set; }
    //     public int ProductId { get; set; }
    //     public int TeamId { get; set; }
    //     public int PriorityId { get; set; }
    //     public int RecordUserId { get; set; }
    //     public int CustomerId { get; set; }
    //     public int TicketStateId { get; set; }
    // }

    public static async Task SeedTickets(DataContext context)
        {
            if (await context.Tickets.AnyAsync()) return;

            var tickets = new List<Ticket>
            {
                new Ticket
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    CustomerTicketId = 1,
                    RecordDate = DateTime.Now,
                    ClosedDate = DateTime.Now,
                    FirstResponseTime = DateTime.Now,
                    Subject = "Subject 1 (delete me)",
                    Description = "Description 1 (delete me)",
                    ProductId = 1,
                    TeamId = 1,
                    UserId = 1,
                    PriorityId = 1,
                    CustomerId = 1,
                    TicketStateId = 1,
                    TicketTypeId = 2
                },
                new Ticket
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    CustomerTicketId = 2,
                    RecordDate = DateTime.Now,
                    ClosedDate = DateTime.Now,
                    FirstResponseTime = DateTime.Now,
                    Subject = "Subject 2 (delete me)",
                    Description = "Description 2 (delete me)",
                    ProductId = 2,
                    TeamId = 2,
                    UserId = 1,
                    PriorityId = 2,
                    CustomerId = 2,
                    TicketStateId = 2,
                    TicketTypeId = 1
                },
            };

            foreach (var ticket in tickets)
            {
                context.Tickets.Add(ticket);
            }

            await context.SaveChangesAsync();
        }

        public static async Task SeedPermissionProfiles(DataContext context)
        {
            if (await context.Profiles.AnyAsync()) return;

            var permissionProfiles = new List<PermissionProfile>
            {
                new PermissionProfile
                {
                    ProfileName = "everything (delete me)",
                    Permission = "1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16|17|18|19|20|21|22|23|24|25|26|27|28|29|30|31|32|33|34|35|36|37|38|39|40|41|42|43|44|45|46|47|48|49|50|51|52|53|54|55|56|57|58|59|60|61|62|63|64|65|66|67|68|69|70|71|72|73|74|75|76|77|78|79|80|81|82|83|84|85|86|87|88|89|90|91|92|93|94|95|96|97|98|99|100|101|102|103|104|105|106|107|108|109|110|111|112|113|114|115|116|117|118|119|120|121|122|123|124|125|126|127|128|129|130|131|132|133|134|135|136|137|138|139|140|141|142|143|144|145|146|147|148|149|150|151|152|153|154|155|156|157|158|159|160|161|162|163|164|165|166|167|168|169|170|171|172|173|174|175|176|177|178|179|180|181|182|183|184|185|186|187|188|189|190|191|192|193|194|195|196|197|198|199|200|201|202|203|204|205|206|207|208|209|210|211|212|213|214|215|216|217|218|219|220|221|"
                },
                new PermissionProfile
                {
                    ProfileName = "ProductControls (delete me)",
                    Permission = "140|141|142|143"
                },
            };

            foreach (var permissionProfile in permissionProfiles)
            {
                context.Profiles.Add(permissionProfile);
            }

            await context.SaveChangesAsync();
        }

    }
}