//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebFinal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_Customer()
        {
            this.tbl_Bill = new HashSet<tbl_Bill>();
        }
    
        public int idCustomer { get; set; }
        public string addressCustomer { get; set; }
        public string imageCustomer { get; set; }
        public string username { get; set; }
    
        public virtual tbl_Account tbl_Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_Bill> tbl_Bill { get; set; }
    }
}
