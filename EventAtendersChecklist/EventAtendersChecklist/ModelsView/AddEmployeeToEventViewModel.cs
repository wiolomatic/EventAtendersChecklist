using EventAtendersChecklist.Models;

namespace EventAtendersChecklist.ModelsView
{
    public class AddEmployeeToEventViewModel
    {
        public int EventId { get; set; }
        public Employee _employee { get; set; }
    }
}