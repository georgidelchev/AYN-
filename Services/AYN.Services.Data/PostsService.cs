using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Posts;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data
{
    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public PostsService(
            IDeletableEntityRepository<Post> postsRepository)
        {
            this.postsRepository = postsRepository;
        }

        public async Task<IEnumerable<T>> GetUserAllPostsAsync<T>(string userId)
            => await this.postsRepository
                .All()
                .Where(p => p.AddedByUserId == userId)
                .To<T>()
                .ToListAsync();

        public async Task CreateAsync(string title, string content, string userId)
        {
            var post = new Post()
            {
                AddedByUserId = userId,
                Content = content,
                Title = title,
            };

            await this.postsRepository.AddAsync(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task EditAsync(EditPostInputModel input)
        {
            var post = this.postsRepository
                .All()
                .FirstOrDefault(p => p.Id == input.Id);

            post.Title = input.Title;
            post.Content = input.Content;

            this.postsRepository.Update(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public async Task<T> GetById<T>(int id)
            => await this.postsRepository
                .All()
                .Where(p => p.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task DeleteAsync(int postId)
        {
            var post = this.postsRepository
                .All()
                .FirstOrDefault(p => p.Id == postId);

            this.postsRepository.Delete(post);
            await this.postsRepository.SaveChangesAsync();
        }

        public string GetTitleById(int postId)
            => this.postsRepository
                .All()
                .FirstOrDefault(p => p.Id == postId)
                ?.Title;
    }
}
