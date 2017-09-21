using EventAtendersChecklist.Models;
using System.Collections.Generic;

namespace EventAtendersChecklist.ModelsView
{
    public class TestView
    {
        public int? EventId { get; set; }
        public IEnumerable<ActionDictionary> ActionDictionaryList { get; set; }
        public IEnumerable<EventAttender> EventAttenderList { get; set; }
    }
    public class EventAttender
    {
        public int AttenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<ActionValue> Actions { get; set; }
    }

    public class ActionValue
    {
        public string ActionName { get; set; }
        public int ActionId { get; set; }
        public bool Value { get; set; }
    }
}