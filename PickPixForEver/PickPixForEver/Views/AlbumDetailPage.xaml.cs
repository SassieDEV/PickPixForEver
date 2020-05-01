using PickPixForEver.Models;
using PickPixForEver.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (viewModel!=null && viewModel.Album!=null)
            {
                this.viewModel.LoadAlbumPicturesCommand.Execute(viewModel.Album.Id);
            }           
            BindingContext = this.viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateAlbumImages();

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

       void PopulateAlbumImages()
        {
            stackImages.Children.Clear();
            var imageScrollView = new ScrollView();
            var innerStackLayout = new StackLayout();
            imageScrollView.Content = innerStackLayout;
            stackImages.Children.Add(imageScrollView);
            
            foreach (var picture in this.viewModel.Pictures)
            {
                // stackTags.Children.Add(new Button() { Text = picture.Privacy });
                Image image = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(picture.RawData)) };

                Label notes = new Label();
                notes.Text = picture.Notes;
                notes.Style = (Style)Application.Current.Resources["SubHeaderLabel"];
                notes.HorizontalOptions = LayoutOptions.StartAndExpand;

                Label privacy = new Label();
                privacy.Text = picture.Privacy;
                privacy.Style = (Style)Application.Current.Resources["DescriptionLabel"];
                privacy.HorizontalOptions = LayoutOptions.StartAndExpand;
                privacy.Margin = new Thickness(2, 2, 5, 5);
                Frame frame = new Frame();
                frame.BorderColor = Color.Gray;
                frame.CornerRadius = 5;
                frame.Padding = 8;
                frame.Margin = new Thickness(5, 7, 5, 0);


                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition() { Height = 45 });
                grid.RowDefinitions.Add(new RowDefinition() { Height = 35 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 120 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 95 });


                grid.Children.Add(image, 0, 0);
                Grid.SetRowSpan(image, 2);
                grid.Children.Add(notes, 1, 0);
                grid.Children.Add(privacy, 1, 1);
                frame.Content = grid;
                innerStackLayout.Children.Add(frame);
            }
        }

        private async void btnSlideShow_Clicked(object sender, EventArgs e)
        {
            if(this.viewModel.Pictures!=null && this.viewModel.Pictures.Count > 0)
            {
                await Navigation.PushAsync(new SlideViewer(new SlideShowViewModel(this.viewModel.Pictures.Select(s => new KeyValuePair<int, byte[]>(s.Id, s.RawData)).ToList()))).ConfigureAwait(false);

            }
            else
            {
                await DisplayAlert("Empty", "No pictures to show", "Ok").ConfigureAwait(false);
            }
            
        }
    }
}