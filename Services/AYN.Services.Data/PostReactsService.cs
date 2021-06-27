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
            //if (this.postReactsRepository
            //    .AllAsNoTracking()
            //    .Any(v => v.PostId == postId &&
            //              v.ApplicationUserId == userId))
            //{
            //    return;
            //}

            var react = new PostReact()
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
