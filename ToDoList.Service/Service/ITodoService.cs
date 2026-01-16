using ToDoList.Service.DTO;

public interface IToDoService
{
    public Task<int> AddToDoItemAsync(ToDoListDTO toDoListDTO);
    public Task<bool> UpdateItemAsync(int id);
    public Task<bool> DeleteItemAsync(int id);
    public Task<List<ToDoListDTO>> GetListAsync(ToDoListFilter todolistfilter);

}