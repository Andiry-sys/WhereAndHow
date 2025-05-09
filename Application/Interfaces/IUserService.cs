using Core.Domain.DTOs;
using Data.DTOs;

namespace Application.Interfaces;
public interface IUserService
{
    Task<UserDTO?> GetUserByIdAsync(string id);
    Task<AuthResponseDto?> UpdateUserAsync(string id, UserUpdateDTO dto);
}
