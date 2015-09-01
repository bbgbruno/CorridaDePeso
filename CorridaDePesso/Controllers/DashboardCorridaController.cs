using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CorridaDePesso.Models;
using System.Collections.Generic;
using System.Text;
using CorridaDePesso.Models.ViewModel;
using System;

namespace CorridaDePesso.Controllers
{
    public class DashboardCorridaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int corridaId)
        {

            var corrida = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == corridaId).ToList();
            var corredores = RetornarListaDeCorredores(corrida);
            return View(corredores.OrderBy(x => x.PesoAtual - x.PesoObjetivo));
        }

        [HttpGet]
        public JsonResult GetPesagemCorredorGeral(int id, int corridaId)
        {

            var listGrafico = new List<object>();

            var grafico = new Grafico();

            var corredor = db.Corredors.Find(id);

            grafico.categories.Add(corredor.Nome);
            grafico.categories.Add("Meta Semanal");

            var pesagems = (from peso in db.Pesagems
                            where peso.Corredor.Nome.Equals(corredor.Nome)
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

            List<string> chave = new List<string>();
            chave.Add("Meta");
            List<object> valor = new List<object>();

            var corrida = db.Corridas.Find(corridaId);
            int dias = (corrida.DataFinal.Subtract(corrida.DataInicio)).Days;
            var qtdePesagens = Math.Truncate((double)dias / 7)+2;
            var pesoPerder = (corredor.PesoIcinial - corredor.PesoObjetivo);
            var perdaConstante = (pesoPerder / qtdePesagens);
            qtdePesagens = qtdePesagens - pesagems.Count;
            double ultimoPeso = 0;
            for (int i = 0; i < pesagems.Count; i++)
            {
                if (i == 0)
                    valor.Add(new { y = corredor.PesoIcinial, type = "spline" });
                else
                    valor.Add(new { y = Math.Round( corredor.PesoIcinial - (perdaConstante * i),1), type = "spline" });
            }

            ultimoPeso = pesagems.Last().Valor;
            pesoPerder = (ultimoPeso - corredor.PesoObjetivo);
            perdaConstante = (pesoPerder / qtdePesagens);

            for (int i = 0; i < qtdePesagens; i++)
            {
                valor.Add(new { y = Math.Round( ultimoPeso - (perdaConstante * (i + 1)),1), type = "spline" });
            }

            var meta = new
            {
                Chave = pesagems.Select(desp => desp.Chave.Day + "/" + desp.Chave.Month).ToArray(),
                Valor = valor.ToArray()
            };
            listGrafico.Add(meta);

            return Json(new { categories = grafico.categories, Data = listGrafico }, "json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCorredorPeso(int id)
        {
            var corrida = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == id).FirstOrDefault();
            var corredores = corrida.Participantes.Where(x => x.Aprovado == true).Select(dado => new { dado.Nome, dado.PesoIcinial, dado.PesoAtual, dado.PesoObjetivo }).OrderByDescending(x => (x.PesoIcinial - x.PesoAtual)).ToList();

            var retorno = new
            {
                Chave = corredores.Select(desp => desp.Nome).ToArray(),
                Valor = corredores.Select(desp => new { name = " Já Perdeu ", y = (desp.PesoIcinial - desp.PesoAtual) }).ToArray(),
                Dado = corredores.Select(desp => new { name = " Objetivo ", y = (desp.PesoIcinial - desp.PesoObjetivo) }).ToArray()
            };

            return Json(new { Data = retorno }, "json", Encoding.UTF8, JsonRequestBehavior.AllowGet);

        }

        private IEnumerable<CorredorViewModel> RetornarListaDeCorredores(List<Corrida> corridasPublicas)
        {
            foreach (var item in corridasPublicas)
            {
                foreach (var corredor in item.Participantes.Where(x => x.Aprovado == true))
                {
                    yield return new CorredorViewModel
                    {
                        Id = corredor.Id,
                        TituloCorrida = item.Titulo,

                        PesoAtual = corredor.PesoAtual,
                        PesoIcinial = corredor.PesoIcinial,
                        PesoObjetivo = corredor.PesoObjetivo,
                        Nome = corredor.Nome,
                        Aprovado = corredor.Aprovado,
                        Corrida = item,
                        urlImagemCorredor = corredor.urlImagemCorredor

                    };
                }
            }
        }
    }
}