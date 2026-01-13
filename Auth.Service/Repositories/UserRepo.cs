using System.Security.Cryptography.X509Certificates;
using ArnoCloud.User.Data.Data;
using ArnoCloud.User.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.Repository;

public class UserRepo
{
    private readonly UserDbContext _userDbContext;
    public UserRepo(UserDbContext userDbContext)
    {
        _userDbContext=userDbContext;   
    }
    public async Task<User?> GetUserDetails(String username)
    {
        return await _userDbContext.Users
            .AsNoTracking()
            .Include(x=>x.UserRoles)
            .ThenInclude(x=>x.Role)
            .FirstOrDefaultAsync(x=>x.UserName==username || x.Email==username);
       
    }
}