namespace EventAtendersChecklist.ModelsView
{
    using EventAtendersChecklist.Models;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="AddActionToEventViewModel" />
    /// </summary>
    public class AddActionToEventViewModel
    {
        /// <summary>
        /// Gets or sets the EventID
        /// </summary>
        [Required(ErrorMessage = "Event id is Requirde")]
        public int EventID { get; set; }

        /// <summary>
        /// Gets or sets the ActionDictionaryProp
        /// </summary>
        public ActionDictionary ActionDictionaryProp { get; set; }
    }
}
