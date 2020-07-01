using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class DetailUser
    {
        public string username { get; set; }
        public string password { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
        public int idTypeAccount { get; set; }



        //customer
        public int idCustomer { get; set; }
        public string addressCustomer { get; set; }
        public string imageCustomer { get; set; }

        //user
        public int idUser { get; set; }
        public string addressUser { get; set; }
        public string avatarUser { get; set; }
        public string coverImage { get; set; }
        public string desUser { get; set; }
       
    }
}