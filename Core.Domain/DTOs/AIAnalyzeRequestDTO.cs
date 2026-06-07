namespace Core.Domain.DTOs;

public class AIAnalyzeRequestDTO
{
    public string? Description { get; set; }

    // Apartment metadata — all optional for backward compatibility.
    public string? Name { get; set; }
    public float? Price { get; set; }
    public string? TypeRoom { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }

    // Detected output language ("Ukrainian" | "English"). Defaults to "English" when absent.
    public string? Language { get; set; }
}
