using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using AutoMapper;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;
using AYN.Services.Mapping;
using Microsoft.AspNetCore.Http;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Ads
{
    public class EditAdInputModel : IMapFrom<Ad>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "You should enter a name.")]
        [MinLength(AdNameMinLength)]
        [MaxLength(AdNameMaxLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You should enter a description.")]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Range(typeof(decimal), "0.01", "10000", ErrorMessage = "Weight should be between 0.01 and 10,000 kilos.")]
        public decimal? Weight { get; set; }

        [Range(typeof(decimal), "0.01", "1000000.0", ErrorMessage = "Price should be between 0.01 and 1 million.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "You should choose a town.")]
        public int TownId { get; set; }

        [Required(ErrorMessage = "You should choose an address.")]
        public int AddressId { get; set; }

        [Required(ErrorMessage = "You should choose a category.")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "You should choose an subCategory.")]
        public int SubCategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> SubCategories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Towns { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Addresses { get; set; }

        [Required(ErrorMessage = "You should choose the condition of the product.")]
        public ProductCondition ProductCondition { get; set; }

        [Required(ErrorMessage = "You should choose the type of the product.")]
        public AdType AdType { get; set; }

        [Required(ErrorMessage = "You should choose who will pay the delivery tax.")]
        public DeliveryTake DeliveryTake { get; set; }

        public IEnumerable<string> ImagesUrls { get; set; }

        public IEnumerable<IFormFile> Images { get; set; }
            = new HashSet<IFormFile>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Ad, EditAdInputModel>()
                .ForMember(m => m.ImagesUrls, opt => opt.MapFrom(o => o.Images.Select(i => i.ImageUrl)))
                .ForMember(m => m.Images, opt => opt.Ignore());
        }
    }
}
