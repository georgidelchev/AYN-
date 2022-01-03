using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Categories;

public class CategoryViewModel : IMapFrom<Category>
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string ImageUrl { get; set; }
}
