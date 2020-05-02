using PickPixForEver.Models;
using PickPixForEver.Services;
using PickPixForEver.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickPixForEver.ViewModel
{
    public class AlbumsViewModel : BaseViewModel
    {
        public ObservableCollection<Album> Albums { get; set; }
        public IDataStore<Album> DataStore { get; set; }

        public AlbumsViewModel(string filePath)
        {
            Title = "Albums";
            DataStore = new AlbumRepository(filePath);
            Albums = new ObservableCollection<Album>();
            LoadItemCommand = new Command(async () => await ExecuteLoadAlbumsCommand().ConfigureAwait(false));
            SearchItemComand = new Command<string>(async (searchTerm) => await ExecuteSearchAlbumsCommand(searchTerm).ConfigureAwait(false));
            // Handle "SaveAlbum" message
            MessagingCenter.Subscribe<AddAlbum, Album>(this, "SaveAlbum", async (sender, album) =>
            {
                //Add album to database
                await ExecuteAddAlbumCommand(album).ConfigureAwait(false);
            });
        }


        async Task ExecuteLoadAlbumsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Albums.Clear();
                var albums = await DataStore.GetItemsAsync().ConfigureAwait(false);
                foreach (var album in albums)
                {
                    Albums.Add(album);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteSearchAlbumsCommand(string searchTerm)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Albums.Clear();
                var albums = await DataStore.GetItemsAsync(searchTerm).ConfigureAwait(false);
                foreach (var album in albums)
                {
                    Albums.Add(album);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task<bool> ExecuteAddAlbumCommand(Album album)
        {
            if (IsBusy)
                return false;
            int albumId = 0;
            IsBusy = true;
            try
            {
               
                albumId = await DataStore.AddItemAsync(album).ConfigureAwait(false);
                if(albumId!=-1)
                Albums.Add(album);
            }
            catch (Exception ex)
            {
                //If save to database failed remove album from collection
                Albums.Remove(album);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
            return (albumId != -1);
        }
    }
}