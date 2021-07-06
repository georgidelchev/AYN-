using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AYN.Services.Data.Interfaces
{
    public interface IImageProcessingService
    {
        string GetImageExtension(IFormFile image);

        bool IsExtensionValid(string extension);

        Task SaveImageLocallyAsync(string fullPhysicalPath, int imageWidth, int imageHeight);
    }
}
