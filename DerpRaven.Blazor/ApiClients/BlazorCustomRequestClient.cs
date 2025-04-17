using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;
namespace DerpRaven.Blazor.ApiClients;

public class BlazorCustomRequestClient : ICustomRequestClient
{
    private readonly HttpClient _httpClient;

    public BlazorCustomRequestClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CustomRequestDto>?> GetAllCustomRequestsAsync()
    {
        var response = await _httpClient.GetAsync("api/CustomRequest");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<CustomRequestDto>>();
    }

    public async Task<bool> ChangeStatusAsync(int id, string status)
    {
        var response = await _httpClient.PatchAsync($"api/CustomRequest/{id}/status", new StringContent(status));
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CreateCustomRequestAsync(CustomRequestDto customRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByStatusAsync(string status)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByTypeAsync(string productType)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByUserEmailAsync()
    {
        throw new NotImplementedException();
    }
}
