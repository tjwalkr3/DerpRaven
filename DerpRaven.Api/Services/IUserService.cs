﻿using DerpRaven.Shared.Dtos;

namespace DerpRaven.Api.Services
{
    public interface IUserService
    {
        Task<bool> CreateUserAsync(UserDto dto);
        Task<bool> EmailExistsAsync(string email);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(UserDto dto);
    }
}