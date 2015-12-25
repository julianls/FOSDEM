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
        private RuntimeModel model;

        partial void OnNavigateSpecial();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            model = new RuntimeModel((Application.Current as App).Conference);
            DataContext = model;

            OnNavigateSpecial();
            base.OnNavigatedTo(e);
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
