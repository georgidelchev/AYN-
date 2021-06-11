using System.Threading.Tasks;

namespace AYN.Services.Data
{
    public interface IImageService
    {
        Task CreateAsync(string id, string extension);
    }
}
