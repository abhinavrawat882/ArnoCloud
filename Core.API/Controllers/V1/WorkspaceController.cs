using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v{version:apiVersion}/workspace")]
public class WorkspaceController:ControllerBase
{
    
    public WorkspaceController(){}

    [HttpGet]
    public async Task<IActionResult> GetWorkSpaces()
    {
        throw new NotImplementedException();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetWorkspaceDetails([FromRoute] int id)
    {
        throw new NotImplementedException();
    }
    [HttpPost]
    public async Task<IActionResult> AddWorkspace()
    {
        throw new NotImplementedException();
    }

}