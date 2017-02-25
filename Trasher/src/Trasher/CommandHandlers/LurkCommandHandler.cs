using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Trasher.CommandHandlers
{
    public class LurkCommandHandler
    {
        private const string BaseUri = @"https://lurkmore.co";
        private const string Query = @"/api.php?format=json&action=query&generator=random&grnnamespace=0&prop=revisions&rvprop=content";
        private static readonly string _skypeLineSeparator = "  " + Environment.NewLine;

        public string GetInfo(string command)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseUri);
            using (httpClient)
            {
                HttpResponseMessage response = httpClient.
                    GetAsync(Query)
                    .Result;

                if (response.IsSuccessStatusCode == false)
                {
                    return "Oшика :(";
                }

                string responseResult = response
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                dynamic queryResult = JsonConvert.DeserializeObject<dynamic>(responseResult);
                dynamic pagesObject = queryResult.query.pages;
                Match match = Regex.Match(pagesObject.ToString(), @"""pageid"": (?<PageId>\d*)");
                string pageId = match.Groups["PageId"].Value;
                dynamic pageObject = pagesObject[pageId];
                string title = (string)pageObject.title.ToString();
                string curRevision = (string)pageObject.revisions[0]["*"].ToString();

                string[] paragraphs = curRevision.Split(new [] { '\n' } , StringSplitOptions.RemoveEmptyEntries);

                int index = 0;
                if(paragraphs.Length > 6)
                {
                    index = new Random().Next(3, paragraphs.Length - 2);
                }

                string result = title 
                              + _skypeLineSeparator
                              + paragraphs[index]
                              + _skypeLineSeparator
                              + paragraphs[index + 1];

                return result
                    .Replace("[[", string.Empty)
                    .Replace("[[", string.Empty)
                    .Replace("{{", string.Empty)
                    .Replace("}}", string.Empty)
                    .Replace("|", " ,");
            }
        }
    }
}