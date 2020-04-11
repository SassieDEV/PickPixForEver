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
            var colCount = Application.Current.MainPage.Width / 100;
            for (int i=0; i<colCount; i++)
            {
                galleryView.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength((float)1/(float)colCount, GridUnitType.Star) });
            }

            for (int row = 0; row < 12; row++) //pictures.ToList().Count
            {
                galleryView.RowDefinitions.Add(new RowDefinition() { Height = 100 });
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
                        },
                        NumberOfTapsRequired = 1
                    };
                    img.GestureRecognizers.Add(tapAction);
                    galleryView.Children.Add(img, col, row);
                }
            }
            PhotoGallery.Content = galleryView;
        }
    }
}