using System.Collections.Generic;
using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces;

public interface IWordsBlacklistService
{
    Task<IEnumerable<T>> AllAsync<T>();

    bool IsGivenWordInBlacklist(string word);

    int Count();

    Task DeleteAsync(int id);
}
