using System;
using System.Threading.Tasks;

namespace AYN.Data.Common
{
    public interface IDbQueryRunner : IDisposable
    {
        Task RunQueryAsync(string query, params object[] parameters);
    }
}
