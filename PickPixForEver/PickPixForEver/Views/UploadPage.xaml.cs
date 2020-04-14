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
            throw new NotImplementedException();
        }

            private async Task PickPic(string[] fileTypes) {
            try
            {
                var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);
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
                if (pickedFile != null)
                {
                    //string label = pickedFile.FileName;
                    //string path = pickedFile.FilePath;
                    //selectedImage.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                    //selectedImageName.Text = pickedFile.FileName;
                    Image img = new Image();
                    img.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                    ImagePreview.Children.Add(img);

                    int pictureId = await picRep.EnterImgDataSource(pickedFile.GetStream());
                    System.Diagnostics.Debug.WriteLine("========================================= " + pictureId);

                    /* TODO - Implement file metadata storage from xaml page here
                    if (Device.RuntimePlatform == "Android")
                    {
                        Uri uri = new Uri(pickedFile.FilePath);
                        pickedImage.Source = ImageSource.FromUri(uri);
                        await picRep.EnterPictureSource(pickedImage);
                    }
                    else if (Device.RuntimePlatform == "UWP")
                    {
                        string path reee= pickedFile.FilePath;
                        pickedImage.Source = ImageSource.FromFile(path);
                        //await picRep.EnterPictureSource(pickedImage);
                        //var file = image.Source.GetValue(FileImageSource.FileProperty);
                        IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(path);
                    }
                    //TODO: Figure out iOS
                    else if (Device.RuntimePlatform == "iOS")
                    {
                    }*/
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}