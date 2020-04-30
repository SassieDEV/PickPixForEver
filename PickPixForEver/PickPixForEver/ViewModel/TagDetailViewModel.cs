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
    public class TagDetailViewModel: BaseViewModel
    {
        public Tag Tag { get; set; }
        public ITagRepository DataStore { get; set; }
        public Command LoadTagPicturesCommand { get; set; }


        //True when adding a new tag, false when updating existing tag
        public bool IsNewTag { get; set; }


        public TagDetailViewModel(string filePath,Tag tag = null)
        {
            DataStore = new TagRepository(filePath);
            Pictures = new ObservableCollection<Picture>();
            IsNewTag = tag == null;
            Title = IsNewTag ? "Create tag" : "Edit tag";
            Tag = tag ?? new Tag();
            LoadTagPicturesCommand = new Command<int>(async (tagId) => await ExecuteLoadTagPicturesCommand(tagId).ConfigureAwait(false));


            // Handle "SaveTag" message
            MessagingCenter.Subscribe<EditTag, Tag>(this, "UpdateTag", async (sender, ttg) =>
            {
                await ExecuteUpdateTagCommand(ttg).ConfigureAwait(false);
            });

        }

        async Task<bool> ExecuteUpdateTagCommand(Tag tag)
        {
            if (IsBusy)
                return false;
            bool result = false;
            IsBusy = true;
            try
            {
                Updated = DateTime.Now;
                Name = tag.Name;
                TagType = tag.TagType;
                result = await DataStore.UpdateItemAsync(tag).ConfigureAwait(false); 
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

        async Task ExecuteLoadTagPicturesCommand(int tagId)
        {
            if (IsBusy)
                return;
            IsBusy = true;

            try
            {
                Pictures.Clear();
                var pictures = await DataStore.GetTaggedPictures(tagId).ConfigureAwait(false);
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
            get { return Tag.TagId; }
            set
            {
                Tag.TagId = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return Tag.Name; }
            set
            {
                Tag.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string TagType
        {
            get { return Tag.TagType; }
            set
            {
                Tag.TagType = value;
                OnPropertyChanged();
            }
        }
        public DateTime Created
        {
            get { return Tag.Created; }
            set
            {
                Tag.Created = value;
                OnPropertyChanged();
            }
        }
        public DateTime Updated
        {
            get { return Tag.Updated; }
            set
            {
                Tag.Updated = value;
                OnPropertyChanged();
            }
        }
        public int UserId
        {
            get { return Tag.UserId; }
            set
            {
                Tag.UserId = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Picture> Pictures { get; set; }
        #endregion



    }
}
