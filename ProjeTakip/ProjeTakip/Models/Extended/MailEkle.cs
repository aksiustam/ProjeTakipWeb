using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjeTakip.Models.Extended
{
    public class MailEkle
    {

        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        public int id { get; set; }
    }
}