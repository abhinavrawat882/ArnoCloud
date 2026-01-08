using Asp.Versioning;
using Auth.Service.DTO;
using Microsoft.AspNetCore.Mvc;
using Notes.Service.DTO;
using Notes.Service.Services;

namespace Core.API.Controllers.V1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/products")]
public class AuthController : ControllerBase
{
    public AuthController(){
        
    }

    // [HttpPost("Login")]
    // public IActionResult Login(LoginCredDTO)
    // {
        
    // }
}