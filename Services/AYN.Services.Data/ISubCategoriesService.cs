using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Web.ViewModels.SubCategories;

namespace AYN.Services.Data
{
    public interface ISubCategoriesService
    {
        Task CreateAsync(CreateSubCategoryInputModel input, int categoryId);

        IQueryable<T> GetAllByCategoryId<T>(int categoryId);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        Task<T> GetByIdAsync<T>(int id);

        bool IsSubCategoryExisting(string subCategoryName);
    }
}
