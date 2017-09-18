using System;
using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class Event
    {
        public Event()
        {
            this.EmployeeEventAssignments = new HashSet<EmployeeEventAssignment>();
            this.ActionGroups = new HashSet<ActionGroup>();
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<EmployeeEventAssignment> EmployeeEventAssignments { get; set; }
        public ICollection<ActionGroup> ActionGroups { get; set; }

    }
}