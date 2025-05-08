namespace Core.Domain.DTOs;

public class SearchApartamentDTO
{
    public string? Name { get; set; }
    public float? minValue { get; set; }
    public float? maxValue { get; set; }
    public string? TypeRoom { get; set; }
    public string? City { get; set; }
}
