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
    public class PicturesViewModel: BaseViewModel
    {
        public Picture Picture { get; set; }
        public IDataStore<Picture> DataStore { get; set; }

        //True when adding a new picture, false when updating existing note
        public bool IsNewPicture{ get; set; }

        public PicturesViewModel(string filePath, Picture picture = null)
        {
            DataStore = new PicturesRepository(filePath);
            IsNewPicture = picture == null;

            Picture = picture ?? new Picture();

            AddItemCommand = new Command<Picture>(async (pic) => await ExecuteAddPictureCommand(pic).ConfigureAwait(false));
        }

        async Task<bool> ExecuteAddPictureCommand(Picture picture)
        {
            if (IsBusy)
                return false;
            bool result = false;
            IsBusy = true;
            try
            {
                Privacy = picture.Privacy;
                Notes = picture.Notes;
                PictureMetaData = picture.PictureMetaData;
                AlbumId = picture.AlbumId;
                result = await DataStore.AddItemAsync(picture).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                //To-do: Implement logging
            }
            finally
            {
                IsBusy = false;
            }

            return result;
        }


        public string Privacy
        {
            get { return Picture.Privacy; }
            set 
            { 
                Picture.Privacy = value;
                OnPropertyChanged();
            }
        }
        public string Notes
        {
            get { return Picture.Notes; }
            set
            {
                Picture.Notes = value;
                OnPropertyChanged();
            }
        }

        public string PictureMetaData
        {
            get { return Picture.PictureMetaData; }
            set
            {
                Picture.PictureMetaData = value;
                OnPropertyChanged();
            }
        }

        public int AlbumId
        {
            get { return Picture.AlbumId; }
            set 
            {
                Picture.AlbumId = value;
                OnPropertyChanged();
            }
        }

    }
}
