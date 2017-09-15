using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EventAtendersChecklist.Models
{
    public class ModelContext : DbContext
    {
        public ModelContext()
            : base("name=ModelContext")
        { }

        public virtual DbSet<Attender> Attenders { get; set; }
        public virtual DbSet<Checkbox> Checkboxes { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Header> Headers { get; set; }
    }
}