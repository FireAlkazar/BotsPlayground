using System;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Trasher.CommandHandlers.Services
{
    public class RandomWordsService
    {
        private const string BaseUri = "https://ru.wikipedia.org";
        private const string Query = @"/w/api.php?format=json&action=query&generator=random";

        public string GetRandomWords()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseUri);
            using (httpClient)
            {
                HttpResponseMessage response = httpClient
                    .GetAsync(Query)
                    .Result;

                if (response.IsSuccessStatusCode == false)
                {
                    return string.Empty;
                }

                string responseContent = response
                    .Content
                    .ReadAsStringAsync()
                    .Result;

                var regex = new Regex("\"title\":\"(?<Title>.*)\"");
                string titleUnescaped = regex
                    .Match(responseContent)
                    .Groups["Title"]
                    .Value;

                string title = Regex.Replace(
                    titleUnescaped,
                    @"\\[Uu]([0-9A-Fa-f]{4})",
                    m => char.ToString(
                        (char)ushort.Parse(m.Groups[1].Value, NumberStyles.AllowHexSpecifier)));

                return title
                    .Replace("Обсуждение:", string.Empty)
                    .Replace("Категория:", string.Empty)
                    .Replace("Обсуждение участника:", string.Empty);
            }
        }
    }
}