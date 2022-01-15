﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MaiMaiApp
{
    static class NetworkManager
    {
        public static CookieContainer Cookie = new CookieContainer();

        private readonly static string maimaiUrl = "https://lng-tgk-aime-gw.am-all.net/common_auth/login?redirect_url=https%3A%2F%2Fmaimaidx-eng.com%2Fmaimai-mobile%2F&site_id=maimaidxex&back_url=https%3A%2F%2Fmaimai.sega.com%2F&alof=0";

        private readonly static string loginUrl = "https://lng-tgk-aime-gw.am-all.net/common_auth/login/sid/";

        private readonly static string recordsUrl = "https://maimaidx-eng.com/maimai-mobile/record/";

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
            //string url = loginUrl;
            //string result;

            //string data = "retention=1&sid=vinoo39&password=qusdpals39";

            //var request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "POST";
            ////request.Timeout = 30 * 1000;
            //request.ContentLength = data.Length;
            //request.KeepAlive = true;
            //request.ContentType = "application/x-www-form-urlencoded";
            //request.UserAgent =
            //    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36";
            //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9";
            //request.Host = "lng-tgk-aime-gw.am-all.net";
            //request.Referer =
            //    "https://lng-tgk-aime-gw.am-all.net/common_auth/login?site_id=maimaidxex&redirect_url=https://maimaidx-eng.com/maimai-mobile/&back_url=https://maimai.sega.com/";

            //request.Proxy = null;
            //request.Credentials = CredentialCache.DefaultCredentials;

            //ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            //request.CookieContainer = cookieContainer;


            //StreamWriter writer = new StreamWriter(request.GetRequestStream());
            //writer.Write(data);
            //writer.Close();

            //try
            //{
            //    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //    {
            //        HttpStatusCode status = response.StatusCode;
            //        Console.WriteLine(status);

            //        using (var reader = new StreamReader(response.GetResponseStream()))
            //        {
            //            result = reader.ReadToEnd();
            //            return result;
            //        }
            //    }
            //}
            //catch (WebException exception)
            //{
            //    string pageContent = new StreamReader(exception.Response.GetResponseStream()).ReadToEnd().ToString();
            //    return pageContent;
            //}

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
                {"sid","vinoo39"},
                {"password","qusdpals39"}
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
    }
}