namespace Core.Domain.DTOs;

public class OrderDTO
{
    public string? UserId { get; set; }
    public string? ApartamentId { get; set; }

    public string? CheckInDate { get; set; }
    public string? CheckOutDate { get; set; }
}
