using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using AYN.Web.ViewModels.Administration.Emojis;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class EmojisService : IEmojisService
{
    private readonly IDeletableEntityRepository<Emoji> emojisRepository;

    public EmojisService(
        IDeletableEntityRepository<Emoji> emojisRepository)
    {
        this.emojisRepository = emojisRepository;
    }

    public async Task CreateAsync(CreateEmojiInputModel input)
    {
        var emoji = new Emoji()
        {
            Image = input.Emoji,
        };

        await this.emojisRepository.AddAsync(emoji);
        await this.emojisRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> GetAll<T>()
        => await this.emojisRepository
            .All()
            .To<T>()
            .ToListAsync();

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

    public async Task Delete(int id)
    {
        var emoji = this.emojisRepository
            .All()
            .FirstOrDefault(e => e.Id == id);

        this.emojisRepository.Delete(emoji);
        await this.emojisRepository.SaveChangesAsync();
    }

    public int Count()
        => this.emojisRepository
            .All()
            .Count();

    public bool IsExisting(string emoji)
        => this.emojisRepository
            .All()
            .Any(e => e.Image == emoji);
}
