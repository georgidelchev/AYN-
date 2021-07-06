using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AYN.Services.Data.Interfaces
{
    public interface IImageService
    {
        Task CreateAsync(string id, string extension);

        string GetImageExtension(IFormFile image);

        bool IsExtensionValid(string extension);

        Task SaveImageLocallyAsync(string fullPhysicalPath, int imageWidth, int imageHeight);
    }
}
