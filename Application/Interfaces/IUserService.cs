using Core.Domain.DTOs;
using Core.Domain.Models;
using Data.DTOs;

namespace Application.Interfaces;
public interface IUserService
{
    Task<User?> GetUserByIdAsync(string id);
    Task<AuthResponseDto?> UpdateUserAsync(string id, UserUpdateDTO dto);
}
