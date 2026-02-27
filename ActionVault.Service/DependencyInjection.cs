using ActionVault.Service.Data;
using ActionVault.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ActionVault.Service;

public static class ActionVaultExtensions
{
    public static IServiceCollection AddActionVaultModule(this IServiceCollection services)
    {
        services.AddDbContext<ActionVaultContext>(options =>
            options.UseNpgsql("Host=devserver1;Database=ArnoCloud;Username=ArnoCloud;Password=ArnoCloud@123"));

        services.AddScoped<IActionVaultRepository, ActionVaultRepository>();
        services.AddScoped<IActionVaultService, Services.ActionVaultService>();

        return services;
    }
}
