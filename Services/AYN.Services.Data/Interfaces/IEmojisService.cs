using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IEmojisService
    {
        Task<IEnumerable<T>> GetAll<T>();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAll();

        Task Delete(int id);

        int Count();
    }
}
