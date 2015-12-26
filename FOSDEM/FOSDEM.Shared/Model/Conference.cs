using System;
using System.Collections.Generic;
using System.Text;

namespace FOSDEM.Model
{
    public class Conference
    {
        public Conference()
        {
            EventTypes = new List<EventType>();
            Tracks = new List<Track>();
            Persons = new List<Person>();
            Rooms = new List<Room>();
            Days = new List<Day>();
            Events = new List<Event>();
        }

        //<title>FOSDEM 2016</title>
        public string Title { get; set; }

        //<subtitle/>
        public string Subtitle { get; set; }

        //<venue>ULB(Université Libre de Bruxelles)</venue>
        public string Venue { get; set; }

        //<city>Brussels</city>
        public string City { get; set; }

        //<start>2016-01-30</start>
        public DateTime Start { get; set; }

        //<end>2016-01-31</end>
        public DateTime End { get; set; }

        //<days>2</days>
        public int NumberOfDays { get; set; }

        //<day_change>09:00:00</day_change>
        public TimeSpan DayChange { get; set; }

        //<timeslot_duration>00:05:00</timeslot_duration>
        public TimeSpan TimeslotDuration { get; set; }

        public List<EventType> EventTypes { get; set; }

        public List<Track> Tracks { get; set; }

        public List<Person> Persons { get; set; }

        public List<Room> Rooms { get; set; }

        public List<Day> Days { get; set; }

        public List<Event> Events { get; set; }
    }
}
