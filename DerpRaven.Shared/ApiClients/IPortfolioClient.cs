using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients
{
    public interface IPortfolioClient
    {
        Task<bool> CreatePortfolioAsync(PortfolioDto portfolio);
        Task<bool> DeletePortfolioAsync(int id);
        Task<List<PortfolioDto>> GetAllPortfoliosAsync();
        Task<PortfolioDto?> GetPortfolioByIdAsync(int id);
        Task<List<PortfolioDto>> GetPortfoliosByNameAsync(string name);
        Task<List<PortfolioDto>> GetPortfoliosByTypeAsync(string productType);
        Task<bool> UpdatePortfolioAsync(int id, PortfolioDto portfolio);
    }
}