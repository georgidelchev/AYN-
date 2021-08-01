using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Models.Enumerations;
using Microsoft.AspNetCore.Http;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Ads
{
    public class CreateAdInputModel
    {
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

        public IEnumerable<KeyValuePair<string, string>> Towns { get; set; }

        [Required(ErrorMessage = "You should choose the condition of the product.")]
        public ProductCondition ProductCondition { get; set; }

        [Required(ErrorMessage = "You should choose the type of the product.")]
        public AdType AdType { get; set; }

        [Required(ErrorMessage = "You should choose who will pay the delivery tax.")]
        public DeliveryTake DeliveryTake { get; set; }

        [Required(ErrorMessage = "You should upload some pictures about the product.")]
        public IEnumerable<IFormFile> Pictures { get; set; }
            = new HashSet<IFormFile>();
    }
}
