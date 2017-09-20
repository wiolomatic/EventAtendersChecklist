﻿using EventAtendersChecklist.Models;
using System.Collections.Generic;

namespace EventAtendersChecklist.ModelsView
{
    public class TestView
    {
        public ICollection<ActionDictionary> actionDictionaryList { get; set; }
        public IEnumerable<EventAttender> eventAttenderList { get; set; }
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
        public int ActionId { get; set; }
        public bool Value { get; set; }
    }
}