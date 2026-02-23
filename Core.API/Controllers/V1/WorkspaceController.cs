using Microsoft.AspNetCore.Mvc;
using Workspace.Service.Interface;
using Workspace.Service.DTO;

namespace Core.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/workspaces")]
public class WorkspaceController : ControllerBase
{
    private readonly IWorkspaceService _workspaceService;

    public WorkspaceController(IWorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<WorkspaceSummaryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWorkspaces([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _workspaceService.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(WorkspaceDetailDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWorkspaceDetails([FromRoute] Guid id)
    {
        var result = await _workspaceService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(WorkspaceDetailDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddWorkspace([FromBody] CreateWorkspaceRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var result = await _workspaceService.CreateAsync(request);
        return CreatedAtAction(nameof(GetWorkspaceDetails), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(WorkspaceDetailDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateWorkspace([FromRoute] Guid id, [FromBody] UpdateWorkspaceRequest request)
    {
        if (request == null || id != request.Id)
        {
            return BadRequest();
        }

        try
        {
            var result = await _workspaceService.UpdateAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteWorkspace([FromRoute] Guid id)
    {
        var result = await _workspaceService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}