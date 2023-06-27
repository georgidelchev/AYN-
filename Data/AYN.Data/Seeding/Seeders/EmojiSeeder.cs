using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Models;
using AYN.Data.Seeding.Interfaces;

namespace AYN.Data.Seeding.Seeders;

public class EmojiSeeder : ISeeder
{
    public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
    {
        if (!dbContext.Emojis.Any())
        {
            var emojis = new List<string>
            {
                "😀", "😃", "😄", "😁", "😆", "😅", "🤣", "😂", "🙂", "🙃",
                "😉", "😊", "😇", "🥰", "😍", "🤩", "😘", "😗", "☺️", "😚",
                "😙", "😋", "😛", "😜", "😝", "🤪", "😞", "😠", "😡", "😔",
                "😖", "😣", "😢", "😭", "😪", "😥", "😰", "😅", "😓", "🤤",
                "😴", "🤯", "😧", "😨", "😱", "😳", "🥵", "🥶", "🥴", "😵",
                "🤬", "😈", "👿", "💀", "☠️", "💩", "🤡", "👹", "👺", "👻",
                "👽", "👾", "🤖", "😺", "😸", "😹", "😻", "😼", "😽", "🙀",
                "😿", "😾", "🙈", "🙉", "🙊", "💋", "💌", "💘", "💝", "💖",
                "💗", "💓", "💞", "💕", "💟", "❣️", "💔", "❤️", "🧡", "💛",
                "💚", "💙", "💜", "🖤", "💯", "💢", "💥", "💫", "💦", "💨",
                "🕳️", "💣", "💬", "🗯️", "💭", "💤", "👋", "🤚", "🖐️", "✋",
            };

            var emojisToAdd = emojis
                .Select(emoji => new Emoji { Image = emoji, })
                .ToList();

            await dbContext.Emojis.AddRangeAsync(emojisToAdd);
            await dbContext.SaveChangesAsync();
        }
    }
}
