using ToDoList.Service.DTO;

namespace ToDoList.Service.Repository;

public interface IToDoListRepo
{
    public Task<List<ToDoListDTO>> GetToDOListAsync(ToDoListFilter filter);
    public Task<ToDoListDTO> GetToDOListItemAsync(int id);
    public Task<int> AddToDoListItemAsync(ToDoListDTO toDoListDTO);
    public Task UpdateItemAsync(ToDoListDTO toDoListDTO);
    public Task DeleteItemAsync(int id);


}