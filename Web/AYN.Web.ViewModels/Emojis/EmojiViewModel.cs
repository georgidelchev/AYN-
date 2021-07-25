using System;

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Emojis
{
    public class EmojiViewModel : IMapFrom<Emoji>
    {
        public int Id { get; set; }

        public string Image { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
