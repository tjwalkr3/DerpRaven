using DerpRaven.Shared.ApiClients;
using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;
namespace DerpRaven.Blazor.ApiClients;

public class BlazorCustomRequestClient : IBlazorCustomRequestClient
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
        var response = await _httpClient.PatchAsync($"api/CustomRequest/{id}/status", JsonContent.Create(status));
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
}
