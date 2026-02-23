namespace ToDoList.Service.Entity;

using ToDoList.Service.Enums;

public class Todolist
{
    public int Id{get;set;}
    public string Body{get;set;}
    public string CreatedAt{get;set;}
    public string CompletedAt{get;set;}
    public TodoState State{get;set;}
}