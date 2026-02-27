using ActionVault.Service.DTO;

namespace ActionVault.Service.Interfaces;

public interface IActionVaultService
{
    // VaultTask operations
    Task<VaultTaskDTO> CreateTaskAsync(CreateVaultTaskRequest request);
    Task<VaultTaskDTO?> GetTaskByIdAsync(Guid id);
    Task<PagedResult<VaultTaskSummaryDTO>> GetTasksByWorkspaceAsync(Guid workspaceId, int page, int pageSize);
    Task<VaultTaskDTO> UpdateTaskAsync(UpdateVaultTaskRequest request);
    Task<bool> DeleteTaskAsync(Guid id);

    // DailyQueue operations
    Task<DailyQueueDTO> QueueTaskForTodayAsync(QueueTaskRequest request);
    Task<IEnumerable<DailyQueueDTO>> GetDailyQueueAsync(Guid workspaceId, DateOnly? date = null);
    Task<bool> MarkCompletedAsync(Guid dailyQueueId);
    Task<bool> DequeueTaskAsync(Guid dailyQueueId);
}
