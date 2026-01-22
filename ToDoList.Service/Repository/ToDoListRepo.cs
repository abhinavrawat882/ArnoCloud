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

    public Task<List<ToDoListDTO>> GetToDOListAsync(ToDoListFilter filter)
    {
        throw new NotImplementedException();
    }

    public Task<ToDoListDTO> GetToDOListItemAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ToDoListDTO> UpdateItemAsync(ToDoListDTO toDoListDTO)
    {
        throw new NotImplementedException();
    }

    Task<ToDoListDTO> IToDoListRepo.DeleteItemAsync(int id)
    {
        throw new NotImplementedException();
    }
}