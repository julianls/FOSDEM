using FOSDEM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Data;
using System;

namespace FOSDEM
{
    public class RuntimeModel : INotifyPropertyChanged
    {
        private Conference conference;

        public event PropertyChangedEventHandler PropertyChanged;

        public RuntimeModel(Conference conference)
        {
            this.conference = conference;
            Update();
        }


        public CollectionViewSource HeaderEvents { get; private set; }

        public CollectionViewSource FirstDayEvents { get; private set; }

        public CollectionViewSource SecondDayEvents { get; private set; }

        public bool IsOnlyGoingVisible
        {   get
            {
                return App.IsOnlyGoingVisible;
            }

            set
            {
                App.IsOnlyGoingVisible = value;
                Update();
            }
        }

        internal void Update()
        {
            HeaderEvents = new CollectionViewSource() { Source = new List<Event>(conference.Events.Where(item => item.IsSelected).OrderBy(item => item.Start)), IsSourceGrouped = false };
            if(PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("HeaderEvents"));

            List<Event> firstDayEvents = new List<Event>(conference.Events.Where(item => isVisibleEvent(item, 1)));
            FirstDayEvents = new CollectionViewSource() { Source = firstDayEvents.ToGroups(x => x.Start, x => x.Track.Name), IsSourceGrouped = true };
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("FirstDayEvents"));

            List<Event> secondDayEvents = new List<Event>(conference.Events.Where(item => isVisibleEvent(item, 2)));
            SecondDayEvents = new CollectionViewSource() { Source = secondDayEvents.ToGroups(x => x.Start, x => x.Track.Name), IsSourceGrouped = true };
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("SecondDayEvents"));
        }

        private bool isVisibleEvent(Event e, int dayIndex)
        {
            return e.Day.Index == dayIndex && 
                (e.IsSelected || IsOnlyGoingVisible == false);
        }
    }
}
