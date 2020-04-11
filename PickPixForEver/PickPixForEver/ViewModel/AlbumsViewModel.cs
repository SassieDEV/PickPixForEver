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
        public Command LoadAlbumsCommand { get; set; }
        public Command SearchAlbumsComand { get; set; }

        public Command AddAlbumCommand { get; set; }

        public AlbumsViewModel(string filePath)
        {
            Title = "Albums";
            DataStore = new AlbumRepository(filePath);
            Albums = new ObservableCollection<Album>();
            LoadAlbumsCommand = new Command(async () => await ExecuteLoadAlbumsCommand().ConfigureAwait(false));
            SearchAlbumsComand = new Command<string>(async (searchTerm) => await ExecuteSearchAlbumsCommand(searchTerm).ConfigureAwait(false));
            //AddAlbumCommand = new Command<Album>(async (album) => await ExecuteAddAlbumCommand(album).ConfigureAwait(false));

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

        async Task ExecuteSearchAlbumsCommand( string searchTerm)
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
            bool result = false;
            IsBusy = true;
            try
            {
                Albums.Add(album);
                result = await DataStore.AddItemAsync(album).ConfigureAwait(false);
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
            return result;
        }
    }
}
