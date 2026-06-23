using Application.Interfaces;
using Core.Domain.DTOs;
using Core.Domain.Models;
using Data.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WhereAndHow.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthenticateController(UserManager<User> userManager,SignInManager<User> signInManager, IAuthService authService): ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IAuthService _authService = authService;

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);
        if(!signInResult.Succeeded)
        {
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });
        }

        var user = await _signInManager.UserManager.FindByNameAsync(model.Email);
        var authClaims = await _signInManager.CreateUserPrincipalAsync(user);
        ((ClaimsIdentity)authClaims.Identity).AddClaim(new Claim("IsLosser", user.IsLosser.ToString()));

        var token = _authService.GetToken(authClaims.Claims.ToList());

        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }


    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserSignUpDTO model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if(userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        User user = new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = model.Email,
            PhoneNumber = model.PhoneNumber,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Email,
            SureName = model.SureName,
            Name = model.Name,
            IsLosser = false
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if(!result.Succeeded)
        {
            return Unauthorized(result.Errors);
        }
        var authClaims = await _signInManager.CreateUserPrincipalAsync(user);
        ((ClaimsIdentity)authClaims.Identity).AddClaim(new Claim("IsLosser", user.IsLosser.ToString()));

        var token = _authService.GetToken(authClaims.Claims.ToList());


        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }

    [HttpGet]
    [Route("refresh")]
    [Authorize]
    public async Task<IActionResult> RefreshToken()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null)
            return Unauthorized(new AuthResponseDto { ErrorMessage = "User not found" });

        var authClaims = await _signInManager.CreateUserPrincipalAsync(user);
        ((ClaimsIdentity)authClaims.Identity!).AddClaim(new Claim("IsLosser", user.IsLosser.ToString()));

        var token = _authService.GetToken(authClaims.Claims.ToList());
        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }
}
