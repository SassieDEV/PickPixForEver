﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.FilePicker;
using PickPixForEver.Models;
using PickPixForEver.Services;
using MetadataExtractor;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PickPixForEver.ViewModel;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPage : ContentPage
    {
        //TODO: Refactor dbContext and possibly PictureRepository to main page after login
        PicturesRepository picRep = new PicturesRepository(App.FilePath);
        //set fileTypes for image picking
        string[] fileTypes = null;
        int userId;
        public Dictionary<Stream, string> Streams { get; set; }
        AlbumsViewModel albumsViewModel;

        public UploadPage()
        {
            InitializeComponent();
            Privacy.SelectedIndex = 1;
            Streams = new Dictionary<Stream, string>();
            userId = Preferences.Get("userId", 1);
            albumsViewModel = new AlbumsViewModel(App.FilePath);

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            albumsViewModel.LoadItemCommand.Execute(null);
            entAlbums.ItemsSource = albumsViewModel.Albums.Select(s => s.Name).ToList();
        }
        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {

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
            await PickPic(sender, e).ConfigureAwait(false);
        }
        private async void SelectUploadButton_Clicked(object sender, EventArgs e)
        {
            //TODO: Will throw exception if no image was uploaded, need to add warning to UploadPage.xaml
            View[] allImages = ImagePreview.Children.ToArray<View>();
            string[][] megatags = new string[5][];
            string[] albums = Array.Empty<string>();
            string text_entNotes = "";

            //TODO: See if there's a more elegant solution to checking if each entry is null before assinging string[]
            string[] text_entPeople = Array.Empty<string>();
            if (entPeople.Text != null)
            {
                text_entPeople = await FormatTagsAlbums(entPeople.Text).ConfigureAwait(false);
            }
            string[] text_entPlaces = Array.Empty<string>();
            if (entPlaces.Text != null)
            {
                text_entPlaces = await FormatTagsAlbums(entPlaces.Text).ConfigureAwait(false);
            }
            string[] text_entEvents = Array.Empty<string>();
            if (entEvents.Text != null)
            {
                text_entEvents = await FormatTagsAlbums(entEvents.Text).ConfigureAwait(false);
            }
            string[] text_entCustom = Array.Empty<string>();
            if (entCustom.Text != null)
            {
                text_entCustom = await FormatTagsAlbums(entCustom.Text).ConfigureAwait(false);
            }
            string[] text_entRelationships = Array.Empty<string>();
            if (entRelationships.Text != null)
            {
                text_entRelationships = await FormatTagsAlbums(entRelationships.Text).ConfigureAwait(false);
            }
            string[] text_entAlbums = Array.Empty<string>();
            int albumId = 0;
            if (albumsViewModel.Albums != null && albumsViewModel.Albums.Count > 0 && entAlbums.SelectedIndex > -1)
            {
                albumId = albumsViewModel.Albums.Where(s => s.Name == (string)entAlbums.SelectedItem).Select(s => s.Id).SingleOrDefault();
            }
            if (entNotes.Text != null)
            {
                text_entNotes = entNotes.Text;
            }

            megatags = new string[][] { text_entPeople, text_entPlaces, text_entEvents, text_entCustom, text_entRelationships };
            albums = text_entAlbums;
            await picRep.HandleImageCommit(userId, Streams, megatags, albumId, (string)Privacy.SelectedItem, text_entNotes).ConfigureAwait(false);
            await ClearPagePics().ConfigureAwait(false);
        }
        private async Task<int> ClearPagePics()
        {
            ImagePreview.Children.Clear();
            Streams = new Dictionary<Stream, string>();
            return 0;
        }

        private async Task<String[]> FormatTagsAlbums(string tagAlbumLine)
        {
            string[] tagArray =  tagAlbumLine.Split(';').Where(x => !string.IsNullOrEmpty(x)).Distinct().ToArray();
            for (int i = 0; i < tagArray.Length; i++)
                tagArray[i] = tagArray[i].Trim();
            return tagArray;
        }

        private async Task<int> PickPic(object s, EventArgs e)
        {
            var pickedFile = await CrossFilePicker.Current.PickFile(fileTypes).ConfigureAwait(true);

            if (pickedFile != null)
            {
                Image img = new Image();
                img.Source = ImageSource.FromStream(() => pickedFile.GetStream());
                ImagePreview.Children.Add(img);
                IReadOnlyList<MetadataExtractor.Directory> metaDataDirectories = ImageMetadataReader.ReadMetadata(pickedFile.GetStream());
                Streams.Add(pickedFile.GetStream(), pickedFile.FilePath);
            }
            return 0;
        }
    }


}