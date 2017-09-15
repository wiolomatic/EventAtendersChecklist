namespace EventAtendersChecklist.Models
{
    public class Checkbox
    {
        public int Id { get; set; }
        public string IsChecked { get; set; }

        public virtual Event Event { get; set; }
        public virtual Attender Attender { get; set; }
    }
}