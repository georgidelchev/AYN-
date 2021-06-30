using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IAddressesService
    {
        Task<IEnumerable<T>> GetAllByTownIdAsync<T>(int townId);
    }
}
