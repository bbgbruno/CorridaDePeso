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

        public  async Task<ActionResult> Aprovar(int id)
        {
            var corredor = db.Corredors.Find(id);
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
            }
            
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
