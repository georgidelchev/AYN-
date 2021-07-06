using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;
using AYN.Services.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AYN.Services.Data.Implementations
{
    public class ImageService : IImageService
    {
        private readonly string[] allowedImageExtensions = { "jpg", "png", "jfif", "exif", "gif", "bmp", "ppm", "pgm", "pbm", "pnm", "heif", "bat" };
        private readonly IDeletableEntityRepository<Picture> pictureRepository;

        public ImageService(
            IDeletableEntityRepository<Picture> pictureRepository)
        {
            this.pictureRepository = pictureRepository;
        }

        public async Task CreateAsync(string id, string extension)
        {
            var picture = new Picture()
            {
                Extension = extension,
                AdId = id,
            };

            await this.pictureRepository.AddAsync(picture);
            await this.pictureRepository.SaveChangesAsync();
        }

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
