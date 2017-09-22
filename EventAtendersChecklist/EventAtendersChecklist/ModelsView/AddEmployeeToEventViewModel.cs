using EventAtendersChecklist.Models;
using System.ComponentModel.DataAnnotations;

namespace EventAtendersChecklist.ModelsView
{
    public class AddEmployeeToEventViewModel
    {
        [Required(ErrorMessage = "Event id is Requirde")]
        public int EventId { get; set; }
        public Employee _employee { get; set; }
    }
}