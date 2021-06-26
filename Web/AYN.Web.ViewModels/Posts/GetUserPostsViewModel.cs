using System;

using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Posts
{
    public class GetUserPostsViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string AddedByUserId { get; set; }

        public string CreatedByFirstName { get; set; }

        public string CreatedByLastName { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AvatarExtension { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Post, GetUserPostsViewModel>()
                .ForMember(m => m.CreatedByFirstName, opt => opt.MapFrom(o => o.ApplicationUser.FirstName))
                .ForMember(m => m.CreatedByLastName, opt => opt.MapFrom(o => o.ApplicationUser.LastName))
                .ForMember(m => m.AvatarExtension, opt => opt.MapFrom(o => o.ApplicationUser.AvatarExtension));
        }
    }
}
