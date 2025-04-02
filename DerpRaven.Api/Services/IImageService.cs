using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IImageService
    {
        Task<bool> DeleteImageAsync(int id);
        Task<string> GetFileName(int id);
        Task<Stream?> GetImageAsync(int id);
        Task<ImageDto?> GetImageInfoAsync(int id);
        Task<List<ImageDto>> ListImagesAsync();
        Task<bool> UpdateImageDescriptionAsync(int id, string alt);
        Task<bool> UploadImageAsync(string fileName, string alt, Stream stream);
    }
}