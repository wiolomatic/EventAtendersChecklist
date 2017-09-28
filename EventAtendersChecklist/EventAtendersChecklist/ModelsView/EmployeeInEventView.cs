using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAtendersChecklist.ModelsView
{
    public class EmployeeInEventView
    {
        public int EventID { get; set; }
        public int EmployeeId { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ActionDictionaryId { get; set; }
        public string ActionName { get; set; }
        public bool ActionValue { get; set; }
    }
}