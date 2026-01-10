using Auth.Service.Entites;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.Data;

public class UserDbContext: DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
    {
        
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserRole>()
        .HasKey(ur=> new {ur.RoleId,ur.UserId});

        modelBuilder.Entity<UserRole>()
        .HasOne(ur=>ur.User)
        .WithMany(ur=>ur.UserRoles)
        .HasForeignKey(ur=>ur.UserId);

        modelBuilder.Entity<UserRole>()
        .HasOne(ur=>ur.Role)
        .WithMany(ur=>ur.UserRoles)
        .HasForeignKey(ur=>ur.RoleId);
        
    }
}