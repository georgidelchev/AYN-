using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Users
{
    public class GetFollowersViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarExtension { get; set; }
    }
}
