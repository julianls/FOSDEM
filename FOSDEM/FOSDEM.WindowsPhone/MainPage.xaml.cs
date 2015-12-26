using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            MapControl.Center =
                new Geopoint(new BasicGeoposition()
                {
                    Latitude = 50.812375,
                    Longitude = 4.380734
                });

            MapControl.ZoomLevel = 14;
            MapControl.LandmarksVisible = true;
            MapControl.Style = Windows.UI.Xaml.Controls.Maps.MapStyle.AerialWithRoads;
        }
    }
}
