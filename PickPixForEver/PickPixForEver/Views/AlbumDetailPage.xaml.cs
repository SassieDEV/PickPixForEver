using PickPixForEver.Models;
using PickPixForEver.ViewModel;
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
    public partial class AlbumDetailPage : ContentPage
    {
        List<Picture> pictures = new List<Picture>();
        AlbumDetailViewModel viewModel;
        public AlbumDetailPage(AlbumDetailViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
            for (int i = 0; i < 10; i++)
            {
                pictures.Add(new Picture() { Notes=$"Test {i}"});
            }
            lvImages.ItemsSource = pictures;
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void btnEditAlbum_Clicked(object sender, EventArgs e)
        {
            Album album = new Album();
            album = this.viewModel.Album;
            await PopupNavigation.Instance.PushAsync(new AddAlbum(new AlbumDetailViewModel(App.FilePath, album))).ConfigureAwait(false);
        }
    }
}