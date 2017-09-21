namespace EventAtendersChecklist.Models
{
    /// <summary>
    /// Defines the <see cref="EmployeeEventAssignment" />
    /// </summary>
    public class EmployeeEventAssignment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeEventAssignment"/> class.
        /// </summary>
        public EmployeeEventAssignment()
        {
        }

        /// <summary>
        /// Gets or sets the ActionDictionaryId
        /// </summary>
        public int ActionDictionaryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ActionValue
        /// </summary>
        public bool ActionValue { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the EventId
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ActionDictionary
        /// </summary>
        public virtual ActionDictionary ActionDictionary { get; set; }

        /// <summary>
        /// Gets or sets the Employee
        /// </summary>
        public virtual Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the Event
        /// </summary>
        public virtual Event Event { get; set; }
    }
}
