using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFinal.Models;

namespace WebFinal.DAO
{
    public class ProductDAO
    {
        readonly EcommerceEntities _db;
        public ProductDAO()
        {
            _db = new EcommerceEntities();
        }
        public tbl_Product getProduct(int id)
        {
            tbl_Product pro = _db.tbl_Product.Single(x =>x.idProduct == id);
            return pro;
        }
    }
}