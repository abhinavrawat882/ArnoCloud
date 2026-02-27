using ActionVault.Service.Entities;
using ActionVault.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ActionVault.Service.Data;

public class ActionVaultRepository : IActionVaultRepository
{
    private readonly ActionVaultContext _context;

    public ActionVaultRepository(ActionVaultContext context)
    {
        _context = context;
    }

    // ── VaultTask CRUD ──────────────────────────────────────────────

    public async Task<VaultTask> CreateTaskAsync(VaultTask task)
    {
        _context.VaultTasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<VaultTask?> GetTaskByIdAsync(Guid id)
    {
        return await _context.VaultTasks.FindAsync(id);
    }

    public async Task<(IEnumerable<VaultTask> Items, int TotalCount)> GetTasksByWorkspaceAsync(
        Guid workspaceId, int page, int pageSize)
    {
        var query = _context.VaultTasks
            .AsNoTracking()
            .Where(t => t.WorkspaceId == workspaceId && t.IsActive);

        int totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.Priority)
            .ThenByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<VaultTask> UpdateTaskAsync(VaultTask task)
    {
        task.UpdatedAt = DateTime.UtcNow;
        _context.VaultTasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var task = await _context.VaultTasks.FindAsync(id);
        if (task == null)
            return false;

        task.IsActive = false;
        task.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    // ── DailyQueue operations ───────────────────────────────────────

    public async Task<DailyQueue> QueueTaskForDayAsync(DailyQueue dailyQueue)
    {
        _context.DailyQueues.Add(dailyQueue);
        await _context.SaveChangesAsync();
        return dailyQueue;
    }

    public async Task<IEnumerable<DailyQueue>> GetDailyQueueAsync(Guid workspaceId, DateOnly date)
    {
        return await _context.DailyQueues
            .AsNoTracking()
            .Include(dq => dq.VaultTask)
            .Where(dq => dq.VaultTask.WorkspaceId == workspaceId && dq.SelectedDate == date)
            .OrderByDescending(dq => dq.VaultTask.Priority)
            .ToListAsync();
    }

    public async Task<DailyQueue?> GetDailyQueueItemAsync(Guid dailyQueueId)
    {
        return await _context.DailyQueues
            .Include(dq => dq.VaultTask)
            .FirstOrDefaultAsync(dq => dq.Id == dailyQueueId);
    }

    public async Task<bool> MarkCompletedAsync(Guid dailyQueueId)
    {
        var item = await _context.DailyQueues.FindAsync(dailyQueueId);
        if (item == null)
            return false;

        item.IsCompleted = true;
        item.CompletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DequeueTaskAsync(Guid dailyQueueId)
    {
        var item = await _context.DailyQueues.FindAsync(dailyQueueId);
        if (item == null)
            return false;

        _context.DailyQueues.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
