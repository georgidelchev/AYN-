using System.ComponentModel.DataAnnotations;

using AYN.Data.Models;
using AYN.Services.Mapping;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Categories
{
    public class EditCategoryInputModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [MinLength(CategoryNameMinLength)]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        [MinLength(PictureExtensionMaxLength)]
        public string PictureExtension { get; set; }
    }
}
