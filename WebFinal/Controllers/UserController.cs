using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.Models;
using PagedList;
using System.Data.Entity.Validation;
using System.Data.Entity;
using System.Configuration;
using WebFinal.DAO;
using WebFinal.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace WebFinal.Controllers
{
    public class UserController : Controller
    {

        readonly EcommerceEntities _db = null;
        public UserController()
        {
            _db = new EcommerceEntities();

            //var inforUser = checkCookie();
            //if (inforUser != null)
            //{
            //    var u = new UserDAO();
            //    tb_User User = u.getInfo(inforUser);
            //    var userSession = new InfoUserLogin();
            //    userSession.idUser = User.idUser;
            //    userSession.nameUser = User.displayName;
            //    Session.Add(commonConstrans.USER_SESSION, userSession);
            //}
        }



        [HttpGet]
        // GET: User


        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        // Post: Admin
        public ActionResult Login(UserLogin user)
        {
            UserDAO check = new UserDAO();

            if (check.Login(user))
            {
                var userSession = new InfoUserLogin();
                var infoUser = check.getInfo(user);
                userSession.username = infoUser.username;
                userSession.password = infoUser.password;
                userSession.displayName = infoUser.displayName;
                userSession.typeAccount = infoUser.idTypeAccount;
                userSession.idUser = check.getIdUser(userSession.username, userSession.typeAccount);
                Session.Add(commonConstrans.USER_SESSION, userSession);
                if (user.remmeberMe)
                {
                    //lưu vào sesstion
                    Response.Cookies["username"].Value = user.username;
                    Response.Cookies["password"].Value = user.passWord;
                }

                if (infoUser.idTypeAccount == 1)
                {
                    return RedirectToAction("Index", "Add", new { area = "Admin" });
                }
                else if (infoUser.idTypeAccount == 2)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Home", "Customer");
                }
            }
            else
            {
                ViewBag.error = "Sai mật khẩu hoặc tài khoản";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Index()
        {

            InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
            if (userLogin == null) return RedirectToAction("Login");
            //var inforUser = checkCookie();

            //if (inforUser != null || userLogin.username != null)
            //{
            //    var account = new UserDAO();
            //    tbl_Account Acc = account.getInfo(inforUser);
            //    var userSession = new InfoUserLogin();
            //    userSession.username = Acc.username;
            //    var user = _db.tbl_User.Single(x =>x.username == Acc.username);
            //    userSession.idUser = user.idUser;
            //    userSession.displayName = Acc.displayName;
            //    userSession.typeAccount = Acc.idTypeAccount;
            //    Session.Add(commonConstrans.USER_SESSION, userSession);
            //}
            //else
            //{
            //    return RedirectToAction("Login");
            //}

            List<ViewProductDetail> list = new List<ViewProductDetail>();

            //Lấy ra danh sách phẩm có cùng idUser
            var listPro = _db.tbl_Product.Where(x => x.idUser == userLogin.idUser).ToList();
            var model = from cate in _db.tbl_Category
                        join pro in _db.tbl_Product
                        on cate.idCate equals pro.idCate
                        select new ViewProductDetail()
                        {
                            idProduct = pro.idProduct,
                            idCate = pro.idCate,
                            nameProduct = pro.nameProduct,
                            nameCate = cate.nameCate,
                            imageProduct = pro.imageProduct,
                            priceProduct = pro.priceProduct,
                            QuanTum = pro.quantum,
                            status = pro.status
                        };


            List<tbl_Category> listCate = _db.tbl_Category.ToList();
            ViewBag.cateList = listCate;//new SelectList(listCate, "idCate", "nameCate");

            return View(model.ToList());

        }
        [HttpPost]
        public ActionResult Index(string nameProduct, string desProduct, int priceProduct, int rateProduct, int quantum, int idCate, HttpPostedFileBase imgfile)
        {
            string path = UploadimgFile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Lỗi định dạng ...";

            }
            else
            {
                InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
             
                tbl_Product pro = new tbl_Product();
                pro.nameProduct = nameProduct;
                pro.imageProduct = path;
                pro.priceProduct =priceProduct;
                pro.desProduct =desProduct;
                pro.idUser = userLogin.idUser;
                pro.idCate = idCate;
                pro.quantum =quantum;
                pro.rateProduct = rateProduct;
                pro.status = true;
                _db.tbl_Product.Add(pro);
                _db.SaveChanges();

              
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult ModalViewProduct(int id = 0)
        {
          
            if (id == 0)
            {
                var model = new ViewProductDetail();
                //{
                //    Categoies = _db.tbl_Category.ToList()
                //};
                this.getlistCate(id);
                return View(model);
            }
               
            else
            {
                using (EcommerceEntities db = new EcommerceEntities())
                {
                    var list = from pro in _db.tbl_Product
                                join cate in _db.tbl_Category
                                on pro.idCate equals cate.idCate
                                select new ViewProductDetail()
                                {
                                    idProduct = pro.idProduct,
                                    idCate = cate.idCate,
                                    nameProduct = pro.nameProduct,
                                    nameCate = cate.nameCate,
                                    imageProduct = pro.imageProduct,
                                    desProduct = pro.desProduct,
                                    rateProduct = pro.rateProduct,
                                    priceProduct = pro.priceProduct,
                                    QuanTum = pro.quantum,
                                    status = pro.status,
                                };
                    ViewProductDetail product = list.Single(x => x.idProduct == id);
                    this.getListStatus(product.status.GetValueOrDefault());
                    this.getlistCate(product.idCate);
                    return View(product);
                }
            }
        }

        public void getlistCate(int id)
        {
            var listCate = _db.tbl_Category.ToList();
            
           var cateList = new SelectList(listCate, "idCate", "nameCate");
         
            if (id == 0)
            {
                ViewBag.cateList = cateList;

                return;
            }
            else
            {
                var selected = cateList.Where(x => x.Value == id.ToString()).First();
                selected.Selected = true;
                ViewBag.cateList = cateList;
            }


        }
        public void getListStatus(bool isTrue)
        {
            List<bool> listStatus = new List<bool>();
            listStatus.Add(true);
            listStatus.Add(false);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (var item in listStatus)
            {
                items.Add(new SelectListItem()
                {
                    Text = item == true ? "Còn Hàng" : "Hết Hàng",
                    Value = item.ToString(),
                    // Put all sorts of business logic in here
                  
                });
            }
            var selected = items.Where(x => x.Value == isTrue.ToString()).First();
            selected.Selected = true;
            ViewBag.StatusPro = items;
        }

        [HttpPost]
        public ActionResult ModalViewProduct(ViewProductDetail pro, HttpPostedFileBase imageProduct)
        {
           

                InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];

                // Xử lý thêm sản phẩm
                if (pro.idProduct == 0)
                {
                    string path = UploadimgFile(imageProduct);
                    if (path.Equals("-1"))
                    {
                        ViewBag.error = "Lỗi định dạng ...";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        tbl_Product product = new tbl_Product();
                        product.status = true;
                        product.nameProduct = pro.nameProduct;
                        product.priceProduct = pro.priceProduct;
                        product.quantum = pro.QuanTum;
                        product.rateProduct = pro.rateProduct;
                        product.idUser = userLogin.idUser;
                        product.imageProduct = path;
                        product.desProduct = pro.desProduct;
                        product.idCate = pro.idCate;
                        _db.tbl_Product.Add(product);
                        _db.SaveChanges();
                        
                        return RedirectToAction("Index");
                    }
                }
                else  // Xử lý Sửa sản phẩm
                {
                    
                tbl_Product product = _db.tbl_Product.Single(x => x.idProduct == pro.idProduct);
                if (imageProduct == null) // không thay đổi ảnh
                    {
                        product.nameProduct = pro.nameProduct;
                        product.priceProduct = pro.priceProduct;
                        product.quantum = pro.QuanTum;
                        product.rateProduct = pro.rateProduct;
                        product.idUser = userLogin.idUser;
                        
                        product.desProduct = pro.desProduct;
                        product.idCate = pro.idCate;
                        product.status = pro.status;

                     
                        _db.SaveChanges();
                        return RedirectToAction("Index");


                    }
                    else //có thay đổi ảnh
                    {
                        string path = UploadimgFile(imageProduct);
                        if (path.Equals("-1"))
                        {
                            ViewBag.error = "Lỗi định dạng ...";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                          
                            product.imageProduct = path;

                            product.nameProduct = pro.nameProduct;
                            product.priceProduct = pro.priceProduct;

                            product.quantum = pro.QuanTum;
                            product.rateProduct = pro.rateProduct;
                            product.idUser = userLogin.idUser;

                            product.desProduct = pro.desProduct;
                            product.idCate = pro.idCate;
                            product.status = pro.status;
                           
                            _db.SaveChanges();

                            return RedirectToAction("Index");

                        }

                    }


                }

            
           

        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                using (EcommerceEntities db = new EcommerceEntities())
                {
                    tbl_Product emp = db.tbl_Product.Where(x => x.idProduct == id).FirstOrDefault<tbl_Product>();
                    db.tbl_Product.Remove(emp);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                //TempData["message"] = "Your Message";
                return Content("<script language='javascript' type='text/javascript'>alert('k duoc xoa!');</script>");
            }
            return RedirectToAction("Index");
        }
        public ActionResult IndexBill()
        {
            var list = from bill in _db.tbl_Bill
                       join customer in _db.tbl_Customer
                       on bill.idCustomer equals customer.idCustomer
                       select new ViewBill()
                       {
                           idBill = bill.idBill,
                           username = customer.username,
                           sumPrice = bill.sumPrice,
                           dateCheckin = bill.dateCheckin,
                           datecheckout = bill.datecheckout,
                           statusBill = bill.statusBill
                       };
            return View(list.ToList());
        }
       
       
        public UserLogin checkCookie()
        {
            UserLogin userLogin = null;
            string username = string.Empty, passWord = string.Empty;

            if (Response.Cookies["username"] != null)
            {
                username = Request.Cookies["username"].Value;

            }
            if (Response.Cookies["password"] != null)
            {
                passWord = Request.Cookies["password"].Value;
            }

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(passWord))
            {
                userLogin = new UserLogin { username = username, passWord = passWord, remmeberMe = true };
            }
            return userLogin;
        }
        public ActionResult SignOut()
        {
            Session.Clear();
            HttpContext.Application.Clear();
            return RedirectToAction("Login");
        }
        public JsonResult DetailPro(int id)
        {

            var DetailProduct = _db.tbl_Product.Single(x =>x.idProduct == id);
           
            return Json(new { data = DetailProduct},JsonRequestBehavior.AllowGet);
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
       



    }

}