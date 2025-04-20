using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace DerpRaven.Shared.ApiClients;

public class PortfolioClient(IApiService apiService) : IPortfolioClient
{
    // does not need authentication
    public async Task<List<PortfolioDto>> GetAllPortfoliosAsync()
    {
        var response = await apiService.GetFromJsonAsyncWithoutAuthorization<List<PortfolioDto>>("api/portfolio");
        return response ?? [];
    }

    // does not need authentication
    public async Task<PortfolioDto?> GetPortfolioByIdAsync(int id)
    {
        var response = await apiService.GetFromJsonAsyncWithoutAuthorization<PortfolioDto>($"api/portfolio/{id}");
        return response;
    }

    // needs authentication
    public async Task<bool> CreatePortfolioAsync(PortfolioDto portfolio)
    {
        var response = await apiService.PostAsJsonAsync("api/portfolio", portfolio);
        return response.IsSuccessStatusCode;
    }

    // needs authentication
    public async Task<bool> UpdatePortfolioAsync(int id, PortfolioDto portfolio)
    {
        var response = await apiService.PutAsJsonAsync<PortfolioDto>($"api/portfolio/{id}", portfolio);
        return response.IsSuccessStatusCode;
    }

    // needs authentication
    public async Task<bool> DeletePortfolioAsync(int id)
    {
        var response = await apiService.DeleteAsync($"api/portfolio/{id}");
        return response.IsSuccessStatusCode;
    }
}