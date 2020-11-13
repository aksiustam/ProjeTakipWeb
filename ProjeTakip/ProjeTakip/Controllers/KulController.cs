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
    public class KulController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();

        // GET: Kul
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View(db.kullanici.ToList());
        }

        // GET: Kul/Details/5
        [Authorize(Roles = "1")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kullanici kullanici = db.kullanici.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // GET: Kul/Create
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Kul/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "kul_id,kul_isim,kul_mail,kul_sifre,kul_telefon,kul_yetki")] kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                db.kullanici.Add(kullanici);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kullanici);
        }

        [Authorize(Roles = "1")]
        // GET: Kul/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kullanici kullanici = db.kullanici.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // POST: Kul/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "kul_id,kul_isim,kul_mail,kul_sifre,kul_telefon,kul_yetki")] kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kullanici).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kullanici);
        }

        // GET: Kul/Delete/5
        [Authorize(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            kullanici kullanici = db.kullanici.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        // POST: Kul/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            kullanici kullanici = db.kullanici.Find(id);
            db.kullanici.Remove(kullanici);
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
