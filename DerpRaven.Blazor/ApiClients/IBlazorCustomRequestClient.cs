using DerpRaven.Shared.Dtos;

namespace DerpRaven.Blazor.ApiClients
{
    public interface IBlazorCustomRequestClient
    {
        Task<bool> ChangeStatusAsync(int id, string status);
        Task<List<CustomRequestDto>?> GetAllCustomRequestsAsync();
    }
}