using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using PickPixForEver.Models;
using PickPixForEver.Services;

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

        private async Task PickPic(string[] fileTypes) {
            try
            {
                if(Device.RuntimePlatform == "UWP")
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
                    var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);
                }
                //TODO: Figure out iOS
                else if (Device.RuntimePlatform == "iOS")
                {
                    var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);
                }
                if (pickedFile != null)
                {
                    //string label = pickedFile.FileName;
                    //string path = pickedFile.FilePath;
                    selectedImage.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                    Image pickedImage = new Image();
                    //Image pickedImage = new Image;
                    if (Device.RuntimePlatform == "Android")
                    {
                        Uri uri = new Uri(pickedFile.FilePath);
                        pickedImage.Source = ImageSource.FromUri(uri);
                        await picRep.EnterPictureSource(pickedImage);
                    }
                    else if (Device.RuntimePlatform == "UWP")
                    {
                        string path = pickedFile.FilePath;
                        pickedImage.Source = ImageSource.FromFile(path);
                        await picRep.EnterPictureSource(pickedImage);
                    }
                    //TODO: Figure out iOS
                    else if (Device.RuntimePlatform == "iOS")
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}