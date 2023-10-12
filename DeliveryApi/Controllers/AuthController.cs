using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;
[Route("api/account/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _AuthService;
    public AuthController(IAuthService authService)
    {
        _AuthService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Register(UserRegistration model)
    {
        return Ok( await _AuthService.CreateUser(model));
    }
    
    [HttpPost]
    public async Task<ActionResult<string>> Login(UserLogin model)
    {
        return Ok( await _AuthService.LoginUser(model));
    }
}