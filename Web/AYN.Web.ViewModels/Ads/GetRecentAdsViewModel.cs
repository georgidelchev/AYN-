using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Ads
{
    public class GetRecentAdsViewModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PictureExtension { get; set; }

        public decimal Price { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Ad, GetRecentAdsViewModel>()
                .ForMember(m => m.PictureExtension, opt => opt.MapFrom(o => o.Pictures.FirstOrDefault().Extension));
        }
    }
}
