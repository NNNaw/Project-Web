using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.Common;
using WebFinal.DAO;
using WebFinal.Models;

namespace WebFinal.Controllers
{
    public class CustomerController : Controller
    {
        readonly EcommerceEntities _db = null;
        public CustomerController()
        {
            _db = new EcommerceEntities();
        }
            // GET: Customer
        public ActionResult Home(int? id,int? page , string search)
        {
            int pageSize = 9, pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            this.getListCate();

            if (id.GetValueOrDefault() == 0 && search == null) //Lấy ra hết
            {
                var list = _db.tbl_Product.Where(x => x.quantum >= 1 && x.status == true).OrderByDescending(x => x.rateProduct).ToList();
                IPagedList<tbl_Product> listPro = list.ToPagedList(pageIndex, pageSize);
                return View(listPro);
              
            }
           else if (id.GetValueOrDefault() > 0 && search == null) //Lấy theo idCate
            {
                var listID = _db.tbl_Product.Where(x => x.idCate == id && x.quantum >= 1 && x.status == true).OrderByDescending(x => x.rateProduct).ToList();
                IPagedList<tbl_Product> listIDPro = listID.ToPagedList(pageIndex, pageSize);
                return View(listIDPro);
            }
            else if(id.GetValueOrDefault() == 0 && search != null) //Lấy theo key
            {
                var listIdNull = _db.tbl_Product.Where(x => x.desProduct.Contains(search) && x.quantum >= 1 && x.status == true).ToList();
                IPagedList<tbl_Product> ListIdNull = listIdNull.ToPagedList(pageIndex, pageSize);
                if (listIdNull.Count == 0)
                {
                    ViewBag.error = "Không có sản phẩm nào '" + search + "'";
                }
                return View(ListIdNull);
            }
            else if (id.GetValueOrDefault() >= 0 && search != null)
            {
                var listIdNull = _db.tbl_Product.Where(x => x.desProduct.Contains(search) && x.quantum >= 1 && x.status == true).ToList();
                IPagedList<tbl_Product> ListIdNull = listIdNull.ToPagedList(pageIndex, pageSize);
                if (listIdNull.Count == 0)
                {
                    ViewBag.error = "Không có sản phẩm nào '" + search + "'";
                }
                return View(ListIdNull);
            }
            else
            {
                var ListAll = _db.tbl_Product.Where(x =>  x.quantum >= 1 && x.status == true).ToList();
                IPagedList<tbl_Product> listAll = ListAll.ToPagedList(pageIndex, pageSize);
              
                return View(listAll);
            }
           
        }

        public ActionResult InfoCustomer()
        {
            InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
            if (userLogin == null) return Redirect("Home");
            this.getListCate();
          
            DetailUser user = new DetailUser();
            var model = from acc in _db.tbl_Account
                       join customer in _db.tbl_Customer
                       on acc.username equals customer.username
                        select new DetailUser()
                       {
                           idCustomer = customer.idCustomer,
                           username = customer.username,
                           addressCustomer = customer.addressCustomer,
                           imageCustomer = customer.imageCustomer,
                           password = acc.password,
                           displayName = acc.displayName,
                           email = acc.email,
                           phonenumber = acc.phonenumber
                       };
            foreach (var item in model)
            {
                if(item.idCustomer == userLogin.idUser)
                {
                    user = item;
                    break;
                }
            }
            return View(user);
        }

