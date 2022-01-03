using System.Collections.Generic;

namespace AYN.Services.Data.Interfaces;

public interface ISettingsService
{
    int GetCount();

    IEnumerable<T> GetAll<T>();
}
