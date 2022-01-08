using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Ads;

public class GetAdsViewModel : IMapFrom<Ad>, IHaveCustomMappings
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string TitleImageUrl { get; set; }

    public decimal Price { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Ad, GetAdsViewModel>()
            .ForMember(m => m.TitleImageUrl, opt => opt.MapFrom(o => o.Images.FirstOrDefault().ImageUrl));
    }
}
