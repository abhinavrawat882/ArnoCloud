
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using ToDoList.Service.Data;
using ToDoList.Service.Repository;
using ToDoList.Service.Service;
// using Microsoft.EntityFrameworkCore.
public static class NotesServiceExtensions
{
    public static IServiceCollection AddToDoListModule(this IServiceCollection services)//, IConfiguration configuration)
    {
        // Register the Database for this module
        // services.AddDbContext<NotesDbContext>(options =>
        //     options.UseSqlServer(configuration.GetConnectionString("NotesDb")));

        // Register module-specific services
        //services.AddScoped<INotesService, NotesService>();

        // In Program.cs
        //services.AddAuthorization,a(typeof(NotesServiceExtensions).Assembly);
        services.AddDbContext<ToDoListDbContext>(options=>
            options.UseSqlite("Data Source=notes.db")
        );
        services.AddScoped<IToDoListRepo, ToDoListRepo>();
        services.AddScoped<ToDoService>();
        return services;
    }
}