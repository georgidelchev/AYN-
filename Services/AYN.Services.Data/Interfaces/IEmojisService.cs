using System.Collections.Generic;
using System.Threading.Tasks;

using AYN.Web.ViewModels.Administration.Emojis;

namespace AYN.Services.Data.Interfaces
{
    public interface IEmojisService
    {
        Task CreateAsync(CreateEmojiInputModel input);

        Task<IEnumerable<T>> GetAll<T>();

        Task<IEnumerable<KeyValuePair<string, string>>> GetAll();

        Task Delete(int id);

        int Count();

        bool IsExisting(string emoji);
    }
}
