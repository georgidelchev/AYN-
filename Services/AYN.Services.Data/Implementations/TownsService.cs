using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class TownsService : ITownsService
{
    private readonly IDeletableEntityRepository<Town> townsRepository;

    public TownsService(
        IDeletableEntityRepository<Town> townsRepository)
    {
        this.townsRepository = townsRepository;
    }

    public async Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync()
        => await this.townsRepository
            .All()
            .Select(t => new
            {
                t.Id,
                t.Name,
            })
            .OrderBy(t => t.Name)
            .Select(t => new KeyValuePair<string, string>(t.Id.ToString(), t.Name))
            .ToListAsync();

    public int GetIdByName(string townName)
        => this.townsRepository
            .All()
            .FirstOrDefault(t => t.Name.ToLower() == townName.ToLower())
            .Id;

    public bool IsExisting(int townId)
        => this.townsRepository
            .All()
            .Any(t => t.Id == townId);

    public bool IsTownContainsGivenAddress(int townId, int addressId)
    {
        var town = this.townsRepository
            .All()
            .Include(t => t.Addresses)
            .FirstOrDefault(t => t.Id == townId);

        return town.Addresses.Any(a => a.Id == addressId);
    }

    public Town GetById(int id)
        => this.townsRepository
            .All()
            .FirstOrDefault(t => t.Id == id);
}
