using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using AYN.Data.Models.Enumerations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Ads
{
    public class BaseAdInputModel
    {
        [Required]
        [MinLength(AdNameMinLength)]
        [MaxLength(AdNameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Range(typeof(decimal), "0.01", "10000")]
        public decimal? Weight { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SubCategoryId { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Categories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> SubCategories { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Towns { get; set; }

        [Required]
        public ProductCondition ProductCondition { get; set; }

        [Required]
        public AdType AdType { get; set; }
    }
}
