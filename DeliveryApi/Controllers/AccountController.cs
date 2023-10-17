using DeliveryApi.Helpers;
using DeliveryApi.Models;
using DeliveryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryApi.Controllers;

[Route("/")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponse),200)]
    [ProducesResponseType(typeof(Response),400)]
    public async Task<IActionResult> Register(UserRegistration model)
    {
        try
        {
            return Ok(new TokenResponse{Token = await _accountService.CreateUser(model)});
        }
        catch (Exception e)
        {
            return BadRequest(new Response { Message = e.Message });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(Response), 400)]
    public async Task<IActionResult> Login(UserLogin model)
    {
        try
        {
            return Ok(new TokenResponse { Token = await _accountService.LoginUser(model) });
        }
        catch (Exception e)
        {
            return BadRequest(new Response { Message = e.Message });
        }
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserProfile>> GetProfile()
    {
        var token = JwtParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        return Ok(await _accountService.GetProfile(token));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> EditProfile(UserEditProfile model)
    {
        var token = JwtParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        await _accountService.EditProfile(token, model);
        return Ok();
    }
}