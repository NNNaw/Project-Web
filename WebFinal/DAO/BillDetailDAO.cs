using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFinal.Models;

namespace WebFinal.DAO
{
    public class BillDetailDAO
    {

        readonly EcommerceEntities _db;
        public BillDetailDAO()
        {
            _db = new EcommerceEntities();
        }

        public void InsertBillDetail(tbl_BillDetails bd)
        {
            tbl_BillDetails billdetail = new tbl_BillDetails();
            billdetail.idBill = bd.idBill;
            billdetail.idProduct = bd.idProduct;
            billdetail.quanTum = bd.quanTum;

            _db.tbl_BillDetails.Add(billdetail);
            _db.SaveChanges();
           
        }
    }
}