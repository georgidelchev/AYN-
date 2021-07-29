using System.Linq;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;

namespace AYN.Services.Data.Implementations
{
    public class WordsBlacklistService : IWordsBlacklistService
    {
        private readonly IDeletableEntityRepository<WordBlacklist> wordsBlacklistRepository;

        public WordsBlacklistService(
            IDeletableEntityRepository<WordBlacklist> wordsBlacklistRepository)
        {
            this.wordsBlacklistRepository = wordsBlacklistRepository;
        }

        public bool IsGivenWordInBlacklist(string word)
            => this.wordsBlacklistRepository
                .All()
                .Any(wb => wb.Content == word);
    }
}
