using System;
using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Comments;

public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
{
    public string Id { get; set; }

    public string Content { get; set; }

    public DateTime CreatedOn { get; set; }

    public string AddedByUserUsername { get; set; }

    public string AddedByUserId { get; set; }

    public string AddedByUserAvatarImageUrl { get; set; }

    public int CommentUpVotesCount { get; set; }

    public int CommentDownVotesCount { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Comment, CommentViewModel>()
            .ForMember(m => m.CommentUpVotesCount, opt => opt.MapFrom(o => o.CommentVotes.Count(a => a.Value == CommentVoteValue.Up)))
            .ForMember(m => m.CommentDownVotesCount, opt => opt.MapFrom(o => o.CommentVotes.Count(a => a.Value == CommentVoteValue.Down)));
    }
}
