using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;
[Route("api/account/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Register(UserRegistration model)
    {
        return Ok( await _authService.CreateUser(model));
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Login(UserLogin model)
    {
        return Ok( await _authService.LoginUser(model));
    }
}