using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using AngleSharp.Html.Parser;
using AYN.Data.Models;

namespace AYN.Data.Seeding;

public class EmojiSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (!dbContext.Emojis.Any())
        {
            var emojisUrl = "https://unicode.org/emoji/charts/full-emoji-list.html";

            var parser = new HtmlParser();
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(emojisUrl);

            var document = parser.ParseDocument(html);

            var emojis = document
                .QuerySelectorAll(".chars")
                .Select(e => e.InnerHtml)
                .ToList();

            var emojisToAdd = emojis
                .Select(emoji => new Emoji { Image = emoji, })
                .ToList();

            await dbContext.Emojis.AddRangeAsync(emojisToAdd);
            await dbContext.SaveChangesAsync();
        }
    }
}
