using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Trasher.CommandHandlers.Services;

namespace Trasher.CommandHandlers
{
    public class RandomImageCommandHandler
    {
        private const string ImageUrlPattern = @"""src"": ""(?<ImageUrl>.*)""";
        private const string TrashKeyword = " трешак";
        private readonly static RandomWordsService _randomWordsService = new RandomWordsService();
        private static readonly string _skypeLineSeparator = "  " + Environment.NewLine;

        public string GetInfo(string command)
        {
            string userWords = GetUserWords(command);

            if (string.IsNullOrEmpty(userWords))
            {
                string randomWords = _randomWordsService.GetRandomWords();
                string randomImageUrl = GetRandomImageUrl(randomWords + TrashKeyword);

                return randomWords
                       + _skypeLineSeparator
                       + randomImageUrl;
            }

            return GetRandomImageUrl(userWords);
        }

        private string GetRandomImageUrl(string userWords)
        {
            List<string> urls = GetUrls(userWords);

            var random = new Random();
            int randomUrlIndex = random.Next(0, urls.Count);
            return urls[randomUrlIndex];
        }

        private static string GetUserWords(string command)
        {
            const string imgCommand = " img ";
            int queryStartIndex = command.IndexOf(imgCommand, StringComparison.Ordinal);
            if (queryStartIndex < 0)
            {
                return string.Empty;
            }

            string userQuery = command.Substring(queryStartIndex).Replace(imgCommand, string.Empty);

            return userQuery + TrashKeyword;
        }

        private List<string> GetUrls(string command)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.googleapis.com");
            var random = new Random();
            int startIndex = random.Next(1, 3);
            var query = $"/customsearch/v1?q={Uri.EscapeDataString(command)}&cx={ConfigurationManager.AppSettings["GoogleCustomSearchId"]}&imgSize=medium&key={ConfigurationManager.AppSettings["GoogleApiKey"]}&start={startIndex}";
            using (httpClient)
            {
                HttpResponseMessage response = httpClient
                    .GetAsync(query)
                    .Result;

                string json = response
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                List<string> urlList = Regex
                    .Matches(json, ImageUrlPattern)
                    .Cast<Match>()
                    .Select(x => x.Groups["ImageUrl"].Value)
                    .Where(x => x.Contains("gstatic.com") == false)
                    .ToList();

                return urlList.Count == 0 
                    ? new List<string> { "Интернет сегодня особенно плох, ничего не найдено!" } 
                    : urlList;
            }
        }
    }
}