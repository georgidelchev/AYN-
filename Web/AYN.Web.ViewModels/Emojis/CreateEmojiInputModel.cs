using System.ComponentModel.DataAnnotations;

namespace AYN.Web.ViewModels.Emojis
{
    public class CreateEmojiInputModel
    {
        [Required]
        public string Emoji { get; set; }
    }
}
