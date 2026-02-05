using ToDoList.Service.DTO;

public interface IToDoService
{
    public Task<int> AddToDoItemAsync(ToDoListDTO toDoListDTO);
    public Task<ToDoListDTO> UpdateItemAsync(ToDoListDTO toDoListDTO);
    public Task<ToDoListDTO> DeleteItemAsync(int id);
    public Task<List<ToDoListDTO>> GetListAsync(ToDoListFilter toDoListFilter);
    public Task<ToDoListDTO> GetTodoItemAsync(int id);

}