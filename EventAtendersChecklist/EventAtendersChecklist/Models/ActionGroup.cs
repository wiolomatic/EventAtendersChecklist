using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventAtendersChecklist.Models
{
    public class ActionGroup
    {
        public ActionGroup()
        {

        }

        public int Id { get; set; }
        public int ActionId { get; set; }
        public int EventId { get; set; }
        public virtual Action Action { get; set; }
        public virtual Event Event { get; set; }
    }
}