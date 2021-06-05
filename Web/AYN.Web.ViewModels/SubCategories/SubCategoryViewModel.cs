using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.SubCategories
{
    public class SubCategoryViewModel : IMapFrom<SubCategory>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AdsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<SubCategory, SubCategoryViewModel>()
                .ForMember(m => m.AdsCount, opt => opt.MapFrom(o => o.Ads.Count));
        }
    }
}
