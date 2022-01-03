using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Users;

public class FolloweeViewModel : IMapFrom<FollowerFollowee>
{
    public string FolloweeId { get; set; }

    public string FolloweeFirstName { get; set; }

    public string FolloweeLastName { get; set; }

    public string FolloweeAvatarImageUrl { get; set; }

    public string FolloweeThumbnailImageUrl { get; set; }
}
