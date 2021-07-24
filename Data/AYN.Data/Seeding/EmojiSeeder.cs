using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using AYN.Data.Models;

namespace AYN.Data.Seeding
{
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

                var index = 0;
                foreach (var emoji in emojis)
                {
                    var emojiToAdd = new Emoji()
                    {
                        Image = emoji,
                    };

                    await dbContext.Emojis.AddAsync(emojiToAdd);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
