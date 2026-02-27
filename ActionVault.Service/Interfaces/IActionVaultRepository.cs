using ActionVault.Service.Entities;

namespace ActionVault.Service.Interfaces;

public interface IActionVaultRepository
{
    // VaultTask CRUD
    Task<VaultTask> CreateTaskAsync(VaultTask task);
    Task<VaultTask?> GetTaskByIdAsync(Guid id);
    Task<(IEnumerable<VaultTask> Items, int TotalCount)> GetTasksByWorkspaceAsync(Guid workspaceId, int page, int pageSize);
    Task<VaultTask> UpdateTaskAsync(VaultTask task);
    Task<bool> DeleteTaskAsync(Guid id);

    // DailyQueue operations
    Task<DailyQueue> QueueTaskForDayAsync(DailyQueue dailyQueue);
    Task<IEnumerable<DailyQueue>> GetDailyQueueAsync(Guid workspaceId, DateOnly date);
    Task<DailyQueue?> GetDailyQueueItemAsync(Guid dailyQueueId);
    Task<bool> MarkCompletedAsync(Guid dailyQueueId);
    Task<bool> DequeueTaskAsync(Guid dailyQueueId);
}
