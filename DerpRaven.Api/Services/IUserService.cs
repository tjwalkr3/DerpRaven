using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDto dto);
        Task<bool> EmailExistsAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<List<UserDto>> GetUsersByNameAsync(string name);
        Task<List<UserDto>> GetUsersByStatusAsync(bool active);
        Task<bool> UpdateUserAsync(UserDto dto);
    }
}