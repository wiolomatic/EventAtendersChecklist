namespace EventAtendersChecklist.Migrations.ApplicationDbContext
{
    using EventAtendersChecklist.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration2 : DbMigrationsConfiguration<EventAtendersChecklist.Models.ApplicationDbContext>
    {
        public Configuration2()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\ApplicationDbContext";
        }

        protected override void Seed(EventAtendersChecklist.Models.ApplicationDbContext context)
        {
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
