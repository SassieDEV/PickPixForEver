using PickPixForEver.Models;
using PickPixForEver.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        GalleryViewModel galleryViewModel;

        public GalleryPage()
        {
            InitializeComponent();
            BindingContext = galleryViewModel = new GalleryViewModel(App.FilePath);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            galleryViewModel.LoadPicturesCommand.Execute(null);
            galleryViewModel.LoadAlbumsCommand.Execute(null);
            galleryViewModel.LoadTagsCommand.Execute(null);
            BindListViews();
            DisplayPictures();
        }

        private void BindListViews()
        {
            var albums = galleryViewModel.Albums;
            var people = galleryViewModel.Tags.Where(s => s.TagType == "People").ToList();
            var places = galleryViewModel.Tags.Where(s => s.TagType == "Places").ToList();
            var events = galleryViewModel.Tags.Where(s => s.TagType == "Events").ToList();
            var custom = galleryViewModel.Tags.Where(s => s.TagType == "Custom").ToList(); ;
            lvAlbums.ItemsSource = albums;
            lvPeople.ItemsSource = people;
            lvPlace.ItemsSource = places;
            lvEvent.ItemsSource = events;
            lvAlbums.HeightRequest = 40 * (albums.Count<=3?albums.Count:3);
            lvPlace.HeightRequest = 40 *  (places.Count<=3?places.Count:3);
            lvPeople.HeightRequest = 40 * (people.Count<=3?people.Count:3);
            lvEvent.HeightRequest = 40 *  (events.Count<=3?events.Count:3);
        }
        private void DisplayPictures()
        {
            stackAllImages.Children.Clear();
            if (galleryViewModel.Pictures!=null && galleryViewModel.Pictures.Count > 0)
            {
                var picturesGroup = galleryViewModel.Pictures.GroupBy(p => new { Month = p.Created.Month, Year = p.Created.Year }).
                    ToDictionary(g => g.Key, g => g.Select(s => s.ImageArray)).OrderByDescending(y=>y.Key.Year).ThenByDescending(m=>m.Key.Month);
                foreach (var item in picturesGroup)
                {
                   AddPictureGroup(item.Key.Month,item.Key.Year,item.Value);
                }
            }
            
        }
        void AddPictureGroup(int month, int year, IEnumerable<byte[]> pictureArray)
        {
            //Month and Year group
            StackLayout monthYearStack = new StackLayout();
            monthYearStack.Orientation = StackOrientation.Horizontal;
            Label monthYearLabel = new Label();
            monthYearLabel.Margin = new Thickness(5, 5, 0, 0);
            monthYearLabel.TextColor = Color.LightGray;
            monthYearLabel.Text = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}";
              
            monthYearStack.Children.Add(monthYearLabel);

            //Pictures group stack
            StackLayout picGroupStack = new StackLayout();
            picGroupStack.Margin = new Thickness(0, 5, 0, 0);
            picGroupStack.Orientation = StackOrientation.Horizontal;

            //Horizontal scrollview for the pictures within a given month
            ScrollView scrollView = new ScrollView();
            scrollView.Orientation = ScrollOrientation.Horizontal;
            scrollView.WidthRequest = 1500;

            //All pictures added within a given month
            StackLayout picStack = new StackLayout();
            picStack.Margin = new Thickness(0, 0, 0, 25);
            picStack.Orientation = StackOrientation.Horizontal;

            scrollView.Content = picStack;
            picGroupStack.Children.Add(scrollView);
            foreach (var picture in pictureArray)
            {
                picStack.Children.Add(AddPictureFrame(picture));
            }
            stackAllImages.Children.Add(monthYearStack);
            stackAllImages.Children.Add(picGroupStack);
        }


        /// <summary>
        /// Converts a byte array into image and returns a picture within a frame
        /// </summary>
        /// <param name="imageArray"></param>
        /// <returns>Frame</returns>
        Frame AddPictureFrame(byte[] imageArray)
        {
            Frame frame = new Frame();
            frame.BorderColor = Color.Gray;
            frame.CornerRadius = 5;
            frame.Padding = 8;
            Image image = new Image();
            image.Source = ImageSource.FromStream(() => new MemoryStream(imageArray));
            image.HeightRequest = 120;
            image.WidthRequest = 120;

            frame.Content = image;
            return frame;
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if (searchBar != null && !string.IsNullOrWhiteSpace(searchBar.Text.Trim()))
            {
                galleryViewModel.SearchItemComand.Execute(searchBar.Text.ToLower().Trim());
            }
            else
            {
                galleryViewModel.LoadPicturesCommand.Execute(null);
            }
            DisplayPictures();
        }

        private void lvAlbums_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Album album)
            {
                galleryViewModel.LoadAlbumPicturesCommand.Execute(album.Id);
                DisplayPictures();
                lvPlace.SelectedItem = null;
                lvPeople.SelectedItem = null;
                lvEvent.SelectedItem = null;
            }
        }

        private void lvPlace_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Tag tag)
            {
                galleryViewModel.LoadTaggedPicturesCommand.Execute(tag.TagId);
                DisplayPictures();
                lvAlbums.SelectedItem = null;
                lvPeople.SelectedItem = null;
                lvEvent.SelectedItem = null;
            }
        }

        private void lvEvent_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Tag tag)
            {
                galleryViewModel.LoadTaggedPicturesCommand.Execute(tag.TagId);
                DisplayPictures();
                lvAlbums.SelectedItem = null;
                lvPeople.SelectedItem = null;
                lvPlace.SelectedItem = null;
            }
        }

        private void lvPeople_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Tag tag)
            {
                galleryViewModel.LoadTaggedPicturesCommand.Execute(tag.TagId);
                DisplayPictures();
                lvAlbums.SelectedItem = null;
                lvPlace.SelectedItem = null;
                lvEvent.SelectedItem = null;
            }
        }

        private async void btnSlideShow_Clicked(object sender, EventArgs e)
        {
            if (this.galleryViewModel.Pictures != null && this.galleryViewModel.Pictures.Count > 0)
            {
                await Navigation.PushAsync(new SlideViewer(new SlideShowViewModel(this.galleryViewModel.Pictures.Select(s => s.ImageArray).ToList()))).ConfigureAwait(false);
            }
            else
            {
                await DisplayAlert("Empty", "No pictures to show", "Ok").ConfigureAwait(false);
            }
        }

        private void btnAdd_Clicked(object sender, EventArgs e)
        {

        }

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            lvAlbums.SelectedItem = null;
            lvPlace.SelectedItem = null;
            lvEvent.SelectedItem = null;
            lvPeople.SelectedItem = null;
            txtSearch.Text = string.Empty;
            galleryViewModel.LoadPicturesCommand.Execute(null);
            DisplayPictures();
        }
    }
}