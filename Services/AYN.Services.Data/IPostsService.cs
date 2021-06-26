using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface IPostsService
    {
        Task CreateAsync(string title, string content, string userId);

        Task<ICollection<T>> GetAllPostsByUserId<T>(string userId);
    }
}
