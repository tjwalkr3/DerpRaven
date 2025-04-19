using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface ICustomRequestService
    {
        Task<bool> ChangeStatusAsync(int id, string status);
        Task<bool> CreateCustomRequestAsync(CustomRequestDto dto);
        Task<List<CustomRequestDto>> GetAllCustomRequestsAsync();
        Task<CustomRequestDto?> GetCustomRequestByIdAsync(int id);
        Task<List<CustomRequestDto>> GetCustomRequestsByUserEmailAsync(string email);
    }
}