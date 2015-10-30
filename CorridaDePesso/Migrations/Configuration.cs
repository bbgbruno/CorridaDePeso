namespace CorridaDePesso.Migrations
{
    using CorridaDePesso.Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CorridaDePesso.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "CorridaDePesso.Models.ApplicationDbContext";
        }

        protected override void Seed(CorridaDePesso.Models.ApplicationDbContext context)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            var user = db.Users.Where(dado => dado.UserName == "adm@adm.com.br").FirstOrDefault();
            if (user == null)
            {
                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("master123");
                user = new ApplicationUser()
                {
                    Email = "adm@adm.com.br",
                    UserName = "adm@adm.com.br",
                    PasswordHash = password,
                    TipoUsuario = TipoConta.Super,
                    SecurityStamp = "adm@adm.com.br"                   
                };
                db.Users.Add(user);
                db.SaveChanges();
            }


            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
