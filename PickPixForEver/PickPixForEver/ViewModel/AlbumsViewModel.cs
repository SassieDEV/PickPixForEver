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

        #region Pirvate fields
            int id;
            string name = string.Empty;
            string description = string.Empty;
            DateTime createdAt;
            DateTime updatedAt;
            int privacy;
            bool active;
            bool isBusy;
        #endregion

        #region Property setters and getters

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set
            {
                createdAt = value;
                OnPropertyChanged();
            }
        }
        public DateTime UpdatedAt
        {
            get { return updatedAt; }
            set
            {
                updatedAt = value;
                OnPropertyChanged();
            }
        }
        public int Privacy
        {
            get { return privacy; }
            set
            {
                privacy = value;
                OnPropertyChanged(nameof(Privacy));
            }
        }

        public bool Active
        {
            get { return active; }
            set
            {
                active = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }


        #endregion


        public AlbumsViewModel(string filePath)
        {
            DataStore = new AlbumRepository(filePath);
            Albums = new ObservableCollection<Album>();
            LoadAlbumsCommand = new Command(async () => await ExecuteLoadAlbumsCommand().ConfigureAwait(false));
            SearchAlbumsComand = new Command<string>(async (searchTerm) => await ExecuteSearchAlbumsCommand(searchTerm).ConfigureAwait(false));
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


    }
}
