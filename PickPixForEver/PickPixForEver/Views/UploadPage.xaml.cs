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
            View [] allImages = ImagePreview.Children.ToArray<View>();
            string[] text_entPeople = entPeople.Text.ToString().Split(';');
            string[] text_entPlaces = entPlaces.Text.ToString().Split(';');
            string[] text_entEvents = entEvents.Text.ToString().Split(';');
            string[] text_entCustom = entCustom.Text.ToString().Split(';');
            string[] text_entAlbums = entAlbums.Text.ToString().Split(';');
            text_entPeople = text_entPeople.Distinct().ToArray();
            text_entPlaces = text_entPlaces.Distinct().ToArray();
            text_entEvents = text_entEvents.Distinct().ToArray();
            text_entCustom = text_entCustom.Distinct().ToArray();
            text_entAlbums = text_entAlbums.Distinct().ToArray();
            string text_entNotes = entNotes.Text.ToString();
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