using DerpRaven.Api.Dtos;

namespace DerpRaven.Api.Services
{
    public interface ICustomRequestService
    {
        Task<bool> ChangeStatusAsync(int id, string status);
        Task<bool> CreateCustomRequestAsync(CustomRequestDto dto);
        Task<List<CustomRequestDto>> GetAllCustomRequestsAsync();
        Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id);
        Task<IEnumerable<CustomRequestDto>> GetCustomRequestsByStatusAsync(string status);
        Task<IEnumerable<CustomRequestDto>> GetCustomRequestsByTypeAsync(string productType);
        Task<IEnumerable<CustomRequestDto>> GetCustomRequestsByUserIdAsync(int id);
    }
}