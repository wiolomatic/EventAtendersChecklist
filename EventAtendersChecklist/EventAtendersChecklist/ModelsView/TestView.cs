namespace EventAtendersChecklist.ModelsView
{
    using EventAtendersChecklist.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="TestView" />
    /// </summary>
    public class TestView
    {
        public int? EventId { get; set; }
        /// Gets or sets the ActionDictionaryList
        /// </summary>
        public IEnumerable<ActionDictionary> ActionDictionaryList { get; set; }

        /// <summary>
        /// Gets or sets the EventAttenderList
        /// </summary>
        public IEnumerable<EventAttender> EventAttenderList { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="EventAttender" />
    /// </summary>
    public class EventAttender
    {
        /// <summary>
        /// Gets or sets the AttenderId
        /// </summary>
        public int AttenderId { get; set; }

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Actions
        /// </summary>
        public IEnumerable<ActionValue> Actions { get; set; }
    }

    /// <summary>
    /// Defines the <see cref="ActionValue" />
    /// </summary>
    public class ActionValue
    {
        /// <summary>
        /// Gets or sets the ActionName
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the ActionId
        /// </summary>
        public int ActionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Value
        /// </summary>
        public bool Value { get; set; }
    }
}
