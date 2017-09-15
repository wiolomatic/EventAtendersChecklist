namespace EventAtendersChecklist.Models
{
    public class Header
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Event Event { get; set; }
    }
}