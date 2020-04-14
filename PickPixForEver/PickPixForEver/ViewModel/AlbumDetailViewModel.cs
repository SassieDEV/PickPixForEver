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
    public class AlbumDetailViewModel: BaseViewModel
    {
        public Album Album { get; set; }
        public IAlbumRepository DataStore { get; set; }
        public Command LoadAlbumPicturesCommand { get; set; }


        //True when adding a new album, false when updating existing note
        public bool IsNewAlbum { get; set; }


        public AlbumDetailViewModel(string filePath,Album album = null)
        {
            DataStore = new AlbumRepository(filePath);
            Pictures = new ObservableCollection<Picture>();
            IsNewAlbum = album == null;
            Title = IsNewAlbum ? "Create album" : "Edit album";
            Album = album ?? new Album();
            LoadAlbumPicturesCommand = new Command<int>(async (albumId) => await ExecuteLoadAlbumPicturesCommand(albumId).ConfigureAwait(false));


            // Handle "SaveAlbum" message
            MessagingCenter.Subscribe<AddAlbum, Album>(this, "UpdateAlbum", async (sender, alb) =>
            {
                await ExecuteUpdateAlbumCommand(alb).ConfigureAwait(false);
            });

        }

        async Task<bool> ExecuteUpdateAlbumCommand(Album album)
        {
            if (IsBusy)
                return false;
            bool result = false;
            IsBusy = true;
            try
            {
                UpdatedAt = DateTime.Now;
                Name = album.Name;
                Description = album.Description;
                Privacy = album.Privacy;
                Active = album.Active;
                result = await DataStore.UpdateItemAsync(album).ConfigureAwait(false); 
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
            return result;
        }

        async Task ExecuteLoadAlbumPicturesCommand(int albumId)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                Pictures.Clear();
                var pictures = await DataStore.GetAlbumPictures(albumId).ConfigureAwait(false);
                foreach (var picture in pictures)
                {
                    Pictures.Add(picture);
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

        #region property getters and setters
        public int Id
        {
            get { return Album.Id; }
            set
            {
                Album.Id = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return Album.Name; }
            set
            {
                Album.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Description
        {
            get { return Album.Description; }
            set
            {
                Album.Description = value;
                OnPropertyChanged();
            }
        }
        public DateTime CreatedAt
        {
            get { return Album.CreatedAt; }
            set
            {
                Album.CreatedAt = value;
                OnPropertyChanged();
            }
        }
        public DateTime UpdatedAt
        {
            get { return Album.UpdatedAt; }
            set
            {
                Album.UpdatedAt = value;
                OnPropertyChanged();
            }
        }
        public string Privacy
        {
            get { return Album.Privacy; }
            set
            {
                Album.Privacy = value;
                OnPropertyChanged();
            }
        }

        public bool Active
        {
            get { return Album.Active; }
            set
            {
                Album.Active = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Picture> Pictures { get; set; }
        #endregion



    }
}
