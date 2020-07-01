using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.Common;
using WebFinal.DAO;
using WebFinal.Models;

namespace WebFinal.Controllers
{
    public class BillController : Controller
    {
        readonly EcommerceEntities _db = null;
        public BillController()
        {
            _db = new EcommerceEntities();
        }
        // GET: Bill
        public ActionResult Paying()
        {
            InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
            if (userLogin == null) return RedirectToAction("Login","User");
            Cart cart = (Cart)Session["cart"];
           
            tbl_Bill bd =  new tbl_Bill();
            bd.idCustomer = userLogin.idUser;
            int sum = 0;
            foreach (var item in cart.getListCart().Values)
            {
                sum += item.numOrder * item.getProduct().priceProduct.GetValueOrDefault();
            }
            bd.sumPrice = sum;
            int id = new BillDAO().InsertBill(bd);

            //insert billdetail
            BillDetailDAO billDetailDAO = new BillDetailDAO();
            foreach (var item in cart.getListCart().Values)
            {
                tbl_BillDetails billDetails = new tbl_BillDetails();
                billDetails.idBill = id;
                billDetails.idProduct = item.getProduct().idProduct;
                billDetails.quanTum = item.numOrder;
                billDetailDAO.InsertBillDetail(billDetails);
               
            }
            cart = new Cart();
            Session["cart"] = cart;
            return RedirectToAction("Index","Cart");
        }

        public ActionResult ComfirmBill(int id)
        {
            BillDAO bill = new BillDAO();
            bill.ComfirmBillDAO(id);
            return RedirectToAction("IndexBill","User");
        }
        [HttpGet]
        public ActionResult ViewModalBills(int id)
        {
            ViewBill bdetail = new ViewBill();
            var list = from bill in _db.tbl_Bill
                       join bd in _db.tbl_BillDetails
                       on bill.idBill equals bd.idBill
                       select new ViewBill()
                       {
                           idBill = bill.idBill,

                           sumPrice = bill.sumPrice,
                           quantum = bd.quanTum,

                           idProduct = bd.idProduct,

                       };
            foreach (var item in list)
            {
                if (item.idBill == id)
                {
                    var temp = _db.tbl_Product.Single(x => x.idProduct == item.idProduct);
                    item.nameProduct = temp.nameProduct;
                    item.priceProduct = temp.priceProduct.GetValueOrDefault();
                    item.totalPrice = temp.priceProduct.GetValueOrDefault() * item.quantum;
                    bdetail = item;
                    break;
                }

            }

            return View(bdetail);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (EcommerceEntities db = new EcommerceEntities())
            {
                tbl_Bill bill = db.tbl_Bill.Where(x => x.idBill == id).FirstOrDefault<tbl_Bill>();

                DeleteBillsDetail(bill.idBill);

                db.tbl_Bill.Remove(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public void DeleteBillsDetail(int idBill)
        {
            var listBD = _db.tbl_BillDetails.Where(x => x.idBill == idBill).ToList();
            foreach (var item in listBD)
            {
                _db.tbl_BillDetails.Remove(item);
                _db.SaveChanges();
            }
        }
    }
}