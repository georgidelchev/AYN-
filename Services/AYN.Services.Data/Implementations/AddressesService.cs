using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
{
    public class AddressesService : IAddressesService
    {
        private readonly IDeletableEntityRepository<Address> addressesRepository;
        private readonly ITownsService townsService;

        public AddressesService(
            IDeletableEntityRepository<Address> addressesRepository,
            ITownsService townsService)
        {
            this.addressesRepository = addressesRepository;
            this.townsService = townsService;
        }

        public async Task<IEnumerable<T>> GetAllByTownIdAsync<T>(int townId)
            => await this.addressesRepository
                .All()
                .Where(a => a.Towns.All(t => t.Id == townId))
                .OrderBy(a => a.Name)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllByTownIdAsKeyValuePairsAsync(int townId)
        {
            var town = this.townsService.GetById(townId);

            var a = await this.addressesRepository
                .All()
                .Where(a => a.Towns.Contains(town))
                .Select(sc => new
                {
                    sc.Id,
                    sc.Name,
                })
                .OrderBy(sc => sc.Name)
                .Select(sc => new KeyValuePair<string, string>(sc.Id.ToString(), sc.Name))
                .ToListAsync();
            return a;
        }
    }
}
