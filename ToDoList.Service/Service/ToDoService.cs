using ToDoList.Service.DTO;
using ToDoList.Service.Repository;

namespace ToDoList.Service.Service;

public class ToDoService : IToDoService
{
    private readonly IToDoListRepo _repository;

    public ToDoService(IToDoListRepo repository)
    {
        _repository = repository;
    }

    public Task<int> AddToDoItemAsync(ToDoListDTO toDoListDTO)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteItemAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ToDoListDTO>> GetListAsync(ToDoListFilter todolistfilter)
    {
        throw new NotImplementedException();
    }

    public Task<List<ToDoListDTO>> GetListAsync(int id)
    {
        throw new NotImplementedException();
    }


    public Task<ToDoListDTO> UpdateItemAsync(ToDoListDTO toDoListDTO)
    {
        throw new NotImplementedException();
    }
}
