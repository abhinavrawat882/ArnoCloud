using Microsoft.EntityFrameworkCore;
using Workspace.Service.Interface;

namespace Workspace.Service.Data;

public class WorkspaceRepository : IWorkspaceRepository
{
    private readonly WorkspaceContext _context;

    public WorkspaceRepository(WorkspaceContext context)
    {
        _context = context;
    }

    public async Task<Entites.Workspace> CreateAsync(Entites.Workspace workspace)
    {
        _context.Workspaces.Add(workspace);
        await _context.SaveChangesAsync();
        return workspace;
    }

    public async Task<Entites.Workspace?> GetByIdAsync(Guid id)
    {
        return await _context.Workspaces.FindAsync(id);
    }

    public async Task<(IEnumerable<Entites.Workspace> Items, int TotalCount)> GetAllAsync(int page, int pageSize)
    {
        var query = _context.Workspaces.AsNoTracking();

        int totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Entites.Workspace> UpdateAsync(Entites.Workspace workspace)
    {
        _context.Workspaces.Update(workspace);
        await _context.SaveChangesAsync();
        return workspace;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var workspace = await _context.Workspaces.FindAsync(id);
        if (workspace == null)
            return false;

        _context.Workspaces.Remove(workspace);
        await _context.SaveChangesAsync();
        return true;
    }
}
