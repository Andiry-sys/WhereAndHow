using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Interfaces;

public interface IAuthService
{
    JwtSecurityToken GetToken(List<Claim> authClaims);
}
