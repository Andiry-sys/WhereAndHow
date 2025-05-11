using System.Security.Claims;

namespace Application.Interfaces;

public interface IAuthService
{
    string GetToken(List<Claim> authClaims);
}
