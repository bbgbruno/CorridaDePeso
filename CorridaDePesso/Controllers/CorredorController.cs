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

    public class CorredorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Corredor
        public ActionResult Index()
        {
            return View(db.Corredors.OrderByDescending(dado => dado.PesoIcinial-dado.PesoAtual).ToList());
        }

     
        // GET: Corredor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Corredor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,PesoIcinial,PesoObjetivo,Nome")] Corredor corredor)
        {
            if (ModelState.IsValid)
            {
                db.Corredors.Add(corredor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(corredor);
        }
       
        // GET: Corredor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Corredor corredor = db.Corredors.Find(id);
            if (corredor == null)
            {
                return HttpNotFound();
            }
            return View(corredor);
        }

        // POST: Corredor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Corredor corredor = db.Corredors.Find(id);
            db.Corredors.Remove(corredor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetPesagemCorredorGeral(int id)
        {

            var listGrafico = new List<object>();
            
            var grafico = new Grafico();

            var corredores = db.Corredors.Where(x => x.id==id).Select(dado => dado.Nome ).ToList();

            foreach (var item in corredores)
            {
                grafico.categories.Add(item.ToString());

                var pesagems = (from peso in db.Pesagems
                                where peso.Corredor.Nome.Equals(item.ToString())
                                orderby peso.Data
                                select new
                                {  Chave = peso.Data,
                                    Valor = peso.Peso
                                }).ToList();

                var retorno = new 
                {
                    Chave = pesagems.Select(desp => desp.Chave.Day + "/" + desp.Chave.Month).ToArray(),
                    Valor = pesagems.Select(desp => new { y = desp.Valor, type = "spline" }).ToArray()
                };
                listGrafico.Add(retorno);
            }

            var pesoCorredores = db.Corredors.Select(dado => new { dado.Nome, dado.PesoIcinial, dado.PesoAtual }).OrderByDescending(x => (x.PesoIcinial - x.PesoAtual)).ToList();

           /* var dados = new
            {
                Chave = pesoCorredores.Select(desp => desp.Nome).ToArray(),
                Valor = pesoCorredores.Select(desp => Math.Round(desp.PesoIcinial - desp.PesoAtual,2)).ToArray()
            };*/

            return Json(new { categories = grafico.categories, Data = listGrafico }, "json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCorredorPeso()
        {
            var corredores = db.Corredors.Select(dado => new { dado.Nome, dado.PesoIcinial, dado.PesoAtual, dado.PesoObjetivo}).OrderByDescending(x => (x.PesoIcinial-x.PesoAtual)).ToList();
             
            var retorno = new
            {
                Chave = corredores.Select(desp => desp.Nome).ToArray(),
                Valor = corredores.Select(desp => new { name = " Ja Perdeu " + desp.Nome, y = (desp.PesoIcinial-desp.PesoAtual) }).ToArray(),
                Dado = corredores.Select(desp => new { name = " Falta Perde " + desp.Nome, y = (desp.PesoIcinial - desp.PesoObjetivo) - (desp.PesoIcinial - desp.PesoAtual) }).ToArray()
            };

            return Json(new { Data = retorno }, "json", Encoding.UTF8, JsonRequestBehavior.AllowGet);

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
