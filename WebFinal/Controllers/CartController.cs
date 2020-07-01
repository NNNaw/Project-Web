using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.DAO;
using WebFinal.Models;

namespace WebFinal.Controllers
{
    public class CartController : Controller
    {
        readonly EcommerceEntities _db = null;
       
        public CartController()
        {
            _db = new EcommerceEntities();
        }
        // GET: Cart
        public ActionResult Index()
        {
            this.getListCate();
            return View();
        }
        public ActionResult AddToCart(int id, int quantum)
        {
            Cart cart = (Cart)Session["cart"];
            ProductDAO DAO =new ProductDAO();
            tbl_Product pro = DAO.getProduct(id);
            if(cart.getListCart().ContainsKey(id))
            {
                cart.AddToCart(id,new Items(pro,cart.getListCart()[id].getNumOrder()),quantum); 
            }else
            {
                cart.AddToCart(id, new Items(pro, quantum), quantum);
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteItem(int id, string returnUrl)
        {
            Cart cart = (Cart)Session["cart"];
            cart.DeleteItem(id);
            return Redirect(returnUrl);
        }
        public void getListCate()
        {
            List<tbl_Category> listCate = _db.tbl_Category.ToList();
            ViewBag.cateList = listCate;
        }
    }
}