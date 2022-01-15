using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Threading.Tasks;
using System.Net;



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
        private Dictionary<string, SongData> songData;

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

            songData = new Dictionary<string, SongData>();

            string songDataPage = NetworkManager.GetSongDataPage(NetworkManager.Cookie);

            htmlDocument.LoadHtml(songDataPage);

            var songDataNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]").ChildNodes;

            var nodeDocument = new HtmlDocument();

            int count = 0;

            foreach (var node in songDataNode)
            {
                if (node.Name == "div" && node.Attributes["class"].Value == "w_450 m_15 p_r f_0")
                {
                    count++;
                    var songNodes = node.ChildNodes[1].ChildNodes[1].ChildNodes;
                    string songIdx = "";
                    if (songNodes.Count > 11)
                        songIdx = songNodes[21].Attributes["value"].Value;
                    else
                        songIdx = songNodes[9].Attributes["value"].Value;
                    string sondDataHtml = NetworkManager.GetSongData(NetworkManager.Cookie, songIdx);
                    nodeDocument.LoadHtml(sondDataHtml);
                    var songNameNode = nodeDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[1]/div[2]");

                    if(songNameNode != null)
                      songData[songNameNode.InnerText] = new SongData();
                }
            }

            
            if (count > songData.Count)
            {
                songDataPage = NetworkManager.GetSongDataPage(NetworkManager.Cookie);

                htmlDocument.LoadHtml(songDataPage);

                songDataNode = htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[2]").ChildNodes;


                foreach (var node in songDataNode)
                {
                    if (node.Name == "div" && node.Attributes["class"].Value == "w_450 m_15 p_r f_0")
                    {
                        var songNodes = node.ChildNodes[1].ChildNodes[1].ChildNodes;
                        string songIdx = "";
                        if (songNodes.Count > 11)
                            songIdx = songNodes[21].Attributes["value"].Value;
                        else
                            songIdx = songNodes[9].Attributes["value"].Value;
                        string sondDataHtml = NetworkManager.GetSongData(NetworkManager.Cookie, songIdx);
                        nodeDocument.LoadHtml(sondDataHtml);
                        var songNameNode = nodeDocument.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[1]/div[2]");

                        if (songNameNode != null && songData.ContainsKey(songNameNode.InnerText) == false)
                            songData[songNameNode.InnerText] = new SongData();
                    }
                }
            }
        }

        public List<KeyValuePair<string, LatestSongData>> GetLatestRecords()
        {
            return latestSongData.ToList();
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

        public SongDifficultyData BasicInfo { get; set; } = null;
        public SongDifficultyData AdvancedInfo { get; set; } = null;
        public SongDifficultyData ExpertInfo { get; set; } = null;
        public SongDifficultyData MasterInfo { get; set; } = null;
        public SongDifficultyData RemasterInfo { get; set; } = null;
    }

    class SongDifficultyData
    {
        public int Level { get; set; }
        public string ClearClass { get; set; }
        public string Score { get; set; }
        public string DeluxScore { get; set; }
        public string LatestPlayedDate { get; set; }
        public string PlayCount { get; set; }
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
        POPS_AND_ANIME,
        NICONICO,
        TOUHOU,
        GAME_AND_VARIETY,
        MAIMAI,
        ONGEKI_AND_CHUNITHM
    }
}
