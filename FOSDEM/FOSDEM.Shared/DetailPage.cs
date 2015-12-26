using FOSDEM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FOSDEM
{
    public sealed partial class DetailPage : Page
    {

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string eventId = (string)e.Parameter;
            this.DataContext = App.Conference.Events.FirstOrDefault(item => item.Id == eventId);
        }

        private async void AppBarToggleButtonGoing_Checked(object sender, RoutedEventArgs e)
        {
            (DataContext as Event).IsSelected = true;
            App.Model.Update();
            await App.SaveModel();
        }

        private async void AppBarToggleButtonGoing_Unchecked(object sender, RoutedEventArgs e)
        {
            (DataContext as Event).IsSelected = false;
            App.Model.Update();
            await App.SaveModel();
        }
    }
}
