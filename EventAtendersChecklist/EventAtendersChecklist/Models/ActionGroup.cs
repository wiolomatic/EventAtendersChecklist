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
        public int ActionDictionaryId { get; set; }
        public int EventId { get; set; }
        public virtual ActionDictionary ActionDictionary { get; set; }
        public virtual Event Event { get; set; }
    }
}