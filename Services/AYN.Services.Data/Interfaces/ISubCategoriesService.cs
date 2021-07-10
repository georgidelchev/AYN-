using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Administration.Categories;

namespace AYN.Services.Data.Interfaces
{
    public interface ISubCategoriesService
    {
        Task CreateAsync(AddSubCategoryInputModel input, int categoryId);

        IQueryable<T> GetAllByCategoryId<T>(int categoryId);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllByCategoryIdAsKeyValuePairsAsync(int categoryId);

        Task<T> GetByIdAsync<T>(int id);

        bool IsSubCategoryExisting(string subCategoryName);
    }
}
