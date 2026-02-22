namespace Workspace.Service.DTO;

public class WorkspaceSummaryDTO
{
    public Guid Id{get;set;}
    public required string Name{get;set;}
    public string? Description{get;set;}
}