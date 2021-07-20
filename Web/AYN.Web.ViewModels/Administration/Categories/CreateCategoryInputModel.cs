using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using AYN.Web.ViewModels.SubCategories;
using Microsoft.AspNetCore.Http;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Administration.Categories
{
    public class CreateCategoryInputModel
    {
        [Required]
        [MinLength(CategoryNameMinLength)]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        [Required]
        public IFormFile Picture { get; set; }

        [DisplayName("SubCategory")]
        public ICollection<CreateSubCategoryInputModel> SubCategories { get; set; }
            = new HashSet<CreateSubCategoryInputModel>();
    }
}
