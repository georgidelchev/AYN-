using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Administration.Categories;

using EditCategoryInputModel = AYN.Web.ViewModels.Administration.Categories.EditCategoryInputModel;

namespace AYN.Services.Data.Interfaces
{
    public interface ICategoriesService
    {
        Task CreateAsync(CreateCategoryInputModel input, string imagePath);

        IQueryable<T> GetAll<T>();

        Task<IEnumerable<T>> GetAllWithDeletedAsync<T>();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        Task AddSubCategoryAsync(AddSubCategoryInputModel input, int categoryId);

        Task<T> GetByIdAsync<T>(int categoryId);

        Task UpdateAsync(EditCategoryInputModel input, int categoryId, string imagePath);

        Task DeleteAsync(int id);

        Task UnDeleteAsync(int id);

        Tuple<int, int> GetCounts();

        int GetTotalCount();

        bool IsExisting(int categoryId);

        bool IsCategoryContainsGivenSubCategory(int categoryId, int subCategoryId);
    }
}
