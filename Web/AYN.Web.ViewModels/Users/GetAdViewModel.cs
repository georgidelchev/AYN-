using System.Linq;
using AutoMapper;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Users
{
    public class GetAdViewModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string PictureExtension { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, GetAdViewModel>()
                .ForMember(m => m.PictureExtension, opt => opt.MapFrom(o => o.Pictures.FirstOrDefault().Extension));
        }
    }
}
