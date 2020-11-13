using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeTakip.Models.Extended
{
    public class PassResetModel
    {
        [Required(ErrorMessage="Yeni Parola gereklidir!",AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPass { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPass",ErrorMessage="Yeni parola ile aynı olmalıdır!")]
        public string ConfPass { get; set; }

        [Required]
        public string ResetCode { get; set; }


    }
}