using System.Collections.Generic;

namespace AYN.Services.Data
{
    public interface IAddressesService
    {
        IEnumerable<T> GetAllByTownId<T>(int townId);
    }
}
