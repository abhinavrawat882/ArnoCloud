using UserEntity = ArnoCloud.User.Data.Entities.User;
using ArnoCloud.User.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArnoCloud.User.Data.Data;

public class UserDbContext: DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options):base(options)
    {
        
    }

    public DbSet<UserEntity> Users => Set<UserEntity>();
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