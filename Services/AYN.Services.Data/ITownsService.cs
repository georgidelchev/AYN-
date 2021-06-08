using System.Collections.Generic;

namespace AYN.Services.Data
{
    public interface ITownsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}
