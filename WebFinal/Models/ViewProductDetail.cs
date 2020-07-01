using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFinal.Models
{
    public class ViewProductDetail
    {
        public int idProduct { get; set; }
        [Required(ErrorMessage ="Không bỏ trống!!")]
        public string nameProduct { get; set; }
       
        public string imageProduct { get; set; }
        [Required(ErrorMessage = "Không bỏ trống!!")]
        public string desProduct { get; set; }
        public Nullable<bool> status{ get; set; }
        [Required(ErrorMessage = "Không bỏ trống!!")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá không là số âm.")]
        public Nullable<int> priceProduct { get; set; }
        [Required(ErrorMessage = "Không bỏ trống!!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng là số âm.")]
        public Nullable<int> QuanTum { get; set; }
        [Required(ErrorMessage = "Không bỏ trống!!")]
        [Range(1, 5, ErrorMessage = "Đánh giá 1 -> 5.")]
        public Nullable<int> rateProduct { get; set; }

        public Nullable<int>  fk_idCate { get; set; }

        public Nullable<int> fk_idUser { get; set; }
       
     
        public IEnumerable<tbl_Category> Categoies { get; set; }
        public int idCate { get; set; }
        public string nameCate { get; set; }
      

        public string displayName { get; set; }
        public string imageUser { get; set; }
        public string email { get; set; }
        public string phoneUser { get; set; }
        public string username { get; set; }
        public string coverimage { get; set; }
        public string desUser { get; set; }
        public string addressUser { get; set; }
    }
}