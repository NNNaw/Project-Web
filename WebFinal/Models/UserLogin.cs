using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class UserLogin
    {
        public string username { get; set; }
        public string passWord { get; set; }
        public bool remmeberMe { get; set; }
       
    }
}