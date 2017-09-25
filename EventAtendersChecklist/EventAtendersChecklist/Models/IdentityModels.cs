using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EventAtendersChecklist.Migrations;

namespace EventAtendersChecklist.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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

            //Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration2>());
            var context = new ApplicationDbContext();
            if (!context.Roles.AnyAsync())
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

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}