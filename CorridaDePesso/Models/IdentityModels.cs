using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.ModelConfiguration;

namespace CorridaDePesso.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public enum TipoConta
    {
        Administrador = 1,
        Corredor = 2,
        Super = 3
    }

    public class ApplicationUser : IdentityUser
    {
        public TipoConta TipoUsuario { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CorridaConfig());
            base.OnModelCreating(modelBuilder);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<CorridaDePesso.Models.Corredor> Corredors { get; set; }

        public System.Data.Entity.DbSet<CorridaDePesso.Models.Pesagem> Pesagems { get; set; }

        public System.Data.Entity.DbSet<CorridaDePesso.Models.Corrida> Corridas { get; set; }


    }

    public class CorridaConfig : EntityTypeConfiguration<Corrida>
    {
        public CorridaConfig()
        {
            // MAPEAMENTO DE MUITOS PARA MUITOS
            HasMany(f => f.Participantes)
                .WithMany( f => f.Corridas)
                .Map(me =>
                {
                    me.MapLeftKey("CorridaId");
                    me.MapRightKey("CorredorId");
                    me.ToTable("CorridaParticipantes");
                });
        }
    }

}
