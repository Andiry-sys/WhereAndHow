using Core.Domain.Enums;

namespace Core.Domain.Models;

public class PartnerRequest
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string UserId { get; set; } = default!;
    public User User { get; set; } = default!;
    public PartnerRequestStatus Status { get; set; } = PartnerRequestStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }
}
