using FOSDEM.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace FOSDEM
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        private const string CacheFileName = "conference.cache";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
        }

        public Conference Conference { get; private set; }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                await LoadConferenceDataOnStart();

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        private async Task LoadConferenceDataOnStart()
        {
            if (Conference == null)
                await LoadFromLocalSorage();

            if (Conference == null)
                await LoadFromWeb();
        }

        private async Task LoadFromLocalSorage()
        {
            try
            {
                var folder = Package.Current.InstalledLocation;
                var file = await folder.GetFileAsync(CacheFileName);
                var read = await FileIO.ReadTextAsync(file);

                XmlSerializer serializer = new XmlSerializer(typeof(Conference));
                using (Stream reader = await file.OpenStreamForReadAsync())
                {
                    // Call the Deserialize method to restore the object's state.
                    this.Conference = serializer.Deserialize(reader) as Conference;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading from local storage: " + ex);
            }
        }

        private async Task LoadFromWeb()
        {
            try
            {
                HttpClient http = new System.Net.Http.HttpClient();
                string url = "https://archive.fosdem.org/2015/schedule/xml";
                HttpResponseMessage response = await http.GetAsync(url);
                string xml = await response.Content.ReadAsStringAsync();

                LoadFromXml(xml);

                var folder = Package.Current.InstalledLocation;
                var file = await folder.CreateFileAsync(CacheFileName);
                XmlSerializer serializer = new XmlSerializer(typeof(Conference));
                using (Stream writer = await file.OpenStreamForWriteAsync())
                {
                    // Call the Deserialize method to restore the object's state.
                    serializer.Serialize(writer, this.Conference);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading from web: " + ex);
            }
        }

        private void LoadFromXml(string xml)
        {
            ModelLoader modelLoader = new ModelLoader(xml);
            modelLoader.Load();

            if (Conference == null)
                Conference = modelLoader.Conference;
            else
            {
                Conference currentConference = Conference;
                Conference = modelLoader.Conference;
                foreach (var newEvent in Conference.Events)
                {
                    Event currentEvent = currentConference.Events.FirstOrDefault(item => item.Id == newEvent.Id);
                    if (currentEvent != null)
                        newEvent.IsSelected = currentEvent.IsSelected;
                }
            }
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}