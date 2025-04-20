using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients
{
    public interface ICustomRequestClient
    {
        Task<bool> ChangeStatusAsync(int id, string status);
        Task<bool> CreateCustomRequestAsync(CustomRequestDto customRequest);
        Task<List<CustomRequestDto>?> GetAllCustomRequestsAsync();
        Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id);
        Task<List<CustomRequestDto>?> GetCustomRequestsByUserEmailAsync();
    }
}