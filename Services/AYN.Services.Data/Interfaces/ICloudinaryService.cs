using System.Threading.Tasks;

namespace AYN.Services.Data.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadPictureAsync(byte[] data, string fileName, string folderName, int? width, int? height);
    }
}
