
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Options;
using Notes.Configuration;
using Notes.Service;
using Notes.Service.Entity;
using Notes.Service.Services;
// using Microsoft.EntityFrameworkCore.
public static class NotesServiceExtensions
{
    public static IServiceCollection AddNotesModule(this IServiceCollection services,NotesOptions notesOptions)//, IConfiguration configuration)
    {
        // Register the Database for this module
        // services.AddDbContext<NotesDbContext>(options =>
        //     options.UseSqlServer(configuration.GetConnectionString("NotesDb")));

        // Register module-specific services
        //services.AddScoped<INotesService, NotesService>();

        // In Program.cs
        services.AddAutoMapper(typeof(NotesServiceExtensions).Assembly);
        services.AddDbContext<NotesDbContext>(options=>
            options.UseSqlite("Data Source=notes.db")
        );
        services.AddScoped<INoteRepository, NoteRepository>();
        services.AddScoped<NoteService>();
        return services;
    }
}