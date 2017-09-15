using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAtendersChecklist.Models
{
    public class AttendersViewModel
    {
        public int Id { get; set; }
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}