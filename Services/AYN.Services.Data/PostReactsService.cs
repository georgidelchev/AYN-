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
        private readonly INotificationsService notificationsService;
        private readonly IPostsService postsService;

        public PostReactsService(
            IDeletableEntityRepository<PostReact> postReactsRepository,
            INotificationsService notificationsService,
            IPostsService postsService)
        {
            this.postReactsRepository = postReactsRepository;
            this.notificationsService = notificationsService;
            this.postsService = postsService;
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

            await this.notificationsService.CreateAsync($"You've got new {(ReactionType)reactValue} reaction to your post!", $"/Users/Profile?id={userId}#{postId}", userId);
        }

        public int GetTotalReacts(int postId)
            => this.postReactsRepository
                .All()
                .Count(pr => pr.PostId == postId);
    }
}
