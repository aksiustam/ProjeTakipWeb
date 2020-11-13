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
    public class BolumController : Controller
    {
        private WebProjeDBEntities db = new WebProjeDBEntities();

        // GET: Bolum
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            return View(db.bolum.ToList());
        }

        // GET: Bolum/Details/5
        [Authorize(Roles = "1")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bolum bolum = db.bolum.Find(id);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            return View(bolum);
        }

        // GET: Bolum/Create
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bolum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "bolum_ad,bolum_kisa")] bolum bolum)
        {
            if (ModelState.IsValid)
            {
                db.bolum.Add(bolum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bolum);
        }

        // GET: Bolum/Edit/5
        [Authorize(Roles="1")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bolum bolum = db.bolum.Find(id);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            return View(bolum);
        }

        // POST: Bolum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "1")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "bolum_id,bolum_ad,bolum_kisa")] bolum bolum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bolum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bolum);
        }

        // GET: Bolum/Delete/5
        [Authorize(Roles = "1")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bolum bolum = db.bolum.Find(id);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            return View(bolum);
        }

        // POST: Bolum/Delete/5
        [Authorize(Roles = "1")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            bolum bolum = db.bolum.Find(id);
            db.bolum.Remove(bolum);
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
