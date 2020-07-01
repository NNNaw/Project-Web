using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.Models;

namespace WebFinal.Areas.Admin.Controllers
{
    public class AccountAdminController : Controller
    {
        // GET: Admin/AccountAdmin
        EcommerceEntities _dbContext = new EcommerceEntities();
        [HttpGet]
        // GET: Admin
        public ActionResult Login()
        {

            return View();
        }

      //  [HttpPost]
        // Post: Admin
        //public ActionResult Login(tb_Admin admin)
        //{
        //    tb_Admin ad = _dbContext.tb_Admin.Where(x => x.userName == admin.userName && x.password == admin.password).SingleOrDefault();
        //    if (ad != null)
        //    {
        //        Session["idAdmin"] = ad.idAdmin.ToString();
        //        return RedirectToAction("ViewCategory", "View", new { area = "Admin" });
        //    }
        //    else
        //    {
        //        ViewBag.error = "Sai mật khẩu hoặc tài khoản";
        //    }
        //    return View();
        
    }
}