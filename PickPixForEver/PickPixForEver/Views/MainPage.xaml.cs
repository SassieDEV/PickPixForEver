using PickPixForEver.AuthViews;
using PickPixForEver.ViewModel;
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
        BaseViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            BindingContext = viewModel = new BaseViewModel();

            //MenuPage.add
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set("fullName", string.Empty);
            Preferences.Set("email", string.Empty);
            Preferences.Set("userId", string.Empty);
            this.viewModel.IsLoggedIn = false;
            this.viewModel.DisplayName = "Guest";
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

        private void Album_Tapped(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AlbumPage());
            IsPresented = false;
        }

        private void Login_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}