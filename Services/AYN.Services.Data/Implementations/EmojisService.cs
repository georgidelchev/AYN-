using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations
{
    public class EmojisService : IEmojisService
    {
        private readonly IDeletableEntityRepository<Emoji> emojisRepository;

        public EmojisService(
            IDeletableEntityRepository<Emoji> emojisRepository)
        {
            this.emojisRepository = emojisRepository;
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetAll()
            => await this.emojisRepository
                .All()
                .Select(e => new
                {
                    e.Id,
                    e.Image,
                })
                .Select(e => new KeyValuePair<string, string>(e.Id.ToString(), e.Image))
                .ToListAsync();
    }
}
