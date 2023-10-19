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
    [ProducesResponseType(typeof(TokenResponse), 200)]
    [ProducesResponseType(typeof(Response), 400)]
    public async Task<IActionResult> Register(UserRegistration model)
    {
        try
        {
            return Ok(new TokenResponse { Token = await _accountService.CreateUser(model) });
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
    [HttpGet("logout")]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> Logout()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            await _accountService.LogoutUser(token);
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }

        return Ok();
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<ActionResult<UserProfile>> GetProfile()
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            return Ok(await _accountService.GetProfile(token));
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }

    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(Response), 500)]
    public async Task<IActionResult> EditProfile(UserEditProfile model)
    {
        var token = JwtTokenParseHelper.NormalizeToken(Request.Headers["Authorization"]);
        try
        {
            await _accountService.EditProfile(token, model);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new Response { Message = e.Message });
        }
    }
}