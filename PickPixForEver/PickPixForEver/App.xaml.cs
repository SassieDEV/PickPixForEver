using PickPixForEver.AuthViews;
using PickPixForEver.Views;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver
{
    public partial class App : Application
    {
        public static string FilePath { get; private set; }

        public App(string filePath)
        {
            InitializeComponent();
            Debug.WriteLine($"Database located at: {filePath}");
            FilePath = filePath;

            bool isLoggedIn = Device.RuntimePlatform == Device.macOS ?
                !string.IsNullOrEmpty(App.Current.Properties["email"].ToString()) :
                !string.IsNullOrEmpty(Preferences.Get("email", ""));
            if (isLoggedIn)
            {
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new Login();
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
