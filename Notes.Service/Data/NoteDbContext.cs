using Microsoft.EntityFrameworkCore;

namespace Notes.Service.Entity;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions options) : base(options)
    {
    }
    internal DbSet<Note> Notes {get;set;}
}