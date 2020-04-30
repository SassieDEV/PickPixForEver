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
    public class TagsViewModel : BaseViewModel
    {
        public ObservableCollection<Tag> Tags { get; set; }
        public IDataStore<Tag> DataStore { get; set; }

        public TagsViewModel(string filePath)
        {
            Title = "Tags";
            DataStore = new TagRepository(filePath);
            Tags = new ObservableCollection<Tag>();
            LoadItemCommand = new Command(async () => await ExecuteLoadTagsCommand().ConfigureAwait(false));
            SearchItemComand = new Command<string>(async (searchTerm) => await ExecuteSearchTagsCommand(searchTerm).ConfigureAwait(false));

            // Handle "SaveTag" message
            MessagingCenter.Subscribe<EditTag, Tag>(this, "SaveTag", async (sender, tag) =>
            {
                //Add tag to database
                await ExecuteAddTagCommand(tag).ConfigureAwait(false);
            });       
        }


    


    async Task ExecuteLoadTagsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Tags.Clear();
                var tags = await DataStore.GetItemsAsync().ConfigureAwait(false);
                foreach (var tag in tags)
                {
                    Tags.Add(tag);
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

        async Task ExecuteSearchTagsCommand(string searchTerm)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Tags.Clear();
                var tags = await DataStore.GetItemsAsync(searchTerm).ConfigureAwait(false);
                foreach (var tag in tags)
                {
                    Tags.Add(tag);
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
        async Task<bool> ExecuteAddTagCommand(Tag tag)
        {
                
            if (IsBusy)
                return false;
            int tagId = 0;
            IsBusy = true;
            try
            {
                Tags.Add(tag);
                tagId = await DataStore.AddItemAsync(tag).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Tags.Remove(tag);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
            return (!(tagId == 0));
        }


    }

}
