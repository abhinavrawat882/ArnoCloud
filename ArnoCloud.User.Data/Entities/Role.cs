using Microsoft.Identity.Client;

namespace ArnoCloud.User.Data.Entities;

public class Role
{
    public int Id {get;set;}
    public required string Name {get;set;}

    public ICollection<UserRole> UserRoles {get;set;} = new List<UserRole>();
}