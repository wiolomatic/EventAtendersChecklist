using System;
using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<Checkbox> Checkboxes { get; set; }
        public virtual ICollection<Header> Headers { get; set; }

    }
}