using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorridaDePesso.Models;
using System.Text;

namespace CorridaDePesso.Controllers
{
    public class Grafico
    {
        public List<string> categories = new List<string>();
        public Dictionary<string[], string[]> series = new Dictionary<string[], string[]>();
    }

    public class CorredorController : ApplicationController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
                        
        // GET: Corredor
        public ActionResult Index()
        {
           var userId = UsuarioSessao().Id;
           return View(db.Corredors.Where(x => x.UserId == userId).OrderByDescending(dado => dado.PesoIcinial - dado.PesoAtual).ToList());
        }

        // GET: Corredor/Create
        public ActionResult Create(int corridaId)
        {
            var corredor = new Corredor();
            corredor.CorridaId = corridaId; 
            return View(corredor);
        }

        // POST: Corredor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Corredor corredor)
        {
            if (ModelState.IsValid)
            {
                corredor.Corrida = db.Corridas.Find(corredor.CorridaId);
                corredor.UserId = corredor.Corrida.UserId;
                corredor.PesoAtual = corredor.PesoIcinial;
                corredor.PesoObjetivo = RetornarPesoObjetivo(corredor.Corrida, corredor.PesoAtual);
                db.Corredors.Add(corredor);
                db.SaveChanges();
                return View("EnvioConfirmado");
            }

            return View(corredor);
        }

        
        public ActionResult Aprovar(int id)
        {
            var corredor = db.Corredors.Find(id);
            corredor.Aprovado = true;
            db.Entry(corredor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }
        
        public ActionResult Regeitar(int id)
        {
            var corredor = db.Corredors.Find(id);
            corredor.Aprovado = false;
            db.Entry(corredor).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
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
