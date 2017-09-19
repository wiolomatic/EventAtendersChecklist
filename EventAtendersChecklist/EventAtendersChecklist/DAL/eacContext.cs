using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EventAtendersChecklist.Models;

namespace EventAtendersChecklist.DAL
{
    public class eacContext : DbContext
        {
            public eacContext()
                : base("DefaultConnection")
            {
            }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<ActionName> ActionNames { get; set; }
        public DbSet<ActionGroup> ActionGroups { get; set; }
        public DbSet<EmployeeEventAssignment> EmployeeEventAssignments { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          //  base.OnModelCreating(modelBuilder);
        }
    }
}
