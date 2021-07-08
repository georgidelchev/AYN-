using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;

namespace AYN.Services.Data.Implementations
{
    public class ChatConversationsService : IChatConversationsService
    {
        private readonly IDeletableEntityRepository<ChatConversation> chatConversationsRepository;

        public ChatConversationsService(
            IDeletableEntityRepository<ChatConversation> chatConversationsRepository)
        {
            this.chatConversationsRepository = chatConversationsRepository;
        }

        public async Task CreateConversationAsync(string senderId, string receiverId)
        {
            var chatConversation = new ChatConversation()
            {
                ReceiverId = receiverId,
                SenderId = senderId,
            };

            await this.chatConversationsRepository.AddAsync(chatConversation);
            await this.chatConversationsRepository.SaveChangesAsync();
        }
    }
}
