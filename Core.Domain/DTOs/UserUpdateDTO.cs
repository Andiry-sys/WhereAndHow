namespace Core.Domain.DTOs;

public class UserUpdateDTO
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? SureName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsLosser { get; set; }
}
