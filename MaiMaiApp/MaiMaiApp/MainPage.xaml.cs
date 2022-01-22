using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MaiMaiApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            LatestButton.Clicked += OnLatestButton;
            SongDataButton.Clicked += OnSongDataButton;
        }

        public async void OnLatestButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListView());
        }

        public async void OnSongDataButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SongDataPage());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
