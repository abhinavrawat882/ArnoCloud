using Microsoft.AspNetCore.Mvc;
using ToDoList.Service.DTO;
using ToDoList.Service.Service;

namespace Core.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/toDoList")]
class ToDoListController : ControllerBase
{
    private readonly ToDoService _toDoService;
    public ToDoListController(ToDoService toDoService)
    {
        _toDoService=toDoService;
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ToDoListDTO),StatusCodes.Status200OK)]
    
    public async Task<IActionResult> GetToDoListItem(int id)
    {
            return Ok (await _toDoService.GetTodoItemAsync(id));
    }
    [HttpGet]
    [ProducesResponseType(typeof(List<ToDoListDTO>),StatusCodes.Status200OK)]
    public async Task<IActionResult> GetToDoList(ToDoListFilter toDoListFilter)
    {
        return Ok (await _toDoService.GetListAsync(toDoListFilter));
    }
}