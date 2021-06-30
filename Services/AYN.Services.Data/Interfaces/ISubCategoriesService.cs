using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AYN.Web.ViewModels.Categories;

namespace AYN.Services.Data.Interfaces
{
    public interface ISubCategoriesService
    {
        Task CreateAsync(AddSubCategoryViewModel input, int categoryId);

        IQueryable<T> GetAllByCategoryId<T>(int categoryId);

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        Task<T> GetByIdAsync<T>(int id);

        bool IsSubCategoryExisting(string subCategoryName);
    }
}
