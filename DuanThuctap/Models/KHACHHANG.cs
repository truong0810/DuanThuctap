﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuanThuctap.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class KHACHHANG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KHACHHANG()
        {
            this.DONHANGs = new HashSet<DONHANG>();
        }
        [DisplayName("Mã khách hàng")]
        public int MAKH { get; set; }
        [DisplayName("Mã tài khoản")]
        [Required]
        public Nullable<int> MATK { get; set; }
        [DisplayName("Tên khuyến mãi")]
        [Required]
        public string TENKH { get; set; }
        [DisplayName("Email")]
        public string EMAIL { get; set; }
        [Required]
        [DisplayName("Số điện thoại")]
        public string SDT { get; set; }
        [DisplayName("Giới tính")]
        [Required]
        public string GIOITINH { get; set; }
        [DisplayName("Địa chỉ")]
        [Required]
        public string DIACHI { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DONHANG> DONHANGs { get; set; }
        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}
