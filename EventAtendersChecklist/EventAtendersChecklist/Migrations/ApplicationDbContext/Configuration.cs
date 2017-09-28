namespace EventAtendersChecklist.Migrations.ApplicationDbContext
{
    using EventAtendersChecklist.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EventAtendersChecklist.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ApplicationDbContext";
        }

        protected override void Seed(EventAtendersChecklist.Models.ApplicationDbContext context)
        {
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

            if (!context.Roles.Any())
            {
                var roleStoreHR = new RoleStore<IdentityRole>(context);
                var roleManagerHR = new RoleManager<IdentityRole>(roleStoreHR);
                var roleHR = new IdentityRole
                {
                    Name = "HR"
                };
                roleManagerHR.Create(roleHR);
                var roleStoreTL = new RoleStore<IdentityRole>(context);
                var roleManagerTL = new RoleManager<IdentityRole>(roleStoreTL);
                var roleTL = new IdentityRole
                {
                    Name = "TL"
                };
                roleManagerTL.Create(roleTL);

                var roleStoreCP = new RoleStore<IdentityRole>(context);  //Change Password 
                var roleManagerCP = new RoleManager<IdentityRole>(roleStoreTL);
                var roleCP = new IdentityRole
                {
                    Name = "CP"
                };
                roleManagerTL.Create(roleCP);
            }

            if (!context.Users.Any())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new ApplicationUserManager(userStore);

                var user = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    UserName = "admin@admin.com"
                };
                userManager.Create(user, "Admin12#");
                userManager.AddToRole(user.Id, "HR");
            }
        
    }
    }
}
