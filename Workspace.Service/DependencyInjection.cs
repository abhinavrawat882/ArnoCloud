using Microsoft.Extensions.DependencyInjection;
using Workspace.Service.Data;

namespace Workspace.Service;

public static class WorkspaceExtentions
{
    public static IServiceCollection AddWorkspaceModule(this IServiceCollection services)
    {
        services.AddDbContext<WorkspaceContext>();
        
        services.AddScoped<Workspace.Service.Interface.IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<Workspace.Service.Interface.IWorkspaceService, Workspace.Service.Services.WorkspaceService>();

        return services;
    }
} 