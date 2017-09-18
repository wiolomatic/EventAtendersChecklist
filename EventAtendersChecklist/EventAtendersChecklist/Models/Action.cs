﻿using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class Action
    {
        public Action()
        {
            this.ActionGroups = new HashSet<ActionGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ActionGroup> ActionGroups { get; set; }
        public virtual ICollection<EmployeeEventAssignment> EmployeeEventAssigments { get; set; }

    }
}