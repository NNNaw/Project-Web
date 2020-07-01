using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class InforUserCustomer
    {
        public int id { get; set; }
        public string username { get; set; }
        public string displayName { get; set; }
        public string phonenumer { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public int typeAccount { get; set; }
    }
}