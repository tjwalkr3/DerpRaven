using DerpRaven.Shared.Dtos;

namespace DerpRaven.Shared.ApiClients {
    public interface IUserClient {
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}