using System.Linq;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Categories;

namespace AYN.Services.Data
{
    public interface ICategoriesService
    {
        Task CreateAsync(CreateCategoryInputModel input, string imagePath);

        IQueryable<T> GetAll<T>();

        Task AddSubCategoryAsync(AddSubCategoryViewModel input, int categoryId);

        T GetById<T>(int categoryId);

        Task UpdateAsync(EditCategoryInputModel input, int categoryId, string imagePath);

        Task DeleteAsync(int id);
    }
}
