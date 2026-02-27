using ActionVault.Service.Enums;

namespace ActionVault.Service.DTO;

public class VaultTaskSummaryDTO
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public TaskPriority Priority { get; set; }
    public bool IsRecurring { get; set; }
    public RecurrenceType RecurrenceType { get; set; }
}
