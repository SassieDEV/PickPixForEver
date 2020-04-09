using PickPixForEver.Models;
using PickPixForEver.ViewModel;
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
    public partial class AlbumPage : ContentPage
    {
        AlbumsViewModel albumsViewModel;
        private static readonly int COL_LENGTH=5;
        public AlbumPage()
        {
            InitializeComponent();
            BindingContext = albumsViewModel = new AlbumsViewModel(App.FilePath);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

                albumsViewModel.LoadAlbumsCommand.Execute(null);
                populateGrid();
                
        }


        private void Album_Tapped(object sender, EventArgs e)
        {
           //To Do
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if(searchBar!=null && !string.IsNullOrEmpty(searchBar.Text.Trim())){
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
            albumsGrid.Children.Clear();
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

                    lblPrivacy.Text = $"Privacy: {GetPrivacy(album.Privacy)}";
                    lblPrivacy.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblPrivacy.Style = (Style)Application.Current.Resources["DescriptionLabel"];

                    //Stacklayout click handler
                    var tapGestureRecognizer = new TapGestureRecognizer();
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


        private string GetPrivacy(int value)
        {
            //Remember to refactor this method
            switch (value)
            {
                case 1:
                    return "Private";
                case 2:
                    return "Public";
                default:
                    return "Public";
            }
        }

        
    }
}