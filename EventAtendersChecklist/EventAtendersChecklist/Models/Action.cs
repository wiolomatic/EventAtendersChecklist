using System.Collections.Generic;

namespace EventAtendersChecklist.Models
{
    public class Action
    {
        public Action()
        {
            this.ActionGroups = new HashSet<ActionGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ActionGroup> ActionGroups { get; set; }

    }
}