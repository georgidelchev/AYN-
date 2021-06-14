using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data
{
    public class AddressesService : IAddressesService
    {
        private readonly IDeletableEntityRepository<Address> addressesRepository;

        public AddressesService(IDeletableEntityRepository<Address> addressesRepository)
        {
            this.addressesRepository = addressesRepository;
        }

        public async Task<IEnumerable<T>> GetAllByTownIdAsync<T>(int townId)
            => await this.addressesRepository
                .All()
                .Where(a => a.Towns.All(t => t.Id == townId))
                .OrderBy(a => a.Name)
                .To<T>()
                .ToListAsync();
    }
}
