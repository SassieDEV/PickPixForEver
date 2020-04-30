using PickPixForEver.Helpers;
using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.AuthViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        IAccountRepository accountRepository;
        public RegistrationPage()
        {
            InitializeComponent();
            accountRepository = new AccountRepository(App.FilePath);
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entFirstName.Text) || string.IsNullOrEmpty(entLastName.Text)
                || string.IsNullOrEmpty(entEmail.Text) || string.IsNullOrEmpty(entPassword.Text) || string.IsNullOrEmpty(entConfirmPassword.Text))
            {
                await DisplayAlert("Error", "Required field are missing", "Ok").ConfigureAwait(false);
                return;
            }
            else if (entPassword.Text.Trim() != entConfirmPassword.Text.Trim())
            {
                await DisplayAlert("Error", "Password didn't match", "Ok").ConfigureAwait(false);
                return;
            }

            KeyValuePair<string, string> passwordHashPair = PasswordUtility.EncryptPassword(entPassword.Text);
            var newUser = new User()
            {
                FirstName = entFirstName.Text.Trim(),
                LastName = entLastName.Text.Trim(),
                Email = entEmail.Text.Trim(),
                Password = passwordHashPair.Key,
                PasswordHash = passwordHashPair.Value
            };

            var user = await this.accountRepository.GetUser(newUser.Email).ConfigureAwait(false);
            if(user!=null)
            {
                await DisplayAlert("Error", "Email address already taken", "Ok").ConfigureAwait(false);
                return;
            }

            var userId = await this.accountRepository.RegisterUser(newUser).ConfigureAwait(false);
            if (userId<=0)
            {
               await DisplayAlert("Error", "An error occured while processing your request", "Ok").ConfigureAwait(false);
                return;
            }
            else
            {
               await DisplayAlert("", "Account created successfully", "Ok").ConfigureAwait(true);
               Application.Current.MainPage = new Login();
            }
        }

        private void TapSignIn_Tapped(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Login();
        }
    }
}