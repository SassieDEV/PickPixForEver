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
    public partial class TagsPage : PopupPage
    {
        TagsViewModel viewModel;

        public TagsPage()
        {
            InitializeComponent();
            this.viewModel = new TagsViewModel(App.FilePath);
            BindingContext = this.viewModel;
        }

        public TagsPage(TagsViewModel tViewModel)
        {
            InitializeComponent();
            this.viewModel = tViewModel;
            BindingContext = this.viewModel;
        }


        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync(true);
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "UpdatePictureDetails", this.viewModel.picTag);
            await PopupNavigation.Instance.PopAsync(true).ConfigureAwait(false);
        }
    }
}