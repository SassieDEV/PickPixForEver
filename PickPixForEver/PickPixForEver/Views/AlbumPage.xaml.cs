using PickPixForEver.Models;
using PickPixForEver.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AlbumPage : ContentPage
    {
        AlbumsViewModel albumsViewModel;
        private static readonly int COL_LENGTH = 5;
        public AlbumPage()
        {
            InitializeComponent();
            BindingContext = albumsViewModel = new AlbumsViewModel(App.FilePath);
            MessagingCenter.Subscribe<AddAlbum>(this, "CreateAlbumPopupClosed",  (sender) =>
            {
                populateGrid();
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            albumsViewModel.LoadAlbumsCommand.Execute(null);
            populateGrid();

        }


        private async void Album_Tapped(object sender, EventArgs e)
        {
            StackLayout stackLayout = (StackLayout)sender;
            if (stackLayout.GestureRecognizers.Count > 0)
            {
                try
                {
                    var gesture = (TapGestureRecognizer)stackLayout.GestureRecognizers[0];
                    Album album = (Album)gesture.CommandParameter;
                    await Navigation.PushAsync(new AlbumDetailPage(new AlbumDetailViewModel(App.FilePath, album))).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if (searchBar != null && !string.IsNullOrEmpty(searchBar.Text.Trim()))
            {
                albumsViewModel.SearchAlbumsComand.Execute(searchBar.Text.ToLower().Trim());
            }
            else
            {
                albumsViewModel.LoadAlbumsCommand.Execute(null);
            }
            populateGrid();
        }

        private void populateGrid()
        {
            albumsStackLayout.Children.Clear();
            var albumsScrollView = new ScrollView();
            var albumsGrid = new Grid();
            albumsGrid.RowSpacing = 5;
            albumsGrid.ColumnSpacing = 5;
            albumsGrid.WidthRequest = 1200;

            albumsScrollView.Content = albumsGrid;
            albumsStackLayout.Children.Add(albumsScrollView);
            if (albumsViewModel.Albums.Count == 0)
                return;

            int cnt = albumsViewModel.Albums.Count;
            int rowLength = cnt % 5 == 0 ? cnt / 5 : (cnt / 5) + 1;
            for (int r = 0; r < rowLength; r++)
            {
                albumsGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int c = 0; c < COL_LENGTH; c++)
            {
                albumsGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = 240
                });
            }

            var index = 0;
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < COL_LENGTH; j++)
                {
                    if (index >= cnt)
                    {
                        break;
                    }

                    var album = albumsViewModel.Albums[index];
                    index++;

                    //Initialize Controls
                    var frame = new Frame();
                    var stackLayout = new StackLayout();
                    var albumCover = new Image();
                    var lblAlbumName = new Label();
                    var lblPrivacy = new Label();


                    //Frame properties
                    frame.BorderColor = Color.Gray;
                    frame.CornerRadius = 5;
                    frame.Padding = 8;

                    //Image album cover properties
                    albumCover.HorizontalOptions = LayoutOptions.StartAndExpand;
                    albumCover.Source = ImageSource.FromFile("cover1.png");


                    //Label properties
                    lblAlbumName.Text = album.Name;
                    lblAlbumName.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblAlbumName.Style = (Style)Application.Current.Resources["SubHeaderLabel"];

                    lblPrivacy.Text = $"Privacy: {album.Privacy}";
                    lblPrivacy.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblPrivacy.Style = (Style)Application.Current.Resources["DescriptionLabel"];

                    //Stacklayout click handler
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.CommandParameter = album;
                    tapGestureRecognizer.Tapped += Album_Tapped;
                    stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                    frame.Content = stackLayout;
                    stackLayout.Children.Add(albumCover);
                    stackLayout.Children.Add(lblAlbumName);
                    stackLayout.Children.Add(lblPrivacy);


                    albumsGrid.Children.Add(frame, j, i);
                }
            }
        }
        private async void btnCreate_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new AddAlbum()).ConfigureAwait(false);
        }
    }
}