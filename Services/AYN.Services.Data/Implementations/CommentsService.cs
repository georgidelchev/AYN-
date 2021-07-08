using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;

namespace AYN.Services.Data.Implementations
{
    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentsService(
            IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task Create(string content, string adId, string userId)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            var comment = new Comment()
            {
                AdId = adId,
                AddedByUserId = userId,
                Content = content,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }
    }
}
