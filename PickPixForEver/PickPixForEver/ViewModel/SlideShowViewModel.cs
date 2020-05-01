using PickPixForEver.Models;
using PickPixForEver.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace PickPixForEver.ViewModel
{
    public class SlideShowViewModel : BaseViewModel
    {
        private ObservableCollection<PictureSliderModel> _pictureNode;
        private List<KeyValuePair<int, byte[]>> PicturesArray;

        ObservableCollection<Picture> PicturesData { get; set; }
        private Image PictureSource { get; set; }
        private int PictureId { get; set; }
        public SlideShowViewModel(Image image, int pictureId)
        {
            this.PictureId = pictureId;
            PictureSource = image;
            PopulatePictureCollection();

        }
        public SlideShowViewModel(List<KeyValuePair<int, byte[]>> picturesArray)
        {
            this.PicturesArray = picturesArray;
            PopulatePictureCollection();

        }

        public ObservableCollection<PictureSliderModel> PictureNode
        {
            get { return _pictureNode; }
            set
            {
                _pictureNode = value;
                OnPropertyChanged();
            }
        }

        private async void PopulatePictureCollection()
        {
            List<PictureSliderModel> list = new List<PictureSliderModel>();
            PicturesRepository picRep = new PicturesRepository(App.FilePath);
            if (this.PicturesArray != null)
                foreach (var byteArray in this.PicturesArray)
                {
                    var pic = await picRep.FindItemAsync(byteArray.Key).ConfigureAwait(false);
                    var tags = await picRep.FindTagByPictureIdAsync(byteArray.Key).ConfigureAwait(false);
                    list.Add(new PictureSliderModel()
                    {
                        Title = pic.Notes,
                        Tags = string.Join(" - ", tags.Select(c => c.Name)),
                        Color = "White",
                        ImageData = ImageSource.FromStream(() => new MemoryStream(byteArray.Value))
                    });
                }
            else
            {
                var pic = await picRep.FindItemAsync(PictureId).ConfigureAwait(false);
                var tags = await picRep.FindTagByPictureIdAsync(PictureId).ConfigureAwait(false);
                list.Add(new PictureSliderModel()
                {
                    Title = pic.Notes,
                    Tags = string.Join(" -  ", tags.Select(c => c.Name)),
                    Color = "White",
                    ImageData = PictureSource.Source
                });
            }

            PictureNode = new ObservableCollection<PictureSliderModel>(list);
        }
    }
    public class PictureSliderModel
    {
        public string Title { get; set; }
        public string Tags { get; set; }

        public string Color { get; set; }
        public ImageSource ImageData { get; set; }

    }
}
