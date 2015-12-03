using System;
using System.Collections.Generic;
using System.Text;

namespace FOSDEM.Model
{
    public class Event
    {
        public string Id { get; set; }

        public DateTime Start { get; set; }

        public TimeSpan Duration { get; set; }

        public Room Room { get; set; }

        public Day Day { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public Track Track { get; set; }

        public EventType Type { get; set; }

        public string Language { get; set; }

        public string Abstract { get; set; }

        public string Description { get; set; }

        public List<Person> Persons { get; set; }

        public List<string> Links { get; set; }

        public bool IsSelected { get; set; }
    }
}
