using System.Collections.Generic;
using System.Linq;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;

namespace AYN.Services.Data
{
    public class TownsService : ITownsService
    {
        private readonly IDeletableEntityRepository<Town> townsRepository;

        public TownsService(
            IDeletableEntityRepository<Town> townsRepository)
        {
            this.townsRepository = townsRepository;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs()
            => townsRepository
                .All()
                .Select(t => new
                {
                    t.Id,
                    t.Name,
                })
                .ToList()
                .OrderBy(t => t.Name)
                .Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.Name));

        public int GetIdByName(string townName)
            => townsRepository
                .All()
                .FirstOrDefault(t => t.Name == townName)
                .Id;
    }
}
