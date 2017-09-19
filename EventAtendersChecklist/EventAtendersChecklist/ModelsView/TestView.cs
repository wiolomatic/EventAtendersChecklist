using EventAtendersChecklist.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAtendersChecklist.ModelsView
{
    public class TestView
    {
        public List<Employee> EmployeeList { get; set; }
        public List<ActionDictionary> ActionNameList { get; set; }
        public List<EmployeeEventAssignment> EmployeeEventAsignmentList { get; set; }
    }
}