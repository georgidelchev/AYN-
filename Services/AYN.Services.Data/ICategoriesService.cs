using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Categories;

namespace AYN.Services.Data
{
    public interface ICategoriesService
    {
        Task CreateAsync(CreateCategoryInputModel input, string imagePath);

        IQueryable<T> GetAll<T>();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        Task AddSubCategoryAsync(AddSubCategoryViewModel input, int categoryId);

        Task<T> GetByIdAsync<T>(int categoryId);

        Task UpdateAsync(EditCategoryInputModel input, int categoryId, string imagePath);

        Task DeleteAsync(int id);
    }
}
