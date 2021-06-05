using System.Linq;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Services.Data
{
    public class SubCategoriesService : ISubCategoriesService
    {
        private readonly IDeletableEntityRepository<SubCategory> subCategoriesRepository;

        public SubCategoriesService(
            IDeletableEntityRepository<SubCategory> subCategoriesRepository)
        {
            this.subCategoriesRepository = subCategoriesRepository;
        }

        public IQueryable<T> GetAllByCategoryId<T>(int categoryId)
            => this.subCategoriesRepository
                .All()
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Name)
                .To<T>();
    }
}
