﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorridaDePesso.Models;
using Microsoft.AspNet.Identity;
using CorridaDePesso.Controllers.HelperController;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using CorridaDePesso.Email;
using System.Collections.Generic;
using CorridaDePesso.Models.ViewModel;

namespace CorridaDePesso.Controllers
{

    public class CorredorController : ApplicationController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Corredor
        public ActionResult Index()
        {
            var userId = UsuarioSessao().Id;
            var corridas = db.Corridas.Include(x => x.Participantes).Where(x => x.UserId == userId).ToList();
            var corredores = RetornarListaDeCorredores(corridas);
            return View(corredores);
        }

        private IEnumerable<CorredorViewModel> RetornarListaDeCorredores(List<Corrida> corridasPublicas)
        {
            foreach (var item in corridasPublicas)
            {
                foreach (var corredor in item.Participantes)
                {
                    yield return new CorredorViewModel
                    {
                        Id = corredor.Id,
                        TituloCorrida = item.Titulo,
   
                     PesoAtual = corredor.PesoAtual,
                        PesoIcinial = corredor.PesoIcinial,
                        PesoObjetivo = corredor.PesoObjetivo,
                        Nome = corredor.Nome,
                        Aprovado = corredor.Aprovado
                    };
                }
            }
        }

        // GET: Corredor/Create
        public ActionResult Create(int corridaId)
        {
            var corredor = new Corredor();
            var corrida = db.Corridas.Where(x => x.Id== corridaId).FirstOrDefault();
            corredor.Corridas.Add(corrida);
            return View(corredor);
        }

        // POST: Corredor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Corredor novoCorredor)
        {
            if (ModelState.IsValid)
            {
                var corrida = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == novoCorredor.Corridas.FirstOrDefault().Id).FirstOrDefault();
                var corredor = db.Corredors.Include(x => x.Corridas).Where(dado => dado.Email == novoCorredor.Email).FirstOrDefault();
                if ( corredor == null)
                {
                    novoCorredor.PesoAtual = novoCorredor.PesoIcinial;
                    novoCorredor.PesoObjetivo = RetornarPesoObjetivo(novoCorredor.Corridas.FirstOrDefault(), novoCorredor.PesoAtual);
                    db.Corredors.Add(novoCorredor);
                    corredor = novoCorredor;
                }

                corrida.Participantes.Add(corredor);
                db.Entry(corrida).State = EntityState.Modified;
                db.SaveChanges();
                NotificaPorEmail.NotificarNovoCorredor(corrida.EmailADM, "O corredor " + corredor.Nome + " Deseja participar da corrida " + corrida.Titulo + " Faça seu login vá em corredores e aprove seu cadastro");
                return View("EnvioConfirmado");
            }

            return View(novoCorredor);
        }

        public async Task<ActionResult> Aprovar(int id)
        {
            var corredor = db.Corredors.Include(x => x.Corridas).Where(x => x.Id == id).FirstOrDefault();
            var user = db.Users.Where(dado => dado.UserName == corredor.Email).FirstOrDefault();

            if (user == null)
            {
                var passwordHash = new PasswordHasher();
                string password = TratamentoString.CalcularMD5Hash(corredor.Email).Substring(1, 8);

                user = new ApplicationUser { UserName = corredor.Email, Email = corredor.Email, TipoUsuario = TipoConta.Corredor };
                var result = await UserManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.ToString());
                    if (!ModelState.IsValid)
                        return RedirectToAction("index");
                }

                NotificaPorEmail.NotificarNovoCorredor(user.Email, "Seu cadastro na Corrida foi aprovado Seu Usuario é " + user.Email + " Sua Senha é " + password);

            }

            corredor.Aprovado = true;
            corredor.UserId = user.Id;
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
