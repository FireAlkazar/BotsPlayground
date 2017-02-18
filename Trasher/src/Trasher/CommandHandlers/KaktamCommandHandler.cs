using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Trasher.CommandHandlers
{
    public class KaktamCommandHandler
    {
        private const string BaseUri = @"http://kaktam.ru";
        private const string NewsPath = @"/data.php";
        private const string NewsTitlePattern = @"<div class=""newscard__title js-newscard__title"">(?<NewsTitle>.*)<\/div>";
        private static readonly string _skypeLineSeparator = "  " + Environment.NewLine;
        private static readonly string _newsSeparator = _skypeLineSeparator + "- - - - - - - - - - - - - - - - - - - - -" + _skypeLineSeparator;

        public string GetInfo(string query)
        {
            HttpClient httpClient = GetHttpClient();
            using (httpClient)
            {
                HttpResponseMessage httpResponseMessage = httpClient
                    .GetAsync(NewsPath)
                    .Result;

                string html = httpResponseMessage
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                List<string> news = Parse(html);
                return string.Join(_newsSeparator, news);
            }
        }

        private static List<string> Parse(string html)
        {
            return Regex
                .Matches(html, NewsTitlePattern)
                .Cast<Match>()
                .Select(x => x.Groups["NewsTitle"].Value)
                .ToList();
        }

        private static HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseUri);
            return httpClient;
        }
    }
}