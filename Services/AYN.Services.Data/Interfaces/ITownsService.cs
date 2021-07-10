using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Data.Models;

namespace AYN.Services.Data.Interfaces
{
    public interface ITownsService
    {
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        int GetIdByName(string townName);

        bool IsExisting(int townId);

        bool IsTownContainsGivenAddress(int townId, int addressId);

        Town GetById(int id);
    }
}
