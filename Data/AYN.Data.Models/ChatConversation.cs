using System.Collections.Generic;

using AYN.Data.Common.Models;

namespace AYN.Data.Models
{
    public class ChatConversation : BaseDeletableModel<int>
    {
        public ICollection<Message> Messages { get; set; }
            = new HashSet<Message>();
    }
}
