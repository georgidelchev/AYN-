using System.Threading.Tasks;

using AYN.Web.ViewModels.Posts;

namespace AYN.Services.Data
{
    public interface IPostsService
    {
        Task CreateAsync(string title, string content, string userId);

        Task EditAsync(EditPostInputModel input);

        Task<T> GetById<T>(int id);

        Task DeleteAsync(int postId);
    }
}
