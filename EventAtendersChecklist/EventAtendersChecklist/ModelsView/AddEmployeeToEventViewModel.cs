namespace EventAtendersChecklist.ModelsView
{
    using EventAtendersChecklist.Models;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AddEmployeeToEventViewModel" />
    /// </summary>
    public class AddEmployeeToEventViewModel
    {
        /// <summary>
        /// Gets or sets the EventId
        /// </summary>
        [Required(ErrorMessage = "Event id is Requirde")]
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the _employee
        /// </summary>
        public Employee _employee { get; set; }
    }
}
