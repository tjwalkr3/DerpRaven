using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;

namespace DerpRaven.Blazor.ApiClients
{
    public interface IBlazorImageClient
    {
        Task<bool> DeleteImageAsync(int id);
        Task<ImageDto?> GetImageInfoAsync(int id);
        Task<List<ImageDto>> GetImageInfoManyAsync(List<int> ids);
        Task<List<ImageDto>> ListImagesAsync();
        Task<bool> UploadImageAsync(IBrowserFile file, string description);
    }
}