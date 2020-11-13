using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeTakip.Models
{
    public class UserLogin
    {
        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli")]
        public String Mail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Gerekli")]
        [DataType(DataType.Password)]
        public string Pass { get; set; }

        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }



        public string error { get; set; }

    }
}