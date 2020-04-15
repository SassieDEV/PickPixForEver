using PickPixForEver.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace PickPixForEver.ViewModel
{
    public class SlideShowViewModel:BaseViewModel
    {
        private ObservableCollection<PictureSliderModel> _pictureNode;
        ObservableCollection<Picture> PicturesData { get; set; }
        public SlideShowViewModel(ObservableCollection<Picture> PicturesData)
        {
            this.PicturesData = PicturesData;
            PopulatePictureCollection();
            
        }
        public ObservableCollection<PictureSliderModel> PictureNode
        {
            get { return _pictureNode; }
            set {
                _pictureNode = value;
                OnPropertyChanged();
            }
        }
        private void PopulatePictureCollection()
        {
            List<PictureSliderModel> list = new List<PictureSliderModel>();
            foreach (var node in PicturesData)
            {
                list.Add(new PictureSliderModel()
                {
                    Color = "White",
                    ImageData= ImageSource.FromStream(() => new MemoryStream(node.ImageArray))
                });
            }
            PictureNode = new ObservableCollection<PictureSliderModel>(list);
        }
    }
        public class PictureSliderModel
        {
            public string Color { get; set; }
            public ImageSource ImageData { get; set; }
            
    }
    }
