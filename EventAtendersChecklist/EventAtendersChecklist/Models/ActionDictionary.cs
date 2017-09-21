namespace EventAtendersChecklist.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ActionDictionary" />
    /// </summary>
    public class ActionDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDictionary"/> class.
        /// </summary>
        public ActionDictionary()
        {
            this.ActionGroups = new HashSet<ActionGroup>();
            this.EmployeeEventAssigments = new HashSet<EmployeeEventAssignment>();
        }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ActionGroups
        /// </summary>
        public virtual ICollection<ActionGroup> ActionGroups { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeEventAssigments
        /// </summary>
        public virtual ICollection<EmployeeEventAssignment> EmployeeEventAssigments { get; set; }
    }
}
