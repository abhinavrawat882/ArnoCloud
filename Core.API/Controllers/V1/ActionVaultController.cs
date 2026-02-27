using ActionVault.Service.DTO;
using ActionVault.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.API.Controllers.V1;

[ApiController]
[Route("api/v{version:apiVersion}/action-vault")]
public class ActionVaultController : ControllerBase
{
    private readonly IActionVaultService _actionVaultService;

    public ActionVaultController(IActionVaultService actionVaultService)
    {
        _actionVaultService = actionVaultService;
    }

    // ── VaultTask endpoints ─────────────────────────────────────────

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<VaultTaskSummaryDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTasks(
        [FromQuery] Guid workspaceId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _actionVaultService.GetTasksByWorkspaceAsync(workspaceId, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VaultTaskDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskDetails([FromRoute] Guid id)
    {
        var result = await _actionVaultService.GetTaskByIdAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(VaultTaskDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateVaultTaskRequest request)
    {
        if (request == null)
            return BadRequest();

        var result = await _actionVaultService.CreateTaskAsync(request);
        return CreatedAtAction(nameof(GetTaskDetails), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(VaultTaskDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid id, [FromBody] UpdateVaultTaskRequest request)
    {
        if (request == null || id != request.Id)
            return BadRequest();

        try
        {
            var result = await _actionVaultService.UpdateTaskAsync(request);
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
    public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
    {
        var result = await _actionVaultService.DeleteTaskAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    // ── DailyQueue endpoints ────────────────────────────────────────

    [HttpGet("daily-queue")]
    [ProducesResponseType(typeof(IEnumerable<DailyQueueDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDailyQueue(
        [FromQuery] Guid workspaceId,
        [FromQuery] DateOnly? date = null)
    {
        var result = await _actionVaultService.GetDailyQueueAsync(workspaceId, date);
        return Ok(result);
    }

    [HttpPost("daily-queue")]
    [ProducesResponseType(typeof(DailyQueueDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> QueueTask([FromBody] QueueTaskRequest request)
    {
        if (request == null)
            return BadRequest();

        try
        {
            var result = await _actionVaultService.QueueTaskForTodayAsync(request);
            return Created(string.Empty, result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPatch("daily-queue/{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkCompleted([FromRoute] Guid id)
    {
        var result = await _actionVaultService.MarkCompletedAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }

    [HttpDelete("daily-queue/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DequeueTask([FromRoute] Guid id)
    {
        var result = await _actionVaultService.DequeueTaskAsync(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
