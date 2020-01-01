using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Windows.UI.Xaml;

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

        public List<Link> Links { get; set; }

        public bool IsSelected { get; set; }

        [XmlIgnore]
        public string DayOfWeek { get { return Start.DayOfWeek.ToString(); } }

        [XmlIgnore]
        public string StartHour { get { return Start.TimeOfDay.ToString(); } }

        [XmlIgnore]
        public string EndHour { get { return Start.Add(Duration).TimeOfDay.ToString(); } }

        [XmlIgnore]
        public Visibility SelectedVisibility { get { return IsSelected ? Visibility.Visible : Visibility.Collapsed; } }

        [XmlIgnore]
        public string ImagePath
        {
            get
            {
                switch (Room.Name.ToUpper()[0])
                {
                    case 'J':
                        return @"Assets/buildingj.png";
                    case 'K':
                        return @"Assets/buildingk.png";
                    case 'H':
                        return @"Assets/buildingh.png";
                    case 'U':
                        return @"Assets/buildingu.png";
                    case 'S':
                        return @"Assets/buildings.png";
                    default:
                        return @"Assets/buildingaw.png";
                }
            }
        }
    }
}
