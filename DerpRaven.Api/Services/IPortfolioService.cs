﻿using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IPortfolioService
    {
        Task<bool> CreatePortfolioAsync(PortfolioDto dto);
        Task<bool> DeletePortfolioAsync(int id);
        Task<List<PortfolioDto>> GetAllPortfoliosAsync();
        Task<PortfolioDto?> GetPortfolioByIdAsync(int id);
        Task<List<PortfolioDto>> GetPortfoliosByNameAsync(string name);
        Task<List<PortfolioDto>> GetPortfoliosByTypeAsync(string productType);
        Task<bool> UpdatePortfolioAsync(PortfolioDto dto);
    }
}