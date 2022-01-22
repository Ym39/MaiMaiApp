using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using System.Diagnostics;

namespace MaiMaiApp
{
    

    class MaimaiData
    {
        private static MaimaiData instance;

        public static MaimaiData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaimaiData();
                    return instance;
                }
                else
                    return instance;
            }
        }

        private Dictionary<string, LatestSongData> latestSongData;
        private Dictionary<string, SongData> songData = new Dictionary<string, SongData>();

        HtmlDocument globalHtmlDocument = new HtmlDocument();
        StringBuilder StringBuilder = new StringBuilder();

        int taskCount = 0;
        public MaimaiData()
        {
            //songData = new Dictionary<string, MaiMaiSong>();

            //NetworkManager.GetMaimaiHome(NetworkManager.Cookie);
            //NetworkManager.LoginMaiMai(NetworkManager.Cookie);
            //var resultHtml = NetworkManager.GotoRecords(NetworkManager.Cookie);

            //var htmlDocument = new HtmlDocument();
            //htmlDocument.LoadHtml(resultHtml);

            //var recordNodes = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]").ChildNodes;

            //foreach(var node in recordNodes)
            //{
            //    if(node.Name == "div")
            //    {
            //        var songNode = node.ChildNodes[3];
            //        var titleNode = songNode.ChildNodes[1];
            //        string title = titleNode.InnerText;
            //        string imgUri = "";
            //        string score = "";
            //        if (songNode.ChildNodes.Count > 4)
            //        {
            //            var imgNode = songNode.ChildNodes[3]?.ChildNodes;
            //            if (imgNode != null && imgNode.Count > 2)
            //            {
            //                imgUri = songNode.ChildNodes[3].ChildNodes[1].Attributes["src"].Value;
            //                score = songNode.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText != "" ? songNode.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText : songNode.ChildNodes[3].ChildNodes[5].ChildNodes[5].InnerText;
            //            }
            //        }

            //        songData[title] = new MaiMaiSong(title, score, imgUri);
            //    }
            //}
        }

        public async Task Initalize()
        {
            latestSongData = new Dictionary<string, LatestSongData>();

            NetworkManager.GetMaimaiHome(NetworkManager.Cookie);
            await NetworkManager.LoginMaiMai(NetworkManager.Cookie);
            var resultHtml = NetworkManager.GotoRecords(NetworkManager.Cookie);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(resultHtml);

            var recordNodes = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]").ChildNodes;

            foreach (var node in recordNodes)
            {
                if (node.Name == "div")
                {
                    var songNode = node.ChildNodes[3];
                    var titleNode = songNode.ChildNodes[1];
                    string title = titleNode.InnerText;
                    string imgUri = "";
                    string score = "";
                    if (songNode.ChildNodes.Count > 4)
                    {
                        var imgNode = songNode.ChildNodes[3]?.ChildNodes;
                        if (imgNode != null && imgNode.Count > 2)
                        {
                            imgUri = songNode.ChildNodes[3].ChildNodes[1].Attributes["src"].Value;
                            score = songNode.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText != "" ? songNode.ChildNodes[3].ChildNodes[5].ChildNodes[3].InnerText : songNode.ChildNodes[3].ChildNodes[5].ChildNodes[5].InnerText;
                        }
                    }

                    Difficulty difficulty;
                    switch (songNode.Attributes["class"].Value)
                    {
                        case "playlog_basic_container":
                            difficulty = Difficulty.Basic;
                            break;
                        case "playlog_advenced_container":
                            difficulty = Difficulty.Advanced;
                            break;
                        case "playlog_expert_container":
                            difficulty = Difficulty.Expert;
                            break;
                        case "playlog_master_container":
                            difficulty = Difficulty.Master;
                            break;
                        case "playlog_remaster_container":
                            difficulty = Difficulty.ReMaster;
                            break;
                        default:
                            difficulty = Difficulty.Unknown;
                            break;
                    }


                    if (title != "" && title != string.Empty)
                    {
                        latestSongData[title] = new LatestSongData(title, score, imgUri, difficulty);
                    }
                }
            }

           

            //string songDataPage = await NetworkManager.GetSongDataPage(NetworkManager.Cookie,Difficulty.Master);

            //htmlDocument.LoadHtml(songDataPage);

            //var songDataNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]").ChildNodes;

            //var nodeDocument = new HtmlDocument();

            //int count = 0;

            //Category currentCategory = Category.POPS_AND_ANIME;

            //StringBuilder stringBuilder = new StringBuilder();

            //foreach (var node in songDataNode)
            //{
            //    if (node.Name == "div" && node.Attributes["class"].Value == "w_450 m_15 p_r f_0")
            //    {
            //        var songNodes = node.ChildNodes[1].ChildNodes[1].ChildNodes;
            //        var newSongData = new SongData();
            //        newSongData.category = currentCategory;
            //        var songDifficultyData = new SongDifficultyData();

            //        string name = songNodes[7].InnerText;
            //        songDifficultyData.Level = songNodes[5].InnerText;

            //        if (node.Attributes.Count > 1)
            //        {
            //            if (!node.Attributes["id"].Value.Contains("dx"))
            //            {
            //                stringBuilder.Clear();
            //                stringBuilder.Append(name);
            //                stringBuilder.Append("_STANDARD");
            //                name = stringBuilder.ToString();
            //            }
            //        }

            //        if (songNodes.Count > 11)
            //        {
            //            songDifficultyData.Score = songNodes[9].InnerText;
            //            songDifficultyData.DeluxScore = string.Join("", songNodes[11].InnerText.Split('\t', '\n'));
            //        }

            //        newSongData.MasterInfo = songDifficultyData;

            //        songData[name] = newSongData;


            //        count++;
            //        //string songIdx = "";
            //        //if (songNodes.Count > 11)
            //        //    songIdx = songNodes[21].Attributes["value"].Value;
            //        //else
            //        //    songIdx = songNodes[9].Attributes["value"].Value;
            //        //string sondDataHtml = NetworkManager.GetSongData(NetworkManager.Cookie, songIdx);
            //        //nodeDocument.LoadHtml(sondDataHtml);
            //        //var songNameNode = nodeDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[1]/div[2]");

            //        //if(songNameNode != null)
            //        //  songData[songNameNode.InnerText] = new SongData();
            //    }
            //    else if (node.Name == "div" && node.Attributes["class"].Value == "screw_block m_15 f_15 scroll_point")
            //    {
            //        switch (node.InnerText)
            //        {
            //            case "POPS＆ANIME":
            //                currentCategory = Category.POPS_AND_ANIME;
            //                break;
            //            case "niconico＆VOCALOID™":
            //                currentCategory = Category.NICONICO;
            //                break;
            //            case "東方Project":
            //                currentCategory = Category.TOUHOU;
            //                break;
            //            case "GAME＆VARIETY":
            //                currentCategory = Category.GAME_AND_VARIETY;
            //                break;
            //            case "maimai":
            //                currentCategory = Category.MAIMAI;
            //                break;
            //            case "オンゲキ＆CHUNITHM":
            //                currentCategory = Category.ONGEKI_AND_CHUNITHM;
            //                break;
            //        }
            //    }
            //}

            await ParseSongData(Difficulty.Master);
            await ParseSongData(Difficulty.Expert);
            await ParseSongData(Difficulty.Advanced);
            await ParseSongData(Difficulty.Basic);
            await ParseSongData(Difficulty.ReMaster);

            StringBuilder stringBuilder = new StringBuilder();

            LoadWikiUrl("https://maimai.wiki.fc2.com/wiki/%E6%9B%B2%E3%83%AA%E3%82%B9%E3%83%88%28%E3%83%AC%E3%83%99%E3%83%AB%E9%A0%86%29");
            LoadWikiUrl("https://maimai.wiki.fc2.com/wiki/%E6%9B%B2%E3%83%AA%E3%82%B9%E3%83%88%28%E6%97%A7%E6%9B%B2%E3%83%AC%E3%83%99%E3%83%AB%E9%A0%86%2013%EF%BD%9E15%29");
            LoadWikiUrl("https://maimai.wiki.fc2.com/wiki/%E6%9B%B2%E3%83%AA%E3%82%B9%E3%83%88%28%E6%97%A7%E6%9B%B2%E3%83%AC%E3%83%99%E3%83%AB%E9%A0%86%2011%EF%BD%9E12%EF%BC%8B%29");

            if (Application.Current.Properties.Count < 10)
            {
                await ImageTask();
            }
        }

        private async Task ImageTask()
        {
            List<Task> tasks = new List<Task>();

            int count = 0;
            foreach(var data in songData)
            {
                count++;
                tasks.Add(Task.Run(() => 
                {
                    string imageUrl = data.Value.ImageUrl;
                }
                ));

                if(count>=32)
                {
                    await Task.WhenAll(tasks);
                    tasks.Clear();
                    count = 0;
                }
            }
        }


        public string LoadWikiImageUrl(string wikiUrl)
        {
            string html = NetworkManager.GetSongDataFromWiki(wikiUrl,NetworkManager.Cookie);
            HtmlDocument doc = new HtmlDocument();

            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[2]/div[1]/table[1]/tr[1]/td[1]/img[1]");
            if(node == null)
                return "https://upload.wikimedia.org/wikipedia/commons/5/5f/Red_X.svg";

            if (node.Attributes.Contains("src") == true)
            {
                string imageUri = node.Attributes["src"].Value;
                StringBuilder.Clear();
                StringBuilder.Append(NetworkManager.wikiUrl);
                StringBuilder.Append(imageUri);
                return StringBuilder.ToString();
            }
            else
            {
                return "https://upload.wikimedia.org/wikipedia/commons/5/5f/Red_X.svg";
            }
        }

        private void LoadWikiUrl(string wikiUrl)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string songListHtml = NetworkManager.GetSongListMaiMaiWiki(wikiUrl,NetworkManager.Cookie);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(songListHtml);
            var rootNode = htmlDocument.DocumentNode.SelectSingleNode("//*[@id=\"main\"]/div[1]/div[2]");

            foreach (var child in rootNode.ChildNodes)
            {
                if (child.Name == "div" && child.HasChildNodes && child.ChildNodes.Count >= 2)
                {
                    if (child.ChildNodes[1].Name == "table")
                    {
                        var tableNode = child.ChildNodes[1];
                        string diffi = tableNode.ChildNodes[1].ChildNodes[1].InnerText;
                        bool isStd;

                        if (IsSongTable(diffi, out isStd) == true && tableNode.ChildNodes.Count >= 6)
                        {
                            var tableChild = tableNode.ChildNodes;
                            for (int num = 5; num < tableChild.Count; num++)
                            {
                                if (tableChild[num].Name != "tr")
                                    continue;

                                string curName;
                                string wikiUri;
                                string constLevel;
                                if (tableChild[num].ChildNodes.Count > 9)
                                {
                                    curName = tableChild[num].ChildNodes[3].ChildNodes[0].Attributes["title"].Value;
                                    wikiUri = tableChild[num].ChildNodes[3].ChildNodes[0].Attributes["href"].Value;
                                    constLevel = tableChild[num].ChildNodes[9].InnerText;
                                }
                                else
                                {
                                    curName = tableChild[num].ChildNodes[1].ChildNodes[0].Attributes["title"].Value;
                                    wikiUri = tableChild[num].ChildNodes[1].ChildNodes[0].Attributes["href"].Value;
                                    constLevel = tableChild[num].ChildNodes[7].InnerText;
                                }

                                if (songData.ContainsKey(curName) == true)
                                {
                                    songData[curName].WikiUrl = wikiUri;

                                    switch (diffi)
                                    {
                                        case "MASTER":
                                        case "MASTER (スタンダード譜面)":
                                            if (songData[curName].MasterInfo != null)
                                                songData[curName].MasterInfo.ConstLevel = constLevel;
                                            break;
                                        case "Re:MASTER":
                                        case "Re:MASTER (スタンダード譜面)":
                                            if (songData[curName].RemasterInfo != null)
                                                songData[curName].RemasterInfo.ConstLevel = constLevel;
                                            break;
                                        case "EXPERT":
                                        case "EXPERT (スタンダード譜面)":
                                            if (songData[curName].ExpertInfo != null)
                                                songData[curName].ExpertInfo.ConstLevel = constLevel;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool IsSongTable(string difficulty, out bool isStandard)
        {
            if(difficulty == "MASTER"            
                || difficulty == "Re:MASTER"
                || difficulty == "EXPERT"
                )
            {
                isStandard = false;
                return true;
            }
            else if(difficulty == "Re:MASTER (スタンダード譜面)"
                || difficulty == "MASTER (スタンダード譜面)"
                || difficulty == "EXPERT (スタンダード譜面)")
            {
                isStandard = true;
                return true;
            }

            isStandard = false;
            return false;
        }

        private async Task ParseSongData(Difficulty difficulty)
        {
            string songDataPage = await NetworkManager.GetSongDataPage(NetworkManager.Cookie,difficulty);

            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(songDataPage);

            var songDataNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]").ChildNodes;

            int count = 0;

            Category currentCategory = Category.POPS_AND_ANIME;

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var node in songDataNode)
            {
                if (node.Name == "div" && node.Attributes["class"].Value == "w_450 m_15 p_r f_0")
                {
                    var songNodes = node.ChildNodes[1].ChildNodes[1].ChildNodes;
                    string name = songNodes[7].InnerText;

                    if(songData.ContainsKey(name) == false)
                    {
                        songData[name] = new SongData();
                        songData[name].Name = name;
                    }

                    var currentSongData = songData[name];
                    currentSongData.category = currentCategory;
                    var songDifficultyData = new SongDifficultyData();

                    songDifficultyData.Level = songNodes[5].InnerText;

                    if (node.Attributes.Count > 1)
                    {
                        if (!node.Attributes["id"].Value.Contains("dx"))
                        {
                            stringBuilder.Clear();
                            stringBuilder.Append(name);
                            stringBuilder.Append("_STANDARD");
                            name = stringBuilder.ToString();
                        }
                    }

                    if (songNodes.Count > 11)
                    {
                        songDifficultyData.Score = songNodes[9].InnerText;
                        songDifficultyData.DeluxScore = string.Join("", songNodes[11].InnerText.Split('\t', '\n'));
                    }


                    switch(difficulty)
                    {
                        case Difficulty.ReMaster:
                            currentSongData.RemasterInfo = songDifficultyData;
                            break;
                        case Difficulty.Master:
                            currentSongData.MasterInfo = songDifficultyData;
                            break;
                        case Difficulty.Expert:
                            currentSongData.ExpertInfo = songDifficultyData;
                            break;
                        case Difficulty.Advanced:
                            currentSongData.AdvancedInfo = songDifficultyData;
                            break;
                        case Difficulty.Basic:
                            currentSongData.BasicInfo = songDifficultyData;
                            break;
                        default:
                            currentSongData.MasterInfo = songDifficultyData;
                            break;
                    }    

                    count++;
                }
                else if (node.Name == "div" && node.Attributes["class"].Value == "screw_block m_15 f_15 scroll_point")
                {
                    switch (node.InnerText)
                    {
                        case "POPS＆ANIME":
                            currentCategory = Category.POPS_AND_ANIME;
                            break;
                        case "niconico＆VOCALOID™":
                            currentCategory = Category.NICONICO;
                            break;
                        case "東方Project":
                            currentCategory = Category.TOUHOU;
                            break;
                        case "GAME＆VARIETY":
                            currentCategory = Category.GAME_AND_VARIETY;
                            break;
                        case "maimai":
                            currentCategory = Category.MAIMAI;
                            break;
                        case "オンゲキ＆CHUNITHM":
                            currentCategory = Category.ONGEKI_AND_CHUNITHM;
                            break;
                    }
                }
            }
        }

        public List<KeyValuePair<string, LatestSongData>> GetLatestRecords()
        {
            return latestSongData.ToList();
        }

        public List<KeyValuePair<string, SongData>> GetSongDataList()
        {
            return songData.ToList();
        }
    }

    class LatestSongData
    {
        public string Name { get; set; }
        public string Score { get; set; }
        public string imageUrl { get; set; }

        public Difficulty Difficulty { get; set; }

        public LatestSongData(string title, string score, string imgUrl, Difficulty difficulty)
        {
            Name = title;
            Score = score;
            imageUrl = imgUrl;
            Difficulty = difficulty;
        }
    }

    class SongData
    {
        public string Name { get; set; }

        public Category category;

        public string WikiUrl { get; set; } = null;

        private string imageUrl = null;

        public string ImageUrl 
        {
            get
            {
                if (imageUrl != null)
                    return imageUrl;

                if (Application.Current.Properties.ContainsKey(Name) == true)
                {
                    imageUrl = (string)Application.Current.Properties[Name];
                }
                else
                {
                    imageUrl = MaimaiData.Instance.LoadWikiImageUrl(WikiUrl);
                    Application.Current.Properties[Name] = imageUrl;
                }

                return imageUrl;
            }
        }

        public SongDifficultyData BasicInfo { get; set; } = null;
        public SongDifficultyData AdvancedInfo { get; set; } = null;
        public SongDifficultyData ExpertInfo { get; set; } = null;
        public SongDifficultyData MasterInfo { get; set; } = null;
        public SongDifficultyData RemasterInfo { get; set; } = null;

        public bool WhereLevel(string level)
        {
            if (level == "")
                return true;

            if (BasicInfo?.Level == level ||
                AdvancedInfo?.Level == level ||
                ExpertInfo?.Level == level ||
                MasterInfo?.Level == level ||
                RemasterInfo?.Level == level)
                return true;

            return false;
        }

        public SongDifficultyData SelectLevel(string level)
        {
            if (BasicInfo?.Level == level)
                return BasicInfo;
            if (AdvancedInfo?.Level == level)
                return AdvancedInfo;
            if (ExpertInfo?.Level == level)
                return ExpertInfo;
            if (MasterInfo?.Level == level)
                return MasterInfo;
            if (RemasterInfo?.Level == level)
                return RemasterInfo;

            return null;
        }

        public Color GetDifficultyColor(string level)
        {
            if (BasicInfo?.Level == level)
                return Color.LightGreen;
            if (AdvancedInfo?.Level == level)
                return Color.LightYellow;
            if (ExpertInfo?.Level == level)
                return Color.LightPink;
            if (MasterInfo?.Level == level)
                return Color.MediumPurple;
            if (RemasterInfo?.Level == level)
                return Color.Plum;

            return Color.White;
        }
    }

    class SongDifficultyData
    {
        public string Level { get; set; } = null;
        public string ConstLevel { get; set; } = null;
        public string ClearRank{ get => ScoreToRank(Score); }
        public string Score { get; set; } = null;
        public string DeluxScore { get; set; } = null;
        public string LatestPlayedDate { get; set; } = null;
        public string PlayCount { get; set; } = null;

        public static string ScoreToRank(string score)
        {
            if (score == null)
                return "기록 없음";

            string pureScore = string.Join("", score.Split('%'));
            float floatScore = (float.Parse(pureScore));

            if (floatScore >= 100.5000f)
            {
                return "SSS+";
            }
            else if (floatScore >= 100.0000f)
            {
                return "SSS";
            }
            else if (floatScore >= 99.5000f)
            {
                return "SS+";
            }
            else if (floatScore >= 99.0000f)
            {
                return "SS";
            }
            else if (floatScore >= 98.0000f)
            {
                return "S+";
            }
            else if (floatScore >= 97.0000f)
            {
                return "S";
            }
            else if (floatScore >= 94.0000f)
            {
                return "AAA";
            }
            else if (floatScore >= 90.0000f)
            {
                return "AA";
            }
            else if (floatScore >= 80.0000f)
            {
                return "A";
            }
            else
            {
                return "B 이하";
            }
        }
    }

    enum Difficulty
    {
        Basic = 0,
        Advanced = 1,
        Expert = 2,
        Master = 3,
        ReMaster = 4,
        Unknown = 5
    }

    enum Category
    {
        NONE = 0,
        POPS_AND_ANIME = 1,
        NICONICO =2,
        TOUHOU=3,
        GAME_AND_VARIETY=5,
        MAIMAI=4,
        ONGEKI_AND_CHUNITHM=6
    }

}
