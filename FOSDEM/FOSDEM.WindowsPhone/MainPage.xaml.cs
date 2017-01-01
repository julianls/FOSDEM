using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FOSDEM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        partial void OnNavigateSpecial()
        {
            BasicGeoposition location = new BasicGeoposition()
            {
                Latitude = 50.812375,
                Longitude = 4.380734
            };

            MapControl.Center = new Geopoint(location);

            var pin = new Grid()
            {
                Width = 24,
                Height = 24,
                Margin = new Windows.UI.Xaml.Thickness(-12)
            };

            pin.Children.Add(new Ellipse()
            {
                Fill = new SolidColorBrush(Colors.DodgerBlue),
                Stroke = new SolidColorBrush(Colors.White),
                StrokeThickness = 3,
                Width = 24,
                Height = 24
            });

            pin.Children.Add(new TextBlock()
            {
                Text = "x",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center
            });

            Windows.UI.Xaml.Controls.Maps.MapControl.SetLocation(pin, new Geopoint(location));
            MapControl.Children.Add(pin);

            MapControl.ZoomLevel = 14;
            MapControl.LandmarksVisible = true;
            MapControl.Style = Windows.UI.Xaml.Controls.Maps.MapStyle.AerialWithRoads;
        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            await App.Refresh();
            DataContext = App.Model;
            PivotMain.SelectedIndex = 1;
        }
    }
}
