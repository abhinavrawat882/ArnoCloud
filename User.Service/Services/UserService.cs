using ArnoCloud.User.Data.Data;
using UserEntity = ArnoCloud.User.Data.Entities.User;
using ArnoCloud.User.Data.Entities;
using Microsoft.EntityFrameworkCore;
using User.Service.DTO;
using User.Service.Exceptions;
using User.Service.Interfaces;

namespace User.Service.Services;

public class UserService : IUserService
{
    private readonly UserDbContext _context;

    public UserService(UserDbContext context)
    {
        _context = context;
    }

    public async Task<UserResponse> AddUserAsync(CreateUserRequest request)
    {
        // Check for duplicate email
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            throw new DuplicateUserException(request.Email);
        }

        // Check duplicate username
        if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
        {
             throw new DuplicateUserException(request.UserName); // Reusing exception for username too
        }

        // Find Role
        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName);
        if (role == null)
        {
            throw new ArgumentException($"Role '{request.RoleName}' does not exist.");
        }

        var newUser = new UserEntity
        {
            UserName = request.UserName,
            Email = request.Email,
            Password = request.Password, // Storing plain text as per existing Auth.Service
            UserRoles = new List<UserRole>()
        };

        newUser.UserRoles.Add(new UserRole
        {
            Role = role
        });

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return new UserResponse
        {
            Id = newUser.Id,
            UserName = newUser.UserName,
            Email = newUser.Email,
            Roles = new List<string> { role.Name }
        };
    }

    public async Task UpdatePasswordAsync(int userId, UpdatePasswordRequest request)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        user.Password = request.NewPassword;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task SetRoleAsync(int userId, SetUserRoleRequest request)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == request.RoleName);
        if (role == null)
        {
             throw new ArgumentException($"Role '{request.RoleName}' does not exist.");
        }

        // Check if user already has this role
        if (user.UserRoles.Any(ur => ur.RoleId == role.Id))
        {
            return; // Already has role
        }

        user.UserRoles.Add(new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        });

        await _context.SaveChangesAsync();
    }
}
