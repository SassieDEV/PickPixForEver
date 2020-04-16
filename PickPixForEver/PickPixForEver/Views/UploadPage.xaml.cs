using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using PickPixForEver.Models;
using PickPixForEver.Services;
using MetadataExtractor;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPage : ContentPage
    {
        //TODO: Refactor dbContext and possibly PictureRepository to main page after login
        PictureRepository picRep = new PictureRepository(App.FilePath);
        //set fileTypes for image picking
        string[] fileTypes = null;
        List<int> picIds = new List<int>();

        public UploadPage()
        {
            InitializeComponent();
        }
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
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
            await PickPic(sender, e);
        }
        private async void SelectUploadButton_Clicked(object sender, EventArgs e)
        {
            //TODO: Will throw exception if no image was uploaded, need to add warning to UploadPage.xaml
            View [] allImages = ImagePreview.Children.ToArray<View>();
            string[][] megatags = new string[5][];
            string[] albums = Array.Empty<string>();
      
            //TODO: See if there's a more elegant solution to checking if each entry is null before assinging string[]
            string[] text_entPeople = Array.Empty<string>();
            if (entPeople.Text != null)
            {
                text_entPeople = await FormatTagsAlbums(entPeople.Text);
            }
            string[] text_entPlaces = Array.Empty<string>();
            if (entPlaces.Text != null)
            {
                text_entPlaces = await FormatTagsAlbums(entPlaces.Text);
            }
            string[] text_entEvents = Array.Empty<string>();
            if (entEvents.Text != null)
            {
                text_entEvents = await FormatTagsAlbums(entEvents.Text);
            }
            string[] text_entCustom = Array.Empty<string>();
            if (entCustom.Text != null)
            {
                text_entCustom = await FormatTagsAlbums(entCustom.Text);
            }
            string[] text_entAlbums = Array.Empty<string>();
            if (entAlbums.Text != null) {
                text_entAlbums = await FormatTagsAlbums(entAlbums.Text);
            }

            //TODO: This throws null if no notes, fix and reimplement
            //string text_entNotes = entNotes.Text.ToString();
            string text_entNotes = "";

            megatags = new string[][]{ text_entPeople, text_entPlaces, text_entEvents, text_entCustom };
            albums = text_entAlbums;

            await picRep.HandleImageCommit( megatags, albums, text_entNotes);
        }

        private async Task<String[]> FormatTagsAlbums(string tagAlbumLine)
        {
            string[] tagArray = tagAlbumLine.Split(';').Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
            foreach (string s in tagArray)
                s.Trim();
            return tagArray;
        }

        private async Task<int> PickPic(object s, EventArgs e)
        {
            var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);

            if (pickedFile != null)
            {
                int picIdNew = 0;
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                ImagePreview.Children.Add(img);
                IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories = ImageMetadataReader.ReadMetadata(pickedFile.GetStream());
                await picRep.InitPic(pickedFile.GetStream(), pickedFile.FilePath, metaDataDirectories);
            }
            return 0;
        }
    }
}