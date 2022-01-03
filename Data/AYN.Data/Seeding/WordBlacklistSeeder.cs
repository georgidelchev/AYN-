using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AngleSharp.Html.Parser;
using AYN.Data.Models;

namespace AYN.Data.Seeding;

public class WordBlacklistSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (!dbContext.WordsBlacklist.Any())
        {
            var wordsUrl = "https://www.cs.cmu.edu/~biglou/resources/bad-words.txt";

            var parser = new HtmlParser();
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(wordsUrl);

            var document = parser.ParseDocument(html);

            var words = document
                .QuerySelectorAll("body")
                .Select(e => e.InnerHtml.Split())
                .ToList();

            var wordsToAdd =
                (from word in words[0]
                    where !string.IsNullOrEmpty(word)
                    select new WordBlacklist() { Content = word, }).ToList();

            await dbContext.WordsBlacklist.AddRangeAsync(wordsToAdd);
            await dbContext.SaveChangesAsync();
        }
    }
}
