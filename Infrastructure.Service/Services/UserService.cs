using System.Security.Claims;
using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Data.DTOs;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service.Services;

internal class UserService(
    UserContext context,
    SignInManager<User> signInManager,
    IAuthService authService,
    UserManager<User> userManager
) : IUserService
{
    private readonly UserContext _context = context;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IAuthService _authService = authService;
    private readonly UserManager<User> _userManager = userManager;

    public async Task<UserDTO> GetUserByIdAsync(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if(user == null)
            return new();
        return new UserDTO { IsLosser = user.IsLosser, Name = user.Name, SureName = user.SureName, Email = user.Email, PhoneNumber = user.PhoneNumber, Id = user.Id };
    }

    public async Task<AuthResponseDto?> UpdateUserAsync(string id, UserUpdateDTO dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return null;

        user.Name = dto.Name;
        user.SureName = dto.SureName;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.UserName = dto.Email;


        if (!string.IsNullOrEmpty(dto.Password))
        {
            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (!removeResult.Succeeded)
                    return null;
            }

            var addResult = await _userManager.AddPasswordAsync(user, dto.Password);
            if (!addResult.Succeeded)
                return null;
        }

        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            return null;

        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        ((ClaimsIdentity)principal.Identity).AddClaim(
            new Claim("IsLosser", user.IsLosser.ToString())
        );

        var token = _authService.GetToken(principal.Claims.ToList());

        return new AuthResponseDto { IsAuthSuccessful = true, Token = token.ToString(), };
    }
}
