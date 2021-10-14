using System.ComponentModel.DataAnnotations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Chat
{
    public class ChatSendMessageInputModel
    {
        [Required]
        public string Receiver { get; set; }

        [Required]
        [MinLength(MessageContentMinLength)]
        [MaxLength(MessageContentMaxLength)]
        public string Message { get; set; }
    }
}
