using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;
namespace DerpRaven.Blazor.ApiClients;

public class BlazorPortfolioClient : IBlazorPortfolioClient
{
    private readonly HttpClient _httpClient;

    public BlazorPortfolioClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> CreatePortfolioAsync(PortfolioDto portfolio)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Portfolio", portfolio);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePortfolioAsync(PortfolioDto portfolio)
    {
        var response = await _httpClient.PutAsJsonAsync("api/Portfolio", portfolio);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }

    public async Task<List<PortfolioDto>> GetAllPortfoliosAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<List<PortfolioDto>>("api/portfolio");
        return response ?? [];
    }
}
