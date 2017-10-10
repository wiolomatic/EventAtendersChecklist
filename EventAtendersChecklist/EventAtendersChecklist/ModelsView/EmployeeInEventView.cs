namespace EventAtendersChecklist.ModelsView
{
    /// <summary>
    /// Defines the <see cref="EmployeeInEventView" />
    /// </summary>
    public class EmployeeInEventView
    {
        /// <summary>
        /// Gets or sets the EventID
        /// </summary>
        public int EventID { get; set; }

        /// <summary>
        /// Gets or sets the EmployeeId
        /// </summary>
        public int EmployeeId { get; set; }

        /// <summary>
        /// Gets or sets the FirtName
        /// </summary>
        public string FirtName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the ActionDictionaryId
        /// </summary>
        public int ActionDictionaryId { get; set; }

        /// <summary>
        /// Gets or sets the ActionName
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ActionValue
        /// </summary>
        public bool ActionValue { get; set; }
    }
}
