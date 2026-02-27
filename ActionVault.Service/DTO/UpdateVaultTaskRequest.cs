using ActionVault.Service.Enums;

namespace ActionVault.Service.DTO;

public class UpdateVaultTaskRequest
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; } = TaskPriority.None;
    public bool IsRecurring { get; set; }
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
    public int? RecurrenceIntervalMinutes { get; set; }
}
