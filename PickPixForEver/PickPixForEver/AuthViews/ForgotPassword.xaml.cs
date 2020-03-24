using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.AuthViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private async void btnForgotPassword_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert("Info", "Password reset is not implemented yet", "Ok").ConfigureAwait(false);
            Application.Current.MainPage = new Login();
        }
    }
}