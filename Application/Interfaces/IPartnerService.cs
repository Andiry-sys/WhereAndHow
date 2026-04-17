namespace Application.Interfaces;

public interface IPartnerService
{
    /// <summary>
    /// Creates a pending partner request, generates a signed confirmation token,
    /// and sends an email to the configured admin address.
    /// </summary>
    Task<(bool Success, string Message)> RequestPartnerAsync(string userId);

    /// <summary>
    /// Validates the confirmation token, marks the user as a partner and the
    /// request as confirmed. Returns an HTML page suitable for direct browser display.
    /// </summary>
    Task<string> ConfirmPartnerAsync(string token);
}
