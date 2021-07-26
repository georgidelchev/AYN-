using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Models;
using AYN.Web.ViewModels.Administration.Categories;
using AYN.Web.ViewModels.Administration.SubCategories;

namespace AYN.Services.Data.Interfaces
{
    public interface ISubCategoriesService
    {
        Task CreateAsync(AddSubCategoryInputModel input, int categoryId);

        IQueryable<T> GetAllByCategoryId<T>(int categoryId);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllByCategoryIdAsKeyValuePairsAsync(int categoryId);

        Task<T> GetByIdAsync<T>(int id);

        SubCategory Get(int id);

        Task DeleteAsync(int id);

        Task UnDeleteAsync(int id);

        Task EditAsync(EditSubCategoryInputModel input);

        bool IsSubCategoryExisting(string subCategoryName);

        bool IsSubCategoryExisting(int id);
    }
}
