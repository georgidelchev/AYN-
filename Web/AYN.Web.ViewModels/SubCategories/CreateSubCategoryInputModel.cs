using System.ComponentModel.DataAnnotations;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.SubCategories
{
    public class CreateSubCategoryInputModel
    {
        [Required]
        [MinLength(CategoryNameMinLength)]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }
    }
}
