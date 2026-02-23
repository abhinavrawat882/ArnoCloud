using Workspace.Service.DTO;
using Workspace.Service.Interface;

namespace Workspace.Service.Services;

public class WorkspaceService : IWorkspaceService
{
    private readonly IWorkspaceRepository _repository;

    public WorkspaceService(IWorkspaceRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkspaceDetailDTO> CreateAsync(CreateWorkspaceRequest request)
    {
        var entity = new Entites.Workspace
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };

        var created = await _repository.CreateAsync(entity);

        return new WorkspaceDetailDTO
        {
            Id = created.Id,
            Name = created.Name
        };
    }

    public async Task<WorkspaceDetailDTO?> GetByIdAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
            return null;

        return new WorkspaceDetailDTO
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task<PagedResult<WorkspaceSummaryDTO>> GetAllAsync(int page, int pageSize)
    {
        var (items, totalCount) = await _repository.GetAllAsync(page, pageSize);

        var dtos = items.Select(x => new WorkspaceSummaryDTO
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        });

        return new PagedResult<WorkspaceSummaryDTO>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<WorkspaceDetailDTO> UpdateAsync(UpdateWorkspaceRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
            throw new KeyNotFoundException($"Workspace with ID {request.Id} not found.");

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _repository.UpdateAsync(entity);

        return new WorkspaceDetailDTO
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }
}
