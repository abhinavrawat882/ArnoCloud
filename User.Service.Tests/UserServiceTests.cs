using ArnoCloud.User.Data.Data;
using UserEntity = ArnoCloud.User.Data.Entities.User;
using ArnoCloud.User.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using User.Service.DTO;
using User.Service.Exceptions;
using User.Service.Services;
using Xunit;

namespace User.Service.Tests;

public class UserServiceTests
{
    private UserDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new UserDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task AddUserAsync_ShouldAddUser_WhenValid()
    {
        // Arrange
        using var context = GetDbContext();
        context.Roles.Add(new Role { Id = 1, Name = "User" });
        await context.SaveChangesAsync();

        var service = new UserService(context);
        var request = new CreateUserRequest 
        { 
            UserName = "testuser", 
            Email = "test@example.com", 
            Password = "password",
            RoleName = "User"
        };

        // Act
        var result = await service.AddUserAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.UserName);
        Assert.Equal("test@example.com", result.Email);
        Assert.Contains("User", result.Roles);
        
        var dbUser = await context.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(dbUser);
        Assert.Single(dbUser.UserRoles);
    }

    [Fact]
    public async Task AddUserAsync_ShouldThrowDuplicate_WhenEmailExists()
    {
        // Arrange
        using var context = GetDbContext();
        context.Users.Add(new UserEntity 
        { 
            UserName = "existing", 
            Email = "test@example.com", 
            Password = "pass",
            UserRoles = new List<UserRole>()
        });
        await context.SaveChangesAsync();

        var service = new UserService(context);
        var request = new CreateUserRequest 
        { 
            UserName = "newuser", 
            Email = "test@example.com", 
            Password = "password" 
        };

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateUserException>(() => service.AddUserAsync(request));
    }

    [Fact]
    public async Task AddUserAsync_ShouldThrowArgumentException_WhenRoleNotFound()
    {
        // Arrange
        using var context = GetDbContext(); // Empty DB, no roles
        
        var service = new UserService(context);
        var request = new CreateUserRequest 
        { 
            UserName = "testuser", 
            Email = "test@example.com", 
            Password = "password",
            RoleName = "Admin"
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => service.AddUserAsync(request));
    }

    [Fact]
    public async Task UpdatePasswordAsync_ShouldUpdate_WhenUserExists()
    {
        // Arrange
        using var context = GetDbContext();
        var user = new UserEntity 
        { 
            Id = 1,
            UserName = "user", 
            Email = "user@example.com", 
            Password = "oldpassword",
            UserRoles = new List<UserRole>()
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var service = new UserService(context);
        var request = new UpdatePasswordRequest { NewPassword = "newpassword" };

        // Act
        await service.UpdatePasswordAsync(1, request);

        // Assert
        var dbUser = await context.Users.FindAsync(1);
        Assert.Equal("newpassword", dbUser!.Password);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldRemoveUser_WhenExists()
    {
        // Arrange
        using var context = GetDbContext();
        var user = new UserEntity 
        { 
            Id = 1,
            UserName = "user", 
            Email = "user@example.com", 
            Password = "password",
            UserRoles = new List<UserRole>()
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var service = new UserService(context);

        // Act
        await service.DeleteUserAsync(1);

        // Assert
        var dbUser = await context.Users.FindAsync(1);
        Assert.Null(dbUser);
    }

    [Fact]
    public async Task SetRoleAsync_ShouldAddRole_WhenValid()
    {
        // Arrange
        using var context = GetDbContext();
        var role = new Role { Id = 1, Name = "Admin" };
        var user = new UserEntity 
        { 
            Id = 1,
            UserName = "user", 
            Email = "user@example.com", 
            Password = "password",
            UserRoles = new List<UserRole>()
        };
        context.Roles.Add(role);
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var service = new UserService(context);
        var request = new SetUserRoleRequest { RoleName = "Admin" };

        // Act
        await service.SetRoleAsync(1, request);

        // Assert
        var dbUser = await context.Users.Include(u => u.UserRoles).FirstAsync(u => u.Id == 1);
        Assert.Contains(dbUser.UserRoles, ur => ur.RoleId == 1);
    }
}
