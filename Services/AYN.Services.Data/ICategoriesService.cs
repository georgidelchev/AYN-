using System.Linq;

namespace AYN.Services.Data
{
    public interface ICategoriesService
    {
        void Create();

        IQueryable<T> GetAll<T>();
    }
}
