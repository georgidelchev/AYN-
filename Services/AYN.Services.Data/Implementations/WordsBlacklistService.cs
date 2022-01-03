using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using AYN.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace AYN.Services.Data.Implementations;

public class WordsBlacklistService : IWordsBlacklistService
{
    private readonly IDeletableEntityRepository<WordBlacklist> wordsBlacklistRepository;

    public WordsBlacklistService(
        IDeletableEntityRepository<WordBlacklist> wordsBlacklistRepository)
    {
        this.wordsBlacklistRepository = wordsBlacklistRepository;
    }

    public async Task<IEnumerable<T>> AllAsync<T>()
        => await this.wordsBlacklistRepository
            .All()
            .OrderByDescending(wb => wb.CreatedOn)
            .To<T>()
            .ToListAsync();

    public bool IsGivenWordInBlacklist(string word)
        => this.wordsBlacklistRepository
            .All()
            .Any(wb => wb.Content == word);

    public int Count()
        => this.wordsBlacklistRepository
            .All()
            .Count();

    public async Task DeleteAsync(int id)
    {
        var word = this.wordsBlacklistRepository
            .All()
            .FirstOrDefault(w => w.Id == id);

        this.wordsBlacklistRepository.Delete(word);
        await this.wordsBlacklistRepository.SaveChangesAsync();
    }
}
