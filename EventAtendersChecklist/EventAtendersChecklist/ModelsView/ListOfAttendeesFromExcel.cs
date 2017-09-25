namespace EventAtendersChecklist.ModelsView
{
    using EventAtendersChecklist.Models;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ListOfAttendeesFromExcel" />
    /// </summary>
    public class ListOfAttendeesFromExcel
    {
        /// <summary>
        /// Gets or sets the listOfEmployee
        /// </summary>
        public List<Employee> ListOfEmployee { get; set; }

        /// <summary>
        /// Gets or sets the ListOfActionDictionary
        /// </summary>
        public List<ActionDictionary> ListOfActionDictionary { get; set; }
    }
}
