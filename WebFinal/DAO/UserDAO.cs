using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFinal.Models;

namespace WebFinal.DAO
{
    public class UserDAO
    {
        readonly EcommerceEntities _db;
        public UserDAO ()
        {
            _db = new EcommerceEntities();
        }

        public bool Login(UserLogin user)
        {

            int count = _db.tbl_Account.Count(x => x.username == user.username && x.password == user.passWord);
            if(count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }
        public tbl_Account getInfo (UserLogin user)
        {
            return _db.tbl_Account.SingleOrDefault(x => x.username == user.username);
        }

        public int getIdUser(string username, int typeAccount)
        {
            int id = 0;
            switch (typeAccount)
            {
                case 1:
                    tbl_Admin admin = _db.tbl_Admin.Single(x=>x.username == username);
                    id = admin.idAdmin;
                    break;
                case 2:
                    tbl_User user = _db.tbl_User.Single(x => x.username == username);
                    id = user.idUser;
                    break;
                case 3:
                    tbl_Customer customer = _db.tbl_Customer.Single(x => x.username == username);
                    id = customer.idCustomer;
                    break;
                default:
                    id = 0;
                    break;
            }
            return id;
        }


        public string checkRegister(tbl_Account acc)
        {
            int checkUserName = _db.tbl_Account.Count(x => x.username == acc.username);
            int checkEmail = _db.tbl_Account.Count(x => x.email == acc.email);
            int checkPhoneNumber = _db.tbl_Account.Count(x => x.phonenumber == acc.phonenumber);
            if(checkUserName > 0)
            {
                return "Tài khoản đã tồn tại.";
            }
            else if (checkEmail > 0)
            {
                return "Email đã tồn tại.";
            }
            else if (checkPhoneNumber > 0)
            {
                return "Số điện thoại đã tồn tại.";
            }
            else
            {
                return "";
            }
        }
    }
}