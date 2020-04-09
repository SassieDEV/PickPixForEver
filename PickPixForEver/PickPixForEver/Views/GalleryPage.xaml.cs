using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PickPixForEver.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        private Grid galleryView = new Grid();
        public GalleryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CreateGrid(null);
        }

        private void CreateGrid(IEnumerable<Picture> pictures)
        {
            galleryView.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });
            galleryView.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0.5, GridUnitType.Star) });

            for (int row = 0; row < 12; row++) //pictures.ToList().Count
            {
                galleryView.RowDefinitions.Add(new RowDefinition() { Height = 100 });
                for (int col = 0; col < 2; col++)
                {
                    var embeddedImage = new Image()
                    {
                        Source = ImageSource.FromResource("PickPixForEver.Resources.Applogo.png")
                        //, typeof(PickPixForEver).GetTypeInfo().Assembly
                         // )
                    };
                    var img = new Image()
                    {
                        Source = "logo.png"//PickPixForEver.Resources.logo.png
                    };
                    var localImg = new Image()
                    {
                        Source = ImageSource.FromUri(new Uri("file://Users/salehsultan/Desktop/AppLogo.png"))
                    };

                    galleryView.Children.Add(localImg, col, row);
                }
            }
            PhotoGallery.Content = galleryView;
        }
    }
}