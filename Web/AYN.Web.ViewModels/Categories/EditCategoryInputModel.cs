using System.ComponentModel.DataAnnotations;
using AYN.Data.Models;
using AYN.Services.Mapping;
using Microsoft.AspNetCore.Http;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Categories
{
    public class EditCategoryInputModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(CategoryNameMinLength)]
        [MaxLength(CategoryNameMaxLength)]
        public string Name { get; set; }

        public string PictureExtension { get; set; }

        public IFormFile Picture { get; set; }
    }
}
