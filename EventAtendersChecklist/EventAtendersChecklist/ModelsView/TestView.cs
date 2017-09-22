namespace EventAtendersChecklist.ModelsView
{
    using EventAtendersChecklist.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Id is Requirde")]
        public int AttenderId { get; set; }

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        [Required(ErrorMessage = "First Name is Requirde")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        [Required(ErrorMessage = "Last is Requirde")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the Email
        /// </summary>
        [Required(ErrorMessage = "Email is Requirde")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                    ErrorMessage = "Email is not valid")]
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
        [Required(ErrorMessage = "Action name is Requirde")]
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the ActionId
        /// </summary>
        [Required(ErrorMessage = "Id is Requirde")]
        public int ActionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Value
        /// </summary>
        [Required(ErrorMessage = "Value is Requirde")]
        public bool Value { get; set; }
    }
}
