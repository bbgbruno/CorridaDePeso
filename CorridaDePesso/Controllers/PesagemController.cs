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
                var primeiraPesagem = ( db.Pesagems.Where(x => x.CorridaId == pesagem.CorridaId && x.CorredorId == pesagem.CorredorId).Count() == 0);
                if (primeiraPesagem)
                {
                    corredor.PesoIcinial = pesagem.Peso;
                    corredor.PesoObjetivo = RetornarPesoObjetivo(db.Corridas.Find(pesagem.CorridaId), pesagem.Peso); 
                }
                
                db.Pesagems.Add(pesagem);
                db.SaveChanges();
                return RedirectToAction("MinhasCorridas", "Corrida");
            }

            return View(pesagem);
        }
       
        
        // POST: Pesagems/Delete/5
        public ActionResult ListarPesagem(int id)
        {
            var pesagens = db.Pesagems.Where(x => x.CorredorId == id).ToList();
            return View("_listapesagem", pesagens);
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

        private double RetornarPesoObjetivo(Corrida corrida, double pesoAtual)
        {
            double valorObjetivo = 0;
            double fatorCalculado = 0;
            if (corrida.Evolucao == TipoEvolucao.Percentual)
                fatorCalculado = ((pesoAtual * corrida.FatorCalculo) / 100);
            if (corrida.Evolucao == TipoEvolucao.ValorFixo)
                fatorCalculado = corrida.FatorCalculo;
            if (corrida.TipoCorrida == TipoCalculo.PerdaDePeso)
                valorObjetivo = (pesoAtual - fatorCalculado);
            else
                valorObjetivo = (pesoAtual + fatorCalculado);

            return valorObjetivo;
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
