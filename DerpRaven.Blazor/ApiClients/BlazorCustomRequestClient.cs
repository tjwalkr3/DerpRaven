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
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public async Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByStatusAsync(string status)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByTypeAsync(string productType)
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }

    public async Task<List<CustomRequestDto>?> GetCustomRequestsByUserEmailAsync()
    {
        await Task.Delay(1000);
        throw new NotImplementedException();
    }
}
