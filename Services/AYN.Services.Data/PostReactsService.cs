using System;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Data.Models.Enumerations;

namespace AYN.Services.Data
{
    public class PostReactsService : IPostReactsService
    {
        private readonly IDeletableEntityRepository<PostReact> postReactsRepository;

        public PostReactsService(
            IDeletableEntityRepository<PostReact> postReactsRepository)
        {
            this.postReactsRepository = postReactsRepository;
        }

        public async Task SetReactAsync(int postId, string userId, int reactValue)
        {
            var react = this.postReactsRepository
                .AllAsNoTracking()
                .FirstOrDefault(v => v.PostId == postId &&
                          v.ApplicationUserId == userId);

            if (react != null)
            {
                react.ReactionType = (ReactionType)reactValue;

                this.postReactsRepository.Update(react);
                await this.postReactsRepository.SaveChangesAsync();

                return;
            }

            react = new PostReact()
            {
                ApplicationUserId = userId,
                PostId = postId,
                ReactionType = (ReactionType)reactValue,
            };

            await this.postReactsRepository.AddAsync(react);
            await this.postReactsRepository.SaveChangesAsync();
        }

        public int GetTotalReacts(int postId)
            => this.postReactsRepository
                .All()
                .Count(pr => pr.PostId == postId);
    }
}
