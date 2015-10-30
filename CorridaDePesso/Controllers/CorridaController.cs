using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CorridaDePesso.Models;
using Microsoft.AspNet.Identity;
using CorridaDePesso.Controllers.HelperController;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Collections.Generic;
using CorridaDePesso.Email;


namespace CorridaDePesso.Controllers
{
    public class Grafico
    {
        public List<string> categories = new List<string>();
        public Dictionary<string[], string[]> series = new Dictionary<string[], string[]>();
    }

    public class CorridaController : ApplicationController
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MinhasCorridas()
        {
            var userId = UsuarioSessao().Id;
            if (UsuarioSessao().TipoUsuario == TipoConta.Administrador)
            {
                var corridas = RetornarListaDeCorridas(db.Corridas.Include(p => p.Participantes).Where(x => x.UserId == userId).ToList());
                return View("Corridas", corridas);
            }
            else
            {
                var corredor = db.Corredors.Include(p => p.Corridas).Where(x => x.UserId == userId).FirstOrDefault();
                return View("Corridas", RetornarListaDeCorridas(corredor.Corridas.ToList()));
            }


        }

        public ActionResult Link(string id)
        {

            var corridasPublicas = db.Corridas.Where(dado => dado.Link.Equals(id)).ToList();
            var corridas = RetornarListaDeCorridas(corridasPublicas);
            return View("Corridas", corridas);
        }

        public ActionResult CorridasPublicas()
        {

            var corridasPublicas = db.Corridas.Include(x => x.Participantes).Where(dado => dado.Publica == true).ToList();
            var corridas = RetornarListaDeCorridas(corridasPublicas);
            return View("Corridas", corridas);
        }

        public ActionResult AdmCorridas()
        {
            var corridas = RetornarListaDeCorridas(db.Corridas.Include(x => x.Participantes).ToList());
            return View(corridas);
        }

        private IEnumerable<CorridaViewModel> RetornarListaDeCorridas(List<Corrida> corridasPublicas)
        {
            foreach (var item in corridasPublicas)
            {

                var Corredores = db.Corridas.Include(x => x.Participantes).Where(x => x.Id == item.Id).FirstOrDefault().Participantes;
                yield return new CorridaViewModel
                {
                    Id = item.Id,
                    Publica = item.Publica,
                    Titulo = item.Titulo,
                    Email = item.EmailADM,
                    DataInicial = item.DataInicio,
                    DataFinal = item.DataFinal,
                    NumeroCorredores = Corredores.Where(x => x.Aprovado == true).Count(),
                    CorredorLider = Corredores.OrderByDescending(dado => (dado.PesoIcinial - dado.PesoAtual)).FirstOrDefault()
                };
            }
        }

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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Corrida corrida)
        {
            var user = db.Users.Where(dado => dado.UserName == corrida.EmailADM).FirstOrDefault();
            if (ModelState.IsValid)
            {
                string password = "Sua Senha já foi Cadastrada Enteriormente";

                if (user == null)
                {
                    var passwordHash = new PasswordHasher();
                    password = TratamentoString.CalcularMD5Hash(corrida.EmailADM).Substring(1, 8);
                    user = new ApplicationUser { UserName = corrida.EmailADM, Email = corrida.EmailADM, TipoUsuario = TipoConta.Administrador };
                    var result = await UserManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.ToString());
                        if (!ModelState.IsValid)
                            return View(corrida);
                    }

                }

                var link = "http://www.corridadepeso.com.br/corrida/link/" + TratamentoString.CalcularMD5Hash(corrida.Titulo);
                corrida.UserId = user.Id;
                corrida.Link = TratamentoString.CalcularMD5Hash(corrida.Titulo);
                db.Corridas.Add(corrida);
                db.SaveChanges();
                NotificaPorEmail.NotificarNovoCadastro(user.Email, password, user.Email, link);
                return RedirectToAction("CorridasPublicas");
            }

            return View(corrida);

        }

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
        public ActionResult Edit(Corrida corrida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(corrida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(corrida);
        }

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

        public ActionResult MarcarPublicar(int id)
        {
            Corrida corrida = db.Corridas.Find(id);
            if (corrida != null)
            {
                corrida.Publica = true;
                db.Entry(corrida).State = EntityState.Modified;
                db.SaveChanges();

            }
            return View();
        }

        public ActionResult MarcarDespublicar(int id)
        {
            Corrida corrida = db.Corridas.Find(id);
            if (corrida != null)
            {
                corrida.Publica = true;
                db.Entry(corrida).State = EntityState.Modified;
                db.SaveChanges();

            }
            return View();
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Corrida corrida = db.Corridas.Find(id);
            db.Corridas.Remove(corrida);
            db.SaveChanges();
            return RedirectToAction("AdmCorridas");
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
