namespace ActionVault.Service.Entities;

public class DailyQueue
{
    public required Guid Id { get; set; }
    public required Guid VaultTaskId { get; set; }
    public required DateOnly SelectedDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation
    public VaultTask VaultTask { get; set; } = null!;
}
