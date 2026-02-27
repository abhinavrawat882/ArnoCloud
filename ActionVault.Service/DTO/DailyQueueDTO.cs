using ActionVault.Service.Enums;

namespace ActionVault.Service.DTO;

public class DailyQueueDTO
{
    public Guid Id { get; set; }
    public Guid VaultTaskId { get; set; }
    public required string TaskName { get; set; }
    public string? TaskDescription { get; set; }
    public TaskPriority TaskPriority { get; set; }
    public DateOnly SelectedDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}
