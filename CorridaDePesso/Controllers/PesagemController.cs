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
    public class PesagemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pesagems
        public ActionResult Index()
        {
            var pesagems = db.Pesagems.Include(p => p.Corredor);
            return View(pesagems.OrderBy(dado => new {dado.Corredor.Nome, dado.Data}).ToList());
        }

        // GET: Pesagems/Create
        public ActionResult Create()
        {
            ViewBag.CorredorId = new SelectList(db.Corredors, "id", "Nome");
            return View();
        }

        // POST: Pesagems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,CorredorId,Data,Peso")] Pesagem pesagem)
        {
            if (ModelState.IsValid)
            {
                var corredor = db.Corredors.Find(pesagem.CorredorId);
                corredor.PesoAtual = pesagem.Peso;
                db.Entry(corredor).State = EntityState.Modified;
                
                db.Pesagems.Add(pesagem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CorredorId = new SelectList(db.Corredors, "id", "Nome", pesagem.CorredorId);
            return View(pesagem);
        }

       

        // GET: Pesagems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pesagem pesagem = db.Pesagems.Find(id);
            if (pesagem == null)
            {
                return HttpNotFound();
            }
            return View(pesagem);
        }

        // POST: Pesagems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pesagem pesagem = db.Pesagems.Find(id);
            db.Pesagems.Remove(pesagem);
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
