using DerpRaven.Api.Services;
using DerpRaven.Shared.Dtos;
using System.Security.Claims;

namespace DerpRaven.Api.Controllers;

public static class ControllerHelpers
{
    // This method takes in a ClaimsPrinciple (HttpContext.User) from the controller, and checks if 
    // the email is in the database with either the @snow.edu or @students.snow.edu domain.
    // It then uses the UserService to get the user by email and return it. 
    public static async Task<UserDto?> GetCurrentUser(ClaimsPrincipal user, IUserService userService)
    {
        if (user == null) return null;
        string? userEmail = user?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

        if (userEmail == null)
        {
            userEmail = user?.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            if (userEmail != null)
            {
                if (await userService.EmailExistsAsync(userEmail + "@snow.edu"))
                {
                    userEmail = userEmail + "@snow.edu";
                }
                else if (await userService.EmailExistsAsync(userEmail + "@students.snow.edu"))
                {
                    userEmail = userEmail + "@students.snow.edu";
                }
                else
                {
                    return null;
                }
            }
        }

        if (userEmail == null) return null;
        return await userService.GetUserByEmailAsync(userEmail);
    }
}
