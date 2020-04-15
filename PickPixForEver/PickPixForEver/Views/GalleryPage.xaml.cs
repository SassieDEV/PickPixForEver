using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PickPixForEver.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;
using PickPixForEver.ViewModel;
using System.Diagnostics;
using Rg.Plugins.Popup.Services;
//using Windows.Storage.Pickers;
//using Windows.UI.Xaml.Media.Imaging;
//using Windows.Storage;
//using Windows.Storage.Streams;
using System.IO;
//using System.Runtime.InteropServices.WindowsRuntime;
using Plugin.FilePicker;
using PickPixForEver.Services;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        PictureRepository picRep = new PictureRepository(App.FilePath);
        private static string[] fileTypes = new string[] { ".jpg", ".jpeg", ".png" };
        private Grid galleryView = new Grid();
        public GalleryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //CreateGrid(null);
            this.loadImages();
        }


        private async void loadImages()
        {
            /*var pics = await this.picRep.GetItemsAsync().ConfigureAwait(false) ;
            foreach (Picture pic in pics)
            {
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => new MemoryStream(pic.RawData));
                ImagePreview.Children.Add(img);
            }*/
        }


        private async void new_imagePicker(object s, EventArgs e)
        {
            var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);

            if (pickedFile != null)
            {
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                ImagePreview.Children.Add(img);

                int pictureId = await picRep.EnterImgDataSource(pickedFile.GetStream());
                System.Diagnostics.Debug.WriteLine("========================================= " + pictureId);
            }
        }


        /*private async void SelectMultiImage_Tapped(object sender, EventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");

            // For multiple image selection
            var files = await openPicker.PickMultipleFilesAsync();
            foreach (var singleImage in files)
            {
                var stream = await singleImage.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                var image = new WriteableBitmap(200, 200);// BitmapImage();
                image.SetSource(stream);

                //var path = singleImage.Path;
                var str = image.PixelBuffer.AsStream();
                byte[] pixels = new byte[str.Length];
                str.Read(pixels, 0, pixels.Length);

                
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => new MemoryStream(pixels));
                String b64Str = Convert.ToBase64String(pixels);
                System.Diagnostics.Debug.WriteLine(b64Str);
                ImagePreview.Children.Add(img);


                //TestImg.Source = ImageSource.FromStream(() => new MemoryStream(pixels));
                //ImagePreview.Children.Add(new Image() { Source = "logo.png" });

                Image Nimg = new Image();
                Nimg.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(b64Str)));
                ImagePreview.Children.Add(Nimg);
            }

            foreach (var file in files)
            {
                //var stream = await file.OpenStreamForReadAsync(); //await file.OpenAsync(FileAccessMode.Read);
                //BitmapImage image = new BitmapImage();
                //image.SetSource(stream); 

                //StorageFile Fpath = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///logo.png"));
                //String b64Str = Convert.ToBase64String(System.IO.File.ReadAllBytes(Fpath.Path));
                //Image img = new Image();
                //img.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(b64Str)));
                //ImagePreview.Children.Add(img);
            }
        }*/


        private void CreateGrid(IEnumerable<Image> pictures)
        {
            var colCount = Application.Current.MainPage.Width / 180;
            for (int i=0; i<colCount; i++)
            {
                galleryView.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((float)1/(float)colCount, GridUnitType.Star) });
            }

            for (int row = 0; row < 12; row++) //pictures.ToList().Count
            {
                galleryView.RowDefinitions.Add(new RowDefinition() { Height = 180 });
                for (int col = 0; col < colCount; col++)
                {
                    var img = new Image()
                    {
                        Source = "logo.png"//PickPixForEver.Resources.logo.png
                    };
                    var tapAction = new TapGestureRecognizer
                    {
                        TappedCallback = (v, o) => {
                            Console.WriteLine("Image clicked");
                            PopupNavigation.Instance.PushAsync(new AddAlbum()).ConfigureAwait(false);
                        },
                        NumberOfTapsRequired = 1
                    };
                    img.GestureRecognizers.Add(tapAction);
                    galleryView.Children.Add(img, col, row);
                }
            }
            //PhotoGallery.Content = galleryView;
        }
    }
}