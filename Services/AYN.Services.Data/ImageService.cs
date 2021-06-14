using System.Threading.Tasks;

using AYN.Data.Common.Repositories;
using AYN.Data.Models;

namespace AYN.Services.Data
{
    public class ImageService : IImageService
    {
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
    }
}
