using FOSDEM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FOSDEM
{
    public sealed partial class MainPage : Page
    {
        partial void OnNavigateSpecial();

        private DispatcherTimer dispatcherTimer;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = App.Model;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler<object>(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 15, 0);
            dispatcherTimer.Start();

            EnsureCurrentEventVisible();

            OnNavigateSpecial();
            base.OnNavigatedTo(e);
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {
            EnsureCurrentEventVisible();
        }

        private void EnsureCurrentEventVisible()
        {
            //throw new NotImplementedException();
            Event eventToShow = null;
            TimeSpan timeSpan = TimeSpan.MaxValue;
            DateTime checkTime = DateTime.Now;

            foreach (Event currentEvent in App.Model.HeaderEvents.View)
            {
                TimeSpan currentTimeSpan = currentEvent.Start - checkTime;
                if (eventToShow == null || currentTimeSpan < timeSpan)
                {
                    eventToShow = currentEvent;
                    timeSpan = currentTimeSpan;
                }
            }

            if (eventToShow != null)
                ListViewHome.ScrollIntoView(eventToShow);
        }

        partial void EnsureHomeEventVisible(Event eventToShow);

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Frame.Navigate(typeof(DetailPage), (e.ClickedItem as Event).Id);
        }

        private void AppBarToggleButtonFilter_Checked(object sender, RoutedEventArgs e)
        {
            App.Model.IsOnlyGoingVisible = true;
        }

        private void AppBarToggleButtonFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Model.IsOnlyGoingVisible = false;
        }

        private async void AppBarButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            await App.Refresh();
            DataContext = App.Model;
            EnsureCurrentEventVisible();
        }
    }
}
