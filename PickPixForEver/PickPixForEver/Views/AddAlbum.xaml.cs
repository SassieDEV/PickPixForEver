using PickPixForEver.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAlbum : PopupPage
    {
        AlbumDetailViewModel viewModel;

        public AddAlbum()
        {
            InitializeComponent();
            
            this.viewModel = new AlbumDetailViewModel(App.FilePath);
            BindingContext = this.viewModel;
        }
        public AddAlbum(AlbumDetailViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            if (viewModel.IsNewAlbum)
            {
                MessagingCenter.Send(this,"CreateAlbumPopupClosed");
            }
            else
            {
                MessagingCenter.Send(this, "EditAlbumPopupClosed");
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(viewModel.Name) || 
                string.IsNullOrWhiteSpace(viewModel.Description) || 
                string.IsNullOrWhiteSpace(viewModel.Privacy))
            {
                await DisplayAlert("Validation Error", "Required fields are missing", "Ok").ConfigureAwait(false);
                return;
            }
            if (viewModel.IsNewAlbum)
            {
                MessagingCenter.Send(this, "SaveAlbum", this.viewModel.Album);
            }
            else
            {
                MessagingCenter.Send(this, "UpdateAlbum", this.viewModel.Album);
            }
            await PopupNavigation.Instance.PopAsync(true).ConfigureAwait(false);

        }
    }
}