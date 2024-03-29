﻿using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Comments;

namespace AYN.Web.ViewModels.Ads;

public class GetDetailsViewModel : IMapFrom<Ad>, IHaveCustomMappings
{
    public string Id { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    public string Description { get; set; }

    public decimal? Weight { get; set; }

    public string AddedByUserId { get; set; }

    public string AddedByUserAvatarImageUrl { get; set; }

    public string Town { get; set; }

    public int TownId { get; set; }

    public string Category { get; set; }

    public int CategoryId { get; set; }

    public string SubCategory { get; set; }

    public int SubCategoryId { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedOnAsString
        => this.CreatedOn.ToString("d");

    public DateTime ModifiedOn { get; set; }

    public string ModifiedOnAsString
        => this.ModifiedOn.ToString("d");

    public ProductCondition ProductCondition { get; set; }

    public AdType AdType { get; set; }

    public DeliveryTake DeliveryTake { get; set; }

    public string AddedByUserPhoneNumber { get; set; }

    public string AddedByUserUsername { get; set; }

    public DateTime AddedByUserCreatedOn { get; set; }

    public string AddedByUserCreatedOnAsString
        => this.AddedByUserCreatedOn.ToString("d");

    public int ActiveAdsCount { get; set; }

    public int TotalCommentsCount { get; set; }

    public int TotalViews { get; set; }

    public IEnumerable<string> ImagesUrls { get; set; }

    public ICollection<CommentViewModel> Comments { get; set; }

    public bool IsItInFavoritesForCurrentUser { get; set; }

    public bool IsPromoted { get; set; }

    public DateTime PromotedUntil { get; set; }

    public string PromotedUntilAsString
        => this.PromotedUntil.ToString("g");

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration
            .CreateMap<Ad, GetDetailsViewModel>()
            .ForMember(m => m.Category, opt => opt.MapFrom(o => o.Category.Name))
            .ForMember(m => m.SubCategory, opt => opt.MapFrom(o => o.SubCategory.Name))
            .ForMember(m => m.Town, opt => opt.MapFrom(o => o.Town.Name))
            .ForMember(m => m.ActiveAdsCount, opt => opt.MapFrom(o => o.AddedByUser.Ads.Count(a => !a.IsDeleted && !a.IsArchived)))
            .ForMember(m => m.ImagesUrls, opt => opt.MapFrom(o => o.Images.Select(a => a.ImageUrl)))
            .ForMember(m => m.TotalCommentsCount, opt => opt.MapFrom(o => o.Comments.Count(a => !a.IsDeleted)))
            .ForMember(m => m.TotalViews, opt => opt.MapFrom(o => o.UserAdViews.Count(uav => uav.AdId == o.Id)));
    }
}
