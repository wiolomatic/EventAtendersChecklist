namespace EventAtendersChecklist.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EventAtendersChecklist.DAL.eacContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EventAtendersChecklist.DAL.eacContext context)
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
            context.Employees.AddOrUpdate(employee => employee.Id,
                new Models.Employee
                {
                    Id = 1,
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    Email = "jankowalski@wp.pl"
                },
                new Models.Employee
                {
                    Id = 2,
                    FirstName = "Zygmunt",
                    LastName = "Szmuda",
                    Email = "zygmuntszmuda@wp.pl",
                },
                new Models.Employee
                {
                    Id = 3,
                    FirstName = "Marcin",
                    LastName = "Pilarczyk",
                    Email = "marcinpilarczyk@wp.pl"
                },
                new Models.Employee
                {
                    Id = 4,
                    FirstName = "Jan",
                    LastName = "Trol",
                    Email = "jantrol@wp.pl"
                });

            //New Event
            context.Events.AddOrUpdate(e => e.Id,
                new Models.Event { Id = 1, Name = "First Integration" },
                new Models.Event { Id = 2, Name = "Floryda" });

            // New Event connection
            context.EmployeeEventAssignments.AddOrUpdate(eea => eea.Id,

                //Event 1
                new Models.EmployeeEventAssignment { Id = 1, EventId = 1, EmployeeId = 1, ActionId = 1, ActionValue = true },
                new Models.EmployeeEventAssignment { Id = 2, EventId = 1, EmployeeId = 2, ActionId = 1, ActionValue = true },
                new Models.EmployeeEventAssignment { Id = 3, EventId = 1, EmployeeId = 3, ActionId = 1, ActionValue = true },
                new Models.EmployeeEventAssignment { Id = 4, EventId = 1, EmployeeId = 4, ActionId = 1, ActionValue = true },

                // Event 2
                new Models.EmployeeEventAssignment { Id = 5, EventId = 2, EmployeeId = 2, ActionId = 1, ActionValue = true },
                new Models.EmployeeEventAssignment { Id = 6, EventId = 2, EmployeeId = 3, ActionId = 1, ActionValue = true },
                new Models.EmployeeEventAssignment { Id = 7, EventId = 2, EmployeeId = 4, ActionId = 1, ActionValue = true },
                new Models.EmployeeEventAssignment { Id = 8, EventId = 2, EmployeeId = 2, ActionId = 2, ActionValue = false },
                new Models.EmployeeEventAssignment { Id = 9, EventId = 2, EmployeeId = 3, ActionId = 3, ActionValue = false },
                new Models.EmployeeEventAssignment { Id = 10, EventId = 2, EmployeeId = 4, ActionId = 2, ActionValue = true }
            );

            // New action
            context.Actions.AddOrUpdate(a => a.Id,
                new Models.Action { Id = 1, Name = "Avilable" },
                new Models.Action { Id = 2, Name = "T-Shirt" },
                new Models.Action { Id = 3, Name = "Coach" });


            // New action assigment
            context.ActionGroups.AddOrUpdate(a => a.Id,
                // Event 1
                new Models.ActionGroup { Id = 1, EventId = 1, ActionId = 1 },
                new Models.ActionGroup { Id = 2, EventId = 1, ActionId = 2 },
                new Models.ActionGroup { Id = 3, EventId = 1, ActionId = 3 },

                // Event 2
                new Models.ActionGroup { Id = 4, EventId = 2, ActionId = 1 },
                new Models.ActionGroup { Id = 5, EventId = 2, ActionId = 3 });
        }
    }
}
