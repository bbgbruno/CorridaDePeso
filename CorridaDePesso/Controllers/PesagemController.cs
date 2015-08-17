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
    public class PesagemController : ApplicationController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        // GET: Pesagems
        public ActionResult Index()
        {
            var userId = UsuarioSessao().Id;
            var pesagems = db.Pesagems.Where(dado => dado.UserId == userId).Include(p => p.Corredor);
            return View(pesagems.OrderBy(dado => new {dado.Corredor.Nome, dado.Data}).ToList());
        }

        // GET: Pesagems/Create
        public ActionResult Create(int corridaId)
        {
            var corrida = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == corridaId).FirstOrDefault();
            ViewBag.CorredorId = new SelectList(corrida.Participantes.Where(dado => dado.Aprovado == true), "id", "Nome");
            var pesagem = new Pesagem();
            pesagem.CorridaId = corridaId;
            return View(pesagem);
        }

        // POST: Pesagems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pesagem pesagem)
        {
            if (ModelState.IsValid)
            {
                var userId = UsuarioSessao().Id;
                var corredor = db.Corredors.Find(pesagem.CorredorId);
                corredor.PesoAtual = pesagem.Peso;
                db.Entry(corredor).State = EntityState.Modified;
                pesagem.UserId = userId;
                db.Pesagems.Add(pesagem);
                db.SaveChanges();
                return RedirectToAction("MinhasCorridas", "Corrida");
            }

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
