using ProjeTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjeTakip.Controllers
{
    public class AnasayfaController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();
        // GET: Anasayfa
        [AllowAnonymous]
        public ActionResult Index()
        {
            var proje = db.proje;
            return View(proje.ToList());

        }
        [Authorize(Roles="1")]
        public ActionResult Ayarlar()
        {
            var ayarlar = db.ayarlar.Find(1);
            if (ayarlar == null)
            {
                return HttpNotFound();
            }
     
            return View(ayarlar);

        }

    }
}