namespace Core.Domain.Models;

[Serializable]
public class Comment
{
    public string? Id { get; set; }
    public string? Text { get; set; }
    public string? RoomId { get; set; }
    public Apartament? Room { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }
}