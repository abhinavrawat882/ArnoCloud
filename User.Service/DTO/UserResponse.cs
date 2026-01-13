namespace User.Service.DTO;

public class UserResponse
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public List<string> Roles { get; set; } = new();
}
