using FOSDEM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FOSDEM
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        public DetailPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Windows.Phone.UI.Input.HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            e.Handled = true;
            Frame.GoBack();
        }

        private async void ListViewSpeakers_ItemClick(object sender, ItemClickEventArgs e)
        {
            string personid = (e.ClickedItem as Person).Name.Replace(' ', '_');
            personid = personid.ToLower();
            string url = string.Format("https://fosdem.org/2016/schedule/speaker/{0}/", personid);
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private async void ListViewLinks_ItemClick(object sender, ItemClickEventArgs e)
        {
            string url = (e.ClickedItem as Link).Url;
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
