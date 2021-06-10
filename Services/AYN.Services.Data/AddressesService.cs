using System.Collections.Generic;
using System.Linq;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Services.Data
{
    public class AddressesService : IAddressesService
    {
        private readonly IDeletableEntityRepository<Address> addressesRepository;

        public AddressesService(IDeletableEntityRepository<Address> addressesRepository)
        {
            this.addressesRepository = addressesRepository;
        }

        public IEnumerable<T> GetAllByTownId<T>(int townId)
            => this.addressesRepository
                .All()
                .Where(a => a.Towns.All(t => t.Id == townId))
                .OrderBy(a => a.Name)
                .To<T>()
                .ToList();
    }
}
