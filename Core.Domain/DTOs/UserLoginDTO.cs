using System.ComponentModel.DataAnnotations;

namespace Core.Domain.DTOs;

public class UserLoginDTO
{
    [Required(ErrorMessage = "The email is required!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "The password is required!")]
    public string? Password { get; set; }
}
