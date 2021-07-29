using System.ComponentModel.DataAnnotations;

namespace AYN.Web.ViewModels.Administration.Emojis
{
    public class CreateEmojiInputModel
    {
        [Required]
        public string Emoji { get; set; }
    }
}
