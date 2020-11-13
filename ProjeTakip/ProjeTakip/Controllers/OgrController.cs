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
    public class OgrController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();

        // GET: Ogr
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View(db.ogrenci.ToList());
        }

        // GET: Ogr/Details/5
        [Authorize(Roles = "1")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ogrenci ogrenci = db.ogrenci.Find(id);
            if (ogrenci == null)
            {
                return HttpNotFound();
            }
            return View(ogrenci);
        }

        // GET: Ogr/Create
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ogr/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ogr_id,ogr_isim,ogr_kul,ogr_sifre,ogr_mail,ogr_yetki,ogr_yil,ogr_arsiv,ogr_alt")] ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                db.ogrenci.Add(ogrenci);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ogrenci);
        }

        // GET: Ogr/Edit/5
        [Authorize(Roles = "1")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ogrenci ogrenci = db.ogrenci.Find(id);
            if (ogrenci == null)
            {
                return HttpNotFound();
            }
            return View(ogrenci);
        }

        // POST: Ogr/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ogr_id,ogr_isim,ogr_kul,ogr_sifre,ogr_mail,ogr_yetki,ogr_yil,ogr_arsiv,ogr_alt")] ogrenci ogrenci)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ogrenci).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ogrenci);
        }

        // GET: Ogr/Delete/5
        [Authorize(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ogrenci ogrenci = db.ogrenci.Find(id);
            if (ogrenci == null)
            {
                return HttpNotFound();
            }
            return View(ogrenci);
        }

        // POST: Ogr/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ogrenci ogrenci = db.ogrenci.Find(id);
            db.ogrenci.Remove(ogrenci);
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
