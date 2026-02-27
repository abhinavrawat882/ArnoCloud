using ActionVault.Service.Enums;

namespace ActionVault.Service.Entities;

public class VaultTask
{
    public required Guid Id { get; set; }
    public required Guid WorkspaceId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public bool IsRecurring { get; set; }
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
    public int? RecurrenceIntervalMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<DailyQueue> DailyQueues { get; set; } = new List<DailyQueue>();
}
