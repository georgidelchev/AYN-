using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Ads;

public class MoreAdsByUserViewModel : IMapFrom<Ad>, IHaveCustomMappings
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string ShortName
        => this.Name.Length >= 4 ? this.Name.Substring(0, 4) : this.Name;

    public decimal Price { get; set; }

    public string TitleImageUrl { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration
            .CreateMap<Ad, MoreAdsByUserViewModel>()
            .ForMember(m => m.TitleImageUrl, opt => opt.MapFrom(o => o.Images.FirstOrDefault().ImageUrl));
    }
}
