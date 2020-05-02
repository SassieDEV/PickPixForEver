using PickPixForEver.ViewModel;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PickPixForEver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideViewer : ContentPage
    {
        SlideShowViewModel viewModel;
        public SlideViewer(SlideShowViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
            BindingContext = this.viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Load("SlideshowMusic.mp3");
            player.Play();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            player.Stop();
        }
    }
}