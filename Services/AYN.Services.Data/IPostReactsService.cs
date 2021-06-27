using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface IPostReactsService
    {
        Task SetReactAsync(int postId, string userId, int reactValue);

        int GetTotalReacts(int postId);
    }
}
