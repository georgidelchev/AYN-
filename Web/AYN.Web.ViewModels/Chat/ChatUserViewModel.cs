using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Chat
{
    public class ChatUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string AvatarImageUrl { get; set; }
    }
}
