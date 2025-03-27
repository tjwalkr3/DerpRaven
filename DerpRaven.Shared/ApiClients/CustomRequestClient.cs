using System.Net.Http.Json;

namespace DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Dtos;

public class CustomRequestClient
{
    private readonly HttpClient _httpClient;
    public CustomRequestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CustomRequestDto>?> GetAllCustomRequestsAsync()
    {
        var response = await _httpClient.GetAsync("api/CustomRequest");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    public async Task<bool> CreateCustomRequestAsync(CustomRequestDto customRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/CustomRequest", customRequest);
        return response.IsSuccessStatusCode;
    }

    public async Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/CustomRequest/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CustomRequestDto>();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"api/CustomRequest/user/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByStatusAsync(string status)
    {
        var response = await _httpClient.GetAsync($"api/CustomRequest/status/{status}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }


    public async Task<List<CustomRequestDto>?> GetCustomRequestsByTypeAsync(string productType)
    {
        var response = await _httpClient.GetAsync($"api/CustomRequest/type/{productType}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    public async Task<bool> ChangeStatusAsync(int id, string status)
    {
        var response = await _httpClient.PatchAsync($"api/CustomRequest/{id}/status", new StringContent(status));
        return response.IsSuccessStatusCode;
    }
}
