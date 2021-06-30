using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AYN.Services.Data.Interfaces
{
    public interface IImageProcessingService
    {
        string GetImageExtension(IFormFile image);

        void ValidateImageExtension(string extension);

        Task SaveImageLocallyAsync(string fullPhysicalPath, int imageWidth, int imageHeight);
    }
}
