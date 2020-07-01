using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Common
{
    public class InfoUserLogin
    {
        public string username { get; set; }
        public string password { get; set; }
        public int idUser{ get; set; }
        public string displayName { get; set; }
        public int typeAccount { get; set; }
    }
}