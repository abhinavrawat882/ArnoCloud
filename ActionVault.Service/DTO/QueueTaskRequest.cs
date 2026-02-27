namespace ActionVault.Service.DTO;

public class QueueTaskRequest
{
    public required Guid VaultTaskId { get; set; }
    public DateOnly? SelectedDate { get; set; }
}
