using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickPixForEver.ViewModel
{
    class GalleryViewModel: BaseViewModel
    {
        public ObservableCollection<Picture> Pictures { get; set; }
        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }

        public IPictureRepository PicturesDataStore { get; set; }
        public IAlbumRepository AlbumsDataStore { get; set; }
        public ITagRepository TagsDataStore { get; set; }

        public Command LoadPicturesCommand { get; set; }
        public Command LoadAlbumsCommand { get; set; }
        public Command LoadTagsCommand { get; set; }
        public Command LoadAlbumPicturesCommand { get; set; }
        public Command LoadTaggedPicturesCommand { get; set; }
        public GalleryViewModel(string filePath)
        {
            PicturesDataStore = new PicturesRepository(filePath);
            AlbumsDataStore = new AlbumRepository(filePath);
            TagsDataStore = new TagRepository(filePath);

            Pictures = new ObservableCollection<Picture>();
            Albums = new ObservableCollection<Album>();
            Tags = new ObservableCollection<Tag>();

            LoadPicturesCommand = new Command(async () => await ExectueLoadPicturesCommand().ConfigureAwait(false));
            LoadAlbumsCommand = new Command(async () => await ExectueLoadAlbumsCommand().ConfigureAwait(false));
            LoadTagsCommand = new Command(async () => await ExectueLoadTagsCommand().ConfigureAwait(false));
            LoadAlbumPicturesCommand = new Command<int>(async (albumId) => await ExecuteLoadAlbumPicturesCommand(albumId).ConfigureAwait(false));
            LoadTaggedPicturesCommand = new Command<int>(async (tagId) => await ExecuteLoadTaggedPicturesCommand(tagId).ConfigureAwait(false));
            SearchItemComand = new Command<string>(async (searchTerm) => await ExecuteSearchPicturesCommand(searchTerm).ConfigureAwait(false));
        }

        private async Task ExecuteSearchPicturesCommand(string searchTerm)
        {
            Pictures.Clear();
            var pictures = await PicturesDataStore.GetItemsAsync(searchTerm).ConfigureAwait(false);
            Pictures = new ObservableCollection<Picture>(pictures);
        }

        private async Task ExecuteLoadTaggedPicturesCommand(int tagId)
        {
            Pictures.Clear();
            var pictures = await Task.FromResult(TagsDataStore.GetTaggedPictures(tagId)).ConfigureAwait(false);
            Pictures = new ObservableCollection<Picture>(pictures);
        }

        private async Task ExecuteLoadAlbumPicturesCommand(int albumId)
        {
            Pictures.Clear();
            var pictures = await AlbumsDataStore.GetAlbumPictures(albumId).ConfigureAwait(false);
            Pictures = new ObservableCollection<Picture>(pictures);
        }

        private async Task ExectueLoadPicturesCommand()
        {
            Pictures.Clear();
            var pictures = await PicturesDataStore.GetItemsAsync().ConfigureAwait(false);
            Pictures = new ObservableCollection<Picture>(pictures);
        }

        private async Task ExectueLoadAlbumsCommand()
        {
            Albums.Clear();
            var albums = await AlbumsDataStore.GetItemsAsync().ConfigureAwait(false);
            Albums =new ObservableCollection<Album>(albums);
        }


        private async Task ExectueLoadTagsCommand()
        {
            Tags.Clear();
            var tags = await TagsDataStore.GetItemsAsync().ConfigureAwait(false);
            Tags = new ObservableCollection<Tag>(tags);
        }
    }
}
