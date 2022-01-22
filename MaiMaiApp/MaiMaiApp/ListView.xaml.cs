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
                    Name = song.Name,
                    Status = song.Score,
                    Difficulty = song.Difficulty.ToString(),
                    Color = Contect.GetColor(song.Difficulty),
                    ImageSource = new UriImageSource
                    {
                        Uri = new Uri(song.imageUrl),
                    }
                }); 
            }


            listView.ItemsSource = list;
        }
    }

    class Contect
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Difficulty { get; set; }
        public ImageSource ImageSource { get; set; }
        public Color Color { get; set; }

        public static Color GetColor(Difficulty difficulty)
        {
            switch(difficulty)
            {
                case MaiMaiApp.Difficulty.ReMaster:
                    return Color.FromHex("FAE6FF");
                case MaiMaiApp.Difficulty.Master:
                    return Color.MediumPurple;
                case MaiMaiApp.Difficulty.Expert:
                    return Color.LightPink;            
                case MaiMaiApp.Difficulty.Advanced:
                    return Color.LightYellow;
                case MaiMaiApp.Difficulty.Basic:
                    return Color.LightGreen;
                default:
                    return Color.White;
            }
        }
    }
}