using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProjeTakip.Models
{

    public class ProjeAtaList
    {

        public List<OgrList> Ogr { get; set; }

        public List<ProList> Pro { get; set; }
        public int Proid { get; set; }

        public string error { get; set; }
    }
}