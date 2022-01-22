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
    public partial class SongDataPage : ContentPage
    {
        
        public SongDataPage()
        {
            InitializeComponent();

            CategoryPicker.SelectedIndex = 2;
            LevelPicker.SelectedIndex = 3;
            Criteria.SelectedIndex = 0;
            SongListView.ItemsSource = GetList(CategoryPicker.SelectedIndex, LevelPicker.SelectedIndex, Criteria.SelectedIndex);

            CategoryPicker.SelectedIndexChanged += PickerSelectedIndexChanged;
            LevelPicker.SelectedIndexChanged += PickerSelectedIndexChanged;
            Criteria.SelectedIndexChanged += PickerSelectedIndexChanged;
        }

        public void PickerSelectedIndexChanged(object sender, EventArgs e)
        {
            SongListView.ItemsSource = GetList(CategoryPicker.SelectedIndex, LevelPicker.SelectedIndex, Criteria.SelectedIndex);
        }

        private List<ViewCellContect> GetList(int categoryIndex, int levelIndex, int criteriaIndex)
        {
            var songData = MaimaiData.Instance.GetSongDataList();
            string level = "";
            switch(levelIndex)
            {
                case 0:
                    level = "15";
                    break;
                case 1:
                    level = "14+";
                    break;
                case 2:
                    level = "14";
                    break;
                case 3:
                    level = "13+";
                    break;
                case 4:
                    level = "13";
                    break;
                case 5:
                    level = "12+";
                    break;
                case 6:
                    level = "12";
                    break;
                case 7:
                    level = "";
                    break;
            }

            
            if (criteriaIndex == 0)
            {
                var list = songData.Where(data => data.Value.category == (Category)categoryIndex)
                    .Where(data => data.Value.WhereLevel(level) == true)
                    .OrderByDescending(data => data.Value.SelectLevel(level).ConstLevel)
                    .Select(data => new ViewCellContect
                    {
                        Name = data.Key,
                        Level = data.Value.SelectLevel(level).Level,
                        ConstLevel = data.Value.SelectLevel(level).ConstLevel,
                        ClearRank = data.Value.SelectLevel(level).ClearRank,
                        Score = data.Value.SelectLevel(level).Score,
                        Color = data.Value.GetDifficultyColor(level),
                        Image = new UriImageSource
                        {
                            Uri = new Uri(data.Value.ImageUrl),
                            CachingEnabled = true,
                            CacheValidity = TimeSpan.FromHours(1)
                        }
                    });

                return list.ToList();
            }
            else
            {
                var list = songData.Where(data => data.Value.category == (Category)categoryIndex)
                    .Where(data => data.Value.WhereLevel(level) == true)
                    .OrderByDescending(data => data.Value.SelectLevel(level).Score)
                    .Select(data => new ViewCellContect
                    {
                        Name = data.Key,
                        Level = data.Value.SelectLevel(level).Level,
                        ConstLevel = data.Value.SelectLevel(level).ConstLevel,
                        ClearRank = data.Value.SelectLevel(level).ClearRank,
                        Score = data.Value.SelectLevel(level).Score,
                        Color = data.Value.GetDifficultyColor(level),
                        Image = new UriImageSource
                        {
                            Uri = new Uri(data.Value.ImageUrl),
                            CachingEnabled = true,
                            CacheValidity = TimeSpan.FromHours(1)
                        }
                    });

                return list.ToList();
            }
        }

        class ViewCellContect
        {
            public string Name { get; set; }
            public string Level { get; set; } = null;
            public string ConstLevel { get; set; } = null;
            public string ClearRank { get; set; }
            public string Score { get; set; } = null;
            public Color Color { get; set; }
            public ImageSource Image { get; set; }
        }
    }

    
}