using System.Threading.Tasks;
using Asp.Versioning;
using Auth.Service.DTO;
using Auth.Service.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Service.DTO;
using Notes.Service.Services;

namespace Core.API.Controllers.V1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class AuthController : ControllerBase
{
    private readonly LoginService _loginService;
    public AuthController(LoginService loginService){
        _loginService=loginService;
    }

    [HttpPost("Login")]
    [ProducesResponseType(typeof(LoginCredDTO),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login(LoginCredDTO loginCredDTO)
    {
        
        try
        {
            var loginCred = await _loginService.Login(loginCredDTO);

            return Ok(loginCred);
        }
        catch(UnauthorizedAccessException ex)
        {
            return Unauthorized(new {message="Invalid Username or Password"});
        }
        catch(Exception ex)
        {
            return StatusCode(500,"An internal server error occurred. Please try again later");
        }
        
    }
}