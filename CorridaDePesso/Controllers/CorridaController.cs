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

namespace CorridaDePesso.Controllers
{
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
            return View(db.Corridas.Where(x => x.UserId==userId).ToList());
        }

        // GET: Corrida
        public ActionResult CorridasPublicas()
        {
            //return View(db.Corridas.Where(x => x.Publica == true).ToList());
            return View("Index", db.Corridas.ToList());
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
        public  async Task<ActionResult> Create(Corrida corrida)
        {
            var user = db.Users.Where(dado => dado.UserName == corrida.EmailADM).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var passwordHash = new PasswordHasher();
                string password = TratamentoString.CalcularMD5Hash(corrida.EmailADM).Substring(1, 8);
                if (user == null)
                {
                   user = new ApplicationUser { UserName = corrida.EmailADM, Email = corrida.EmailADM, TipoUsuario = TipoConta.Administrador };
                   var result = await UserManager.CreateAsync(user, password);
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
