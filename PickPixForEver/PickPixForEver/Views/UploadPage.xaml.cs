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

        public UploadPage()
        {
            InitializeComponent();
        }
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            //set fileTypes for image picking
            string[] fileTypes = null; 

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

            await PickPic(fileTypes);
        }
        private async void SelectUploadButton_Clicked(object sender, EventArgs e)
        {
            //TODO: Will throw exception if no image was uploaded, need to add warning to UploadPage.xaml
            View [] allImages = ImagePreview.Children.ToArray<View>();
            string[][] megatags = new string[5][];

            //TODO: See if there's a more elegant solution to checking if each entry is null before assinging string[]
            string[] text_entPeople = Array.Empty<string>();
            if (entPeople.Text != null) 
            {
                text_entPeople = entPeople.Text.ToString().Split(';');
                text_entPeople = text_entPeople.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                text_entPeople = text_entPeople.Distinct().ToArray();
            }
            string[] text_entPlaces = Array.Empty<string>();
            if (entPlaces.Text != null)
            {
                text_entPlaces = entPlaces.Text.ToString().Split(';');
                text_entPlaces = text_entPlaces.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                text_entPlaces = text_entPlaces.Distinct().ToArray();
            }
            string[] text_entEvents = Array.Empty<string>();
            if (entEvents.Text != null)
            {
                text_entEvents = entEvents.Text.ToString().Split(';');
                text_entEvents = text_entEvents.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                text_entEvents = text_entEvents.Distinct().ToArray();
            }
            string[] text_entCustom = Array.Empty<string>();
            if (entCustom.Text != null)
            {
                text_entCustom = entCustom.Text.ToString().Split(';');
                text_entCustom = text_entCustom.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                text_entCustom = text_entCustom.Distinct().ToArray();
            }
            string[] text_entAlbums = Array.Empty<string>();
            if (entAlbums.Text != null) {
                text_entAlbums = entAlbums.Text.ToString().Split(';');
                text_entAlbums = text_entAlbums.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                text_entAlbums = text_entAlbums.Distinct().ToArray();
            }

            //TODO: This throws null if no notes, fix and reimplement
            //string text_entNotes = entNotes.Text.ToString();
            string text_entNotes = "";

            megatags = new string[][]{ text_entPeople, text_entPlaces, text_entEvents, text_entCustom, text_entAlbums };
            await picRep.HandleImage(allImages, megatags, text_entNotes);
        }
            
        private async Task PickPic(string[] fileTypes) {
            try
            {
                //var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);
                /*(if (Device.RuntimePlatform == "UWP")
                {
                    var picker = new Windows.Storage.Pickers.FileOpenPicker();
                    picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                    picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                    picker.FileTypeFilter.Add(".jpg");
                    picker.FileTypeFilter.Add(".jpeg");
                    picker.FileTypeFilter.Add(".png");

                    Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
                }
                else if(Device.RuntimePlatform == "Android")
                {
                }
                //TODO: Figure out iOS
                else if (Device.RuntimePlatform == "iOS")
                {
                    var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);
                }*/
                //if (pickedFile != null)
                //{
                //string fName = pickedFile.FileName;
                //string fPath = pickedFile.FilePath;
                //byte[] imageArray = System.IO.File.ReadAllBytes(fPath);

                //TODO: Remove this hardcoded file and re-implement filepicker
                string path = "cover1.png";
                FileStream pickedFile = File.OpenRead(path);

                Image img = new Image();
                //img.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                //TODO: Remove the below image source to hardfile and reimplement above to filepicker
                img.Source = ImageSource.FromStream(() => pickedFile);
                byte[] imgByte = null;
                using(MemoryStream mS = new MemoryStream())
                {
                }
                ImagePreview.Children.Add(img);
                //string base64 = Convert.ToBase64String(imageArray);
                Picture p = new Picture();
                //p.RawData = base64;
                p.Privacy = "Public";
                p.Created = DateTime.Now;
                p.Updated = DateTime.Now;
                p.PictureMetaData = "";
                p.Notes = "Family vacation";
                //img.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                //TODO: Remove the below image source to hardfile and reimplement above to filepicker
                int pictureId = await picRep.EnterImgDataSource(pickedFile);
                System.Diagnostics.Debug.WriteLine("========================================= " + pictureId);
                    
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}