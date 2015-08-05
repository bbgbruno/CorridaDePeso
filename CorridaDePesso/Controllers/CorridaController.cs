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

        // GET: Corrida
        public ActionResult Index()
        {
            var userId = UsuarioSessao().Id;
            var corridas = RetornarListadeCorridas(db.Corridas.Where(x => x.UserId == userId).ToList());
            return View(corridas);
        }

        // GET: Corrida
        public ActionResult CorridasPublicas()
        {
        
            var corridasPublicas = db.Corridas.ToList();
            var corridas = RetornarListadeCorridas(corridasPublicas);
            return View("Index", corridas);
        }

        private IEnumerable<CorridaViewModel> RetornarListadeCorridas(List<Corrida> corridasPublicas)
        {
            foreach (var item in corridasPublicas)
            {
                var corredores = db.Corredors.Where(dado => dado.CorridaId == item.Id);
                if (corredores.Count() > 0)
                {
                    yield return new CorridaViewModel
                    {
                        Id = item.Id,
                        Titulo = item.Titulo,
                        DataInicial = item.DataInicio,
                        DataFinal = item.DataFinal,
                        NumeroCorredores = corredores.Count(),
                        CorredorLider = corredores.OrderByDescending(dado => (dado.PesoIcinial - dado.PesoAtual)).FirstOrDefault()
                    };
                }
            }
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Corrida corrida)
        {
            var user = db.Users.Where(dado => dado.UserName == corrida.EmailADM).FirstOrDefault();
            var pass = string.Empty;

            if (ModelState.IsValid)
            {
                if (user == null)
                {
                    var passwordHash = new PasswordHasher();
                    pass = TratamentoString.CalcularMD5Hash(corrida.EmailADM).Substring(1, 8);
                    user = new ApplicationUser { UserName = corrida.EmailADM, Email = corrida.EmailADM, TipoUsuario = TipoConta.Administrador };
                    var result = await UserManager.CreateAsync(user, pass);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", result.Errors.ToString());
                        if (!ModelState.IsValid)
                            return View(corrida);
                    }

                }
                corrida.UserId = user.Id;
                db.Corridas.Add(corrida);
                db.SaveChanges();
                NotificaPorEmail.NotificarNovoCadastro(user.Email, pass, user.Email);
                return RedirectToAction("CorridasPublicas");
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
