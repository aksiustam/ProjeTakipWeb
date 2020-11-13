using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjeTakip.Models;

namespace ProjeTakip.Controllers
{
    public class ProjeController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();

        // GET: Proje
        [AllowAnonymous]
        public ActionResult Index()
        {
            var proje = db.proje.Include(p => p.bolum).Include(p => p.tur);
            return View(proje.ToList());
        }

        // GET: Proje/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proje proje = db.proje.Find(id);

            if (proje == null)
            {
                return HttpNotFound();
            }
     
            return View(proje);
        }

        // GET: Proje/Create
        [Authorize(Roles = "0,1")]
        public ActionResult Create()
        {
            ViewBag.proje_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad");
            ViewBag.proje_tur = new SelectList(db.tur, "tur_id", "tur_ad");
            return View();
        }

        // POST: Proje/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "0,1")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "proje_id,proje_isim,proje_bolum,proje_tur,proje_sayi,proje_yil,proje_mail,proje_data,proje_ata,proje_resim,proje_detay,proje_arsiv")] proje proje)
        {
            if (ModelState.IsValid)
            {
                db.proje.Add(proje);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.proje_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad", proje.proje_bolum);
            ViewBag.proje_tur = new SelectList(db.tur, "tur_id", "tur_ad", proje.proje_tur);
            return View(proje);
        }

        // GET: Proje/Edit/5
        [Authorize(Roles = "1")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proje proje = db.proje.Find(id);
            if (proje == null)
            {
                return HttpNotFound();
            }
            ViewBag.proje_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad", proje.proje_bolum);
            ViewBag.proje_tur = new SelectList(db.tur, "tur_id", "tur_ad", proje.proje_tur);
            return View(proje);
        }

        // POST: Proje/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "proje_id,proje_isim,proje_bolum,proje_tur,proje_sayi,proje_yil,proje_mail,proje_data,proje_ata,proje_resim,proje_detay,proje_arsiv")] proje proje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.proje_bolum = new SelectList(db.bolum, "bolum_id", "bolum_ad", proje.proje_bolum);
            ViewBag.proje_tur = new SelectList(db.tur, "tur_id", "tur_ad", proje.proje_tur);
            return View(proje);
        }

        // GET: Proje/Delete/5
        [Authorize(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proje proje = db.proje.Find(id);
            if (proje == null)
            {
                return HttpNotFound();
            }
            return View(proje);
        }

        // POST: Proje/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proje proje = db.proje.Find(id);
            db.proje.Remove(proje);
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
