using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ToDoList.Service.Entity;

namespace ToDoList.Service.Data;

public class ToDoListDbContext:DbContext
{
    public ToDoListDbContext(DbContextOptions options) : base(options)
    {}

    public DbSet<Todolist> todolists {get;set;} 
    
}