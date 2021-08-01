using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AYN.Services.Data.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadPictureAsync(byte[] data, string fileName, string folderName, int? width, int? height);
    }
}
