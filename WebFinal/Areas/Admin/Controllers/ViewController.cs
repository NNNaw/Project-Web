using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.Models;

namespace WebFinal.Areas.Admin.Controllers
{
    public class ViewController : Controller
    {
        //EmarketingEntities _dbContext = new EmarketingEntities();
        //// GET: Admin/View
        //public ActionResult ViewCategory(int? page)
        //{
        //    int pageSize = 9, pageIndex = 1;
        //    pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    var list = _dbContext.tb_Category.Where(x => x.statusCate == 1).ToList();
        //    IPagedList<tb_Category> listCate = list.ToPagedList(pageIndex, pageSize);
        //    return View(listCate);
        //}
    }
}