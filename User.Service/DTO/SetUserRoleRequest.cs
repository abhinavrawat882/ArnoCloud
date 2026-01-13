using System.ComponentModel.DataAnnotations;

namespace User.Service.DTO;

public class SetUserRoleRequest
{
    [Required]
    public required string RoleName { get; set; }
}
