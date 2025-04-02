using System.Net.Http.Json;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients;


public class CustomRequestClient(IApiService apiService) : ICustomRequestClient
{
    // needs authentication
    public async Task<List<CustomRequestDto>?> GetAllCustomRequestsAsync()
    {
        var response = await apiService.GetAsync("api/CustomRequest");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    // needs authentication
    public async Task<bool> CreateCustomRequestAsync(CustomRequestDto customRequest)
    {
        var response = await apiService.PostAsJsonAsync("api/CustomRequest", customRequest);
        return response.IsSuccessStatusCode;
    }

    // needs authentication
    public async Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id)
    {
        var response = await apiService.GetAsync($"api/CustomRequest/{id}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<CustomRequestDto>();
    }

    // needs authentication
    public async Task<List<CustomRequestDto>?> GetCustomRequestsByUserAsync(int userId)
    {
        var response = await apiService.GetAsync($"api/CustomRequest/user/{userId}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    // needs authentication
    public async Task<List<CustomRequestDto>?> GetCustomRequestsByStatusAsync(string status)
    {
        var response = await apiService.GetAsync($"api/CustomRequest/status/{status}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    // needs authentication
    public async Task<List<CustomRequestDto>?> GetCustomRequestsByTypeAsync(string productType)
    {
        var response = await apiService.GetAsync($"api/CustomRequest/type/{productType}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    // needs authentication
    public async Task<bool> ChangeStatusAsync(int id, string status)
    {
        var response = await apiService.PatchAsync($"api/CustomRequest/{id}/status", new StringContent(status));
        return response.IsSuccessStatusCode;
    }
}
