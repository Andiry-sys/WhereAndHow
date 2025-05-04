using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Data.DTOs;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Service.Services;
internal class UserService(UserContext context,SignInManager<User> signInManager, IAuthService authService): IUserService
{
    private readonly UserContext _context = context;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IAuthService _authService = authService;

    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<AuthResponseDto?> UpdateUserAsync(string id, UserUpdateDTO dto)
    {
        var user = await _context.Users.FindAsync(id);
        if(user == null)
            return null;

        user.Name = dto.Name;
        user.SureName = dto.SureName;
        user.Email = dto.Email;
        user.Password = dto.Password; 
        user.PhoneNumber = dto.PhoneNumber;
        user.UserName = dto.Email;
        user.IsLosser = dto.IsLosser;

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        var principal = await _signInManager.CreateUserPrincipalAsync(user);
        ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("IsLosser", dto.IsLosser?.ToString() ?? "false"));

        var token = _authService.GetToken(principal.Claims.ToList());

        return new AuthResponseDto
        {
            IsAuthSuccessful = true,
            Token = token.ToString(),
        };
    }
}
