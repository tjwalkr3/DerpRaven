using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DerpRaven.Blazor.ApiClients;

public class BlazorImageClient
{
    private readonly HttpClient _httpClient;

    public BlazorImageClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> DeleteImageAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/image/delete/{id}");
        return response.IsSuccessStatusCode;
    }

    public string GetImageAddress(int id)
    {

        Console.WriteLine(_httpClient.BaseAddress);
        if (_httpClient.BaseAddress == null) 
        {
            Console.WriteLine("Base address is null");
            throw new InvalidOperationException("Base address is not set.");
        }
        Console.WriteLine(_httpClient.BaseAddress+ $"api/image/get/{id}");
        return _httpClient.BaseAddress + $"api/image/get/{id}";
    }

    public async Task<ImageDto?> GetImageInfoAsync(int id)
    {
        var response = await _httpClient.GetFromJsonAsync<ImageDto>($"api/image/info/{id}");
        return response;
    }

    public async Task<List<ImageDto>> GetImageInfoManyAsync(List<int> ids)
    {
        string path = $"api/image/info-many/?{string.Join("&", ids.Select(id => $"ids={id}"))}";
        var response = await _httpClient.GetFromJsonAsync<List<ImageDto>>(path);
        return response ?? [];
    }

    public async Task<List<ImageDto>> ListImagesAsync()
    {
        Console.WriteLine("We are in ImageAsync");
        var response = await _httpClient.GetFromJsonAsync<List<ImageDto>>("api/image/list");
        Console.WriteLine("We have gotten the json, returning now");
        return response ?? [];
    }

    public async Task<bool> UploadImageAsync(IBrowserFile file, string description)
    {
        var content = new MultipartFormDataContent
        {
            { new StreamContent(file.OpenReadStream(10_000_000)), "File", file.Name }, // 10 MB limit
            { new StringContent(description), "Description" }
        };

        var response = await _httpClient.PostAsync("api/image/upload", content);
        return response.IsSuccessStatusCode;
    }
}
