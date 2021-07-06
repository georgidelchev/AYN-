using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AYN.Services.Data.Implementations
{
    public class ImageProcessingService : IImageProcessingService
    {
        private readonly string[] allowedImageExtensions = { "jpg", "png", "jfif", "exif", "gif", "bmp", "ppm", "pgm", "pbm", "pnm", "heif", "bat" };

        public string GetImageExtension(IFormFile image)
            => Path
                .GetExtension(image.FileName)
                .TrimStart('.')
                .TrimEnd()
                .ToLower();

        public bool IsExtensionValid(string extension)
            => this.allowedImageExtensions.Contains(extension);

        public async Task SaveImageLocallyAsync(string fullPhysicalPath, int imageWidth, int imageHeight)
        {
            using var image = await Image.LoadAsync(fullPhysicalPath);

            image.Mutate(ci => ci.Resize(imageWidth, imageHeight));

            await image.SaveAsync($"{fullPhysicalPath}");
        }
    }
}
