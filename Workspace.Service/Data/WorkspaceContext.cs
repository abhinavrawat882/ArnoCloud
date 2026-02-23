using Microsoft.EntityFrameworkCore;

namespace Workspace.Service.Data;

public class WorkspaceContext : DbContext
{
    public WorkspaceContext(DbContextOptions<WorkspaceContext> options) : base(options) { }
    public DbSet<Workspace.Service.Entites.Workspace> Workspaces { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("workspace");
        base.OnModelCreating(modelBuilder);
    }
}