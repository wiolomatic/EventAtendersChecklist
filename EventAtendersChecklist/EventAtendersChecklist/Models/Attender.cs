using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class Attender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Checkbox> Checkboxes { get; set; }
    }
}