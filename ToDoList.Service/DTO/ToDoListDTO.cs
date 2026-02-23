namespace ToDoList.Service.DTO;

using ToDoList.Service.Enums;

public class ToDoListDTO
{
    public int Id{get;set;}
    public string Body{get;set;}
    public TodoState State{get;set;}

}