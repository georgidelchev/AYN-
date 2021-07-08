using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IChatConversationsService
    {
        Task CreateConversationAsync(string senderId, string receiverId);
    }
}
