using PickPixForEver.Models;
using PickPixForEver.ViewModel;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TagsPage : ContentPage
    {
        TagsViewModel tagsViewModel;
        private static readonly int COL_LENGTH = 5;

        public TagsPage()
        {
            InitializeComponent();
            BindingContext = tagsViewModel = new TagsViewModel(App.FilePath);


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            tagsViewModel.LoadItemCommand.Execute(null);
            populateGrid();

        }
        private async void Tag_Tapped(object sender, EventArgs e)
        {
            StackLayout stackLayout = (StackLayout)sender;
            if (stackLayout.GestureRecognizers.Count > 0)
            {
                try
                {
                    var gesture = (TapGestureRecognizer)stackLayout.GestureRecognizers[0];
                    Tag tag = (Tag)gesture.CommandParameter;
                    await Navigation.PushAsync(new TagDetailPage(new TagDetailViewModel(App.FilePath, tag))).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            if (searchBar != null && !string.IsNullOrEmpty(searchBar.Text.Trim()))
            {
                tagsViewModel.SearchItemComand.Execute(searchBar.Text.ToLower().Trim());
            }
            else
            {
                tagsViewModel.LoadItemCommand.Execute(null);
            }
            populateGrid();
        }


        private void populateGrid()
        {
            tagsStackLayout.Children.Clear();
            var tagsScrollView = new ScrollView();
            var tagsGrid = new Grid();
            tagsGrid.RowSpacing = 5;
            tagsGrid.ColumnSpacing = 5;
            tagsGrid.WidthRequest = 1200;

            tagsScrollView.Content = tagsGrid;
            tagsStackLayout.Children.Add(tagsScrollView);
            if (tagsViewModel.Tags.Count == 0)
                return;

            int cnt = tagsViewModel.Tags.Count;
            int rowLength = cnt % 5 == 0 ? cnt / 5 : (cnt / 5) + 1;
            for (int r = 0; r < rowLength; r++)
            {
                tagsGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int c = 0; c < COL_LENGTH; c++)
            {
                tagsGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = 240
                });
            }

            var index = 0;
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < COL_LENGTH; j++)
                {
                    if (index >= cnt)
                    {
                        break;
                    }

                    var tag = tagsViewModel.Tags[index];
                    index++;

                    //Initialize Controls
                    var frame = new Frame();
                    var stackLayout = new StackLayout();
                    var tagCover = new Image();
                    var lblTagName = new Label();
                    var lblTagType = new Label();


                    //Frame properties
                    frame.BorderColor = Color.Gray;
                    frame.CornerRadius = 5;
                    frame.Padding = 8;

                    //Image tag cover properties
                    tagCover.HorizontalOptions = LayoutOptions.StartAndExpand;
                    tagCover.Source = ImageSource.FromFile("tagCover.png");


                    //Label properties
                    lblTagName.Text = tag.Name;
                    lblTagName.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblTagName.Style = (Style)Application.Current.Resources["SubHeaderLabel"];

                    lblTagType.Text = $"Type: {tag.TagType}";
                    lblTagType.HorizontalOptions = LayoutOptions.StartAndExpand;
                    lblTagType.Style = (Style)Application.Current.Resources["DescriptionLabel"];

                    //Stacklayout click handler
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.CommandParameter = tag;
                    tapGestureRecognizer.Tapped += Tag_Tapped;
                    stackLayout.GestureRecognizers.Add(tapGestureRecognizer);

                    frame.Content = stackLayout;
                    stackLayout.Children.Add(tagCover);
                    stackLayout.Children.Add(lblTagName);
                    stackLayout.Children.Add(lblTagType);


                    tagsGrid.Children.Add(frame, j, i);
                }
            }
        }

     }
}