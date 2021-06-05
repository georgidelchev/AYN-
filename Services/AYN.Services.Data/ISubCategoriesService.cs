using System.Linq;

namespace AYN.Services.Data
{
    public interface ISubCategoriesService
    {
        IQueryable<T> GetAllByCategoryId<T>(int categoryId);
    }
}
