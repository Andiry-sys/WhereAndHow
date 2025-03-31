namespace Core.Domain.Models;

public class HistoryApartament
{
    public string? Id { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    public string? ApartamentId { get; set; }
    public Apartament? Apartament { get; set; }

    public string? DateArrival { get; set; }
    public string? DateDeparture { get; set; }
}