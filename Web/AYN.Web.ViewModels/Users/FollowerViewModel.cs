using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Users
{
    public class FollowerViewModel : IMapFrom<FollowerFollowee>
    {
        public string FollowerId { get; set; }

        public string FollowerFirstName { get; set; }

        public string FollowerLastName { get; set; }

        public string FollowerAvatarImageUrl { get; set; }

        public string FollowerThumbnailImageUrl { get; set; }
    }
}
