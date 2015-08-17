using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CorridaDePesso.Models;
using System.Collections.Generic;
using System.Text;

namespace CorridaDePesso.Controllers
{
    public class DashboardCorridaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int corridaId)
        {

            var corrida = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == corridaId).FirstOrDefault();
            var corredores = corrida.Participantes.Where(x => x.Aprovado == true).OrderByDescending(dado => dado.PesoIcinial - dado.PesoAtual).ToList();         
            return View(corredores);
        }

        [HttpGet]
        public JsonResult GetPesagemCorredorGeral(int id)
        {

            var listGrafico = new List<object>();

            var grafico = new Grafico();

            var corredores = db.Corredors.Where(x => x.Id == id && x.Aprovado==true).Select(dado => dado.Nome).ToList();

            foreach (var item in corredores)
            {
                grafico.categories.Add(item.ToString());

                var pesagems = (from peso in db.Pesagems
                                where peso.Corredor.Nome.Equals(item.ToString())
                                orderby peso.Data
                                select new
                                {
                                    Chave = peso.Data,
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
        public JsonResult GetCorredorPeso(int id)
        {
            var corrida = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == id).FirstOrDefault();
            var corredores =corrida.Participantes.Select(dado => new { dado.Nome, dado.PesoIcinial, dado.PesoAtual, dado.PesoObjetivo }).OrderByDescending(x => (x.PesoIcinial - x.PesoAtual)).ToList();

            var retorno = new
            {
                Chave = corredores.Select(desp => desp.Nome).ToArray(),
                Valor = corredores.Select(desp => new { name = " Já Perdeu ", y = (desp.PesoIcinial - desp.PesoAtual) }).ToArray(),
                Dado  = corredores.Select(desp => new { name = " Objetivo ", y = (desp.PesoIcinial - desp.PesoObjetivo) }).ToArray()
            };

            return Json(new { Data = retorno }, "json", Encoding.UTF8, JsonRequestBehavior.AllowGet);

        }

    }
}