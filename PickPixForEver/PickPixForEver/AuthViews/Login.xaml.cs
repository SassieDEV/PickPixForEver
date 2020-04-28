using PickPixForEver.Helpers;
using PickPixForEver.Services;
using PickPixForEver.ViewModel;
using PickPixForEver.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.AuthViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        IAccountRepository accountRepository;
        BaseViewModel viewModel;
        public Login()
        {
            InitializeComponent();
            accountRepository = new AccountRepository(App.FilePath);
            BindingContext = viewModel = new BaseViewModel();
        }

        private void TapSignup_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new RegistrationPage();
        }

        private void TapForgotPassword_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new ForgotPassword();
        }

        private void bntLoginView_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new MainPage();
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entEmail.Text) || string.IsNullOrEmpty(entPassword.Text))
            {
                await DisplayAlert("Error", "Email and Password fields are required", "Ok").ConfigureAwait(false);
                return;
            }
            var user = await this.accountRepository.GetUser(entEmail.Text).ConfigureAwait(false);
             if (user == null || (user.Password != PasswordUtility.EncryptPassword(entPassword.Text, user.PasswordHash).Key))
                {
                await DisplayAlert("Error", "Incorrect email or password", "Ok").ConfigureAwait(false);
                return;
               }
            Preferences.Set("userId", user.Id);
            Preferences.Set("fullName",$"{user.FirstName} {user.LastName}");
            Preferences.Set("email", user.Email);
            this.viewModel.IsLoggedIn = true;
            this.viewModel.DisplayName = $"{user.FirstName} {user.LastName}";
            Application.Current.MainPage = new MainPage();
        }        
    }
}