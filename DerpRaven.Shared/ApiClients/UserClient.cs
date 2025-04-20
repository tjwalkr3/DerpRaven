using DerpRaven.Shared.Authentication;
using DerpRaven.Shared.Dtos;
using System.Net.Http.Json;


namespace DerpRaven.Shared.ApiClients;
public class UserClient(IApiService apiService) : IUserClient
{

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        var response = await apiService.GetAsync($"api/User/email/{email}");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserDto>();
    }
}

