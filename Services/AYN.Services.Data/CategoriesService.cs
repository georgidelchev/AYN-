using System.Linq;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;

        public CategoriesService(
            IDeletableEntityRepository<Category> categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        public void Create()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<T> GetAll<T>()
            => this.categoriesRepository
                .All()
                .To<T>();
    }
}
