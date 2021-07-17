using System.IO;
using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace AYN.Services.Data.Implementations
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinaryUtility;

        public CloudinaryService(Cloudinary cloudinaryUtility)
        {
            this.cloudinaryUtility = cloudinaryUtility;
        }

        //public async Task<string> UploadPictureAsync(IFormFile pictureFile, string fileName, string folderName, int? width, int? height)
        //{
        //    byte[] destinationData;

        //    await using (var ms = new MemoryStream())
        //    {
        //        await pictureFile.CopyToAsync(ms);
        //        destinationData = ms.ToArray();
        //    }

        //    UploadResult uploadResult = null;

        //    await using (var ms = new MemoryStream(destinationData))
        //    {
        //        var uploadParams = new ImageUploadParams
        //        {
        //            Folder = folderName,
        //            File = new FileDescription(fileName, ms),
        //            AllowedFormats = new[] { "jpg", "png", "jfif", "exif", "gif", "bmp", "ppm", "pgm", "pbm", "pnm", "heif", "bat" },
        //            Format = "jpg",
        //            Overwrite = true,
        //            Transformation = new Transformation().Width(width).Height(height).Crop("scale"),
        //        };

        //        uploadResult = this.cloudinaryUtility.Upload(uploadParams);
        //    }

        //    return uploadResult?.SecureUri.AbsoluteUri;
        //}


        public async Task<string> UploadPictureAsync(byte[] data, string fileName, string folderName, int? width, int? height)
        {
            await using (var ms = new MemoryStream())
            {
                //await pictureFile.CopyToAsync(ms);
                //destinationData = ms.ToArray();
            }

            UploadResult uploadResult = null;

            await using (var ms = new MemoryStream(data))
            {
                var uploadParams = new ImageUploadParams
                {
                    Folder = folderName,
                    File = new FileDescription(fileName, ms),
                    AllowedFormats = new[] { "jpg", "png", "jfif", "exif", "gif", "bmp", "ppm", "pgm", "pbm", "pnm", "heif", "bat" },
                    Format = "jpg",
                    Overwrite = true,
                    Transformation = new Transformation().Width(width).Height(height).Crop("scale"),
                };

                uploadResult = this.cloudinaryUtility.Upload(uploadParams);
            }

            return uploadResult?.SecureUri.AbsoluteUri;
        }
    }
}
