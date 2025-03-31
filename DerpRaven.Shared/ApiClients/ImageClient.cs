using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace DerpRaven.Shared.ApiClients;

public class ImageClient(IApiService apiService) : IImageClient
{
    // needs authentication
    public async Task<bool> UploadImageAsync(IBrowserFile file, string description)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream(10_000_000)), "File", file.Name }, // 10 MB limit
            { new StringContent(description), "Description" }
        };

        var response = await apiService.PostAsync("api/image/upload", content);
        return response.IsSuccessStatusCode;
    }

    // does not need authentication
    public async Task<List<ImageDto>> ListImagesAsync()
    {
        var response = await apiService.GetFromJsonAsyncWithoutAuthorization<List<ImageDto>>("api/image/list");
        return response ?? [];
    }

    // does not need authentication
    public async Task<byte[]?> GetImageAsync(int id)
    {
        var response = await apiService.GetAsyncWithoutAuthorization($"api/image/get/{id}");

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        return null;
    }

    // needs authentication
    public async Task<bool> DeleteImageAsync(int id)
    {
        var response = await apiService.DeleteAsync($"api/image/delete/{id}");
        return response.IsSuccessStatusCode;
    }
}
