using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace WhereAndHow.Server.Controllers;

[ApiController]
[Route("api/partner")]
public class PartnerController(IPartnerService partnerService) : ControllerBase
{
    private readonly IPartnerService _partnerService = partnerService;

    /// <summary>
    /// Authenticated user requests to become a partner.
    /// Rate-limited to 3 requests per user per hour.
    /// </summary>
    [Authorize]
    [HttpPost("request")]
    [EnableRateLimiting("partner-request")]
    public async Task<IActionResult> RequestPartner()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
            return Unauthorized();

        var (success, message) = await _partnerService.RequestPartnerAsync(userId);

        return success
            ? Ok(new { message })
            : BadRequest(new { message });
    }

    /// <summary>
    /// Admin confirmation link — returns an HTML page, not JSON.
    /// No authentication required (admin arrives via email link).
    /// </summary>
    [HttpGet("confirm")]
    public async Task<ContentResult> ConfirmPartner([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return Content(
                "<h1>Missing token</h1><p>No confirmation token was provided.</p>",
                "text/html");
        }

        var html = await _partnerService.ConfirmPartnerAsync(token);
        return Content(html, "text/html");
    }
}
