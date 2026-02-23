
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Notes.Service.DTO;
using Notes.Service.Services;
namespace Core.API.Controllers.V1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/note")]
public class NotesController : ControllerBase
{   
    private readonly NoteService _noteService;
    public NotesController(NoteService noteService )
    {
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<ActionResult> GetNotes(
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        return Ok(await _noteService.GetNotes(page??0,pageSize??0));
    }           

    [HttpGet("{id}")]
    public async Task<ActionResult> GetNote([FromRoute] int id)
    {
        return Ok();
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteDTO note)
    {
        await _noteService.AddNewNoteAsync(note);
        return Ok();
    }              
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        await _noteService.DeleteNote(id);
        return Ok();   
    }    
    [HttpPut]
    public async Task<ActionResult> Update([FromBody] NoteDTO note)
    {
        await _noteService.UpdateNote(note);
        return Ok();
    }         
}