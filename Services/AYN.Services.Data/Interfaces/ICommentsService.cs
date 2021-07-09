using System.Threading.Tasks;

using AYN.Web.ViewModels.Comments;

namespace AYN.Services.Data.Interfaces
{
    public interface ICommentsService
    {
        Task Create(string content, string adId, string userId);

        Task Delete(string commentId);

        Task Vote(string voteValue, string commentId, string userId);

        bool IsCommentExisting(string commentId);

        Task<T> GetByIdAsync<T>(string commentId);

        Task EditAsync(EditCommentInputModel input);
    }
}
