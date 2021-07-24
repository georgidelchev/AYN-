using System.Collections.Generic;

namespace AYN.Web.ViewModels.Chat
{
    public class ChatWithUserViewModel
    {
        public ChatUserViewModel User { get; set; }

        public IEnumerable<ChatMessagesWithUserViewModel> Messages { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Emojis { get; set; }
    }
}
