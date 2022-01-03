using System;
using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Ads;

public class WishlistAdsViewModel : IMapFrom<Ad>, IHaveCustomMappings
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ShortDescription
        => this.Description.Substring(0, DescriptionMinLength);

    public string ImageUrl { get; set; }

    public decimal Price { get; set; }

    public bool IsPromoted { get; set; }

    public DateTime? PromotedUntil { get; set; }

    public string AddedByUserId { get; set; }

    public string AddedByUserUsername { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Ad, WishlistAdsViewModel>()
            .ForMember(m => m.ImageUrl, opt => opt.MapFrom(o => o.Images.FirstOrDefault().ImageUrl));
    }
}
