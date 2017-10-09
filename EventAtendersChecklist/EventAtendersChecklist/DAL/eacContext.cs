namespace EventAtendersChecklist.DAL
{
    using EventAtendersChecklist.Models;
    using System.Data.Entity;

    /// <summary>
    /// Defines the <see cref="EacContext" />
    /// </summary>
    public class EacContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EacContext"/> class.
        /// </summary>
        public EacContext()
                : base("DefaultConnection")
        {
        }

        /// <summary>
        /// Gets or sets the Employees
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Gets or sets the ActionDictionary
        /// </summary>
        public DbSet<ActionDictionary> ActionDictionary { get; set; }

        /// <summary>
        /// Gets or sets the ActionGroups
        /// </summary>
        public DbSet<ActionGroup> ActionGroups { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeEventAssignments
        /// </summary>
        public DbSet<EmployeeEventAssignment> EmployeeEventAssignments { get; set; }

        /// <summary>
        /// Gets or sets the Events
        /// </summary>
        public DbSet<Event> Events { get; set; }

        /// <summary>
        /// The OnModelCreating
        /// </summary>
        /// <param name="modelBuilder">The <see cref="DbModelBuilder"/></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
