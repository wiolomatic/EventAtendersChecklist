using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class ActionDictionary
    {
        public ActionDictionary()
        {
            this.ActionGroups = new HashSet<ActionGroup>();
            this.EmployeeEventAssigments = new HashSet<EmployeeEventAssignment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ActionGroup> ActionGroups { get; set; }
        public virtual ICollection<EmployeeEventAssignment> EmployeeEventAssigments { get; set; }

    }
}