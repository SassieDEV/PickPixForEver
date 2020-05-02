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
    public partial class EditTag : PopupPage
    {
        TagDetailViewModel viewModel;

        public EditTag()
        {
            InitializeComponent();
            
            this.viewModel = new TagDetailViewModel(App.FilePath);
            BindingContext = this.viewModel;
        }
        public EditTag(TagDetailViewModel viewModel)
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
            if (viewModel.IsNewTag)
            {
                MessagingCenter.Send(this,"CreateTagPopupClosed");
            }
            else
            {
                MessagingCenter.Send(this, "EditTagPopupClosed");
            }
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(viewModel.Name) || 
                string.IsNullOrWhiteSpace(viewModel.TagType)) 
            {
                await DisplayAlert("Validation Error", "Required fields are missing", "Ok").ConfigureAwait(false);
                return;
            }
            if (viewModel.IsNewTag)
            {
                MessagingCenter.Send(this, "SaveTag", this.viewModel.Tag);
            }
            else
            {
                MessagingCenter.Send(this, "UpdateTag", this.viewModel.Tag);
            }
            await PopupNavigation.Instance.PopAsync(true).ConfigureAwait(false);

        }
    }
}