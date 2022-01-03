using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Chat;

public class ChatConversationsViewModel : IMapFrom<ApplicationUser>
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string AvatarImageUrl { get; set; }

    public string LastMessage { get; set; }

    public bool IsRead { get; set; }

    public string LastMessageActivity { get; set; }
}
