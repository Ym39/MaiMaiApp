using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using Xamarin.Forms;

namespace MaiMaiApp
{
    static class NetworkManager
    {
        public static CookieContainer Cookie = new CookieContainer();

        private readonly static string maimaiUrl = "https://lng-tgk-aime-gw.am-all.net/common_auth/login?redirect_url=https%3A%2F%2Fmaimaidx-eng.com%2Fmaimai-mobile%2F&site_id=maimaidxex&back_url=https%3A%2F%2Fmaimai.sega.com%2F&alof=0";

        private readonly static string loginUrl = "https://lng-tgk-aime-gw.am-all.net/common_auth/login/sid/";

        private readonly static string recordsUrl = "https://maimaidx-eng.com/maimai-mobile/record/";

        public readonly static string wikiUrl = "https://maimai.wiki.fc2.com";

        private static StringBuilder stringBuilder = new StringBuilder();
        public static string GetMaimaiHome(CookieContainer cookie)
        {
            string result = String.Empty;

            var request = (HttpWebRequest)WebRequest.Create(maimaiUrl);
            request.Method = "GET";
            request.CookieContainer = cookie;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException exception)
            {
                string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
                Console.WriteLine(pageContent);
                return "Error";
            }
        }

        public async static Task<string> LoginMaiMai(CookieContainer cookieContainer)
        {         
            string url = loginUrl;
            string result;

            string data = "retention=1&sid=vinoo39&password=qusdpals39";

            var hendler = new HttpClientHandler()
            {
                Credentials = CredentialCache.DefaultCredentials,
                CookieContainer = cookieContainer,
                Proxy = null,
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
            };
            HttpClient client = new HttpClient(hendler);
            var values = new Dictionary<string, string>
            {
                {"retention","1"},
                {"sid",(string)Application.Current.Properties["id"]},
                {"password",(string)Application.Current.Properties["password"]}
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new FormUrlEncodedContent(values)
            };
            request.Headers.Host = "lng-tgk-aime-gw.am-all.net";
            request.Headers.Accept.TryParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.Referrer = new Uri("https://lng-tgk-aime-gw.am-all.net/common_auth/login?site_id=maimaidxex&redirect_url=https://maimaidx-eng.com/maimai-mobile/&back_url=https://maimai.sega.com/");
            request.Headers.UserAgent.TryParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36");
            request.Headers.Connection.TryParseAdd("keep-alive");
            request.Headers.ConnectionClose = false;
            request.Headers.AcceptEncoding.TryParseAdd("gzip, deflate, br");
            request.Headers.AcceptLanguage.TryParseAdd("ko-KR,ko;q=0.9,en-US;q=0.8,en;q=0.7");

            try
            {
                var responce = await client.SendAsync(request);

                using (var reader = new StreamReader(await responce.Content.ReadAsStreamAsync()))
                {
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (HttpRequestException exception)
            {
                string pageContent = exception.Message;
                return pageContent;
            }
            catch (ArgumentNullException exception)
            {
                return exception.Message;
            }
        }

        public static string GotoRecords(CookieContainer cookieContainer)
        {
            string url = recordsUrl;
            string result;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 30 * 1000;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Host = "maimaidx-eng.com";
            request.Referer = "https://maimaidx-eng.com/maimai-mobile/home/";

            request.CookieContainer = cookieContainer;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException exception)
            {
                string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
                Console.WriteLine(pageContent);
                return "Error";
            }
        }

        public static string GetSongDataPage(CookieContainer cookieContainer, Difficulty difficulty)
        {
            string url;

            switch(difficulty)
            {
                case Difficulty.ReMaster:
                    url = "https://maimaidx-eng.com/maimai-mobile/record/musicGenre/search/?genre=99&diff=4";
                    break;
                case Difficulty.Master:
                    url = "https://maimaidx-eng.com/maimai-mobile/record/musicGenre/search/?genre=99&diff=3";
                    break;
                case Difficulty.Expert:
                    url = "https://maimaidx-eng.com/maimai-mobile/record/musicGenre/search/?genre=99&diff=2";
                    break;
                case Difficulty.Advanced:
                    url = "https://maimaidx-eng.com/maimai-mobile/record/musicGenre/search/?genre=99&diff=1";
                    break;
                case Difficulty.Basic:
                    url = "https://maimaidx-eng.com/maimai-mobile/record/musicGenre/search/?genre=99&diff=0";
                    break;
                default:
                    url = "https://maimaidx-eng.com/maimai-mobile/record/musicGenre/search/?genre=99&diff=3";
                    break;
            }

            string result;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 30 * 1000;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Host = "maimaidx-eng.com";
            //request.Referer = "https://maimaidx-eng.com/maimai-mobile/home/";

            request.CookieContainer = cookieContainer;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException exception)
            {
                string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
                Console.WriteLine(pageContent);
                return "Error";
            }
        }

        public static string GetSongData(CookieContainer cookieContainer, string songIdx)
        {
            stringBuilder.Clear();
            stringBuilder.Append("https://maimaidx-eng.com/maimai-mobile/record/musicDetail/?idx=");
            stringBuilder.Append(songIdx);
            string url = stringBuilder.ToString();
            string result;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 30 * 1000;
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            request.Host = "maimaidx-eng.com";
            //request.Referer = "https://maimaidx-eng.com/maimai-mobile/home/";

            request.CookieContainer = cookieContainer;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException exception)
            {
                string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
                Console.WriteLine(pageContent);
                return "Error";
            }
        }

        public static string GetSongListMaiMaiWiki(string url, CookieContainer cookieContainer)
        {
            //string url = "https://maimai.wiki.fc2.com/wiki/%E6%9B%B2%E3%83%AA%E3%82%B9%E3%83%88%28%E3%83%AC%E3%83%99%E3%83%AB%E9%A0%86%29";
            string result;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = 30 * 1000;
            //request.KeepAlive = true;
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            //request.Host = "maimaidx-eng.com";
            //request.Referer = "https://maimaidx-eng.com/maimai-mobile/home/";

            request.CookieContainer = cookieContainer;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException exception)
            {
                string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
                Console.WriteLine(pageContent);
                return "Error";
            }
        }

        public static string GetSongDataFromWiki(string url, CookieContainer cookieContainer)
        {
            string result;

            var request = (HttpWebRequest)WebRequest.Create(NetworkManager.wikiUrl+url);
            request.Method = "GET";
            request.Timeout = 30 * 1000;       
            request.CookieContainer = cookieContainer;

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (WebException exception)
            {
                string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
                Console.WriteLine(pageContent);
                return "Error";
            }
        }


        public static CookieContainer CopyContainer(CookieContainer container)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, container);
                stream.Seek(0, SeekOrigin.Begin);
                return (CookieContainer)formatter.Deserialize(stream);
            }
        }
    }
}
