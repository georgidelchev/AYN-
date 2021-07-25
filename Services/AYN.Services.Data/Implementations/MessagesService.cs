using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
{
    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;

        public MessagesService(
            IDeletableEntityRepository<Message> messagesRepository)
        {
            this.messagesRepository = messagesRepository;
        }

        public async Task CreateAsync(string content, string senderId, string receiverId)
        {
            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = receiverId,
            };

            await this.messagesRepository.AddAsync(message);
            await this.messagesRepository.SaveChangesAsync();
        }

        public async Task<string> GetLastActivityAsync(string currentUserId, string userId)
            => await this.messagesRepository.All()
                .Where(m => !m.IsDeleted &&
                            ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                             (m.ReceiverId == userId && m.SenderId == currentUserId)))
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.CreatedOn.ToString("g"))
                .FirstOrDefaultAsync();

        public async Task<string> GetLastMessageAsync(string currentUserId, string userId)
            => await this.messagesRepository.All()
                .Where(m => !m.IsDeleted &&
                            ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                             (m.ReceiverId == userId && m.SenderId == currentUserId)))
                .OrderByDescending(m => m.CreatedOn)
                .Select(m => m.Content)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetAllWithUserAsync<T>(string currentUserId, string userId)
        {
            var messages = this.messagesRepository.All()
                .Where(m => !m.IsDeleted &&
                            ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                             (m.ReceiverId == userId && m.SenderId == currentUserId)))
                .ToList();

            foreach (var message in messages)
            {
                message.IsRead = true;

                this.messagesRepository.Update(message);
                await this.messagesRepository.SaveChangesAsync();
            }

            return await this.messagesRepository.All()
                .Where(m => !m.IsDeleted &&
                            ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                             (m.ReceiverId == userId && m.SenderId == currentUserId)))
                .OrderBy(m => m.CreatedOn)
                .Include(a => a.Receiver)
                .Include(a => a.Sender)
                .To<T>()
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string currentUserId)
        {
            var sentMessages = this.messagesRepository.All()
                .Where(m => !m.IsDeleted &&
                            (m.SenderId == currentUserId || m.ReceiverId == currentUserId))
                .OrderByDescending(m => m.CreatedOn)
                .Include(a => a.Sender)
                .Select(m => m.Sender);

            var receivedMessages = this.messagesRepository.All()
                .Where(m => !m.IsDeleted &&
                            (m.SenderId == currentUserId || m.ReceiverId == currentUserId))
                .OrderByDescending(m => m.CreatedOn)
                .Include(a => a.Receiver)
                .Select(m => m.Receiver);

            var concatenatedMessages = await sentMessages
                .Concat(receivedMessages)
                .Where(u => u.Id != currentUserId)
                .Distinct()
                .To<T>()
                .ToListAsync();

            return concatenatedMessages;
        }

        public bool IsAllMessagesRead(string currentUserId, string userId)
            => this.messagesRepository
                .All()
                .Where(m => m.SenderId == currentUserId && m.ReceiverId == userId)
                .All(m => m.IsRead);

        public int GetUnreadMessagesCount(string userId)
            => this.messagesRepository
                .All()
                .Where(m => m.ReceiverId == userId && !m.IsRead)
                .GroupBy(m => m.SenderId)
                .Count();

    }

}
