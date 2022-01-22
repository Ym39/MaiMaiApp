using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MaiMaiApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListView : ContentPage
    {
        public ListView()
        {
            InitializeComponent();

            //var imageSource = new UriImageSource { Uri = new Uri("https://upload.wikimedia.org/wikipedia/ko/8/87/Kakaofriends.png"), CachingEnabled = false, CacheValidity = TimeSpan.FromHours(1)};

            var latestRecordsList = MaimaiData.Instance.GetLatestRecords();

            var list = new List<Contect>();
            //{
            //    new Contect{Name = "라이언", Status = "ㅎㅇ", ImageSource = new UriImageSource { Uri = new Uri("https://upload.wikimedia.org/wikipedia/ko/8/87/Kakaofriends.png"), CachingEnabled = false, CacheValidity = TimeSpan.FromHours(1)}},
            //    new Contect{Name = "어피치", Status = "ㅎㅇ", ImageSource = new UriImageSource { Uri = new Uri("https://upload.wikimedia.org/wikipedia/ko/8/87/Kakaofriends.png"), CachingEnabled = false, CacheValidity = TimeSpan.FromHours(1)}},
            //    new Contect{Name = "프로도", Status = "ㅎㅇ", ImageSource = new UriImageSource { Uri = new Uri("https://upload.wikimedia.org/wikipedia/ko/8/87/Kakaofriends.png"), CachingEnabled = false, CacheValidity = TimeSpan.FromHours(1)}}
            //}

            foreach(var song in latestRecordsList)
            {
                list.Add(new Contect
                {
                    Name = song.Key,
                    Status = song.Value.Score,
                    Difficulty = song.Value.Difficulty.ToString(),
                    ImageSource = new UriImageSource 
                    {
                        Uri = new Uri(song.Value.imageUrl),
                      

                    }
                });
            }


            listView.ItemsSource = list;
        }
    }
}