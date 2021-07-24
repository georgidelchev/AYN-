using System.Collections.Generic;

namespace AYN.Web.ViewModels.Chat
{
    public class ListChatConversationsViewModel
    {
        public IEnumerable<ChatConversationsViewModel> AllChats { get; set; }
    }
}
