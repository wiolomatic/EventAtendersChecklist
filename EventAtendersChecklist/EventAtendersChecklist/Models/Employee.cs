using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeeEventAssignments = new HashSet<EmployeeEventAssignment>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public ICollection<EmployeeEventAssignment> EmployeeEventAssignments { get; set; }
    }
}