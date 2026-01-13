using System.ComponentModel.DataAnnotations;

namespace User.Service.DTO;

public class UpdatePasswordRequest
{
    [Required]
    public required string NewPassword { get; set; }
}
