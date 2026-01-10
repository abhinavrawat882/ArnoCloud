namespace Auth.Service.Entites;

public class User
{
    public required int Id{get;set;}
    public required string UserName {get;set;}
    public required string Email{get;set;}
    public required string Password{get;set;}

    public required ICollection<UserRole> UserRoles {get;set;}
}