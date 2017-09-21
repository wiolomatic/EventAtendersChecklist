namespace EventAtendersChecklist.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Event" />
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event()
        {
            this.EmployeeEventAssignments = new HashSet<EmployeeEventAssignment>();
            this.ActionGroups = new HashSet<ActionGroup>();
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="StartDate">The <see cref="DateTime"/></param>
        /// <param name="EndTime">The <see cref="DateTime"/></param>
        public Event(DateTime StartDate, DateTime EndTime)
        {
            this.EmployeeEventAssignments = new HashSet<EmployeeEventAssignment>();
            this.ActionGroups = new HashSet<ActionGroup>();
            this.StartDate = StartDate;
            this.EndDate = EndTime;
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
        /// Gets or sets the StartDate
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the EndDate
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeEventAssignments
        /// </summary>
        public ICollection<EmployeeEventAssignment> EmployeeEventAssignments { get; set; }

        /// <summary>
        /// Gets or sets the ActionGroups
        /// </summary>
        public ICollection<ActionGroup> ActionGroups { get; set; }
    }
}
