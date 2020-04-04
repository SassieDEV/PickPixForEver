using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPage : ContentPage
    {
        public UploadPage()
        {
            InitializeComponent();
            //TODO - Ask for permissions. This will need to be figured out for UWP
            //Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
        }
        /// <summary>
        ///     Make sure Permissions are given to the users storage.
        /// </summary>
        /// <returns></returns>
        /*private async Task<bool> AskForPermissions()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, Permission.Photos });
                    storagePermissions = results[Permission.Storage];
                    photoPermissions = results[Permission.Photos];
                }

                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissions Denied!", "Please go to your app settings and enable permissions.", "Ok");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error. permissions not set. here is the stacktrace: \n" + ex.StackTrace);
                return false;
            }
        }*/
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            //Check users permissions.
            //var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            //var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);
            //if (storagePermissions == PermissionStatus.Granted && photoPermissions == PermissionStatus.Granted)
            //{
            //If we are on iOS, call GMMultiImagePicker.
            if (Device.RuntimePlatform == Device.UWP)
            {
                var file = await CrossFilePicker.Current.PickFile();

                if (file != null)
                {
                    lbl.Text = file.FileName;
                }

            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                bool imageModifiedWithDrawings = false;
                if (imageModifiedWithDrawings)
                {
                    //await GMMultiImagePicker.Current.PickMultiImage(true);
                }
                else
                {
                    //await GMMultiImagePicker.Current.PickMultiImage();
                }

                MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                {
                    //If we have selected images, put them into the carousel view.
                    if (images.Count > 0)
                    {
                        //ImgCarouselView.ItemsSource = images;
                        //InfoText.IsVisible = true; //InfoText is optional
                    }
                });
            }
            //If we are on Android, call IMediaService.
            else if (Device.RuntimePlatform == Device.Android)
            {
                // DependencyService.Get<IMediaService>().OpenGallery();

                MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
                MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid", (s, images) =>
                {
                    //If we have selected images, put them into the carousel view.
                    if (images.Count > 0)
                    {
                        //ImgCarouselView.ItemsSource = images;
                        //InfoText.IsVisible = true; //InfoText is optional
                    }
                });
            }
           // }
            //else
            //{
              //  await DisplayAlert("Permission Denied!", "\nPlease go to your app settings and enable permissions.", "Ok");
            //}
        }
    }

}