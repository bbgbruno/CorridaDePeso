using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorridaDePesso.Models;

namespace CorridaDePesso.Controllers
{
    public class CorridaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Corrida
        public ActionResult Index()
        {
            return View(db.Corridas.ToList());
        }

        // GET: Corrida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corrida corrida = db.Corridas.Find(id);
            if (corrida == null)
            {
                return HttpNotFound();
            }
            return View(corrida);
        }

        // GET: Corrida/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Corrida/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Titulo,UserId,Regras,DataInicio,DataFinal,EmailADM")] Corrida corrida)
        {
            if (ModelState.IsValid)
            {
                db.Corridas.Add(corrida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(corrida);
        }

        // GET: Corrida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corrida corrida = db.Corridas.Find(id);
            if (corrida == null)
            {
                return HttpNotFound();
            }
            return View(corrida);
        }

        // POST: Corrida/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Titulo,UserId,Regras,DataInicio,DataFinal,EmailADM")] Corrida corrida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corrida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(corrida);
        }

        // GET: Corrida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corrida corrida = db.Corridas.Find(id);
            if (corrida == null)
            {
                return HttpNotFound();
            }
            return View(corrida);
        }

        // POST: Corrida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Corrida corrida = db.Corridas.Find(id);
            db.Corridas.Remove(corrida);
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
