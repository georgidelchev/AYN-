using System.ComponentModel.DataAnnotations;

using AYN.Data.Models;
using AYN.Services.Mapping;

using static AYN.Common.AttributeConstraints;

namespace AYN.Web.ViewModels.Administration.SubCategories;

public class EditSubCategoryInputModel : IMapFrom<SubCategory>
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(CategoryNameMinLength)]
    [MaxLength(CategoryNameMaxLength)]
    public string Name { get; set; }
}
