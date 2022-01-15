using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;
using System.Threading.Tasks;



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

        private Dictionary<string, MaiMaiSong> songData;

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
            songData = new Dictionary<string, MaiMaiSong>();

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
                        songData[title] = new MaiMaiSong(title, score, imgUri, difficulty);
                    }
                }
            }
        }

        public List<KeyValuePair<string, MaiMaiSong>> GetLatestRecords()
        {
            return songData.ToList();
        }
    }

    class MaiMaiSong
    {
        public string Name { get; set; }
        public string Score { get; set; }
        public string imageUrl { get; set; }

        public Difficulty Difficulty { get; set; }

        public MaiMaiSong(string title, string score, string imgUrl, Difficulty difficulty)
        {
            Name = title;
            Score = score;
            imageUrl = imgUrl;
            Difficulty = difficulty;
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
}
