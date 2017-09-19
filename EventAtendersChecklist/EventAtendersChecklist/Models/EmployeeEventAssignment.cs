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

        public int ActionId { get; set; }
        public bool ActionValue { get; set; }
        public int EmployeeId { get; set; }
        public int EventId { get; set; }
        public int Id { get; set; }
        public virtual ActionName ActionNames { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Event Event { get; set; }

    }
}