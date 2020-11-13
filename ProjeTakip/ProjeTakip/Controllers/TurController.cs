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
    public class TurController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();

        // GET: Tur
         [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View(db.tur.ToList());
        }

        // GET: Tur/Details/5
         [Authorize(Roles = "1")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tur tur = db.tur.Find(id);
            if (tur == null)
            {
                return HttpNotFound();
            }
            return View(tur);
        }

        // GET: Tur/Create
         [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tur/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "tur_id,tur_ad,tur_kisa")] tur tur)
        {
            if (ModelState.IsValid)
            {
                db.tur.Add(tur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tur);
        }

        // GET: Tur/Edit/5
         [Authorize(Roles = "1")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tur tur = db.tur.Find(id);
            if (tur == null)
            {
                return HttpNotFound();
            }
            return View(tur);
        }

        // POST: Tur/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "tur_id,tur_ad,tur_kisa")] tur tur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tur);
        }

        // GET: Tur/Delete/5
         [Authorize(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tur tur = db.tur.Find(id);
            if (tur == null)
            {
                return HttpNotFound();
            }
            return View(tur);
        }

        // POST: Tur/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tur tur = db.tur.Find(id);
            db.tur.Remove(tur);
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
