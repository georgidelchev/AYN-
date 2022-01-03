using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Ads;

public class GetAdViewModel : IMapFrom<Ad>, IHaveCustomMappings
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Ad, GetAdViewModel>()
            .ForMember(m => m.ImageUrl, opt => opt.MapFrom(o => o.Images.FirstOrDefault().ImageUrl));
    }
}
