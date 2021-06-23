using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Users
{
    public class GetSuggestionPeopleViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AvatarExtension { get; set; }

        public int FollowersCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ApplicationUser, GetSuggestionPeopleViewModel>()
                .ForMember(m => m.FollowersCount, opt => opt.MapFrom(o => o.Followers.Count));
        }
    }
}
