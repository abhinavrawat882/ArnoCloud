using System.ComponentModel.DataAnnotations;

namespace User.Service.DTO;

public class CreateUserRequest
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
    
    public string RoleName { get; set; } = "User";
}
