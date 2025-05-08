using Microsoft.AspNetCore.Identity;

namespace Core.Domain.Models;

[Serializable]
public class User : IdentityUser
{
    public string? Name { get; set; }
    public string? SureName { get; set; }
    public bool? IsLosser { get; set; }

    public HistoryApartament? HistoryApartament { get; set; }
    public string? HistoryId { get; set; }

    public Comment? Comment { get; set; }
    public int CommentId { get; set; }

    public string? ApartamentId { get; set; }
    public List<Apartament>? Apartaments { get; set; } = [];
}