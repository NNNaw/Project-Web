using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class ViewBill
    {
        public int idBill { get; set; }
        public int idProduct { get; set; }
        public Nullable<System.DateTime> dateCheckin { get; set; }
        public Nullable<System.DateTime> datecheckout { get; set; }
        public Nullable<bool> statusBill { get; set; }
        public int sumPrice { get; set; } // tỗng  hết các chi tiết 
        public string username{ get; set; }
       
        public string nameProduct { get; set; }
        public int quantum { get; set; }
        public int totalPrice { get; set; }
        public int priceProduct { get; set; }

    }
}