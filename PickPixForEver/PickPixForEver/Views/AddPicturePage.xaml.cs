using MetadataExtractor;
using PickPixForEver.Services;
using PickPixForEver.ViewModel;
using Plugin.FilePicker;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPicturePage : PopupPage
    {
        private const long MAX_IMAGE_SIZE_SUPPORTED = 2500000;
        PicturesRepository picRep = new PicturesRepository(App.FilePath);
        public string[] fileTypes { get; set; }
        public int UserId { get; set; }
        public Dictionary<Stream, string> Streams { get; set; }
        AlbumsViewModel albumsViewModel;

        public AddPicturePage()
        {
            InitializeComponent();
            UserId = Preferences.Get("userId", -1);
            Streams = new Dictionary<Stream, string>();
            albumsViewModel = new AlbumsViewModel(App.FilePath);
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            albumsViewModel.LoadItemCommand.Execute(null);
            entAlbums.ItemsSource = albumsViewModel.Albums.Select(s=>s.Name).ToList();
        }

        private void btnUploadImages_Clicked(object sender, EventArgs e)
        {
           
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                fileTypes = new string[] { "image/png", "image/jpeg" };
            }

            if (Device.RuntimePlatform == Device.iOS)
            {
                fileTypes = new string[] { "public.image" };
            }

            if (Device.RuntimePlatform == Device.UWP)
            {
                fileTypes = new string[] { ".jpg", ".png" };
            }
            await PickPic(sender, e).ConfigureAwait(false);
        }

        private async Task PickPic(object s, EventArgs e)
        {
            var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);

            if (pickedFile != null)
            {
                if(pickedFile.GetStream().Length>MAX_IMAGE_SIZE_SUPPORTED)
                {
                    await DisplayAlert("Info", "Uploaded photo is bigger than the maximum 2.5 MB image size" +
                        " supported by this app. Please upload a smaller size image ", "Ok").ConfigureAwait(false);
                    return;
                }
                Frame frame = new Frame();
                frame.BorderColor = Color.Gray;
                frame.CornerRadius = 5;
                frame.Padding = 3;
                Image image = new Image();
                image.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                image.HeightRequest = 150;
                image.WidthRequest = 150;              

                frame.Content = image;
                ImagePreview.Children.Add(frame);
                //IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories = ImageMetadataReader.ReadMetadata(pickedFile.GetStream());
                Streams.Add(pickedFile.GetStream(), pickedFile.FilePath);
            }
        }

        private string[] FormatTagsAlbums(string tagAlbumLine)
        {
            if (tagAlbumLine == null)
                return Array.Empty<string>();
            return tagAlbumLine.Split(';').Where(x => !string.IsNullOrEmpty(x)).Distinct().Select(s=>s.Trim()).ToArray();
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            if (ImagePreview.Children.Count == 0)
            {
                await DisplayAlert("Error", "No image selected", "Ok").ConfigureAwait(false);
                return;
            }

            string[][] megatags = new string[5][];
 
            string[]  text_entPeople =  FormatTagsAlbums(entPeople.Text);
            string[]  text_entPlaces =  FormatTagsAlbums(entPlaces.Text);
            string[]  text_entEvents =  FormatTagsAlbums(entEvents.Text);
            string[]  text_entRelationships = FormatTagsAlbums(entRelationships.Text);
            string[]  text_entCustom =  FormatTagsAlbums(entCustom.Text);            


            string text_entNotes = !string.IsNullOrWhiteSpace(entNotes.Text)? entNotes.Text.Trim():string.Empty;
            megatags = new string[][] { text_entPeople, text_entPlaces, text_entEvents, text_entCustom, text_entRelationships };
            int albumId = 0;
            if(albumsViewModel.Albums!=null && albumsViewModel.Albums.Count>0 && entAlbums.SelectedIndex > -1)
            {
                albumId = albumsViewModel.Albums.Where(s => s.Name == (string)entAlbums.SelectedItem).Select(s => s.Id).SingleOrDefault();
            }
            try
            {
                await picRep.HandleImageCommit(this.UserId, Streams, megatags, albumId, (string)Privacy.SelectedItem, text_entNotes).ConfigureAwait(false);
                DisplayAlert("Success", "Your pictures saved successfully!","Ok");
                ResetControls();
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "An error occured while processing your request", "Ok").ConfigureAwait(false);
            }          
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "OnPopupClosed");
        }

        void ResetControls()
        {
            ImagePreview.Children.Clear();
            Streams = new Dictionary<Stream, string>();
            entPeople.Text = string.Empty;
            entPlaces.Text = string.Empty;
            entEvents.Text = string.Empty;
            entCustom.Text = string.Empty;
            entRelationships.Text = string.Empty;
            entNotes.Text = string.Empty;
            entAlbums.SelectedIndex = -1;
            
        }
    }
}