namespace Core.Domain.DTOs;

public class AIAnalyzeResponseDTO
{
    public string? ImprovedDescription { get; set; }
    public List<string> Amenities { get; set; } = new();
    public int? QualityScore { get; set; }
    public List<string> Recommendations { get; set; } = new();
}
