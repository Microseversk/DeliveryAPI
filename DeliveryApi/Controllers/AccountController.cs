using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;
[Route("api/[controller]/")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(UserRegistration model)
    {
        return Ok(await _accountService.CreateUser(model));
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLogin model)
    {
        return Ok(await _accountService.LoginUser(model));
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfile>> GetProfile()
    {
        var token = Request.Headers["Authorization"].ToString();

        if (!token.StartsWith("Bearer "))
        {
            return BadRequest();
        }
        token = token.Substring("Bearer ".Length);
        
        return Ok( await _accountService.GetProfile(token));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult> EditProfile(UserEditProfile model)
    {
        var token = Request.Headers["Authorization"].ToString();

        if (!token.StartsWith("Bearer "))
        {
            return BadRequest();
        }
        token = token.Substring("Bearer ".Length);

        await _accountService.EditProfile(token, model);
        return Ok();
    }
}