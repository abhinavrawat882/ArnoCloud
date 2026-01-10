namespace Auth.Service.Entites;

public class UserRole
{
    public int RoleId{get;set;}
    public int UserId{get;set;}

    public required Role Role{get;set;}=null!;
    public required User User{get;set;}=null!;

}