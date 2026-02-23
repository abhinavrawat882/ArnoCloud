using Microsoft.Extensions.DependencyInjection;
using Workspace.Service.Data;
using Microsoft.EntityFrameworkCore;

namespace Workspace.Service;

public static class WorkspaceExtentions
{
    public static IServiceCollection AddWorkspaceModule(this IServiceCollection services)
    {
        services.AddDbContext<WorkspaceContext>(options =>
            options.UseNpgsql("Host=devserver1;Database=ArnoCloud;Username=ArnoCloud;Password=ArnoCloud@123"));
        
        services.AddScoped<Workspace.Service.Interface.IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<Workspace.Service.Interface.IWorkspaceService, Workspace.Service.Services.WorkspaceService>();

        return services;
    }
} 