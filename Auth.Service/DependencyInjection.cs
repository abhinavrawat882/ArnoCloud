
using Auth.Service.Data;
using Auth.Service.Repository;
using Auth.Service.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
// using Microsoft.EntityFrameworkCore.
public static class AuthServiceExtention
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services)//, IConfiguration configuration)
    {

        services.AddDbContext<UserDbContext>(options =>
        {
         options.UseSqlite("Data Source=user.db");   
        });
        services.AddScoped<UserRepo>();
        services.AddScoped<LoginService>();
        return services;
    }
}