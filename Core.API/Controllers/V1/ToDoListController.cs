using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetToDoList(ToDoListFilter toDoListFilter)
    {
        return Ok (await _toDoService.GetListAsync(toDoListFilter));
    }
}