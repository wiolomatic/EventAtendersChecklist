namespace EventAtendersChecklist.Models
{
    /// <summary>
    /// Defines the <see cref="ActionGroup" />
    /// </summary>
    public class ActionGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionGroup"/> class.
        /// </summary>
        public ActionGroup()
        {
        }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ActionDictionaryId
        /// </summary>
        public int ActionDictionaryId { get; set; }

        /// <summary>
        /// Gets or sets the EventId
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the ActionDictionary
        /// </summary>
        public virtual ActionDictionary ActionDictionary { get; set; }

        /// <summary>
        /// Gets or sets the Event
        /// </summary>
        public virtual Event Event { get; set; }
    }
}
