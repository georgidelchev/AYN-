using System.Collections.Generic;

namespace AYN.Services.Data
{
    public interface ISettingsService
    {
        int GetCount();

        IEnumerable<T> GetAll<T>();
    }
}
