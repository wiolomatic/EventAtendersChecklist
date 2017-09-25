namespace EventAtendersChecklist.Models
{
    using System.Data.Entity;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using EventAtendersChecklist.Migrations;
    using EventAtendersChecklist.Migrations.ApplicationDbContext;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// <summary>
    /// Defines the <see cref="ApplicationUser" />
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The GenerateUserIdentityAsync
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{ApplicationUser}"/></param>
        /// <returns>The <see cref="Task{ClaimsIdentity}"/></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    /// <summary>
    /// Defines the <see cref="ApplicationDbContext" />
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: true)
        {
            Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration2>());
        }

        /// <summary>
        /// The Create
        /// </summary>
        /// <returns>The <see cref="ApplicationDbContext"/></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
