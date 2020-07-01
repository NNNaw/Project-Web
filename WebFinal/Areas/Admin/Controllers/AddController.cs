using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebFinal.Common;
using WebFinal.DAO;
using WebFinal.Models;

namespace WebFinal.Areas.Admin.Controllers
{
    public class AddController : Controller
    {

        EcommerceEntities _db = null;
        public AddController()
        {
            _db = new EcommerceEntities();
        }
        //public ActionResult CreateCate()
        //{
        //    if (Session["idAdmin"] == null)
        //        return RedirectToAction("Login");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult CreateCate(tb_Category cate, HttpPostedFileBase imgfile)
        //{
        //    string path = uploadimgFile(imgfile);
        //    if (path.Equals("-1"))
        //    {
        //        ViewBag.error = "Lỗi định dạng ...";

        //    }
        //    else
        //    {
        //        tb_Category cat = new tb_Category();
        //        cat.nameCate = cate.nameCate;
        //        cat.imageCate = path;
        //        cat.statusCate = 1;
        //        cat.adminCate = Convert.ToInt32(Session["idAdmin"].ToString());
        //        _dbContext.tb_Category.Add(cat);
        //        _dbContext.SaveChanges();
        //        return RedirectToAction("CreateCate");
        //    }
        //    return View();
        //}


        public ActionResult Index()
        {
            //Lấy ra danh sách phẩm có cùng idUser
          //  var listPro = _db.tbl_Product.Where(x => x.idUser == userLogin.idUser).ToList();
            var modelUser = from acc in _db.tbl_Account
                        join user in _db.tbl_User
                        on acc.username equals user.username
                        select new InforUserCustomer()
                        {
                           id = user.idUser,
                            username = user.username,
                           displayName = acc.displayName,
                           email = acc.email,
                           address = user.addressUser,
                           phonenumer = acc.phonenumber,
                           typeAccount = acc.idTypeAccount,

                        };

            var modelCustomer = from acc in _db.tbl_Account
                            join user in _db.tbl_Customer
                            on acc.username equals user.username
                            select new InforUserCustomer()
                            {
                                id = user.idCustomer,
                                username = user.username,
                                displayName = acc.displayName,
                                email = acc.email,
                                address = user.addressCustomer,
                                phonenumer = acc.phonenumber,
                                typeAccount = acc.idTypeAccount,

                            };
            List<InforUserCustomer> list = new List<InforUserCustomer>();
            list.AddRange(modelCustomer);
            list.AddRange(modelUser);
            return View(list);
           
        }
        [HttpPost]
        public ActionResult Index(string username,string password,string displayname, string email,string phonenumber)
        {
            tbl_Account account = new tbl_Account();
            account.displayName = displayname;
            account.email = email;
            account.username = username;
            account.password = password;
            account.phonenumber = phonenumber;

            UserDAO valid = new UserDAO();
            if (ModelState.IsValid)
            {
                if (valid.checkRegister(account) == "")
                {

                    tbl_Account acc = new tbl_Account();
                    acc.displayName = account.displayName;
                    acc.email = account.email;
                    acc.password = account.password;
                    acc.phonenumber = account.phonenumber;
                    acc.idTypeAccount = 2;
                    acc.username = account.username;
                  

                    _db.tbl_Account.Add(acc);
                    
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = valid.checkRegister(account);
                    return View();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult IndexCate()
        {
            var modelCate = from add in _db.tbl_Admin
                                join cate in _db.tbl_Category
                                on add.idAdmin equals cate.idAdmin
                                select new ViewDetailCate()
                                {
                                   idCate = cate.idCate,
                                   nameCate = cate.nameCate,
                                   Admin = add.username,
                                   quantumCate = cate.quantumCate,
                                   imageCate = cate.imageCate

                                };
            return View(modelCate.ToList());
        }

        public ActionResult DeleteUser(string id)
        {
            tbl_User acc = _db.tbl_User.Single(x => x.username == id);
            
            _db.tbl_User.Remove(acc);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult CreateCate(string nameCate, HttpPostedFileBase imgfile)
        {
            string path = UploadimgFile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Lỗi định dạng ...";

            }
            else
            {
                InfoUserLogin userLogin = (InfoUserLogin)Session["USER_SESSION"];
                tbl_Admin admin = _db.tbl_Admin.Single(x => x.username == userLogin.username);
                tbl_Category cate = new tbl_Category();
                cate.nameCate = nameCate;
                cate.imageCate = path;
                cate.idAdmin = admin.idAdmin;
                cate.quantumCate = 0;
                _db.tbl_Category.Add(cate);
                _db.SaveChanges();

               
            } 
            return RedirectToAction("IndexCate");
        }
        public ActionResult DeleteCate(int id)
        {
            tbl_Category cate = _db.tbl_Category.Single(x => x.idCate == id);

            _db.tbl_Category.Remove(cate);
            _db.SaveChanges();
            return RedirectToAction("IndexCate");
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
