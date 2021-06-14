using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface ITownsService
    {
        Task<IEnumerable<KeyValuePair<string, string>>> GetAllAsKeyValuePairsAsync();

        int GetIdByName(string townName);
    }
}
