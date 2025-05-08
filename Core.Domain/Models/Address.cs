namespace Core.Domain.Models;

[Serializable]
public class Address
{
    public string? Id { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? NumberHouse { get; set; }
    public string? ApartamentId { get; set; }
    public List<Apartament>? Apartaments { get; set; }
}