using PickPixForEver.AuthViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;

            //MenuPage.add
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("fullName", string.Empty);
            Preferences.Set("email", string.Empty);
            Application.Current.MainPage = new Login();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void Gallery_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new GalleryPage());
            IsPresented = false;
        }

        private void Upload_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new UploadPage());
            IsPresented = false;
        }

        private void AdminView_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AdminPage());
            IsPresented = false;
        }

        private void Tags_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new TagsPage());
            IsPresented = false;
        }
    }
}