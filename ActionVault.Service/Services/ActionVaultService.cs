using ActionVault.Service.DTO;
using ActionVault.Service.Entities;
using ActionVault.Service.Interfaces;

namespace ActionVault.Service.Services;

public class ActionVaultService : IActionVaultService
{
    private readonly IActionVaultRepository _repository;

    public ActionVaultService(IActionVaultRepository repository)
    {
        _repository = repository;
    }

    // ── VaultTask operations ────────────────────────────────────────

    public async Task<VaultTaskDTO> CreateTaskAsync(CreateVaultTaskRequest request)
    {
        var entity = new VaultTask
        {
            Id = Guid.NewGuid(),
            WorkspaceId = request.WorkspaceId,
            Name = request.Name,
            Description = request.Description,
            Priority = request.Priority,
            IsRecurring = request.IsRecurring,
            RecurrenceType = request.RecurrenceType,
            RecurrenceIntervalMinutes = request.RecurrenceIntervalMinutes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var created = await _repository.CreateTaskAsync(entity);
        return MapToDTO(created);
    }

    public async Task<VaultTaskDTO?> GetTaskByIdAsync(Guid id)
    {
        var entity = await _repository.GetTaskByIdAsync(id);
        return entity == null ? null : MapToDTO(entity);
    }

    public async Task<PagedResult<VaultTaskSummaryDTO>> GetTasksByWorkspaceAsync(
        Guid workspaceId, int page, int pageSize)
    {
        var (items, totalCount) = await _repository.GetTasksByWorkspaceAsync(workspaceId, page, pageSize);

        var dtos = items.Select(t => new VaultTaskSummaryDTO
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            Priority = t.Priority,
            IsRecurring = t.IsRecurring,
            RecurrenceType = t.RecurrenceType
        });

        return new PagedResult<VaultTaskSummaryDTO>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<VaultTaskDTO> UpdateTaskAsync(UpdateVaultTaskRequest request)
    {
        var entity = await _repository.GetTaskByIdAsync(request.Id);
        if (entity == null)
            throw new KeyNotFoundException($"VaultTask with ID {request.Id} not found.");

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Priority = request.Priority;
        entity.IsRecurring = request.IsRecurring;
        entity.RecurrenceType = request.RecurrenceType;
        entity.RecurrenceIntervalMinutes = request.RecurrenceIntervalMinutes;

        var updated = await _repository.UpdateTaskAsync(entity);
        return MapToDTO(updated);
    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        return await _repository.DeleteTaskAsync(id);
    }

    // ── DailyQueue operations ───────────────────────────────────────

    public async Task<DailyQueueDTO> QueueTaskForTodayAsync(QueueTaskRequest request)
    {
        var task = await _repository.GetTaskByIdAsync(request.VaultTaskId);
        if (task == null)
            throw new KeyNotFoundException($"VaultTask with ID {request.VaultTaskId} not found.");

        var dailyQueue = new DailyQueue
        {
            Id = Guid.NewGuid(),
            VaultTaskId = request.VaultTaskId,
            SelectedDate = request.SelectedDate ?? DateOnly.FromDateTime(DateTime.UtcNow),
            IsCompleted = false
        };

        var created = await _repository.QueueTaskForDayAsync(dailyQueue);

        return new DailyQueueDTO
        {
            Id = created.Id,
            VaultTaskId = task.Id,
            TaskName = task.Name,
            TaskDescription = task.Description,
            TaskPriority = task.Priority,
            SelectedDate = created.SelectedDate,
            IsCompleted = created.IsCompleted,
            CompletedAt = created.CompletedAt
        };
    }

    public async Task<IEnumerable<DailyQueueDTO>> GetDailyQueueAsync(Guid workspaceId, DateOnly? date = null)
    {
        var targetDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var items = await _repository.GetDailyQueueAsync(workspaceId, targetDate);

        return items.Select(dq => new DailyQueueDTO
        {
            Id = dq.Id,
            VaultTaskId = dq.VaultTaskId,
            TaskName = dq.VaultTask.Name,
            TaskDescription = dq.VaultTask.Description,
            TaskPriority = dq.VaultTask.Priority,
            SelectedDate = dq.SelectedDate,
            IsCompleted = dq.IsCompleted,
            CompletedAt = dq.CompletedAt
        });
    }

    public async Task<bool> MarkCompletedAsync(Guid dailyQueueId)
    {
        return await _repository.MarkCompletedAsync(dailyQueueId);
    }

    public async Task<bool> DequeueTaskAsync(Guid dailyQueueId)
    {
        return await _repository.DequeueTaskAsync(dailyQueueId);
    }

    // ── Private helpers ─────────────────────────────────────────────

    private static VaultTaskDTO MapToDTO(VaultTask entity)
    {
        return new VaultTaskDTO
        {
            Id = entity.Id,
            WorkspaceId = entity.WorkspaceId,
            Name = entity.Name,
            Description = entity.Description,
            Priority = entity.Priority,
            IsRecurring = entity.IsRecurring,
            RecurrenceType = entity.RecurrenceType,
            RecurrenceIntervalMinutes = entity.RecurrenceIntervalMinutes,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            IsActive = entity.IsActive
        };
    }
}
