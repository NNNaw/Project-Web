using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFinal.Models;


namespace WebFinal.DAO
{
     
    public class BillDAO
    {
        readonly EcommerceEntities _db;
        public BillDAO()
        {
            _db = new EcommerceEntities();
        }

        public int InsertBill(tbl_Bill bd)
        {
            tbl_Bill bill = new tbl_Bill();
            bill.dateCheckin = DateTime.Now;
            bill.statusBill = false ;
            bill.sumPrice = bd.sumPrice;
            bill.idCustomer = bd.idCustomer;
            _db.tbl_Bill.Add(bill);
            _db.SaveChanges();
            return bill.idBill;
        }
        public void ComfirmBillDAO(int id)
        {
            tbl_Bill bill =  _db.tbl_Bill.Single(x => x.idBill == id);
            bill.statusBill = true;
            bill.datecheckout = DateTime.Now;
            _db.SaveChanges();
        }
    }
}