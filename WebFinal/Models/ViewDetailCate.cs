using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class ViewDetailCate
    {
        public int idCate { get; set; }
        public string nameCate { get; set; }
        public string imageCate { get; set; }
        public Nullable<int> quantumCate { get; set; }
        public string Admin { get; set; }
    }
}