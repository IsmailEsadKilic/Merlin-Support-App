namespace API.enums
{
    public enum UserPermission
    {

        //1-Müşteri Tanimlama ve Düzeltme

        CustomerCreateAndEdit = 101,

        //2-Müşteri Adina Ticket Acma
        CustomerBehalfTicketCreate = 102,

        //3-Müsteri Kullanicisi tanimlayabilir.
        CreateUsersCustomer = 103, // ?

        //4-Ticket Dağitma(Yönetici)
        ToDistributeTicket = 108,

        //5-Ayni ekipdeki kullanicilarin ticketlerini görebilir
        SameTeamInTicketShow = 109,

        //6-Ayni ekipdeki kullanicilarin ticketlerini düzeltebilir
        SameTeamInTicketEdit = 110,

        //7-Kendisine acilan ticketi başkasina atayabilir
        OneSelfTicketForward = 111,
        //8-Kullanici Tanimlayabilir
        CreateUsers = 112,
        //9-Admin
        Admin = 113,
        DeleteTicket = 114,

        //Raporlamada tüm takimi görebilir
        ShowTeamReport = 115,
        LicenseAdd = 116,
        LicenseEdit = 117,
        LicenseDelete = 118,

        AuthorizedUserAdd = 119,
        AuthorizedUserEdit = 120,
        AuthorizedUserDelete = 121,

        DocumentAdd = 122,
        DocumentEdit = 123,
        DocumentDelete = 124,

        CanGetReport = 125,
        DeleteUsers = 126,
        EditUsers = 127,
        DeleteCustomer = 128,




        CanSeeDahboard = 129,
        CanSeeContactCenter = 130,



        CanSeeCallTransfer = 131,
        CanSeeCallTransferReport = 132,

        CanAddPeriodicTicket = 133,
        CanSeeAllTickets = 134,
        CanAssignTicketOnAdd = 135,

        //team controls

        CanCreateTeam = 136,
        CanEditTeam = 137,
        CanRemoveTeam = 138,
        CanDeleteTeam = 139,

        //product controls

        CanCreateProduct = 140,

        CanEditProduct = 141,

        CanRemoveProduct = 142,

        CanDeleteProduct = 143,

        //ticket Type controls

        CanCreateTicketType = 144,

        CanEditTicketType = 145,

        CanRemoveTicketType = 146,

        CanDeleteTicketType = 147,

        //customer product list controls

        CanCreateCustomerProductList = 148,

        CanEditCustomerProductList = 149,

        CanRemoveCustomerProductList = 150,

        CanDeleteCustomerProductList = 151,

        //priority controls

        CanCreatePriority = 152,

        CanEditPriority = 153,

        CanRemovePriority = 154,

        CanDeletePriority = 155,

        
        CanEditTicket = 156,

        //profile controls
        CanCreateProfile = 157,
        CanDeleteProfile = 158

    }

    class UserPermissionHelper
    {
        public static Dictionary<string, string> GetPermissionsDict()
        {
            var dict = new Dictionary<string, string>
            {
                { "101", "Müşteri Tanimlama ve Düzeltme" },

                { "102", "Müşteri Adina Ticket Acma" },

                { "103", "Müsteri Kullanicisi tanimlayabilir." },

                { "108", "Ticket Dağitma(Yönetici)" },

                { "109", "Ayni ekipdeki kullanicilarin ticketlerini görebilir" },
                { "110", "Ayni ekipdeki kullanicilarin ticketlerini düzeltebilir" },

                { "111", "Kendisine acilan ticketi başkasina atayabilir" },

                { "112", "Kullanici Tanimlayabilir" },

                { "113", "Admin" },

                { "114", "Ticket Silme" },

                { "115", "Raporlamada tüm takimi görebilir" },

                { "116", "Lisans Ekleme" },
                { "117", "Lisans Düzenleme" },
                { "118", "Lisans Silme" },

                { "119", "Yetkili Kullanici Ekleme" },
                { "120", "Yetkili Kullanici Düzenleme" },
                { "121", "Yetkili Kullanici Silme" },

                { "122", "Doküman Ekleme" },
                { "123", "Doküman Düzenleme" },
                { "124", "Doküman Silme" },

                { "125", "Rapor Alma" },

                { "126", "Kullanici Silme" },
                { "127", "Kullanici Düzenleme" },

                { "128", "Müşteri Silme" },

                { "129", "Dashboard Görme" },

                { "130", "Contact Center Görme" },

                { "131", "Çağri Aktarma Görme" },
                { "132", "Çağri Aktarma Raporu Görme" },

                { "133", "Periyodik Ticket Ekleme" },

                { "134", "Tüm Ticketleri Görme" },

                { "135", "Ticket Ekleme" },

                { "136", "Takim Ekleme" },
                { "137", "Takim Düzenleme" },
                { "138", "Takim Kaldirma" },
                { "139", "Takim Silme" },

                { "140", "Ürün Ekleme" },
                { "141", "Ürün Düzenleme" },
                { "142", "Ürün Kaldirma" },
                { "143", "Ürün Silme" },

                { "144", "Ticket Tipi Ekleme" },
                { "145", "Ticket Tipi Düzenleme" },
                { "146", "Ticket Tipi Kaldirma" },
                { "147", "Ticket Tipi Silme" },

                { "148", "Müşteri Ürün Lisansi Ekleme" },
                { "149", "Müşteri Ürün Lisansi Düzenleme" },
                { "150", "Müşteri Ürün Lisansi Kaldirma" },
                { "151", "Müşteri Ürün Lisansi Silme" },

                { "152", "Öncelik Ekleme" },
                { "153", "Öncelik Düzenleme" },
                { "154", "Öncelik Kaldirma" },
                { "155", "Öncelik Silme" },

                { "156", "Ticket Düzenleme" },

                { "157", "Profil Ekleme" },
                { "158", "Profil Silme" }
            };

            return dict;
        }


    }

}
