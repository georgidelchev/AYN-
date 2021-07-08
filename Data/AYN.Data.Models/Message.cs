using System.ComponentModel.DataAnnotations;

using AYN.Data.Common.Models;

using static AYN.Common.AttributeConstraints;

namespace AYN.Data.Models
{
    public class Message : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(MessageContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ChatConversationId { get; set; }

        public virtual ChatConversation ChatConversation { get; set; }
    }
}
