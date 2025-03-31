namespace Core.Domain.Models;
public class Apartament
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? TypeRoom { get; set; }
    public float? Price { get; set; }
    public string? Description { get; set; }

    public HistoryApartament? History { get; set; }
    public string? HistoryId { get; set; }

    public string? AddressId { get; set; }
    public Address? Address { get; set; }

    public string? PhotoId { get; set; }
    public ICollection<Photo>? Photos { get; set; }

    public string? OwnerId { get; set; }
    public User? Owner { get; set; }

}
