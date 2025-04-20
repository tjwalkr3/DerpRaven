using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.ApiClients
{
    public interface IBlazorPortfolioClient
    {
        Task<bool> CreatePortfolioAsync(PortfolioDto portfolio);
        Task<List<PortfolioDto>> GetAllPortfoliosAsync();
        Task<bool> UpdatePortfolioAsync(PortfolioDto portfolio);
    }
}