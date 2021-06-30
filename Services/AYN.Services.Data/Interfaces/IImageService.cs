using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface IImageService
    {
        Task CreateAsync(string id, string extension);
    }
}
