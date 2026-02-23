using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ToDoList.Service.Data;
using ToDoList.Service.DTO;
using ToDoList.Service.Entity;

namespace ToDoList.Service.Repository;

public class ToDoListRepo : IToDoListRepo
{
    private readonly ToDoListDbContext _toDoListDbContext;
    public ToDoListRepo(ToDoListDbContext  toDoListDbContext)
    {
        _toDoListDbContext=toDoListDbContext;
    }
    public async Task<int> AddToDoListItemAsync(ToDoListDTO toDoListDTO)
    {
        var entity = new Entity.Todolist
            {
                Body = toDoListDTO.Body,
                State = toDoListDTO.State
            };

            _toDoListDbContext.todolists.Add(entity);

            await _toDoListDbContext.SaveChangesAsync();

            // At this point, EF Core has populated entity.Id
            return entity.Id;
    }

    public async Task<List<ToDoListDTO>> GetToDOListAsync(ToDoListFilter filter)
    {
        return await _toDoListDbContext.todolists.AsNoTracking().Where(x=>x.State==filter.State)
        .Skip(filter.page)
        .Take(filter.pageSize)
        .Select(x=>new ToDoListDTO()
        {
            Id=x.Id,
            State=x.State,
            Body=x.Body
        })
        .ToListAsync();
    }

    public Task<ToDoListDTO> GetToDOListItemAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ToDoListDTO> UpdateItemAsync(ToDoListDTO toDoListDTO)
    {
        var toDoItem = await _toDoListDbContext.todolists.Where(x=>x.Id==toDoListDTO.Id).FirstOrDefaultAsync();
        if(toDoItem==null) throw new KeyNotFoundException("No Such Item Found");
        toDoItem.Body=toDoListDTO.Body;
        toDoItem.State=toDoListDTO.State;
        return toDoListDTO;
    }

    public async Task<ToDoListDTO> DeleteItemAsync(int id)
    {
        var toDoItem = await _toDoListDbContext.todolists.Where(x=>x.Id==id).FirstOrDefaultAsync();
        if(toDoItem==null) throw new KeyNotFoundException("No Such Item Found");
        var ret= new ToDoListDTO()
        {
            Id=toDoItem.Id,
            Body=toDoItem.Body,
            State=toDoItem.State
        };
        
        _toDoListDbContext.Remove(toDoItem);
        return ret;
    }
}