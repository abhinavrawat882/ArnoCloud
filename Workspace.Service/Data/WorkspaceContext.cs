using Microsoft.EntityFrameworkCore;

namespace Workspace.Service.Data;

public class WorkspaceContext:DbContext
{
    public WorkspaceContext(DbContextOptions options):base(options){}
    public DbSet<Workspace.Service.Entites.Workspace> Workspaces { get; set; }
}