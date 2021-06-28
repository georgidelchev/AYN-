using System;
using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Posts
{
    public class GetUserPostsViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int TotalReacts { get; set; }

        public ReactionType ReactionType { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Post, GetUserPostsViewModel>()
                .ForMember(m => m.TotalReacts, opt => opt.MapFrom(o => o.PostReacts.Count))
                .ForMember(m => m.ReactionType, opt => opt.MapFrom(o => o.PostReacts.Select(a => a.ReactionType).FirstOrDefault()));
        }
    }
}
