using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAtendersChecklist.Models
{
    public class EmployeeEventAssignment
    {
        public EmployeeEventAssignment()
        {

        }
        public int Id { get; set; }
        public int EmpoyeeId { get; set; }
        public int EventId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Event Event { get; set; }

    }
}