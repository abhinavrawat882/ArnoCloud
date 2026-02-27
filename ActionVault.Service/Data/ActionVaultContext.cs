using ActionVault.Service.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActionVault.Service.Data;

public class ActionVaultContext : DbContext
{
    public ActionVaultContext(DbContextOptions<ActionVaultContext> options) : base(options) { }

    public DbSet<VaultTask> VaultTasks { get; set; }
    public DbSet<DailyQueue> DailyQueues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("actionvault");

        modelBuilder.Entity<VaultTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Description).HasMaxLength(2048);
            entity.Property(e => e.Priority).HasConversion<int>();
            entity.Property(e => e.RecurrenceType).HasConversion<int>();
            entity.HasIndex(e => e.WorkspaceId);
        });

        modelBuilder.Entity<DailyQueue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.VaultTask)
                  .WithMany(v => v.DailyQueues)
                  .HasForeignKey(e => e.VaultTaskId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.VaultTaskId, e.SelectedDate }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}
