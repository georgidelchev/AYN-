using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Mapping;
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

        public async Task<ICollection<T>> GetAllPostsByUserId<T>(string userId)
            => await this.postsRepository
                .All()
                .Where(p => p.AddedByUserId == userId)
                .Include(a => a.ApplicationUser)
                .To<T>()
                .ToListAsync();
    }
}
