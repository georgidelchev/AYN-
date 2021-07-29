namespace AYN.Services.Data.Interfaces
{
    public interface IWordsBlacklistService
    {
        bool IsGivenWordInBlacklist(string word);
    }
}
