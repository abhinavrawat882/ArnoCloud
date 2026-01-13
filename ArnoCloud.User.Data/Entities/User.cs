namespace ArnoCloud.User.Data.Entities;

public class User
{
    public int Id{get;set;}
    public required string UserName {get;set;}
    public required string Email{get;set;}
    public required string Password{get;set;}

    public ICollection<UserRole> UserRoles {get;set;} = new List<UserRole>();
}