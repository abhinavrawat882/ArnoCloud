using Workspace.Service.DTO;

namespace Workspace.Service.Interface;

public interface IWorkspaceService
{
    Task<WorkspaceDetailDTO> CreateAsync(CreateWorkspaceRequest request);
    Task<WorkspaceDetailDTO?> GetByIdAsync(Guid id);
    Task<PagedResult<WorkspaceSummaryDTO>> GetAllAsync(int page, int pageSize);
    Task<WorkspaceDetailDTO> UpdateAsync(UpdateWorkspaceRequest request);
    Task<bool> DeleteAsync(Guid id);
}
