//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjeTakip.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class kullanici
    {
        public int kul_id { get; set; }
        public string kul_isim { get; set; }
        public string kul_mail { get; set; }
        public string kul_sifre { get; set; }
        public string kul_telefon { get; set; }
        public Nullable<int> kul_yetki { get; set; }
        public string kul_Code { get; set; }
    }
}