namespace Workspace.Service.Interface;

public interface IWorkspaceRepository
{
    Task<Entites.Workspace> CreateAsync(Entites.Workspace workspace);
    Task<Entites.Workspace?> GetByIdAsync(Guid id);
    Task<(IEnumerable<Entites.Workspace> Items, int TotalCount)> GetAllAsync(int page, int pageSize);
    Task<Entites.Workspace> UpdateAsync(Entites.Workspace workspace);
    Task<bool> DeleteAsync(Guid id);
}