using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Trasher.CommandHandlers
{
    public class RandomImageCommandHandler
    {
        private const string ImageUrlPattern = @"""src"": ""(?<ImageUrl>.*)""";
        private const string TrashKeyword = " трешак";

        public string GetInfo(string command)
        {
            const string imgCommand = " img ";
            int queryStartIndex = command.IndexOf(imgCommand, StringComparison.Ordinal);
            if (queryStartIndex < 0)
            {
                return string.Empty;
            }

            string userQuery = command.Substring(queryStartIndex).Replace(imgCommand, string.Empty);
            string query = userQuery + TrashKeyword;

            List<string> urls = GetUrls(query);

            var random = new Random();
            int randomUrlIndex = random.Next(0, urls.Count);
            return urls[randomUrlIndex];
        }

        private List<string> GetUrls(string command)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://www.googleapis.com");
            var random = new Random();
            int startIndex = random.Next(0, 10);
            var query = $"/customsearch/v1?q={command}&cx={ConfigurationManager.AppSettings["GoogleCustomSearchId"]}&imgSize=medium&key={ConfigurationManager.AppSettings["GoogleApiKey"]}&start={startIndex}";
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
                    ? new List<string> { json } 
                    : urlList;
            }
        }
    }
}