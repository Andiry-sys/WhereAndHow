using Application.Interfaces;
using Core.Domain.Enums;
using Core.Domain.Models;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Service.Services;

internal class PartnerService(
    UserContext context,
    UserManager<User> userManager,
    IEmailSender emailSender,
    IConfiguration configuration,
    ILogger<PartnerService> logger
) : IPartnerService
{
    private const string PartnerClaimType = "partner_request";

    // -------------------------------------------------------------------------
    // Public API
    // -------------------------------------------------------------------------

    public async Task<(bool Success, string Message)> RequestPartnerAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return (false, "User not found.");

        if (user.IsLosser == true)
            return (false, "User is already a partner.");

        // Allow only one pending request per user
        var hasPending = await context.PartnerRequests
            .AnyAsync(r => r.UserId == userId && r.Status == PartnerRequestStatus.Pending);

        if (hasPending)
            return (false, "A partner request is already pending for this user.");

        var request = new PartnerRequest { UserId = userId };
        context.PartnerRequests.Add(request);
        await context.SaveChangesAsync();

        logger.LogInformation("Partner request {RequestId} created for user {UserId}.", request.Id, userId);

        var token = GenerateConfirmationToken(request.Id, userId);
        var baseUrl = configuration["Partner:BaseUrl"]?.TrimEnd('/') ?? "https://localhost:5293";
        var confirmUrl = $"{baseUrl}/api/partner/confirm?token={token}";
        var adminEmail = configuration["Partner:AdminEmail"] ?? throw new InvalidOperationException("Partner:AdminEmail is not configured.");

        var html = BuildAdminEmail(user, confirmUrl);
        await emailSender.SendHtmlEmailAsync(adminEmail, "New Partner Request — Where & How", html);

        logger.LogInformation("Partner confirmation email sent to {AdminEmail} for request {RequestId}.", adminEmail, request.Id);

        return (true, "Partner request submitted. An administrator will review it shortly.");
    }

    public async Task<string> ConfirmPartnerAsync(string token)
    {
        var (requestId, userId, error) = ValidateToken(token);
        if (error is not null)
        {
            logger.LogWarning("Invalid partner confirmation token: {Error}", error);
            return BuildHtmlPage("Confirmation Failed", error, success: false);
        }

        var request = await context.PartnerRequests
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request is null)
            return BuildHtmlPage("Confirmation Failed", "Request not found.", success: false);

        if (request.Status == PartnerRequestStatus.Confirmed)
            return BuildHtmlPage("Already Confirmed", $"{request.User?.Name} is already a partner.", success: true);

        if (request.Status != PartnerRequestStatus.Pending)
            return BuildHtmlPage("Confirmation Failed", "This request can no longer be confirmed.", success: false);

        // Activate partner status
        var user = await userManager.FindByIdAsync(userId!);
        if (user is null)
            return BuildHtmlPage("Confirmation Failed", "User not found.", success: false);

        user.IsLosser = true;
        var updateResult = await userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            logger.LogError("Failed to update user {UserId}: {Errors}", userId, string.Join(", ", updateResult.Errors.Select(e => e.Description)));
            return BuildHtmlPage("Confirmation Failed", "Could not update the user record. Please try again.", success: false);
        }

        request.Status = PartnerRequestStatus.Confirmed;
        request.ConfirmedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();

        logger.LogInformation("Partner request {RequestId} confirmed. User {UserId} is now a partner.", requestId, userId);

        return BuildHtmlPage(
            "Partner Confirmed",
            $"{user.Name} {user.SureName} has been successfully activated as a partner.",
            success: true);
    }

    // -------------------------------------------------------------------------
    // Token helpers
    // -------------------------------------------------------------------------

    private string GenerateConfirmationToken(string requestId, string userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
        var expiryHours = int.TryParse(configuration["Partner:TokenExpiryHours"], out var h) ? h : 24;

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, requestId),
                new Claim(PartnerClaimType, "true")
            ]),
            Expires = DateTime.UtcNow.AddHours(expiryHours),
            Issuer = configuration["JWT:Issuer"],
            Audience = configuration["JWT:Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        };

        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(handler.CreateToken(descriptor));
    }

    private (string? RequestId, string? UserId, string? Error) ValidateToken(string token)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!));
        var handler = new JwtSecurityTokenHandler();

        try
        {
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JWT:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            var partnerClaim = principal.FindFirstValue(PartnerClaimType);
            if (partnerClaim != "true")
                return (null, null, "Token is not a partner confirmation token.");

            var requestId = principal.FindFirstValue(JwtRegisteredClaimNames.Jti);
            var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (string.IsNullOrWhiteSpace(requestId) || string.IsNullOrWhiteSpace(userId))
                return (null, null, "Token is missing required claims.");

            return (requestId, userId, null);
        }
        catch (SecurityTokenExpiredException)
        {
            return (null, null, "The confirmation link has expired. Please submit a new partner request.");
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Token validation failed.");
            return (null, null, "Invalid or tampered confirmation token.");
        }
    }

    // -------------------------------------------------------------------------
    // Email / HTML builders
    // -------------------------------------------------------------------------

    private static string BuildAdminEmail(User user, string confirmUrl) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="UTF-8"/>
          <style>
            body {{ font-family: Arial, sans-serif; background:#f4f4f4; padding:20px; }}
            .card {{ background:#fff; border-radius:8px; padding:32px; max-width:560px; margin:auto; }}
            h2 {{ color:#333; }}
            table {{ width:100%; border-collapse:collapse; margin:20px 0; }}
            td {{ padding:8px 12px; border-bottom:1px solid #eee; }}
            td:first-child {{ font-weight:bold; color:#555; width:40%; }}
            .btn {{ display:inline-block; padding:14px 28px; background:#2563eb; color:#fff;
                    text-decoration:none; border-radius:6px; font-size:16px; margin-top:24px; }}
            .footer {{ font-size:12px; color:#999; margin-top:24px; }}
          </style>
        </head>
        <body>
          <div class="card">
            <h2>New Partner Request</h2>
            <p>A user has requested partner status on <strong>Where &amp; How</strong>.</p>
            <table>
              <tr><td>Full Name</td><td>{user.Name} {user.SureName}</td></tr>
              <tr><td>Email</td><td>{user.Email}</td></tr>
              <tr><td>Phone</td><td>{user.PhoneNumber ?? "—"}</td></tr>
              <tr><td>User ID</td><td>{user.Id}</td></tr>
              <tr><td>Requested at</td><td>{DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC</td></tr>
            </table>
            <p>Click the button below to approve this request. The link is valid for 24 hours.</p>
            <a href="{confirmUrl}" class="btn">Confirm Partner</a>
            <p class="footer">
              If you did not expect this email, please ignore it.<br/>
              Do not share this link — it grants partner privileges.
            </p>
          </div>
        </body>
        </html>
        """;

    private static string BuildHtmlPage(string title, string message, bool success) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
          <meta charset="UTF-8"/>
          <title>{title}</title>
          <style>
            body {{ font-family:Arial,sans-serif; background:#f4f4f4; display:flex;
                    align-items:center; justify-content:center; min-height:100vh; margin:0; }}
            .card {{ background:#fff; border-radius:8px; padding:48px 40px; max-width:480px;
                    text-align:center; box-shadow:0 2px 12px rgba(0,0,0,.1); }}
            .icon {{ font-size:56px; }}
            h1 {{ color:{(success ? "#16a34a" : "#dc2626")}; margin:16px 0 8px; }}
            p {{ color:#555; font-size:15px; }}
          </style>
        </head>
        <body>
          <div class="card">
            <div class="icon">{(success ? "✅" : "❌")}</div>
            <h1>{title}</h1>
            <p>{message}</p>
          </div>
        </body>
        </html>
        """;
}
