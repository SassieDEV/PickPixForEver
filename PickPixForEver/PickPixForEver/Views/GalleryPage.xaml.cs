using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PickPixForEver.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                    var lbl = new Label()
                    {
                        BackgroundColor = Color.Red,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    lbl.Text = "this is my picture";
                    galleryView.Children.Add(lbl, col, row);
                }
            }
            PhotoGallery.Content = galleryView;
        }
    }
}