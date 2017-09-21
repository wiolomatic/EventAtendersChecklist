namespace EventAtendersChecklist.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="Employee" />
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        public Employee()
        {
            this.EmployeeEventAssignments = new HashSet<EmployeeEventAssignment>();
        }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeEventAssignments
        /// </summary>
        public ICollection<EmployeeEventAssignment> EmployeeEventAssignments { get; set; }
    }
}
