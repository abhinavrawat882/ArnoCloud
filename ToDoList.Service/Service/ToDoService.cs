using System.Runtime.CompilerServices;
using FluentValidation;
using ToDoList.Service.DTO;
using ToDoList.Service.Enums;
using ToDoList.Service.Repository;

namespace ToDoList.Service.Service;

public class ToDoService : IToDoService
{
    private readonly IToDoListRepo _repository;

    public ToDoService(IToDoListRepo repository)
    {
        _repository = repository;
    }

    public async Task<int> AddToDoItemAsync(ToDoListDTO toDoListDTO)
    {
       if(toDoListDTO.Id!=0||string.IsNullOrEmpty(toDoListDTO.Body) || toDoListDTO.State!=TodoState.Active) throw new ArgumentException();
       return await _repository.AddToDoListItemAsync(toDoListDTO);
    }

    public async Task<ToDoListDTO> DeleteItemAsync(int id)
    {
        if(id<=0) throw new ArgumentException("Invalid ID");
         var res = await _repository.DeleteItemAsync(id);
         return res;
    }

    public async Task<List<ToDoListDTO>> GetListAsync(ToDoListFilter todolistfilter)
    {
        //throw new NotImplementedException();
        if(todolistfilter==null) throw new ArgumentNullException("Filters can not be null");
        ToDoFilterValidator validator = new();
        validator.ValidateAndThrow(todolistfilter);

        return await _repository.GetToDOListAsync(todolistfilter);
    }
    public async Task<ToDoListDTO> UpdateItemAsync(ToDoListDTO toDoListDTO)
    {
        if(toDoListDTO.Id<=0 || string.IsNullOrEmpty(toDoListDTO.Body)) throw new ArgumentException("Invalid body or id");
        return await _repository.UpdateItemAsync(toDoListDTO);
    }
}
