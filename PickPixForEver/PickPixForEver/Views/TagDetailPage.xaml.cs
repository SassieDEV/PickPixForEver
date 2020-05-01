using PickPixForEver.Models;
using PickPixForEver.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagDetailPage : ContentPage
    {
        List<Picture> pictures = new List<Picture>();
        TagDetailViewModel viewModel;
        public TagDetailPage(TagDetailViewModel viewModel)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            if (viewModel!=null && viewModel.Tag !=null)
            {
                this.viewModel.LoadTagPicturesCommand.Execute(viewModel.Tag.TagId);
            }           
            BindingContext = this.viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateTagImages();

        }

        private async void btnEditTag_Clicked(object sender, EventArgs e)
        {
            Tag tag = new Tag();
            tag = this.viewModel.Tag;
            await PopupNavigation.Instance.PushAsync(new EditTag(new TagDetailViewModel(App.FilePath, tag))).ConfigureAwait(false);
        }

       void PopulateTagImages()
        {
            stackImages.Children.Clear();
            var imageScrollView = new ScrollView();
            var innerStackLayout = new StackLayout();
            imageScrollView.Content = innerStackLayout;
            stackImages.Children.Add(imageScrollView);
            
            foreach (var picture in this.viewModel.Pictures)
            {
                Image image = new Image() { Source = ImageSource.FromStream(() => new MemoryStream(picture.ImageArray)) };

                Label notes = new Label();
                notes.Text = picture.Notes;
                notes.Style = (Style)Application.Current.Resources["SubHeaderLabel"];
                notes.HorizontalOptions = LayoutOptions.StartAndExpand;

                Label privacy = new Label();
                privacy.Text = picture.Privacy;
                privacy.Style = (Style)Application.Current.Resources["DescriptionLabel"];
                privacy.HorizontalOptions = LayoutOptions.StartAndExpand;
                privacy.Margin = new Thickness(2, 2, 5, 5);
                Frame frame = new Frame();
                frame.BorderColor = Color.Gray;
                frame.CornerRadius = 5;
                frame.Padding = 8;
                frame.Margin = new Thickness(5, 7, 5, 0);


                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition() { Height = 45 });
                grid.RowDefinitions.Add(new RowDefinition() { Height = 35 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 120 });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 95 });


                grid.Children.Add(image, 0, 0);
                Grid.SetRowSpan(image, 2);
                grid.Children.Add(notes, 1, 0);
                grid.Children.Add(privacy, 1, 1);
                frame.Content = grid;
                innerStackLayout.Children.Add(frame);
            }
        }

    }
}