        public ActionResult UpdateCustomer(DetailUser detailUser,HttpPostedFileBase imgfile)
        {
            InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
            if (detailUser == null) return RedirectToAction("InfoCustomer");

            tbl_Customer customer = _db.tbl_Customer.Where(x => x.idCustomer == detailUser.idCustomer).FirstOrDefault();
            tbl_Account account = _db.tbl_Account.Single(x => x.username == detailUser.username);

            if (imgfile == null )
            {
                account.password = detailUser.password;
                account.phonenumber = detailUser.phonenumber;
                account.displayName = detailUser.displayName;
                account.email = detailUser.email;

                customer.addressCustomer = detailUser.addressCustomer;
                _db.SaveChanges();
                return RedirectToAction("InfoCustomer");
            }
            else
            {
                string path = UploadimgFile(imgfile);
                if (path.Equals("-1"))
                {
                    ViewBag.error = "Lỗi định dạng ...";
                }
                else
                {
                    account.password = detailUser.password;
                    account.phonenumber = detailUser.phonenumber;
                    account.displayName = detailUser.displayName;
                    account.email = detailUser.email;
                    customer.imageCustomer = path;
                    customer.addressCustomer = detailUser.addressCustomer;
                    _db.SaveChanges();
                    return RedirectToAction("InfoUser");
                }
            }

            return RedirectToAction("InfoCustomer");
        }
        public ActionResult ManageBills()
        {
            InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
            if (userLogin == null) return Redirect("Home");
            this.getListCate();
          
            var listBil = _db.tbl_Bill.Where(x => x.idCustomer == userLogin.idUser).ToList();
            
            return View(listBil);
        }

        [HttpGet]
        public ActionResult SignUp()
        {

            return View();
        }

        [HttpPost]
        // Post: custormer
        public ActionResult SignUp(tbl_Account account)
        {
         
                try
                {
                    UserDAO valid = new UserDAO();
                    if (ModelState.IsValid)
                     {
                        if(valid.checkRegister(account) == "")
                        {

                            tbl_Account acc = new tbl_Account();
                            acc.displayName = account.displayName;
                            acc.email = account.email;
                            acc.password = account.password;
                            acc.phonenumber = account.phonenumber;
                            acc.idTypeAccount = 3;
                            acc.username = account.username;
                            _db.tbl_Account.Add(acc);
                            _db.SaveChanges();
                            return RedirectToAction("Login","User");
                        }
                        else
                        {
                            ViewBag.Error = valid.checkRegister(account);
                            return View();
                        }
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
            return View();
        }

        public string UploadimgFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/Upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/Upload/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception)
                    {

                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Chỉ chọn jpg , jpeg, png ...');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Xin chọn file...');</script>");
                path = "-1";
            }

            return path;
        }

        public ActionResult DetailPro(int id)
        {
            ViewProductDetail view = new ViewProductDetail();
            tbl_Product pro = _db.tbl_Product.Where(x => x.idProduct == id).FirstOrDefault();
            if (pro == null) return RedirectToAction("Home");
            view.idProduct = pro.idProduct;
            view.desProduct = pro.desProduct;
            view.priceProduct = pro.priceProduct;
            view.imageProduct = pro.imageProduct;
            view.nameProduct = pro.nameProduct;
            view.status = pro.status;
            view.QuanTum= pro.quantum;
            view.rateProduct = pro.rateProduct;

            tbl_Category cate = _db.tbl_Category.Where(x => x.idCate == pro.idCate).FirstOrDefault();
            view.nameCate = cate.nameCate;
            view.idCate = cate.idCate;

            tbl_User user = _db.tbl_User.Where(x => x.idUser == pro.idUser).FirstOrDefault();
            view.username = user.username;
            view.coverimage = user.coverImage;
            view.desUser = user.desUser;
            view.addressUser = user.addressUser;
            view.imageUser = user.avatarUser;

            tbl_Account acc = _db.tbl_Account.Where(x => x.username == user.username).FirstOrDefault();
            view.displayName = acc.displayName;
            view.email = acc.email;
            view.phoneUser = acc.phonenumber;

            this.getListStatus(pro);
            this.getListCate();
            return View(view);
        }

        public void getListCate()
        {
            List<tbl_Category> listCate = _db.tbl_Category.ToList();
            ViewBag.cateList = listCate;
        }
        public void getListStatus(tbl_Product pro)
        {
            List<bool> listStatus = new List<bool>();
            listStatus.Add(true);
            listStatus.Add(false);
            var items = new List<SelectListItem>();
            foreach (var item in listStatus)
            {
                items.Add(new SelectListItem()
                {
                    Text = item == true ? "Còn Hàng" : "Hết Hàng",
                    Value = item.ToString(),
                    // Put all sorts of business logic in here
                    Selected = pro.status == true ? true : false
                });
            }

            ViewBag.StatusPro = items;
        }
    }
}