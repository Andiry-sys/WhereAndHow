namespace Core.Domain.Models;

public class Photo
{
    public string? Id { get; set; }

    public string? ImagePath { get; set; }

    public string? RoomId { get; set; }
    public Apartament? Room { get; set; }
}