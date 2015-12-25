using FOSDEM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace FOSDEM
{
    public class RuntimeModel
    {
        private Conference conference;

        public RuntimeModel(Conference conference)
        {
            this.conference = conference;
            HeaderEvents = new CollectionViewSource() { Source = new List<Event>(conference.Events), IsSourceGrouped = false };

            List<Event> firstDayEvents = new List<Event>(conference.Events.Where(item => item.Day.Index == 1));
            FirstDayEvents = new CollectionViewSource() { Source = firstDayEvents.ToGroups(x => x.Start, x => x.Track.Name), IsSourceGrouped = true };

            List<Event> secondDayEvents = new List<Event>(conference.Events.Where(item => item.Day.Index == 2));
            SecondDayEvents = new CollectionViewSource() { Source = secondDayEvents.ToGroups(x => x.Start, x => x.Track.Name), IsSourceGrouped = true };

            //HeaderEvents = new List<Event>(conference.Events);
            //FirstDayEvents = new List<Event>(conference.Events.Where(item => item.Day.Index == 1));
            //SecondDayEvents = new List<Event>(conference.Events.Where(item => item.Day.Index == 2));
            //HeaderEvents = new CollectionViewSource() { Source = new ObservableCollection<Event>(new List<Event>(conference.Events)) };
            //FirstDayEvents = new CollectionViewSource() { Source = new ObservableCollection<Event>(new List<Event>(conference.Events.Where(item => item.Day.Index == 1))) };
            //SecondDayEvents = new CollectionViewSource() { Source = new ObservableCollection<Event>(new List<Event>(conference.Events.Where(item => item.Day.Index == 2))) };
        }


        //public List<Event> HeaderEvents { get; private set; }

        //public List<Event> FirstDayEvents { get; private set; }

        //public List<Event> SecondDayEvents { get; private set; }

        public CollectionViewSource HeaderEvents { get; private set; }

        public CollectionViewSource FirstDayEvents { get; private set; }

        public CollectionViewSource SecondDayEvents { get; private set; }
    }
}
