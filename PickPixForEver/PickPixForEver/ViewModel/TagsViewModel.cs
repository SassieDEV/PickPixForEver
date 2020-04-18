using PickPixForEver.Models;
using PickPixForEver.Services;
using PickPixForEver.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PickPixForEver.ViewModel
{
    public class TagsViewModel: BaseViewModel
    {
        public Tag picTag { get; set; }
        public ITagRepository DataStore { get; set; }


        public TagsViewModel(string filePath, Tag tag=null)
        {
            DataStore = new TagRepository(filePath);
            Title = "Picture Details";
            picTag = tag;

            // Handle "SaveAlbum" message
            MessagingCenter.Subscribe<TagsPage, Tag>(this, "UpdatePictureDetails", async (sender, pTags) =>
            {
                await ExecuteUpdatePicDetailsCommand(pTags).ConfigureAwait(false);
            });
        }

        async Task<bool> ExecuteUpdatePicDetailsCommand(Tag picTag)
        {
            if (IsBusy)
                return false;
            bool result = false;
            IsBusy = true;
            try
            {
                //UpdatedAt = DateTime.Now;
                //Name = picTag.Name;
                //Description = album.Description;
                //Privacy = album.Privacy;
                //Active = album.Active;
                result = await DataStore.UpdateItemAsync(picTag).ConfigureAwait(false);
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
    }
}
