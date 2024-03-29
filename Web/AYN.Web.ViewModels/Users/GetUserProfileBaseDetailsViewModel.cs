﻿using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Users;

public class GetUserProfileBaseDetailsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
{
    public string Id { get; set; }

    public int PagingId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string About { get; set; }

    public string AvatarImageUrl { get; set; }

    public string ThumbnailImageUrl { get; set; }

    public int FolloweesCount { get; set; }

    public int FollowingsCount { get; set; }

    public bool IsCurrentUserFollower { get; set; }

    public string FacebookUrl { get; set; }

    public string InstagramUrl { get; set; }

    public string TikTokUrl { get; set; }

    public string TwitterUrl { get; set; }

    public string WebsiteUrl { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration
            .CreateMap<ApplicationUser, GetUserProfileBaseDetailsViewModel>()
            .ForMember(m => m.FolloweesCount, opt => opt.MapFrom(o => o.Followings.Count))
            .ForMember(m => m.FollowingsCount, opt => opt.MapFrom(o => o.Followers.Count));
    }
}
