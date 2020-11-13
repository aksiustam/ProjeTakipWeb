using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using ProjeTakip.Models;
using System.Web.Mvc;
using System.Diagnostics;

namespace ProjeTakip.Controllers
{
    public class OgrprojeController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();

        // GET: OgrAta
        [Authorize(Roles = "1")]
        [HttpGet]
        public ActionResult ProjeAta()
        {
            
            var ogrpro = db.ogrpro.Include(o => o.bolum).Include(o => o.ogrenci).Include(o => o.proje).Include(o => o.tur);

            
             var app = new ProjeAtaList();
             var data = db.ogrenci.Select(m => new OgrList()
                 {
                     ogrid = m.ogr_id,
                     ogrAd = m.ogr_isim,
                     ogrNo = m.ogr_kul,
                     

                 });
             var dataa = db.proje.Include(o => o.bolum).Include(o => o.tur).Select(m => new ProList()
             {
                 Proid = m.proje_id,
                 ProAd = m.proje_isim,
                 ProBol = m.bolum.bolum_kisa,
                 ProTur = m.tur.tur_kisa,
                 

             });
             app.error = "";
             app.Ogr = data.ToList();
             app.Pro = dataa.ToList();
             
              
            return View(app);
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult ProjeAta(ProjeAtaList p)
        {
            var data = p.Ogr;
            foreach (var item in data)
            {
                if (item.OgrSelect == true)
                {
                    if (ModelState.IsValid)
                    {
                        proje proje = db.proje.Find(p.Proid);
                        ogrenci ogr = db.ogrenci.Find(item.ogrid);
                        ogrpro value = new ogrpro()
                        { 
                            ogrpro_ogr = ogr.ogr_id, ogrpro_proje = proje.proje_id,
                            ogrpro_bolum = proje.proje_bolum, ogrpro_tur = proje.proje_tur   
                        };
                        db.ogrpro.Add(value);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    
                }
                    
            }
            return View("Index");
        }

        // GET: Ogrproje
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            var ogrpro = db.ogrpro.Include(o => o.bolum).Include(o => o.ogrenci).Include(o => o.proje).Include(o => o.tur);
            return View(ogrpro.ToList());
        }

        [Authorize(Roles = "1")]
        public ActionResult Alt()
        {
            //var ogrpro = db.ogrpro.Include(o => o.bolum).Include(o => o.ogrenci).Include(o => o.proje).Include(o => o.tur);
            

            var hey = from ogrpro in db.ogrpro
                      join dt in
                          (
                              (from ogrpro in db.ogrpro
                               group ogrpro by new
                               {
                                   ogrpro.ogrpro_tur,
                                   ogrpro.ogrpro_ogr
                               } into g
                               where g.Count() > 1
                               select new
                               {
                                   g.Key.ogrpro_tur,
                                   g.Key.ogrpro_ogr,
                                   CountOf = g.Count()
                               }))
                            on new { ogrpro.ogrpro_tur, ogrpro.ogrpro_ogr }
                        equals new { dt.ogrpro_tur, dt.ogrpro_ogr }
                      select ogrpro;

            /*

            string query = "SELECT y.*  FROM ogrpro y INNER JOIN ("
                        +"SELECT ogrpro_tur,ogrpro_ogr, COUNT(*) AS CountOf"
                       + "FROM ogrpro"
                       + "GROUP BY ogrpro_tur,ogrpro_ogr"
                        +"HAVING COUNT(*)>1"
                  + " ) dt ON y.ogrpro_tur=dt.ogrpro_tur AND y.ogrpro_ogr=dt.ogrpro_ogr";

            var data = db.ogrpro.SqlQuery(query).ToList();
            */

            return View(hey.ToList());
        }


        // GET: Ogrproje/Details/5
        [Authorize(Roles = "1")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ogrpro ogrpro = db.ogrpro.Find(id);
            if (ogrpro == null)
            {
                return HttpNotFound();
            }
            return View(ogrpro);
        }

        // GET: Ogrproje/Create
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            ViewBag.ogrpro_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad");
            ViewBag.ogrpro_ogr = new SelectList(db.ogrenci, "ogr_id", "ogr_isim");
            ViewBag.ogrpro_proje = new SelectList(db.proje, "proje_id", "proje_isim");
            ViewBag.ogrpro_tur = new SelectList(db.tur, "tur_id", "tur_ad");
            return View();
        }

        // POST: Ogrproje/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ogrpro_id,ogrpro_ogr,ogrpro_proje,ogrpro_bolum,ogrpro_tur,ogrpro_data,ogrpro_resim,ogrpro_arsiv")] ogrpro ogrpro)
        {
            if (ModelState.IsValid)
            {
                db.ogrpro.Add(ogrpro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ogrpro_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad", ogrpro.ogrpro_bolum);
            ViewBag.ogrpro_ogr = new SelectList(db.ogrenci, "ogr_id", "ogr_isim", ogrpro.ogrpro_ogr);
            ViewBag.ogrpro_proje = new SelectList(db.proje, "proje_id", "proje_isim", ogrpro.ogrpro_proje);
            ViewBag.ogrpro_tur = new SelectList(db.tur, "tur_id", "tur_ad", ogrpro.ogrpro_tur);
            return View(ogrpro);
        }

        // GET: Ogrproje/Edit/5
        [Authorize(Roles = "1")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ogrpro ogrpro = db.ogrpro.Find(id);
            if (ogrpro == null)
            {
                return HttpNotFound();
            }
            ViewBag.ogrpro_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad", ogrpro.ogrpro_bolum);
            ViewBag.ogrpro_ogr = new SelectList(db.ogrenci, "ogr_id", "ogr_isim", ogrpro.ogrpro_ogr);
            ViewBag.ogrpro_proje = new SelectList(db.proje, "proje_id", "proje_isim", ogrpro.ogrpro_proje);
            ViewBag.ogrpro_tur = new SelectList(db.tur, "tur_id", "tur_ad", ogrpro.ogrpro_tur);
            return View(ogrpro);
        }

        // POST: Ogrproje/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ogrpro_id,ogrpro_ogr,ogrpro_proje,ogrpro_bolum,ogrpro_tur,ogrpro_data,ogrpro_resim,ogrpro_arsiv")] ogrpro ogrpro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ogrpro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ogrpro_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad", ogrpro.ogrpro_bolum);
            ViewBag.ogrpro_ogr = new SelectList(db.ogrenci, "ogr_id", "ogr_isim", ogrpro.ogrpro_ogr);
            ViewBag.ogrpro_proje = new SelectList(db.proje, "proje_id", "proje_isim", ogrpro.ogrpro_proje);
            ViewBag.ogrpro_tur = new SelectList(db.tur, "tur_id", "tur_ad", ogrpro.ogrpro_tur);
            return View(ogrpro);
        }

        // GET: Ogrproje/Delete/5
        [Authorize(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ogrpro ogrpro = db.ogrpro.Find(id);
            if (ogrpro == null)
            {
                return HttpNotFound();
            }
            return View(ogrpro);
        }

        // POST: Ogrproje/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ogrpro ogrpro = db.ogrpro.Find(id);
            db.ogrpro.Remove(ogrpro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


}
