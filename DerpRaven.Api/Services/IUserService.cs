using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<List<UserDto>> GetUsersByEmailAsync(string email);
        Task<List<UserDto>> GetUsersByNameAsync(string name);
        Task<List<UserDto>> GetUsersByStatusAsync(bool active);
        Task<bool> UpdateUserAsync(UserDto dto);
    }
}