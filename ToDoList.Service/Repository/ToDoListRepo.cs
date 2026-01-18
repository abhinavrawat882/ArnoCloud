using ToDoList.Service.DTO;

namespace ToDoList.Service.Repository;

public class ToDoListRepo : IToDoListRepo
{
    public Task<int> AddToDoListItemAsync(ToDoListDTO toDoListDTO)
    {
        throw new NotImplementedException();
    }

    public Task DeleteItemAsync(int id)
    {
        throw new NotImplementedException();
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
}